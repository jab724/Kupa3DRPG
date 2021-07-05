using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kupa
{
    public class Option_GamePlay : MonoBehaviourUI
    {
        [SerializeField] private Transform mouseSensitivityObject;

        private TMP_Text mouseSensitivityText;

        private Button mouseSensitivityButtonDown;

        private Button mouseSensitivityButtonUp;

        private Slider mouseSensitivitySlider;

        private int mouseSensitivity;         //마우스 감도

        private OptionCanvas optionCanvas;

        private void Awake()
        {
            InitOptionItem(mouseSensitivityObject, out mouseSensitivityText, out mouseSensitivityButtonDown, out mouseSensitivityButtonUp, out mouseSensitivitySlider, OnClickMouseSensitivityDown, OnClickMouseSensitivityUp, OnValueChangedMouseSensitivity);

            optionCanvas = GetComponentInParent<OptionCanvas>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            mouseSensitivity = PreferenceData.MouseSensitivity;

            UpdateMouseSensitivity();

            optionCanvas.SetApplyOnClickListener(false);
            optionCanvas.SetCloseOnClickListener(true, Close);
        }

        // public void OnClickApply()
        // {
        // }

        protected override void Close()
        {
            UIManager.Self.OpenCanvasOption(false);
        }

        private void OnClickMouseSensitivityDown()
        {
            if (--mouseSensitivity < 1) mouseSensitivity = 1;
            PreferenceData.MouseSensitivity = mouseSensitivity;
            UpdateMouseSensitivity();
        }
        private void OnClickMouseSensitivityUp()
        {
            if (100 < ++mouseSensitivity) mouseSensitivity = 100;
            PreferenceData.MouseSensitivity = mouseSensitivity;
            UpdateMouseSensitivity();
        }

        private void OnValueChangedMouseSensitivity(float volume)
        {
            PreferenceData.MouseSensitivity = mouseSensitivity = Mathf.RoundToInt(volume);
            UpdateMouseSensitivity();
        }

        private void UpdateMouseSensitivity()
        {
            mouseSensitivitySlider.value = mouseSensitivity;
            mouseSensitivityText.text = mouseSensitivity.ToString();
        }

        private void InitOptionItem(Transform itemObj, out TMP_Text valueText, out Button DownBtn, out Button UpBtn, out Slider slider, UnityAction OnClickDownListener, UnityAction OnClickUpListener, UnityAction<float> OnValueChangedListener)
        {
            valueText = itemObj.Find("TMP_Value").GetComponent<TMP_Text>();
            DownBtn = itemObj.Find("Btn_Down").GetComponent<Button>();
            UpBtn = itemObj.Find("Btn_Up").GetComponent<Button>();
            slider = itemObj.Find("Slider").GetComponent<Slider>();

            DownBtn.onClick.AddListener(OnClickDownListener);
            UpBtn.onClick.AddListener(OnClickUpListener);
            slider.onValueChanged.AddListener(OnValueChangedListener);
        }
    }
}