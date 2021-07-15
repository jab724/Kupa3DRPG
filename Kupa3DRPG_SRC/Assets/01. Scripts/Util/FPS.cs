using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Kupa
{
    public class FPS : MonoBehaviour
    {
        private TMP_Text text;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            text.text = string.Format("FPS : {0:0}", 1f / Time.unscaledDeltaTime);
        }
    }
}
