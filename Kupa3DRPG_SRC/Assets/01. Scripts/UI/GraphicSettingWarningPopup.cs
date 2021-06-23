using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kupa
{
    public class GraphicSettingWarningPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button okBtn;
        [SerializeField] private Button cancelBtn;
        [SerializeField] int waitTimeSec = 15;

        private void Awake()
        {
            StartCoroutine("TimerCor");
        }

        public void SetGraphicSettingWarningPopup(UnityAction okListener, UnityAction cancelListener)
        {
            okBtn.onClick.AddListener(okListener);
            cancelBtn.onClick.AddListener(cancelListener);
        }

        private IEnumerator TimerCor()
        {
            int curTime = waitTimeSec;
            var timer = new WaitForSeconds(1f);
            while (waitTimeSec <= 0)
            {
                descriptionText.text = string.Format("그래픽 설정이 변경되었습니다.\n화면이 정상적으로 표시된다면 확인을 눌러주세요.\n{0}초 후 이전 설정으로 돌아갑니다.", curTime);
                yield return timer;
            }

            cancelBtn.onClick.Invoke();
        }

        public void DestroyPopup()
        {
            Destroy(gameObject);
        }
    }
}