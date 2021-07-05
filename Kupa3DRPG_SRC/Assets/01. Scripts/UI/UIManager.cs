using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//UI 관련 전역 관리
namespace Kupa
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Self
        {
            get
            {
                return instance;
            }
        }

        [Header("Prefab")]
        public GameObject commonPopupPrefab;
        public GameObject graphicSettingWarningPopup;
        [Header("Heirarchy")]
        public Transform popupCanvas;
        public GameObject pauseCanvas;
        public GameObject optionCanvas;

        private List<UnityAction> escapeKeyDownEventList = new List<UnityAction>();     //Escape 버튼으로 창을 종료하기 위함. MonoBehaviourUI를 상속받은 UI들은 화면에 노출되면 순서대로 리스트에 들어간다.

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (0 < escapeKeyDownEventList.Count)
                    escapeKeyDownEventList[escapeKeyDownEventList.Count - 1].Invoke();
                else
                    OpenCanvasPause();      //떠있는 UI가 없는 상태에서 Escape 버튼 입력 시 일시정지 화면을 띄운다.
            }
        }

        public void AddEscapeListener(UnityAction listener)
        {
            RemoveEscapeListener(listener);
            escapeKeyDownEventList.Add(listener);
        }

        public void RemoveEscapeListener(UnityAction listener)
        {
            escapeKeyDownEventList.Remove(listener);
        }

        public CommonPopup Create2BtnCommonPopup(out CommonPopup popup, string title, string description, string okText, string cancelText, UnityAction okListener, UnityAction cancelListener)
        {
            popup = Instantiate(commonPopupPrefab, popupCanvas).GetComponent<CommonPopup>();
            popup.Set2BtnPopup(title, description, okText, cancelText, okListener, cancelListener);
            return popup;
        }
        public CommonPopup Create2BtnCommonPopup(out CommonPopup popup, string title, string description, UnityAction okListener, UnityAction cancelListener)
        {
            popup = Instantiate(commonPopupPrefab, popupCanvas).GetComponent<CommonPopup>();
            popup.Set2BtnPopup(title, description, okListener, cancelListener);
            return popup;
        }
        public CommonPopup Create1BtnCommonPopup(out CommonPopup popup, string title, string description, string okText, UnityAction okListener)
        {
            popup = Instantiate(commonPopupPrefab, popupCanvas).GetComponent<CommonPopup>();
            popup.Set1BtnPopup(title, description, okText, okListener);
            return popup;
        }
        public CommonPopup Create1BtnCommonPopup(out CommonPopup popup, string title, string description, UnityAction okListener)
        {
            popup = Instantiate(commonPopupPrefab, popupCanvas).GetComponent<CommonPopup>();
            popup.Set1BtnPopup(title, description, okListener);
            return popup;
        }
        public GraphicSettingWarningPopup CreateGraphicSettingWarningPopup(out GraphicSettingWarningPopup popup, UnityAction okListener, UnityAction cancelListener)
        {
            popup = Instantiate(graphicSettingWarningPopup, popupCanvas).GetComponent<GraphicSettingWarningPopup>();
            popup.SetGraphicSettingWarningPopup(okListener, cancelListener);
            return popup;
        }
        public void OpenCanvasPause(bool isOpen = true)
        {
            pauseCanvas.SetActive(isOpen);
        }
        public void OpenCanvasOption(bool isOpen = true)
        {
            optionCanvas.SetActive(isOpen);
        }
    }
}