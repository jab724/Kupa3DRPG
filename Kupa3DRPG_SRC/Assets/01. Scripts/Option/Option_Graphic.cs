using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//옵션 중 그래픽 관련 옵션 관리
namespace Kupa
{
    public class Option_Graphic : MonoBehaviourUI
    {
        List<(int, int)> resolutionList = new List<(int, int)>() { (960, 540), (1280, 720), (1366, 768), (1600, 900), (1920, 1080), (2560, 1440), (3840, 2160), (7680, 4320) }; //해상도 리스트. 일단 16:9만 사용. 최대 8k까지 감지
        List<int> framerateList = new List<int>() { 30, 60, 120, 144, 240, 0 };     //프레임레이트 리스트.

        //엔진에서 마우스로 끌어서 참조하는 부분을 최소화. 스크립트에서 대부분 처리하도록 
        [SerializeField] private Transform resolutionObject;
        [SerializeField] private Transform fullScreenModeObject;
        [SerializeField] private Transform framerateObject;
        [SerializeField] private Transform textureQualityObject;
        [SerializeField] private Transform shadowQualityObject;
        [SerializeField] private Transform antiAliasingObject;
        [SerializeField] private Transform vSyncObject;
        [SerializeField] private Transform anisotropicFilteringObject;

        private TMP_Text resolutionText;
        private TMP_Text fullScreenModeText;
        private TMP_Text framerateText;
        private TMP_Text textureQualityText;
        private TMP_Text shadowQualityText;
        private TMP_Text antiAliasingText;
        private TMP_Text vSyncText;
        private TMP_Text anisotropicFilteringText;

        private Button resolutionButtonDown;
        private Button fullScreenModeButtonDown;
        private Button framerateButtonDown;
        private Button textureQualityButtonDown;
        private Button shadowQualityButtonDown;
        private Button antiAliasingButtonDown;
        private Button vSyncButtonDown;
        private Button anisotropicFilteringButtonDown;

        private Button resolutionButtonUp;
        private Button fullScreenModeButtonUp;
        private Button framerateButtonUp;
        private Button textureQualityButtonUp;
        private Button shadowQualityButtonUp;
        private Button antiAliasingButtonUp;
        private Button vSyncButtonUp;
        private Button anisotropicFilteringButtonUp;

        private (int, int) resolution;     //해상도
        private int fullScreenMode;        //전체 화면
        private int framerate;             //주사율
        private int textureQuality;        //텍스처
        private int shadowQuality;         //그림자
        private int antiAliasing;          //안티앨리어싱
        private int vSync;                 //수직동기화    
        private int anisotropicFiltering;  //비등방성 필터링

        private OptionCanvas optionCanvas;

        private void Awake()
        {
            for (int i = resolutionList.Count - 1; 0 <= i; --i)
            {
                if (Screen.currentResolution.height < resolutionList[i].Item2)
                    resolutionList.RemoveAt(i);
                else break;
            }

            InitOptionItem(resolutionObject, out resolutionText, out resolutionButtonDown, out resolutionButtonUp, OnClickResolutionDown, OnClickResolutionUp);
            InitOptionItem(fullScreenModeObject, out fullScreenModeText, out fullScreenModeButtonDown, out fullScreenModeButtonUp, OnClickFullScreenModeDown, OnClickFullScreenModeUp);
            InitOptionItem(framerateObject, out framerateText, out framerateButtonDown, out framerateButtonUp, OnClickFramerateDown, OnClickFramerateUp);
            InitOptionItem(textureQualityObject, out textureQualityText, out textureQualityButtonDown, out textureQualityButtonUp, OnClickTextureQualityDown, OnClickTextureQualityUp);
            InitOptionItem(shadowQualityObject, out shadowQualityText, out shadowQualityButtonDown, out shadowQualityButtonUp, OnClickShadowQualityDown, OnClickShadowQualityUp);
            InitOptionItem(antiAliasingObject, out antiAliasingText, out antiAliasingButtonDown, out antiAliasingButtonUp, OnClickAntiAliasingDown, OnClickAntiAliasingUp);
            InitOptionItem(vSyncObject, out vSyncText, out vSyncButtonDown, out vSyncButtonUp, OnClickVSyncDown, OnClickVSyncUp);
            InitOptionItem(anisotropicFilteringObject, out anisotropicFilteringText, out anisotropicFilteringButtonDown, out anisotropicFilteringButtonUp, OnClickAnisotropicFilteringDown, OnClickAnisotropicFilteringUp);

            optionCanvas = GetComponentInParent<OptionCanvas>();    //옵션 내 공용 버튼을 위함. (적용, 닫기 버튼)
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            resolution.Item1 = PreferenceData.ResolutionWidth;
            resolution.Item2 = PreferenceData.ResolutionHeight;
            fullScreenMode = PreferenceData.FullScreenMode;
            framerate = PreferenceData.Framerate;
            textureQuality = PreferenceData.TextureQuality;
            shadowQuality = PreferenceData.ShadowQuality;
            antiAliasing = PreferenceData.AntiAliasing;
            vSync = PreferenceData.VSync;
            anisotropicFiltering = PreferenceData.AnisotropicFiltering;

            UpdateResolution();
            UpdateFullScreenMode();
            UpdateFramerate();
            UpdateTextureQuality();
            UpdateShadowQuality();
            UpdateAntiAliasing();
            UpdateVSync();
            UpdateAnisotropicFiltering();

            optionCanvas.SetApplyOnClickListener(true, OnClickApply);
            optionCanvas.SetCloseOnClickListener(true, Close);
        }

        public void OnClickApply()      //현재 적용 버튼은 그래픽 옵션에서만 사용함. 다른 기능은 즉시 반영하여 적용 버튼이 별도로 필요하지 않음
        {
            if (CheckGraphicSettingChange())    //그래픽 변경 시 정상적으로 표시가 되지 않을 수도 있으므로 15초 이내로 사용자 입력이 없으면 이전 설정으로 되돌아 올 수 있도록 안전장치를 해둔다.
            {
                Screen.SetResolution(resolution.Item1, resolution.Item2, (FullScreenMode)fullScreenMode, framerate);
                QualitySettings.masterTextureLimit = textureQuality;
                if (shadowQuality == -1)
                {
                    QualitySettings.shadows = UnityEngine.ShadowQuality.Disable;
                    QualitySettings.shadowResolution = ShadowResolution.Low;
                }
                else
                {
                    QualitySettings.shadows = UnityEngine.ShadowQuality.All;
                    QualitySettings.shadowResolution = (ShadowResolution)shadowQuality;
                }
                QualitySettings.antiAliasing = antiAliasing;
                QualitySettings.vSyncCount = vSync;
                QualitySettings.anisotropicFiltering = (AnisotropicFiltering)anisotropicFiltering;

                GraphicSettingWarningPopup popup = null;
                UIManager.Self.CreateGraphicSettingWarningPopup(out popup, () =>
                {
                    PreferenceData.ResolutionWidth = resolution.Item1;
                    PreferenceData.ResolutionHeight = resolution.Item2;
                    PreferenceData.FullScreenMode = fullScreenMode;
                    PreferenceData.Framerate = framerate;
                    PreferenceData.TextureQuality = textureQuality;
                    PreferenceData.ShadowQuality = shadowQuality;
                    PreferenceData.AntiAliasing = antiAliasing;
                    PreferenceData.VSync = vSync;
                    PreferenceData.AnisotropicFiltering = anisotropicFiltering;
                    popup.DestroyPopup();
                },
                () =>
                {
                    PreferenceData.ApplyGraphicOptionSetting();
                    OnEnable();
                    popup.DestroyPopup();
                });
            }
        }

        protected override void Close()     //다른 옵션과 달리 그래픽은 바로 적용이 되지 않으므로 적용하지 않고 나가려 하면 경고 팝업을 띄운다.
        {
            if (CheckGraphicSettingChange())
            {
                CommonPopup popup = null;
                UIManager.Self.Create2BtnCommonPopup(out popup, "설정 닫기", "저장되지 않은 변경 사항이 있습니다.\n저장하지 않고 창을 닫습니까?", "닫기", "돌아가기",
                () =>
                {
                    UIManager.Self.OpenCanvasOption(false);
                    popup.DestroyPopup();
                },
                     () =>
                     {
                         popup.DestroyPopup();
                     });
            }
            else
            {
                UIManager.Self.OpenCanvasOption(false);
            }
        }

        private void OnClickResolutionDown()
        {
            if (resolutionList[resolutionList.Count - 1].Item1 < resolution.Item1)
            {
                resolution.Item1 = resolutionList[resolutionList.Count - 1].Item1;
                resolution.Item2 = resolutionList[resolutionList.Count - 1].Item2;
            }
            else
            {
                for (int i = 0; i < resolutionList.Count; ++i)
                {
                    if (resolution.Item1 == resolutionList[i].Item1)
                    {
                        resolution.Item1 = resolutionList[i - 1].Item1;
                        resolution.Item2 = resolutionList[i - 1].Item2;
                        break;
                    }
                }
            }
            UpdateResolution();
        }
        private void OnClickResolutionUp()
        {
            if (resolution.Item1 < resolutionList[0].Item1)
            {
                resolution.Item1 = resolutionList[0].Item1;
                resolution.Item2 = resolutionList[0].Item2;
            }
            else
            {
                for (int i = 0; i < resolutionList.Count; ++i)
                {
                    if (resolution.Item1 == resolutionList[i].Item1)
                    {
                        resolution.Item1 = resolutionList[i + 1].Item1;
                        resolution.Item2 = resolutionList[i + 1].Item2;
                        break;
                    }
                }
            }
            UpdateResolution();
        }
        private void OnClickFullScreenModeDown()
        {
            if (fullScreenMode == 1) fullScreenMode = 0;
            else if (fullScreenMode == 3) fullScreenMode = 1;
            UpdateFullScreenMode();
        }
        private void OnClickFullScreenModeUp()
        {
            if (fullScreenMode == 0) fullScreenMode = 1;
            else if (fullScreenMode == 1) fullScreenMode = 3;
            UpdateFullScreenMode();
        }
        private void OnClickFramerateDown()
        {
            for (int i = 0; i < framerateList.Count; ++i)
            {
                if (framerate == framerateList[i])
                {
                    framerate = framerateList[i - 1];
                    break;
                }
            }
            UpdateFramerate();
        }
        private void OnClickFramerateUp()
        {
            for (int i = 0; i < framerateList.Count; ++i)
            {
                if (framerate == framerateList[i])
                {
                    framerate = framerateList[i + 1];
                    break;
                }
            }
            UpdateFramerate();
        }
        private void OnClickTextureQualityDown()
        {
            ++textureQuality;
            UpdateTextureQuality();
        }
        private void OnClickTextureQualityUp()
        {
            --textureQuality;
            UpdateTextureQuality();
        }
        private void OnClickShadowQualityDown()
        {
            --shadowQuality;
            UpdateShadowQuality();
        }
        private void OnClickShadowQualityUp()
        {
            ++shadowQuality;
            UpdateShadowQuality();
        }
        private void OnClickAntiAliasingDown()
        {
            if (antiAliasing == 2) antiAliasing = 0;
            else if (antiAliasing == 4) antiAliasing = 2;
            else if (antiAliasing == 8) antiAliasing = 4;
            UpdateAntiAliasing();
        }
        private void OnClickAntiAliasingUp()
        {
            if (antiAliasing == 0) antiAliasing = 2;
            else if (antiAliasing == 2) antiAliasing = 4;
            else if (antiAliasing == 4) antiAliasing = 8;
            UpdateAntiAliasing();
        }
        private void OnClickVSyncDown()
        {
            vSync = 0;
            UpdateVSync();
        }
        private void OnClickVSyncUp()
        {
            vSync = 1;
            UpdateVSync();
        }
        private void OnClickAnisotropicFilteringDown()
        {
            anisotropicFiltering = 0;
            UpdateAnisotropicFiltering();
        }
        private void OnClickAnisotropicFilteringUp()
        {
            anisotropicFiltering = 2;
            UpdateAnisotropicFiltering();
        }

        private void UpdateResolution()
        {
            resolutionText.text = resolution.Item1 + " x " + resolution.Item2;

#if UNITY_WEBGL
            resolutionButtonDown.interactable = false;
            resolutionButtonUp.interactable = false;
#else
            resolutionButtonDown.interactable = resolutionList[0].Item1 < resolution.Item1;
            resolutionButtonUp.interactable = resolution.Item1 < resolutionList[resolutionList.Count - 1].Item1;
#endif
        }
        private void UpdateFullScreenMode()
        {
            switch (fullScreenMode)
            {
                case 0:
                    fullScreenModeText.text = "전체 화면";
                    break;
                case 1:
                    fullScreenModeText.text = "전체 창모드";
                    break;
                case 3:
                    fullScreenModeText.text = "창모드";
                    break;
                default:
                    fullScreenModeText.text = "Error";
                    break;
            }
#if UNITY_WEBGL
            fullScreenModeButtonDown.interactable = false;
            fullScreenModeButtonUp.interactable = false;
#else
            fullScreenModeButtonDown.interactable = fullScreenMode != 0;
            fullScreenModeButtonUp.interactable = fullScreenMode != 2;
#endif
        }
        private void UpdateFramerate()
        {
            switch (framerate)
            {
                case 0:
                    framerateText.text = "무한";
                    break;
                default:
                    framerateText.text = framerate + " Hz";
                    break;
            }

            framerateButtonDown.interactable = framerate != framerateList[0];
            framerateButtonUp.interactable = framerate != framerateList[framerateList.Count - 1];
        }
        private void UpdateTextureQuality()
        {

            switch (textureQuality)
            {
                case 0:
                    textureQualityText.text = "높음";
                    break;
                case 1:
                    textureQualityText.text = "중간";
                    break;
                case 2:
                    textureQualityText.text = "낮음";
                    break;
                default:
                    textureQualityText.text = "Error";
                    break;
            }

            textureQualityButtonDown.interactable = textureQuality != 2;
            textureQualityButtonUp.interactable = textureQuality != 0;
        }
        private void UpdateShadowQuality()
        {

            switch (shadowQuality)
            {
                case -1:
                    shadowQualityText.text = "끄기";
                    break;
                case 0:
                    shadowQualityText.text = "낮음";
                    break;
                case 1:
                    shadowQualityText.text = "중간";
                    break;
                case 2:
                    shadowQualityText.text = "높음";
                    break;
                case 3:
                    shadowQualityText.text = "매우 높음";
                    break;
                default:
                    shadowQualityText.text = "Error";
                    break;
            }

            shadowQualityButtonDown.interactable = shadowQuality != -1;
            shadowQualityButtonUp.interactable = shadowQuality != 3;
        }
        private void UpdateAntiAliasing()
        {

            switch (antiAliasing)
            {
                case 0:
                    antiAliasingText.text = "끄기";
                    break;
                case 2:
                    antiAliasingText.text = "MSAA x2";
                    break;
                case 4:
                    antiAliasingText.text = "MSAA x4";
                    break;
                case 8:
                    antiAliasingText.text = "MSAA x8";
                    break;
                default:
                    antiAliasingText.text = "Error";
                    break;
            }

            antiAliasingButtonDown.interactable = antiAliasing != 0;
            antiAliasingButtonUp.interactable = antiAliasing != 8;
        }
        private void UpdateVSync()
        {

            switch (vSync)
            {
                case 0:
                    vSyncText.text = "끄기";
                    break;
                case 1:
                    vSyncText.text = "켜기";
                    break;
                default:
                    vSyncText.text = "Error";
                    break;
            }

            vSyncButtonDown.interactable = vSync != 0;
            vSyncButtonUp.interactable = vSync != 1;
        }
        private void UpdateAnisotropicFiltering()
        {

            switch (anisotropicFiltering)
            {
                case 0:
                    anisotropicFilteringText.text = "끄기";
                    break;
                case 2:
                    anisotropicFilteringText.text = "켜기";
                    break;
                default:
                    anisotropicFilteringText.text = "Error";
                    break;
            }

            anisotropicFilteringButtonDown.interactable = anisotropicFiltering != 0;
            anisotropicFilteringButtonUp.interactable = anisotropicFiltering != 2;
        }

        private void InitOptionItem(Transform itemObj, out TMP_Text valueText, out Button DownBtn, out Button UpBtn, UnityAction OnClickDownListener, UnityAction OnClickUpListener)
        {
            valueText = itemObj.Find("TMP_Value").GetComponent<TMP_Text>();
            DownBtn = itemObj.Find("Btn_Down").GetComponent<Button>();
            UpBtn = itemObj.Find("Btn_Up").GetComponent<Button>();

            DownBtn.onClick.AddListener(OnClickDownListener);
            UpBtn.onClick.AddListener(OnClickUpListener);
        }

        private bool CheckGraphicSettingChange()    //옵션을 변경한게 있는지 체크
        {
            return PreferenceData.ResolutionWidth != resolution.Item1 ||
            PreferenceData.ResolutionHeight != resolution.Item2 ||
            PreferenceData.FullScreenMode != fullScreenMode ||
            PreferenceData.Framerate != framerate ||
            PreferenceData.TextureQuality != textureQuality ||
            PreferenceData.ShadowQuality != shadowQuality ||
            PreferenceData.AntiAliasing != antiAliasing ||
            PreferenceData.VSync != vSync ||
            PreferenceData.AnisotropicFiltering != anisotropicFiltering;
        }
    }
}