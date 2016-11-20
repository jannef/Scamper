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
        /// <summary>
        /// How big an impulse.
        /// </summary>
        public float JumpPower = 5f;

        /// <summary>
        /// Player rigid body reference.
        /// </summary>
        private Rigidbody2D _playerrbRigidbody;

        /// <summary>
        /// Player animator reference.
        /// </summary>
        private AnimationController _playerAnimation;

        /// <summary>
        /// Has this trigger already fired.
        /// </summary>
        private bool _hasFired = false;

        /// <summary>
        /// Gives the impulse.
        /// </summary>
        /// <param name="other">Not used.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_hasFired && other.gameObject == SceneManager.Instance.PlayerGameObject)
            {
                _playerrbRigidbody.AddForce(new Vector2(0, JumpPower), ForceMode2D.Impulse);
                _hasFired = true;
            }
        }

        /// <summary>
        /// Tell the player animator to be on falling state while in the trigger.
        /// </summary>
        /// <param name="other">the triggering collider</param>
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject == SceneManager.Instance.PlayerGameObject)
            {
                _playerAnimation.Falling = true;
            }
        }

        /// <summary>
        /// Change player out of the falling state.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject == SceneManager.Instance.PlayerGameObject)
            {
                _playerAnimation.Falling = false;
            }
        }

        /// <summary>
        /// Get reference to player animator and rigid body.
        /// </summary>
        private void Awake()
        {
            _playerrbRigidbody = SceneManager.Instance.PlayerGameObject.GetComponent<Rigidbody2D>();
            _playerAnimation = SceneManager.Instance.PlayerGameObject.GetComponent<AnimationController>();
            SceneManager.Instance.LevelResetEvent += ResetTrigger;
        }

        /// <summary>
        /// Reset this trigger.
        /// </summary>
        private void ResetTrigger()
        {
            _hasFired = false;
        }
    }
}
