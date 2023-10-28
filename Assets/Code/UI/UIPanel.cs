using System;
using DG.Tweening;
using UnityEngine;

namespace LK.PaintingGame.Code.UI
{
    public class UIPanel : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private Tween _tween;
        
        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _canvasGroup.alpha = 0f;
            Show();
        }

        public void Show()
        {
            StopAnimation();
            gameObject.SetActive(true);
            _tween = _canvasGroup.DOFade(1f, 0.5f);
        }

        public void Hide()
        {
            StopAnimation();
            _tween = _canvasGroup.DOFade(0f, 0.5f);
            _tween.onComplete += () => gameObject.SetActive(false);
        }

        private void StopAnimation()
        {
            if (_tween != null)
            {
                _tween.Kill();
                _tween = null;
            }
        }
    }
}