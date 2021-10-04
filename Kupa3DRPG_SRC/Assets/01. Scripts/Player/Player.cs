﻿using UnityEngine;

namespace Kupa
{
    public class Player : MonoBehaviour
    {
        public static class PublicData
        {
            public static bool Enabled { get { return enabled; } }
            private static bool enabled = false;

            public static Transform Transform { get { return transform; } }
            private static Transform transform;

            public static PlayerState PlayerState { get { return playerState; } }
            private static PlayerState playerState = PlayerState.IDLE;

            public static void SetPlayerPublicData(Transform transform, PlayerState playerState)
            {
                enabled = true;
                PublicData.transform = transform;
                PublicData.playerState = playerState;
            }
        }

        public enum PlayerState { IDLE, ATTACK, UNDER_ATTACK, DEAD }

        [SerializeField] [Tooltip("걷는 속도")] private float walkSpeed = 5.0f;
        [SerializeField] [Tooltip("달리는 속도")] private float runSpeed = 10.0f;
        [SerializeField] [Tooltip("카메라 거리")] [Range(1f, 5f)] private float cameraDistance = 2.5f;
        [Header("Status")]
        public float playerHealthPoint = 1000f;

        private Transform modelTransform;
        private Transform cameraPivotTransform;
        private Transform cameraTransform;
        private CharacterController characterController;
        private Animator animator;

        private Vector3 mouseMove;      //카메라 회전값
        private Vector3 moveVelocity;       //이동 속도
        private PlayerState playerState = PlayerState.IDLE;
        private bool isRun;             //달리기 상태
        private bool IsRun { get { return isRun; } set { isRun = value; animator.SetBool("isRun", value); } }  //값을 변경하면 애니메이터 값도 자동으로 변경되도록
        private bool isGroundedCheck;   //timeScale이 0이 되거나 어떤 이유로 한 프레임만 isGrounded가 false가 되는 경우 모션이 튀는 증상을 막기위한 용도 
        private bool isSuperArmor = false;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            modelTransform = transform.GetChild(0);
            animator = modelTransform.GetComponent<Animator>();
            cameraTransform = Camera.main.transform;
            cameraPivotTransform = cameraTransform.parent;

            SetPlayerPublicData();
        }

        private void Update()   //캐릭터 조정 및 컨트롤 반영은 여기서 진행
        {
            if (Time.timeScale < 0.001f) return;         //일시정지 등 시간을 멈춘 상태에선 입력 방지
            FreezeRotationXZ();     //CharacterController 캡슐이 어떤 이유로든 기울어지지 않도록 방지

            switch (playerState)
            {
                case PlayerState.IDLE:
                    PlayerIdle();
                    break;
                case PlayerState.ATTACK:
                    PlayerAttack();
                    break;
                case PlayerState.UNDER_ATTACK:
                    PlayerUnderAttack();
                    break;
                case PlayerState.DEAD:
                    PlayerDead();
                    break;
                default:
                    break;
            }

            CameraDistanceCtrl();   //카메라 거리 조작
        }

        private void LateUpdate()       //최종 카메라 보정은 여기서 진행
        {
            if (Time.timeScale < 0.001f) return;         //일시정지 등 시간을 멈춘 상태에선 입력 방지

            float cameraHeight = 1.3f;
            cameraPivotTransform.position = transform.position + Vector3.up * cameraHeight;  //캐릭터의 가슴 높이쯤
            mouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * PreferenceData.MouseSensitivity * 0.1f, Input.GetAxisRaw("Mouse X") * PreferenceData.MouseSensitivity * 0.1f, 0);   //마우스의 움직임을 가감
            if (mouseMove.x < -60)  //상하 각도는 제한을 둔다.
                mouseMove.x = -60;
            else if (60 < mouseMove.x)
                mouseMove.x = 60;

            cameraPivotTransform.localEulerAngles = mouseMove;

            RaycastHit cameraWallHit;   //카메라가 벽 뒤로 가서 화면이 가려지는 것을 방지
            if (Physics.Raycast(cameraPivotTransform.position, cameraTransform.position - cameraPivotTransform.position, out cameraWallHit, cameraDistance, ~(1 << LayerMask.NameToLayer("Player"))))       //플레이어의 콜라이더에 막히지 않도록
                cameraTransform.localPosition = Vector3.back * cameraWallHit.distance;
            else
                cameraTransform.localPosition = Vector3.back * cameraDistance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "EnemyAttack")
            {
                PlayerUnderAttackEnter(other.GetComponent<PlayerHitObject>());
            }
        }

        private void PlayerIdle()
        {
            RunCheck();             //달리기 상태 체크

            if (characterController.isGrounded)     //지면에 발이 닿아있는 경우
            {
                if (isGroundedCheck == false)
                    isGroundedCheck = true;
                animator.SetBool("isGrounded", true);
                CalcInputMove();        //이동 입력 계산. 땅에서만 컨트롤 가능
                RaycastHit groundHit;
                if (GroundCheck(out groundHit))  //밑으로 Raycast를 쏘아 땅을 한 번 더 확인. 
                    moveVelocity.y = IsRun ? -runSpeed : -walkSpeed;    //isGounded는 다음 프레임때 velocity.y 만큼 내려가도 바닥에 닿지 않으면 false를 리턴한다. 평지에선 속도와 비례해서 y 힘을 주어야 경사로에서도 isGrounded 값이 true가 된다.
                else
                    moveVelocity.y = -1;    //Raycast는 캡슐의 중앙에서 쏘기에 모서리에 걸치면 Raycast는 false이나 isGrounded는 true인 경우가 발생한다. 보통 높은 곳에서 떨어질때 발생하므로 y값을 최소화 하여 자연스럽게 떨어지도록 한다.

                if (Input.GetButtonDown("Fire1"))
                {
                    playerState = PlayerState.ATTACK;
                    animator.SetTrigger("swordAttackTrigger");
                    animator.SetFloat("speedX", 0);
                    animator.SetFloat("speedY", 0);
                    tt = 0;
                }
            }
            else
            {
                if (isGroundedCheck)
                    isGroundedCheck = false;
                else
                    animator.SetBool("isGrounded", false);
                moveVelocity += Physics.gravity * Time.deltaTime;   //중력 가산
            }

            characterController.Move(moveVelocity * Time.deltaTime);    //최종적으로 CharacterController Move 호출
        }

        float tt = 0;
        private void PlayerAttack()
        {
            tt += Time.deltaTime;
            if (2 <= tt)
                playerState = PlayerState.IDLE;
        }
        private void PlayerUnderAttack()
        {

        }
        private void PlayerUnderAttackEnter(PlayerHitObject playerHit)
        {
            playerState = PlayerState.UNDER_ATTACK;
            if (isSuperArmor == false && playerState != PlayerState.DEAD)
                animator.SetTrigger("UnderAttack");
            playerHealthPoint -= playerHit.damage;
            if (playerHealthPoint <= 0f)
            {
                playerHealthPoint = 0f;
                playerState = PlayerState.DEAD;
                animator.SetBool("isDead", true);
            }
        }
        private void PlayerDead()
        {
        }

        private void FreezeRotationXZ()
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);   //기울어짐 방지
        }

        private void CameraDistanceCtrl()
        {
            cameraDistance -= Input.GetAxisRaw("Mouse ScrollWheel");     //휠로 카메라의 거리를 조절
        }

        private void RunCheck()
        {
            if (IsRun == false && Input.GetKeyDown(KeyCode.LeftShift))  //왼쪽 쉬프트를 누르면 달리기 상태
                IsRun = true;
            if (IsRun && Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)  //이동 입력이 없으면 달리기 취소
                IsRun = false;
        }

        private bool JumpCheck()
        {
            return Input.GetButtonDown("Jump");
        }

        private bool GroundCheck(out RaycastHit hit)
        {
            //CharacterController의 isGrounded는 이전 프레임 때 Move 등의 함수로 땅 쪽으로 향하였을때 접지가 되어야만 true를 리턴한다.
            //이는 경사로를 내려가거나 울퉁불퉁한 지면을 다닐때 수시로 false 값을 리턴하므로 Raycast로 땅을 한 번 더 체크하여 안정성을 강화한다..
            return Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f);
        }

        private void CalcInputMove()
        {
            //가속 과정이 조작감을 떨어뜨린다고 생각하여 GetAxisRaw를 사용하여 가속 과정 생략. normalized를 사용하여 대각선 이동 시 벡터의 길이가 약 1.41배 되는 부분 보정
            moveVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * (IsRun ? runSpeed : walkSpeed);
            animator.SetFloat("speedX", Input.GetAxis("Horizontal"));   //모션은 GetAxis을 써야 자연스러우므로 GetAxis 값 사용
            animator.SetFloat("speedY", Input.GetAxis("Vertical"));
            moveVelocity = transform.TransformDirection(moveVelocity);    //입력 키를 카메라가 보고 있는 방향으로 조정

            //조작 중에만 카메라의 방향에 상대적으로 캐릭터가 움직이도록 한다.
            if (0.01f < moveVelocity.sqrMagnitude)
            {
                Quaternion cameraRotation = cameraPivotTransform.rotation;
                cameraRotation.x = cameraRotation.z = 0;    //y축만 필요하므로 나머지 값은 0으로 바꾼다.
                transform.rotation = cameraRotation;
                if (IsRun)
                {
                    //달리기 상태에선 이동 방향으로 몸을 돌린다.
                    Quaternion characterRotation = Quaternion.LookRotation(moveVelocity);
                    characterRotation.x = characterRotation.z = 0;
                    //모델 회전은 자연스러움을 위해 Slerp를 사용
                    modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, characterRotation, 10.0f * Time.deltaTime);
                }
                else
                {
                    //통상 상태에선 정면을 유지한채 움직인다.
                    modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, cameraRotation, 10.0f * Time.deltaTime);
                }
            }
        }

        private void SetPlayerPublicData()
        {
            PublicData.SetPlayerPublicData(transform, playerState);
        }
    }
}