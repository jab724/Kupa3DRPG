using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

//Web GL 환경 특성상 마우스가 보이며 게임 진행에 있어 거슬리기에 마우스 포인터가 보이지 않도록 하고 
//포커스를 잃을 상황이 빈번하여 이에 관한 불편함 최소화
public class WebGLPause : MonoBehaviour, IPointerDownHandler
{
    private Canvas canvas;
    public TMP_Text text;
    private bool isFocus;
    private bool isPause;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SetPause(true);

        text.text = "focus = " + isFocus + ", pause = " + isPause;
    }

    private void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
        SetPause(!focus);
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        isPause = pauseStatus;
    }

    private void SetPause(bool isPause)
    {
        if (isPause)
        {
            Time.timeScale = 0.0f;
            canvas.enabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1.0f;
            canvas.enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        SetPause(false);
    }
}
