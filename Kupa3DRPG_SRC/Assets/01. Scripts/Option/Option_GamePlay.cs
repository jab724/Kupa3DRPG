using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//옵션 중 게임플레이 관련 옵션 관리
namespace Kupa
{
    public class Option_GamePlay : MonoBehaviourUI
    {
        //엔진에서 마우스로 끌어서 참조하는 부분을 최소화. 스크립트에서 대부분 처리하도록 
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

            optionCanvas = GetComponentInParent<OptionCanvas>();    //옵션 내 공용 버튼을 위함. (적용, 닫기 버튼)
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
            PreferenceData.MouseSensitivity = mouseSensitivity = Mathf.RoundToInt(volume);      //float으로 들어오므로 만에 하나 정상적인 int 값으로 치환되지 않을 수 있어 RoundToInt로 보정해준다.
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