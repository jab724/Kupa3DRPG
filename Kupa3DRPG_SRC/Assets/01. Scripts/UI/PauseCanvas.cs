using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//일시정지 캔버스 (현재 WebGL 플랫폼 특성 반영)
//Web GL 환경 특성상 마우스가 보이며 게임 진행에 있어 거슬리기에 마우스 포인터가 보이지 않도록 하고 
//포커스를 잃을 상황이 빈번하여 이에 관한 불편함 최소화
namespace Kupa
{
    public class PauseCanvas : MonoBehaviour
    {
        [SerializeField] private Button continueBtn;
        [SerializeField] private Button restartBtn;
        [SerializeField] private Button optionBtn;
        [SerializeField] private Button exitBtn;

        private void Awake()
        {
            SetPause(false);

            continueBtn.onClick.AddListener(OnClickContinue);
            restartBtn.onClick.AddListener(OnClickRestart);
            optionBtn.onClick.AddListener(OnClickOption);
            exitBtn.onClick.AddListener(OnClickExit);

#if UNITY_WEBGL
            exitBtn.interactable = false;
            exitBtn.GetComponentInChildren<TMP_Text>().text = "<color=#888888><i>종료 (WebGL 불가능)</i></color>";
#endif
        }
        private void OnEnable()
        {
            SetPause(true);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SetPause(true);
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus == false)
                SetPause(true);
        }

        private void SetPause(bool isPause)
        {
            if (isPause)
            {
                Time.timeScale = 0.0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                gameObject.SetActive(true);
            }
            else
            {
                Time.timeScale = 1.0f;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                gameObject.SetActive(false);
            }
        }
        public void OnClickContinue()
        {
            SetPause(false);
        }
        public void OnClickRestart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
        public void OnClickOption()
        {
            UIManager.Self.OpenCanvasOption();
        }
        public void OnClickExit()
        {
            Application.Quit();
        }
    }
}