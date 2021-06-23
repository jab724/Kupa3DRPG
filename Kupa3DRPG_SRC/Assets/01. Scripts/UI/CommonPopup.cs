using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kupa
{
    public class CommonPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button okBtn;
        [SerializeField] private Button cancelBtn;

        public void Set2BtnDialog(string title, string description, string okText, string cancelText, UnityAction okListener, UnityAction cancelListener)
        {
            titleText.text = title;
            descriptionText.text = description;
            okBtn.GetComponentInChildren<TMP_Text>().text = okText;
            cancelBtn.GetComponentInChildren<TMP_Text>().text = cancelText;
            okBtn.onClick.RemoveAllListeners();
            okBtn.onClick.AddListener(okListener);
            cancelBtn.onClick.RemoveAllListeners();
            cancelBtn.onClick.AddListener(cancelListener);
        }

        public void Set2BtnDialog(string title, string description, UnityAction okListener, UnityAction cancelListener)
        {
            titleText.text = title;
            descriptionText.text = description;
            okBtn.onClick.RemoveAllListeners();
            okBtn.onClick.AddListener(okListener);
            cancelBtn.onClick.RemoveAllListeners();
            cancelBtn.onClick.AddListener(cancelListener);
        }

        public void Set1BtnDialog(string title, string description, string okText, UnityAction okListener)
        {
            titleText.text = title;
            descriptionText.text = description;
            cancelBtn.gameObject.SetActive(false);
            okBtn.GetComponentInChildren<TMP_Text>().text = okText;
            okBtn.onClick.RemoveAllListeners();
            okBtn.onClick.AddListener(okListener);
        }

        public void Set1BtnDialog(string title, string description, UnityAction okListener)
        {
            titleText.text = title;
            descriptionText.text = description;
            cancelBtn.gameObject.SetActive(false);
            okBtn.onClick.RemoveAllListeners();
            okBtn.onClick.AddListener(okListener);
        }

        public void DestroyPopup()
        {
            Destroy(gameObject);
        }
    }
}