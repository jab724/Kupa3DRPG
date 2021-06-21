using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kupa
{
    public class InitializeManager : MonoBehaviour
    {
        private void Awake()
        {
            PreferenceData.ApplyGraphicOptionSetting();
        }
    }
}