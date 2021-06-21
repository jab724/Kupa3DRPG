using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

//사용자 설정 값의 저장 관리를 위한 클래스. 여기선 제일 쉬운 PlayerPrefs를 사용
namespace Kupa
{
    public static class PreferenceData
    {
        //그래픽 설정 값
        private static int resolutionWidth;       //해상도 너비
        private static int resolutionHeight;      //해상도 높이
        private static int fullScreenMode;        //전체 화면               //0: 전체화면, 1: 전체 창보드, 3: 윈도우 (2도 전체 창모드이나 MAC 전용이라 제외)
        private static int framerate;             //주사율
        private static int textureQuality;        //텍스처                  //0 ~ 2, 0이 제일 고해상도 텍스처 사용
        private static int shadowQuality;         //그림자                  //0 ~ 3, 3이 제일 고해상도 텍스처 사용
        private static int antiAliasing;          //안티앨리어싱            //0: 안함, 2: x2, 4: x4, 8: x8
        private static int vSync;                 //수직동기화              //0: 끄기, 1: 켜기
        private static int anisotropicFiltering;  //비등방성 필터링         //0: 끄기, 2: 켜기

        //사운드 설정 값

        //게임플레이 설정 값
        private static float mouseSensitivity;      //마우스 감도

        //단축키 설정 값

        #region get/set
        public static int ResolutionWidth
        {
            get { return resolutionWidth; }
            set { resolutionWidth = value; PlayerPrefs.SetInt(GetMemberName(() => resolutionWidth), value); }
        }
        public static int ResolutionHeight
        {
            get { return resolutionHeight; }
            set { resolutionHeight = value; PlayerPrefs.SetInt(GetMemberName(() => resolutionHeight), value); }
        }
        public static int FullScreenMode
        {
            get { return fullScreenMode; }
            set { fullScreenMode = value; PlayerPrefs.SetInt(GetMemberName(() => fullScreenMode), value); }
        }
        public static int Framerate
        {
            get { return framerate; }
            set { framerate = value; PlayerPrefs.SetInt(GetMemberName(() => framerate), value); }
        }
        public static int TextureQuality
        {
            get { return textureQuality; }
            set { textureQuality = value; PlayerPrefs.SetInt(GetMemberName(() => textureQuality), value); }
        }
        public static int ShadowQuality
        {
            get { return shadowQuality; }
            set { shadowQuality = value; PlayerPrefs.SetInt(GetMemberName(() => shadowQuality), value); }
        }
        public static int AntiAliasing
        {
            get { return antiAliasing; }
            set { antiAliasing = value; PlayerPrefs.SetInt(GetMemberName(() => antiAliasing), value); }
        }
        public static int VSync
        {
            get { return vSync; }
            set { vSync = value; PlayerPrefs.SetInt(GetMemberName(() => vSync), value); }
        }
        public static int AnisotropicFiltering
        {
            get { return anisotropicFiltering; }
            set { anisotropicFiltering = value; PlayerPrefs.SetInt(GetMemberName(() => anisotropicFiltering), value); }
        }

        public static float MouseSensitivity
        {
            get { return mouseSensitivity; }
            set { mouseSensitivity = value; PlayerPrefs.SetFloat(GetMemberName(() => mouseSensitivity), value); }
        }
        #endregion

        static PreferenceData()     //초기화. PlayerPrefs 내 값을 변수에 할당.
        {
            resolutionWidth = PlayerPrefs.GetInt(GetMemberName(() => resolutionWidth), Screen.width);
            resolutionHeight = PlayerPrefs.GetInt(GetMemberName(() => resolutionHeight), Screen.height);
            fullScreenMode = PlayerPrefs.GetInt(GetMemberName(() => fullScreenMode), 0);
            framerate = PlayerPrefs.GetInt(GetMemberName(() => framerate), 60);
            textureQuality = PlayerPrefs.GetInt(GetMemberName(() => textureQuality), 0);
            shadowQuality = PlayerPrefs.GetInt(GetMemberName(() => shadowQuality), 2);
            antiAliasing = PlayerPrefs.GetInt(GetMemberName(() => antiAliasing), 0);
            vSync = PlayerPrefs.GetInt(GetMemberName(() => vSync), QualitySettings.vSyncCount);
            anisotropicFiltering = PlayerPrefs.GetInt(GetMemberName(() => anisotropicFiltering), 2);
            mouseSensitivity = PlayerPrefs.GetFloat(GetMemberName(() => mouseSensitivity), 2f);
        }

        private static string GetMemberName<T>(Expression<Func<T>> memberExpression)    //변수명을 string으로 리턴해주는 함수. 변수명을 그대로 key로 쓰기 위함. 
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        public static void ApplyGraphicOptionSetting()
        {
            Screen.SetResolution(ResolutionWidth, ResolutionHeight, (FullScreenMode)FullScreenMode, Framerate);
            QualitySettings.masterTextureLimit = TextureQuality;
            if (ShadowQuality == -1)
            {
                QualitySettings.shadows = UnityEngine.ShadowQuality.Disable;
                QualitySettings.shadowResolution = ShadowResolution.Low;
            }
            else
            {
                QualitySettings.shadows = UnityEngine.ShadowQuality.All;
                QualitySettings.shadowResolution = (ShadowResolution)ShadowQuality;
            }
            QualitySettings.antiAliasing = AntiAliasing;
            QualitySettings.vSyncCount = VSync;
            QualitySettings.anisotropicFiltering = (AnisotropicFiltering)AnisotropicFiltering;
        }
    }
}