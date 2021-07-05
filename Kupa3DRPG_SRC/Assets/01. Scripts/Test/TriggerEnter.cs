using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kupa
{
    public class TriggerEnter : MonoBehaviour
    {
        public UnityEvent action;

        private void OnTriggerEnter(Collider other)
        {
            action?.Invoke();
        }
    }
}