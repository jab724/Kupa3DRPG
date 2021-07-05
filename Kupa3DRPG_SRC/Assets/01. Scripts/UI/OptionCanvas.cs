using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//설정창 캔버스
namespace Kupa
{
    public class OptionCanvas : MonoBehaviour
    {
        [SerializeField] private Button graphicBtn;
        [SerializeField] private Button soundBtn;
        [SerializeField] private Button gamePlayBtn;
        [SerializeField] private Button hotKeyBtn;

        [SerializeField] private GameObject graphicObject;
        [SerializeField] private GameObject soundObject;
        [SerializeField] private GameObject gamePlayObject;
        [SerializeField] private GameObject hotKeyObject;

        [SerializeField] private Button applyBtn;
        [SerializeField] private Button closeBtn;

        private void Awake()
        {
            graphicBtn.onClick.AddListener(OnClickTabGraphic);
            soundBtn.onClick.AddListener(OnClickTabSound);
            gamePlayBtn.onClick.AddListener(OnClickTabGamePlay);
            hotKeyBtn.onClick.AddListener(OnClickTabHotKey);
        }

        private void OnEnable()
        {
            OnClickTabGraphic();
        }

        public void SetApplyOnClickListener(bool isActive, UnityAction listener = null)
        {
            applyBtn.gameObject.SetActive(isActive);
            if (isActive)
            {
                applyBtn.onClick.RemoveAllListeners();
                applyBtn.onClick.AddListener(listener);
            }
        }
        public void SetCloseOnClickListener(bool isActive, UnityAction listener = null)
        {
            closeBtn.gameObject.SetActive(isActive);
            if (isActive)
            {
                closeBtn.onClick.RemoveAllListeners();
                closeBtn.onClick.AddListener(listener);
            }
        }

        public void OnClickTabGraphic()
        {
            ActiveTab(0);
        }
        public void OnClickTabSound()
        {
            ActiveTab(1);
        }
        public void OnClickTabGamePlay()
        {
            ActiveTab(2);
        }
        public void OnClickTabHotKey()
        {
            ActiveTab(3);
        }
        private void ActiveTab(int seq)
        {
            if (seq < 0 || 3 < seq) seq = 0;
            graphicObject.SetActive(seq == 0);
            soundObject.SetActive(seq == 1);
            gamePlayObject.SetActive(seq == 2);
            hotKeyObject.SetActive(seq == 3);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 200, 30), "디버깅용 저장값 초기화 버튼"))
                PlayerPrefs.DeleteAll();
        }
    }
}