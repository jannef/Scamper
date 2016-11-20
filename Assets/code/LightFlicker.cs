using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] private Sprite AlternateSprite;
        [SerializeField, Range(0.1f, 10f)] private float ActivationTime = 0.5f;

        private SpriteRenderer _renderer;
        private Sprite _originalSprite;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _originalSprite = _renderer.sprite;

            SceneManager.Instance.LevelResetEvent += ResetLight;

            OnAwake();
        }

        protected virtual void OnAwake()
        {
            
        }

        protected virtual void OnReset()
        {
            
        }

        protected void ActivateLight()
        {
            StartCoroutine(FlickerLight(ActivationTime));
        }

        private IEnumerator FlickerLight(float forHowLong)
        {
            ChangeToSprite(AlternateSprite);
            yield return new WaitForSeconds(forHowLong);
            ChangeToSprite(_originalSprite);
        }

        private void ChangeToSprite(Sprite toWhich)
        {
            if (_renderer != null && toWhich != null) _renderer.sprite = toWhich;
        }

        private void ResetLight()
        {
            StopAllCoroutines();
            ChangeToSprite(_originalSprite);

            OnReset();
        }
    }
}