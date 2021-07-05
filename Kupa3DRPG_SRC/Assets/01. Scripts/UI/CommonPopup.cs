using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

        protected override void Close()
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