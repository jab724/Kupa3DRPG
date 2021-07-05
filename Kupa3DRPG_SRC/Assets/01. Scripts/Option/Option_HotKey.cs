using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//옵션 중 단축키 관련 옵션 관리
namespace Kupa
{
    public class Option_HotKey : MonoBehaviourUI
    {
        protected override void Close()
        {
            UIManager.Self.OpenCanvasOption(false);
        }
    }
}