using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//디버깅용. TriggerEnter 시 이벤트 작동
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