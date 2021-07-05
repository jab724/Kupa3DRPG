using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//옵션 중 사운드 관련 옵션 관리
namespace Kupa
{
    public class Option_Sound : MonoBehaviourUI
    {
        //엔진에서 마우스로 끌어서 참조하는 부분을 최소화. 스크립트에서 대부분 처리하도록 
        [SerializeField] private Transform masterObject;
        [SerializeField] private Transform bgmObject;
        [SerializeField] private Transform sfxObject;
        [SerializeField] private Transform voiceObject;

        private TMP_Text masterText;
        private TMP_Text bgmText;
        private TMP_Text sfxText;
        private TMP_Text voiceText;

        private Button masterButtonDown;
        private Button bgmButtonDown;
        private Button sfxButtonDown;
        private Button voiceButtonDown;

        private Button masterButtonUp;
        private Button bgmButtonUp;
        private Button sfxButtonUp;
        private Button voiceButtonUp;

        private Slider masterSlider;
        private Slider bgmSlider;
        private Slider sfxSlider;
        private Slider voiceSlider;

        private int master;         //전체 볼륨
        private int bgm;            //배경음
        private int sfx;            //효과음
        private int voice;          //대사

        private OptionCanvas optionCanvas;

        private void Awake()
        {
            InitOptionItem(masterObject, out masterText, out masterButtonDown, out masterButtonUp, out masterSlider, OnClickMasterDown, OnClickMasterUp, OnValueChangedMaster);
            InitOptionItem(bgmObject, out bgmText, out bgmButtonDown, out bgmButtonUp, out bgmSlider, OnClickBGMDown, OnClickBGMUp, OnValueChangedBGM);
            InitOptionItem(sfxObject, out sfxText, out sfxButtonDown, out sfxButtonUp, out sfxSlider, OnClickSFXDown, OnClickSFXUp, OnValueChangedSFX);
            InitOptionItem(voiceObject, out voiceText, out voiceButtonDown, out voiceButtonUp, out voiceSlider, OnClickVoiceDown, OnClickVoiceUp, OnValueChangedVoice);

            optionCanvas = GetComponentInParent<OptionCanvas>();    //옵션 내 공용 버튼을 위함. (적용, 닫기 버튼)
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            master = PreferenceData.MasterVolume;
            bgm = PreferenceData.BgmVolume;
            sfx = PreferenceData.SfxVolume;
            voice = PreferenceData.VoiceVolume;

            UpdateMaster();
            UpdateBGM();
            UpdateSFX();
            UpdateVoice();

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

        private void OnClickMasterDown()
        {
            if (--master < 0) master = 0;
            PreferenceData.MasterVolume = master;
            UpdateMaster();
        }
        private void OnClickMasterUp()
        {
            if (100 < ++master) master = 100;
            PreferenceData.MasterVolume = master;
            UpdateMaster();
        }
        private void OnClickBGMDown()
        {
            if (--bgm < 0) bgm = 0;
            PreferenceData.BgmVolume = bgm;
            UpdateBGM();
        }
        private void OnClickBGMUp()
        {
            if (100 < ++bgm) bgm = 100;
            PreferenceData.BgmVolume = bgm;
            UpdateBGM();
        }
        private void OnClickSFXDown()
        {
            if (--sfx < 0) sfx = 0;
            PreferenceData.SfxVolume = sfx;
            UpdateSFX();
        }
        private void OnClickSFXUp()
        {
            if (100 < ++sfx) sfx = 100;
            PreferenceData.SfxVolume = sfx;
            UpdateSFX();
        }
        private void OnClickVoiceDown()
        {
            if (--voice < 0) voice = 0;
            PreferenceData.VoiceVolume = voice;
            UpdateVoice();
        }
        private void OnClickVoiceUp()
        {
            if (100 < ++voice) voice = 100;
            PreferenceData.VoiceVolume = voice;
            UpdateVoice();
        }

        private void OnValueChangedMaster(float volume)
        {
            PreferenceData.MasterVolume = master = Mathf.RoundToInt(volume);
            UpdateMaster();
        }
        private void OnValueChangedBGM(float volume)
        {
            PreferenceData.BgmVolume = bgm = Mathf.RoundToInt(volume);
            UpdateBGM();
        }
        private void OnValueChangedSFX(float volume)
        {
            PreferenceData.SfxVolume = sfx = Mathf.RoundToInt(volume);
            UpdateSFX();
        }
        private void OnValueChangedVoice(float volume)
        {
            PreferenceData.VoiceVolume = voice = Mathf.RoundToInt(volume);
            UpdateVoice();
        }

        private void UpdateMaster()
        {
            masterSlider.value = master;
            masterText.text = master.ToString();
        }
        private void UpdateBGM()
        {
            bgmSlider.value = bgm;
            bgmText.text = bgm.ToString();
        }
        private void UpdateSFX()
        {
            sfxSlider.value = sfx;
            sfxText.text = sfx.ToString();
        }
        private void UpdateVoice()
        {
            voiceSlider.value = voice;
            voiceText.text = voice.ToString();
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