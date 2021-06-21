using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kupa
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Self
        {
            get
            {
                return instance;
            }
        }

        public GameObject pauseCanvas;
        public GameObject optionCanvas;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) OpenCanvasPause();
        }

        public void OpenCanvasPause(bool isOpen = true)
        {
            pauseCanvas.SetActive(isOpen);
        }
        public void OpenCanvasOption(bool isOpen = true)
        {
            optionCanvas.SetActive(isOpen);
        }
    }
}