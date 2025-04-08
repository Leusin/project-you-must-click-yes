using System.Collections;
using UnityEngine;

namespace ProjectYouMustClickYes
{
    public class ScreensaverMove : MonoBehaviour
    {
        [Header("Move Like Screensaver")]
        private RectTransform _rect;
        public RectTransform popupRect;
        public RectTransform canvasRect; // UI 전체 크기
        public Vector2 speed = new Vector2(100f, 80f); // 이동 속도

        public Vector2 direction = new Vector2(0.4444f, 0.666f);

        private Coroutine _moveBackCoroutine;
        
        void Awake()
        {
            _rect = gameObject.GetComponent<RectTransform>();
        }

        void Onable()
        {
            if (_moveBackCoroutine != null)
            {
                StopCoroutine(_moveBackCoroutine);
                _moveBackCoroutine = null;
            }            
        }

        void OnDisable()
        {
            _moveBackCoroutine = StartCoroutine(MoveBackToOriginAndDisable());
        }

        void Update()
        {
            MoveWindow();
            CheckBounds();
        }

        void MoveWindow()
        {
            // 현재 위치 업데이트
            _rect.anchoredPosition += direction * speed * Time.deltaTime;

        }

        void CheckBounds()
        {
            Vector2 minBounds = canvasRect.rect.min + (popupRect.rect.size * 0.5f);
            Vector2 maxBounds = canvasRect.rect.max - (popupRect.rect.size * 0.5f);

            Vector2 currentPos = _rect.anchoredPosition;

            // 좌우 벽 충돌 검사
            if (currentPos.x <= minBounds.x || currentPos.x >= maxBounds.x)
            {
                direction.x *= -1; // X 방향 반전
                _rect.anchoredPosition = new Vector2(
                    Mathf.Clamp(currentPos.x, minBounds.x, maxBounds.x), currentPos.y);
            }

            // 상하 벽 충돌 검사
            if (currentPos.y <= minBounds.y || currentPos.y >= maxBounds.y)
            {
                direction.y *= -1; // Y 방향 반전
                _rect.anchoredPosition = new Vector2(currentPos.x,
                    Mathf.Clamp(currentPos.y, minBounds.y, maxBounds.y));
            }
        }

        IEnumerator MoveBackToOriginAndDisable()
        {
            Vector2 start = _rect.anchoredPosition;
            Vector2 target = Vector2.zero;
            float time = 0f;
            float duration = 1.25f;

            while (time < duration)
            {
                _rect.anchoredPosition = Vector2.Lerp(start, target, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            _rect.anchoredPosition = target;
        }
    }
}