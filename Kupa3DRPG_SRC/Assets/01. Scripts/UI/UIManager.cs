using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        public GameObject commonPopupPrefab;
        public Transform popupCanvas;
        public GameObject pauseCanvas;
        public GameObject optionCanvas;

        private CommonPopup commonPopup;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) OpenCanvasPause();
        }

        public CommonPopup Create2BtnCommonDialog(out CommonPopup popup, string title, string description, string okText, string cancelText, UnityAction okListener, UnityAction cancelListener, bool endLoading = false)
        {
            popup = Instantiate(commonPopupPrefab, popupCanvas).GetComponent<CommonPopup>();
            if (this.commonPopup == null) this.commonPopup = popup;
            popup.Set2BtnDialog(title, description, okText, cancelText, okListener, cancelListener);
            return popup;
        }
        public CommonPopup Create2BtnCommonDialog(out CommonPopup popup, string title, string description, UnityAction okListener, UnityAction cancelListener, bool endLoading = false)
        {
            popup = Instantiate(commonPopupPrefab, popupCanvas).GetComponent<CommonPopup>();
            if (this.commonPopup == null) this.commonPopup = popup;
            popup.Set2BtnDialog(title, description, okListener, cancelListener);
            return popup;
        }
        public CommonPopup Create1BtnCommonDialog(out CommonPopup popup, string title, string description, string okText, UnityAction okListener, bool endLoading = false)
        {
            popup = Instantiate(commonPopupPrefab, popupCanvas).GetComponent<CommonPopup>();
            if (this.commonPopup == null) this.commonPopup = popup;
            popup.Set1BtnDialog(title, description, okText, okListener);
            return popup;
        }
        public CommonPopup Create1BtnCommonDialog(out CommonPopup popup, string title, string description, UnityAction okListener, bool endLoading = false)
        {
            popup = Instantiate(commonPopupPrefab, popupCanvas).GetComponent<CommonPopup>();
            if (this.commonPopup == null) this.commonPopup = popup;
            popup.Set1BtnDialog(title, description, okListener);
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