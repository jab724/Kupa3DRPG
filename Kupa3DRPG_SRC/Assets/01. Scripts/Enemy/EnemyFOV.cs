using System.ComponentModel;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kupa
{
    public class EnemyFOV : MonoBehaviour
    {
        [SerializeField] [Range(0f, 200f)] private float detectDistance = 10f;
        [SerializeField] [Range(0f, 360f)] private float fovAngle = 60f;
        [SerializeField] private List<Transform> eyeList = new List<Transform>();
        [SerializeField] [ReadOnly] private float detectValue = 0f;
        [Header("Property")]
        [SerializeField] private Image detectGage;
        [SerializeField] private float playerDetectLostWaitTime = 1;

        private bool isPlayerDetected = false;
        private float playerDistance = 0f;

        private LayerMask blockingVisibilityLayerMask;    //시야에 막히는 레이어 (같은 적이나 유리 같이 시야에 막혔다고 판정되지 않는 물체는 제외한 레이어)
        private IEnumerator DetectingCor = null;

        private void Awake()
        {
            blockingVisibilityLayerMask = 1 << LayerMask.NameToLayer("Enemy");
            blockingVisibilityLayerMask = ~blockingVisibilityLayerMask;
            detectGage.transform.parent.gameObject.SetActive(false);
        }

        private void Update()
        {
            isPlayerDetected = false;
            if (Player.PublicData.PlayerState != Player.PlayerState.DEAD)
            {
                for (int i = 0; i < eyeList.Count; ++i)
                {
                    playerDistance = Vector3.Distance(eyeList[i].position, Player.PublicData.Transform.position);
                    if (playerDistance <= detectDistance &&
                    Vector3.Angle(eyeList[i].forward, (Player.PublicData.Transform.position - (eyeList[i].position)).normalized) < fovAngle * 0.5f)
                    {
                        isPlayerDetected = true;
                        if (DetectingCor == null)
                        {
                            DetectingCor = Detecting();
                            StartCoroutine(DetectingCor);
                        }
                        break;
                    }
                }
            }
        }

        private IEnumerator Detecting()
        {
            bool prevIsPlayerDetected = false;
            float detectLostWaitTime = 0f;
            detectGage.transform.parent.gameObject.SetActive(true);

            while (true)
            {
                if (prevIsPlayerDetected == true && isPlayerDetected == false)
                {
                    detectLostWaitTime = playerDetectLostWaitTime;
                }
                else if (prevIsPlayerDetected == false && isPlayerDetected == true)
                {
                    detectLostWaitTime = 0f;
                }
                if (0 < detectLostWaitTime)
                {
                    detectLostWaitTime -= Time.deltaTime;
                    yield return null;
                }
                else
                {
                    if (isPlayerDetected)
                    {
                        detectValue += Time.deltaTime * detectDistance / playerDistance;
                    }
                    else
                    {
                        detectValue -= Time.deltaTime;
                        if (detectValue <= 0)
                        {
                            detectGage.transform.parent.gameObject.SetActive(false);
                            detectValue = 0;
                            detectGage.fillAmount = detectValue;
                            DetectingCor = null;
                            yield break;
                        }
                    }
                }

                detectGage.fillAmount = detectValue;
                prevIsPlayerDetected = isPlayerDetected;
                yield return null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * detectDistance);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(new Vector3(detectDistance * Mathf.Sin(fovAngle * Mathf.Deg2Rad * 0.5f), transform.position.y, detectDistance * Mathf.Cos(fovAngle * Mathf.Deg2Rad * 0.5f))));
            Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(new Vector3(-(detectDistance * Mathf.Sin(fovAngle * Mathf.Deg2Rad * 0.5f)), transform.position.y, detectDistance * Mathf.Cos(fovAngle * Mathf.Deg2Rad * 0.5f))));
        }
    }
}