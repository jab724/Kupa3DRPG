using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Web GL 환경 특성상 마우스가 보이며 게임 진행에 있어 거슬리기에 마우스 포인터가 보이지 않도록 하고 
//포커스를 잃을 상황이 빈번하여 이에 관한 불편함 최소화
public class WebGLPause : MonoBehaviour, IPointerDownHandler
{
    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SetPause(true);
    }

    private void OnApplicationFocus(bool focus)
    {
        SetPause(!focus);
    }

    private void SetPause(bool isPause)
    {
        if (isPause)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0.0f;
            canvas.enabled = true;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1.0f;
            canvas.enabled = false;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        SetPause(false);
    }
}
