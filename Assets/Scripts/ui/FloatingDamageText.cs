using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace EventBusEx.ui
{
    [RequireComponent(typeof(Text))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class FloatingDamageText : MonoBehaviour
    {
        [SerializeField] private float _speed = 50f;
        [SerializeField] private float _lifetime = 1.5f;
        [SerializeField] private float _fadeStartTime = 0.5f;

        private Text _text;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _text = GetComponent<Text>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(int damageAmount, Vector3 worldPosition)
        {
            _text.text = $"-{damageAmount}";
            
            var pos = Camera.main.WorldToScreenPoint(worldPosition);
            pos.x += Random.Range(-30f, 30f);
            pos.y += Random.Range(-10f, 10f);
            _rectTransform.position = pos;

            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            var elapsedTime = 0f;

            while (elapsedTime < _lifetime)
            {
                _rectTransform.position += Time.deltaTime * _speed * Vector3.up;

                if (elapsedTime >= _fadeStartTime)
                {
                    var fadeProgress = (elapsedTime / _fadeStartTime) / (_lifetime - _fadeStartTime);
                    _canvasGroup.alpha = 1f - fadeProgress;
                }
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            Destroy(gameObject);
        }
    }
}
