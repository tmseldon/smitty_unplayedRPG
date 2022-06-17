using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        //[SerializeField] float fadeTime = 3f;
        
        CanvasGroup canvasGroup;
        TextMeshProUGUI textPortal;
        Coroutine _currentCoroutine = null;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            textPortal = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void InmidiatlyFadeOut()
        {
            textPortal.enabled = false;
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeInFX(float totalTime)
        {
            if(_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }
            
            float alphaCreciente = 0;
            textPortal.enabled = true;

            _currentCoroutine = StartCoroutine(FadeSinFX(totalTime, alphaCreciente, Mathf.PI/2));
            return _currentCoroutine;
        }


        public Coroutine FadeOutFX(float totalTime)
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }

            float alphaCreciente = Mathf.PI/2;
            textPortal.enabled = false;

            _currentCoroutine = StartCoroutine(FadeSinFX(totalTime, alphaCreciente, Mathf.PI));
            return _currentCoroutine;
        }

        /// <summary>
        /// Produce Fade Effect based on a sin curve, since it goes from 0 to 1 between 0 and 90° 
        /// and the other way around from 90° to 180°
        /// </summary>
        /// <param name="totalTime"> 
        /// The initialiser delegate to call when first used. 
        /// </param>
        /// <param name="alphaCreciente"> 
        /// Initial point of the sin curve
        /// </param>
        /// <param name="limit"> 
        /// last point of the sin curve to reach 
        /// </param>
        private IEnumerator FadeSinFX(float totalTime, float argument, float limit)
        {
            while (argument <= limit)
            {
                canvasGroup.alpha = Mathf.Sin(argument);
                yield return null;
                argument += (Mathf.PI * Time.deltaTime) / (2 * totalTime);
            }
        }

    }
}

