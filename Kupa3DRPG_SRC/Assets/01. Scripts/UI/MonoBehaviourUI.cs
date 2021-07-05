using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Escape 버튼으로 닫는게 가능한 UI 용도일 경우 이 MonoBehaviour를 쓰도록 한다.
namespace Kupa
{
    public class MonoBehaviourUI : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            UIManager.Self.AddEscapeListener(Close);
        }

        protected virtual void OnDisable()
        {
            UIManager.Self.RemoveEscapeListener(Close);
        }

        protected virtual void Close()
        {

        }
    }
}