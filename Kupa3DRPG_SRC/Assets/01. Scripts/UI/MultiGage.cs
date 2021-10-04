using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kupa
{
    public class MultiGage : MonoBehaviour
    {
        [Tooltip("게이지 타입")]
        public Image.FillMethod fillMethod = Image.FillMethod.Horizontal;
        [Tooltip("순서대로 그려질 색")]
        public Color[] multiGageColor = { Color.red, Color.yellow };
        [Tooltip("한 줄의 게이지 값")]
        public float gageLineValue = 10;
        [Tooltip("피격 후 딤 게이지가 줄어들기 시작하는 시간")]
        public float dimWaitTime = 0.1f;
        [Tooltip("딤 게이지가 줄어들기 시작 후 다 줄어들때까지 걸리는 시간")]
        public float dimDeleteTime = 0.3f;

        public float targetGageValue = 0f;

        public bool dimEffectOn = true;

        [ReadOnly] public int colorIndex = 0;

        private GameObject uiCanvasObject;
        private RectTransform gage1RectTransform = null;
        private RectTransform gage2RectTransform = null;
        private RectTransform gageDim1RectTransform = null;
        private RectTransform gageDim2RectTransform = null;
        private Canvas gage1Canvas;
        private Canvas gage2Canvas;
        private Canvas gageDim1Canvas;
        private Canvas gageDim2Canvas;
        private Image gage1Image;
        private Image gage2Image;
        private Image gageDim1Image;
        private Image gageDim2Image;
        private IEnumerator gageEffectCor;
        private IEnumerator gageDimEffectCor;
        private float prevGageValue;
        private float dimGageValue;

        private Color nonValueColor = Color.black;

        private void Awake()
        {
            InitProperty();
            ObserveStart();
        }

        public void ObserveStart()
        {
            InitProperty();
            CalcGage();
            uiCanvasObject.SetActive(true);
            dimGageValue = prevGageValue = targetGageValue;

            if (gageEffectCor != null)
                StopCoroutine(gageEffectCor);
            gageEffectCor = ObserveCor();
            StartCoroutine(gageEffectCor);
        }

        public void ObserveEnd()
        {
            if (gageDimEffectCor != null)
                StopCoroutine(gageDimEffectCor);
            if (gageEffectCor != null)
                StopCoroutine(gageEffectCor);
            uiCanvasObject.SetActive(false);
        }

        IEnumerator ObserveCor()
        {
            while (true)
            {
                if (prevGageValue != targetGageValue)
                {
                    CalcGage();
                    if (dimEffectOn)
                    {
                        if (prevGageValue < targetGageValue)
                        {
                            dimGageValue = targetGageValue;
                            gageDim1Canvas.sortingOrder = 10001 + colorIndex * 2;
                            gageDim2Canvas.sortingOrder = 10001 + colorIndex * 2 - 2;
                            gageDim1Image.color = 0 <= colorIndex ? multiGageColor[colorIndex % multiGageColor.Length] * 0.5f : nonValueColor;
                            gageDim2Image.color = 1 <= colorIndex ? multiGageColor[(colorIndex - 1) % multiGageColor.Length] * 0.5f : nonValueColor;
                            gageDim1Image.fillAmount = targetGageValue % gageLineValue / gageLineValue;
                        }
                        if (gageDimEffectCor != null)
                            StopCoroutine(gageDimEffectCor);
                        gageDimEffectCor = GageEffectCor();
                        StartCoroutine(gageDimEffectCor);
                    }
                }
                prevGageValue = targetGageValue;
                yield return null;
            }
        }

        IEnumerator GageEffectCor()
        {
            // float tempPrevGageValue = prevGageValue;
            yield return new WaitForSeconds(dimWaitTime);

            float timer = dimDeleteTime;
            int dimColorIndex;
            while (0 < timer)
            {
                dimGageValue = Mathf.Lerp(targetGageValue, dimGageValue, timer / dimDeleteTime);
                dimColorIndex = Mathf.FloorToInt(dimGageValue / gageLineValue);

                gageDim1Canvas.sortingOrder = 10001 + dimColorIndex * 2;
                gageDim2Canvas.sortingOrder = 10001 + dimColorIndex * 2 - 2;
                gageDim1Image.color = 0 <= dimColorIndex ? multiGageColor[dimColorIndex % multiGageColor.Length] * 0.5f : nonValueColor;
                gageDim2Image.color = 1 <= dimColorIndex ? multiGageColor[(dimColorIndex - 1) % multiGageColor.Length] * 0.5f : nonValueColor;
                gageDim1Image.fillAmount = dimGageValue % gageLineValue / gageLineValue;

                yield return null;
                timer -= Time.deltaTime;
            }

            gageDim1Canvas.sortingOrder = 10001 + colorIndex * 2;
            gageDim2Canvas.sortingOrder = 10001 + colorIndex * 2 - 2;
            gageDim1Image.color = 0 <= colorIndex ? multiGageColor[colorIndex % multiGageColor.Length] * 0.5f : nonValueColor;
            gageDim2Image.color = 1 <= colorIndex ? multiGageColor[(colorIndex - 1) % multiGageColor.Length] * 0.5f : nonValueColor;
            gageDim1Image.fillAmount = targetGageValue % gageLineValue / gageLineValue;
        }

        private void OnValidate()
        {
            if (gage1RectTransform == null || gage2RectTransform == null || gageDim1RectTransform == null || gageDim2RectTransform == null)
            {
                InitProperty();
            }

            if (gage1Canvas == null || gage2Canvas == null || gageDim1Canvas == null || gageDim2Canvas == null || gage1Image == null || gage2Image == null || gageDim1Image == null || gageDim2Image == null)
            {
                Debug.LogError("Gage1, Gage2, GageDim1, GageDim2 오브젝트의 컴포넌트 중 Canvas와 Image가 올바른지 확인하세요.");
            }
            else
            {
                CalcGage();
            }
        }

        private void InitProperty()
        {
            uiCanvasObject = transform.GetChild(0).gameObject;
            Transform gageBackground = uiCanvasObject.transform.GetChild(0);
            gage1RectTransform = gageBackground.Find("Gage1").GetComponent<RectTransform>();
            gage2RectTransform = gageBackground.Find("Gage2").GetComponent<RectTransform>();
            gageDim1RectTransform = gageBackground.Find("GageDim1").GetComponent<RectTransform>();
            gageDim2RectTransform = gageBackground.Find("GageDim2").GetComponent<RectTransform>();
            if (gage1RectTransform == null || gage2RectTransform == null || gageDim1RectTransform == null || gageDim2RectTransform == null)
            {
                Debug.LogError("Gage1, Gage2, GageDim1, GageDim2 오브젝트가 올바른지 확인하세요.");
            }
            else
            {
                gage1Canvas = gage1RectTransform.GetComponent<Canvas>();
                gage2Canvas = gage2RectTransform.GetComponent<Canvas>();
                gageDim1Canvas = gageDim1RectTransform.GetComponent<Canvas>();
                gageDim2Canvas = gageDim2RectTransform.GetComponent<Canvas>();
                gage1Image = gage1RectTransform.GetComponent<Image>();
                gage2Image = gage2RectTransform.GetComponent<Image>();
                gageDim1Image = gageDim1RectTransform.GetComponent<Image>();
                gageDim2Image = gageDim2RectTransform.GetComponent<Image>();
            }

            gage1Canvas.overrideSorting = gage2Canvas.overrideSorting = gageDim1Canvas.overrideSorting = gageDim2Canvas.overrideSorting = true;
            gage1Image.fillMethod = gage2Image.fillMethod = gageDim1Image.fillMethod = gageDim2Image.fillMethod = fillMethod;

            gageDim1Canvas.sortingOrder = gageDim2Canvas.sortingOrder = 10000;
            gageDim1Image.color = gageDim2Image.color = nonValueColor;
            gageDim1Image.fillAmount = gageDim2Image.fillAmount = 0;
            gage2Image.fillAmount = gageDim2Image.fillAmount = 1;
        }

        private void CalcGage()
        {
            colorIndex = Mathf.FloorToInt(targetGageValue / gageLineValue);

            gage1Canvas.sortingOrder = 10002 + colorIndex * 2;
            gage2Canvas.sortingOrder = 10002 + colorIndex * 2 - 2;
            gage1Image.color = 0 <= colorIndex ? multiGageColor[colorIndex % multiGageColor.Length] : nonValueColor;
            gage2Image.color = 1 <= colorIndex ? multiGageColor[(colorIndex - 1) % multiGageColor.Length] : nonValueColor;
            gage1Image.fillAmount = targetGageValue % gageLineValue / gageLineValue;
        }

        private void CalcGageDim()
        {
            gageDim1Canvas.sortingOrder = 10001 + colorIndex * 2;
            gageDim2Canvas.sortingOrder = 10001 + colorIndex * 2 - 2;
            gageDim1Image.color = 0 <= colorIndex ? multiGageColor[colorIndex % multiGageColor.Length] * 0.5f : nonValueColor;
            gageDim2Image.color = 1 <= colorIndex ? multiGageColor[(colorIndex - 1) % multiGageColor.Length] * 0.5f : nonValueColor;
            gageDim1Image.fillAmount = targetGageValue % gageLineValue / gageLineValue;
        }
    }
}