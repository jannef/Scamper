using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Controls the animator of the player character.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
    public class AnimationController : MonoBehaviour
    {
        /// <summary>
        /// Speed at which this character is moving. Set by the script that is movign this game object.
        /// 
        /// Cannot use Rigidbody2D.velocity.x as we are not moving the character using physics.
        /// </summary>
        [HideInInspector] public float Speed = 0;

        /// <summary>
        /// Multiplier applied to animation speed when the character is running.
        /// </summary>
        [SerializeField] private float SpeedMultiplier = 0.34f;

        /// <summary>
        /// Should the falling state be up for the animator.
        /// </summary>
        public bool Falling = false;

        /// <summary>
        /// Should the braking state be up fir the animator.
        /// </summary>
        public bool Braking = false;

        /// <summary>
        /// Animator in this game object.
        /// </summary>
        private Animator _animator;

        /// <summary>
        /// Rigidbody2D in this game object.
        /// </summary>
        private Rigidbody2D _rigidbody2D;

        /// <summary>
        /// Finds the references needed.
        /// </summary>
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Sets the correct speed variable and animation speed.
        /// </summary>
        void Update()
        {
            _animator.SetFloat("speed", Speed);
            _animator.SetBool("braking", Braking);
            _animator.SetBool("falling", Falling);

            if (Speed <= 0.05f)
            {
                _animator.speed = 1f;
            }
            else
            {
                _animator.speed = Speed * SpeedMultiplier;
            }

        }
    }
}
