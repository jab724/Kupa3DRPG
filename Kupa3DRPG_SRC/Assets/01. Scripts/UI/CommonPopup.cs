using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//무난하게 사용할 공통 팝업. 가장 많이 쓰는 유형인 1버튼 (확인), 2버튼(확인/취소) 팝업을 생성
namespace Kupa
{
    public class CommonPopup : MonoBehaviourUI
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button okBtn;
        [SerializeField] private Button cancelBtn;

        private bool is1Btn;
        private bool isEnableEscape;

        public void Set2BtnPopup(string title, string description, string okText, string cancelText, UnityAction okListener, UnityAction cancelListener, bool isEnableEscape = true)
        {
            titleText.text = title;
            descriptionText.text = description;
            okBtn.GetComponentInChildren<TMP_Text>().text = okText;
            cancelBtn.GetComponentInChildren<TMP_Text>().text = cancelText;
            okBtn.onClick.AddListener(okListener);
            cancelBtn.onClick.AddListener(cancelListener);
            is1Btn = false;
            this.isEnableEscape = isEnableEscape;
        }

        public void Set2BtnPopup(string title, string description, UnityAction okListener, UnityAction cancelListener, bool isEnableEscape = true)
        {
            titleText.text = title;
            descriptionText.text = description;
            okBtn.onClick.AddListener(okListener);
            cancelBtn.onClick.AddListener(cancelListener);
            is1Btn = false;
            this.isEnableEscape = isEnableEscape;
        }

        public void Set1BtnPopup(string title, string description, string okText, UnityAction okListener, bool isEnableEscape = true)
        {
            titleText.text = title;
            descriptionText.text = description;
            cancelBtn.gameObject.SetActive(false);
            okBtn.GetComponentInChildren<TMP_Text>().text = okText;
            okBtn.onClick.AddListener(okListener);
            is1Btn = true;
            this.isEnableEscape = isEnableEscape;
        }

        public void Set1BtnPopup(string title, string description, UnityAction okListener, bool isEnableEscape = true)
        {
            titleText.text = title;
            descriptionText.text = description;
            cancelBtn.gameObject.SetActive(false);
            okBtn.onClick.AddListener(okListener);
            is1Btn = true;
            this.isEnableEscape = isEnableEscape;
        }

        protected override void Close()     //일부 팝업은 꼭 버튼을 눌러야만 넘어갈 수 있도록 조건부로 Escape 버튼을 무시하도록 한다.
        {
            if (isEnableEscape)
            {
                if (is1Btn) DestroyPopup();
                else cancelBtn.onClick.Invoke();
            }
        }

        public void DestroyPopup()
        {
            Destroy(gameObject);
        }
    }
}