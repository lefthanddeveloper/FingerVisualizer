using System;
using System.Collections;
using UnityEngine;

namespace FingerVisualizer
{
    public class CopiedMessage : MonoBehaviour
    {
        private float period = 0.5f;
        private Vector3 dir = new Vector3(0, 1, 0);
        private float movingSpeed = 20f;
        private Coroutine fadeAway;
        private CanvasGroup canvasGroup;

        public void Init()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            Hide();
        }


        public void Show(Vector2 position)
        {
            gameObject.SetActive(true);
            
            if(fadeAway != null)
            {
                StopCoroutine(fadeAway);
            }

            this.transform.localPosition = position;
            fadeAway = StartCoroutine(FadeAway());
        }

        private IEnumerator FadeAway()
        {
            canvasGroup.alpha = 1f;
            float passedTime =0f;
            float ratio = 0f;

            while(ratio < 1f)
            {
                passedTime += Time.deltaTime;
                ratio = passedTime / period;

                canvasGroup.alpha = 1f - ratio;
                transform.localPosition += dir * Time.deltaTime * movingSpeed;

                yield return null;
            }
            canvasGroup.alpha = 0f;
            fadeAway = null;

            Hide();
        }
        

        public void Hide()
        {
            if(fadeAway != null)
            {
                StopCoroutine(fadeAway);
            }
            this.gameObject.SetActive(false);
        }
    }
}
