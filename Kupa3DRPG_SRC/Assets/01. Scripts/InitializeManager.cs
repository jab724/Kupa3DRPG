using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Preference에 저장된 값 중 게임이 시작됨과 동시에 반영되어야 할 부분들을 적용. (사운드와 같이 개별적으로 초기화 하는 부분은 제외)
namespace Kupa
{
    public class InitializeManager : MonoBehaviour
    {
        private void Awake()
        {
            PreferenceData.ApplyGraphicOptionSetting();     //그래픽 옵션 반영
        }
    }
}