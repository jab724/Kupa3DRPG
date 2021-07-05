using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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