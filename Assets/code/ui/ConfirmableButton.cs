using UnityEngine;
using System.Collections;
using fi.tamk.game.theone.phys;
using fi.tamk.game.theone.utils;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace fi.tamk.game.theone.ui
{
    public class ConfirmableButton : MonoBehaviour
    {
        [SerializeField] private RectTransform ConfirmationButton;
        [SerializeField, Range(0.1f, 10f)] private float AnimationDuration = 0.66f;
        [SerializeField] private Vector2 OffsetPosition;

        /// <summary>
        /// I am fucking digusted @Unity. Why can't it just serialize delegates? Lazy fucks.
        /// </summary>
        [SerializeField] private UnityEvent OnConfirmation;

        private bool _confirmUp = false;
        private Vector2 _originalPosition;

        private void Awake()
        {
            if (ConfirmationButton == null)
            {
                gameObject.SetActive(false);
                return;
            }

            _originalPosition = ConfirmationButton.anchoredPosition;
        }

        public void PointerDown()
        {
            StartCoroutine(Animate());
        }

        public void PointerUp(BaseEventData eventData)
        {
            StopAllCoroutines();
            _confirmUp = true;

            var pointer = eventData as PointerEventData;
            if (pointer.pointerEnter.GetInstanceID() == ConfirmationButton.gameObject.GetInstanceID())
            {
                OnConfirmation.Invoke();
            }

            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            var timer = 0f;

            var end = _confirmUp? _originalPosition : _originalPosition + OffsetPosition;

            while (timer < AnimationDuration)
            {
                timer += SceneManager.Instance.MenuDeltaTime;
                var ratio = Mathf.Min(1f, timer/AnimationDuration);

                ConfirmationButton.anchoredPosition = Vector2.Lerp(ConfirmationButton.anchoredPosition, end, Interpolations.Smootherstep(ratio));
                ConfirmationButton.localScale = !_confirmUp ? Vector3.one*ratio : Vector3.one - Vector3.one*ratio;

                yield return new WaitForEndOfFrame();
            }

            _confirmUp = !_confirmUp;
        }
    }
}
