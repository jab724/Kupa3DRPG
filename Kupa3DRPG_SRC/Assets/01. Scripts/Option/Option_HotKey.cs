using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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