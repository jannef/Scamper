using System;
using UnityEngine;
using System.Collections;
using fi.tamk.game.theone.phys;
using fi.tamk.game.theone.utils;
using UnityEngine.Events;

namespace fi.tamk.game.theone.ui
{
    public class ToggleButton : MonoBehaviour
    {
        /// <summary>
        /// RectTransform of the animated slider on the button.
        /// </summary>
        [SerializeField] private RectTransform Slider;

        /// <summary>
        /// How long does a state change take.
        /// </summary>
        [SerializeField, Range(0.1f, 10f)] private float TransitionTime = 0.3f;

        /// <summary>
        /// Learn to serialize delegates pls.
        /// </summary>
        [SerializeField] private UnityEvent OnActivate;

        [SerializeField] private UnityEvent OnDeactivate;

        /// <summary>
        /// State this toggle is currently in.
        /// </summary>
        [HideInInspector]
        public bool State
        {
            get { return _state; }
        }

        /// <summary>
        /// Is this button currently transitioning.
        /// </summary>
        private bool _inTransition = false;

        /// <summary>
        /// Backing field for state of this toggle.
        /// </summary>
        private bool _state = true;

        /// <summary>
        /// Off/false position of the slider animation thing.
        /// </summary>
        private Vector2 _offPosition;

        /// <summary>
        /// On/true position of the slider animation thing.
        /// </summary>
        private Vector2 _onPosition;

        public AudioClip switchSound;

        private AudioSource switchSource;

        private const float switchVolume = 0.8f;

        /// <summary>
        /// Disable this this scripts gameObject if slider is not properly set in Unity.
        /// Find positions for the animation.
        /// </summary>
        private void Awake()
        {
            if (Slider == null)
            {
                gameObject.SetActive(false);
                return;
            }

            switchSource = GetComponent<AudioSource>();

            _onPosition = Slider.anchoredPosition;
            _offPosition = -_onPosition;
        }

        /// <summary>
        /// Make transition to non-active sate.
        /// </summary>
        /// <returns>Nothing.</returns>
        private IEnumerator TransitionState()
        {
            var timer = 0f;

            var begin = _state ? _onPosition : _offPosition;
            var end = !_state ? _onPosition :_offPosition; 

            while (timer < TransitionTime)
            {
                timer += SceneManager.Instance.MenuDeltaTime;
                Slider.anchoredPosition = Vector2.Lerp(begin, end, Interpolations.Smootherstep(timer /TransitionTime));
                yield return new WaitForEndOfFrame();
            }

            _state = !_state;
            _inTransition = false;

            if (_state)
            {
                OnActivate.Invoke();
                AudioListener.pause = false;
                switchSource.PlayOneShot(switchSound, switchVolume);
            }
            else
            {
                OnDeactivate.Invoke();
                AudioListener.pause = true;
            }
        }

        /// <summary>
        /// Public method for UI events to call.
        /// </summary>
        public void ToggleState()
        {
            if (_inTransition) return;

            _inTransition = true;
            StartCoroutine(TransitionState());
        }
    }
}
