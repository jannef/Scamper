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
        public Vector2 JumpPower = new Vector2(0, 5.1f);

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
        /// Tell the player animator to be on falling state while in the trigger.
        /// </summary>
        /// <param name="other">the triggering collider</param>
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject == SceneManager.Instance.PlayerGameObject)
            {
                _playerAnimation.Falling = true;

                var dist = Mathf.Pow(1 / Vector2.Distance(transform.position, other.gameObject.transform.position), DistanceModifier);
                Debug.Log(dist);
                _playerrbRigidbody.AddForce(JumpPower * dist, ForceMode2D.Force);
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
            
        }
    }
}
