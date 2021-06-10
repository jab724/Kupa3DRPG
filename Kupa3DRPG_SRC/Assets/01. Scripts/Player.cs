﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kupa
{
    public class Player : MonoBehaviour
    {
        [SerializeField] [Tooltip("걷는 속도")] private float walkSpeed = 5.0f;
        [SerializeField] [Tooltip("달리는 속도")] private float runSpeed = 10.0f;
        [SerializeField] [Tooltip("점프력")] private float jumpPower = 6.0f;
        [SerializeField] [Tooltip("카메라 거리")] [MinMax(0.1f, 10f, ShowEditRange = true)] private MinMaxCurrentValue cameraDistance = new MinMaxCurrentValue(1f, 5f);

        private Transform modelTransform;
        private Transform cameraPivotTransform;
        private Transform cameraTransform;
        private CharacterController characterController;
        private Animator animator;

        private Vector3 mouseMove;      //카메라 회전값
        private Vector3 inputMove;      //현재 입력중인 이동 값(wasd)
        private Vector3 velocity;       //최종적으로 CharacterController에 반영할 값 (중력 포함)
        private bool isRun;             //달리기 상태
        private bool IsRun { get { return isRun; } set { isRun = value; animator.SetBool("isRun", value); } }   //값을 변경하면 애니메이터 값도 자동으로 변경되도록

        void Awake()
        {
            characterController = GetComponent<CharacterController>();
            modelTransform = transform.GetChild(0);
            animator = modelTransform.GetComponent<Animator>();
            cameraTransform = Camera.main.transform;
            cameraPivotTransform = cameraTransform.parent;
        }

        void Update()   //캐릭터 조정 및 컨트롤 반영은 여기서 진행
        {
            if (Time.timeScale < 0.001f) return;         //일시정지 등 시간을 멈춘 상태에선 입력 방지

            FreezeRotationXZ();     //CharacterController 캡슐이 어떤 이유로든 기울어지지 않도록 방지
            CameraDistanceCtrl();   //카메라 거리 조작
            RunCheck();             //달리기 상태 토글

            if (characterController.isGrounded)     //지면에 발이 닿아있는 경우
            {
                animator.SetBool("isGrounded", true);
                if (JumpCheck())     //점프 체크
                {
                    velocity.y = jumpPower;
                }
                else
                {
                    if (GroundCheck())    //경사로 체크
                        velocity.y = -5;
                    else
                        velocity.y = -1;
                }
            }
            else
            {
                if (GroundCheck() == false)
                    animator.SetBool("isGrounded", false);
                velocity += Physics.gravity * Time.deltaTime;   //중력 가산
            }
            MoveCalc();     //움직임 계산

            characterController.Move(velocity * Time.deltaTime);    //최종적으로 CharacterController Move 호출
        }

        void LateUpdate()       //최종 카메라 보정은 여기서 진행
        {
            if (Time.timeScale < 0.001f) return;         //일시정지 등 시간을 멈춘 상태에선 입력 방지

            float playerEyeHeight = 1.7f;
            cameraPivotTransform.position = transform.position + Vector3.up * playerEyeHeight;  //캐릭터의 눈높이쯤
            mouseMove += new Vector3(-Input.GetAxisRaw("Mouse Y") * PreferenceData.MouseSensitivity, Input.GetAxisRaw("Mouse X") * PreferenceData.MouseSensitivity, 0);   //마우스의 움직임을 가감
            if (mouseMove.x < -60)  //상하는 제한을 둔다.
                mouseMove.x = -60;
            else if (60 < mouseMove.x)
                mouseMove.x = 60;

            cameraPivotTransform.localEulerAngles = mouseMove;

            RaycastHit cameraWallHit;   //카메라가 벽 뒤로 가서 화면이 가려지는 것을 방지
            if (Physics.Raycast(cameraPivotTransform.position, cameraTransform.position - cameraPivotTransform.position, out cameraWallHit, cameraDistance.Current))
                cameraTransform.localPosition = Vector3.back * cameraWallHit.distance;
            else
                cameraTransform.localPosition = Vector3.back * cameraDistance.Current;
        }

        void FreezeRotationXZ()
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);   //기울어짐 방지
        }

        void CameraDistanceCtrl()
        {
            cameraDistance.Current -= Input.GetAxisRaw("Mouse ScrollWheel");     //휠로 카메라의 거리를 조절
        }

        void RunCheck()
        {
            if (IsRun == false && Input.GetKeyDown(KeyCode.LeftShift))  //왼쪽 쉬프트를 누르면 달리기 상태
                IsRun = true;
            if (IsRun && Mathf.Abs(inputMove.x) < 0.01f && Mathf.Abs(inputMove.y) < 0.01f)  //움직임을 멈추면 달리기 상태 취소
                IsRun = false;
        }

        bool JumpCheck()
        {
            return Input.GetButtonDown("Jump");
        }

        bool GroundCheck()
        {
            //CharacterController의 isGrounded는 이전 프레임 때 Move 등의 함수로 땅 쪽으로 향하였을때 접지가 되어야만 true를 리턴한다.
            //이는 경사로를 내려가거나 울퉁불퉁한 지면을 다닐때 수시로 false 값을 리턴하므로 Raycast로 땅을 한 번 더 체크하여 안정성을 강화한다..
            return Physics.Raycast(transform.position, Vector3.down, 0.2f);
        }

        void MoveCalc()
        {
            //GetAxisRaw를 사용하여 가속 과정 생략. normalized를 사용하여 대각선 이동 시 벡터의 길이가 약 1.41배 되는 부분 보정
            inputMove = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * (IsRun ? runSpeed : walkSpeed);
            animator.SetFloat("speedX", inputMove.x);
            animator.SetFloat("speedY", inputMove.z);
            inputMove = transform.TransformDirection(inputMove);    //입력 키를 카메라가 보고 있는 방향으로 조정

            //조작 중에만 카메라의 방향에 상대적으로 캐릭터가 움직이도록 한다.
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                Quaternion cameraRotation = cameraPivotTransform.rotation;
                cameraRotation.x = cameraRotation.z = 0;    //y축만 필요하므로 나머지 값은 0으로 바꾼다.
                                                            //자연스러움을 위해 Slerp로 회전시킨다.
                transform.rotation = Quaternion.Slerp(transform.rotation, cameraRotation, 10.0f * Time.deltaTime);
                if (inputMove != Vector3.zero)//Quaternion.LookRotation는 (0,0,0)이 들어가면 경고를 내므로 예외처리 해준다.
                {
                    if (IsRun)
                    {
                        //달리기 상태에선 이동 방향으로 몸을 돌린다.
                        Quaternion characterRotation = Quaternion.LookRotation(inputMove);
                        characterRotation.x = characterRotation.z = 0;
                        modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, characterRotation, 10.0f * Time.deltaTime);
                    }
                    else
                    {
                        //통상 상태에선 정면을 유지한채 움직인다.
                        modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, cameraRotation, 10.0f * Time.deltaTime);
                    }
                }
            }

            velocity = new Vector3(inputMove.x, velocity.y, inputMove.z);   //x, z값 반영
        }
    }
}