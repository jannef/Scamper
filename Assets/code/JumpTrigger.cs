using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Gives entering player a jump impulse.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class JumpTrigger : MonoBehaviour
    {

        public AudioClip fanSound;

        private AudioSource fanSource;

        private const float fanVolume = 0.8f;

        /// <summary>
        /// How big of a force.
        /// </summary>
        public Vector2 JumpPower = new Vector2(0, 5.1f);

        /// <summary>
        /// How much should distance affect the rat.
        /// </summary>
        [SerializeField] private float DistanceModifier = 3f;

        /// <summary>
        /// Player rigid body reference.
        /// </summary>
        private Rigidbody2D _playerrbRigidbody;

        /// <summary>
        /// Player animator reference.
        /// </summary>
        private AnimationController _playerAnimation;

        /// <summary>
        /// Hovers the player and changes it's animation.
        /// </summary>
        /// <param name="other">the triggering collider</param>
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject == SceneManager.Instance.PlayerGameObject)
            {
                _playerAnimation.Falling = true;
                
                var dist = Mathf.Pow(1 / Vector2.Distance(transform.position, other.gameObject.transform.position), DistanceModifier);
                _playerrbRigidbody.AddForce(JumpPower * dist, ForceMode2D.Force);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == SceneManager.Instance.PlayerGameObject)
            {
                fanSource.PlayOneShot(fanSound, fanVolume);
            }
        }

        public static IEnumerator FadeOut (AudioSource source, float fadeTime)
        {
            float startVolume = source.volume;
            while (source.volume > 0)
            {
                source.volume -= startVolume * Time.deltaTime / fadeTime;

                yield return null;
            }

            source.Stop();
            source.volume = startVolume;
        }

        /// <summary>
        /// Change player out of the falling state.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject == SceneManager.Instance.PlayerGameObject)
            {
                StartCoroutine(FadeOut(fanSource, 1.0f));
                _playerAnimation.Falling = false;
            }
        }

        /// <summary>
        /// Get reference to audio source, player animator and rigid body.
        /// </summary>
        private void Awake()
        {
            fanSource = GetComponent<AudioSource>();
            _playerrbRigidbody = SceneManager.Instance.PlayerGameObject.GetComponent<Rigidbody2D>();
            _playerAnimation = SceneManager.Instance.PlayerGameObject.GetComponent<AnimationController>();
            SceneManager.Instance.LevelResetEvent += ResetTrigger;
        }

        /// <summary>
        /// Reset this trigger.
        /// </summary>
        private void ResetTrigger()
        {
            
        }
    }
}
