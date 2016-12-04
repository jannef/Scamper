using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Player game object.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class PPlayerBlock : PGameBlock
    {
        [SerializeField, Range(1f, 999f)] public float GlobalAudioCutoffDistance = 20f;
        [SerializeField, Range(0f, 50f)] public float LinearFadeDistanceAfterCutoff = 7.5f;

        public AudioClip runSound;

        public AudioClip deathSound;

        public AudioSource runSource;

        public AudioSource deathSource;

        public const float runVolume = 0.5f;

        public const float deathVolume = 0.6f;

        /// <summary>
        /// Speed of player's advance once clicked.
        /// </summary>
        [SerializeField] protected Vector2 PlayerSpeed = new Vector2(2.2f, 0);

        /// <summary>
        /// Reference to player animation controller.
        /// </summary>
        [SerializeField] protected AnimationController PlayerAnimation = null;

        /// <summary>
        /// Reference to transfrom that judges if the collision is below player.
        /// 
        /// Used for walkable obstacles so that their other sides are still dangerous.
        /// </summary>
        [SerializeField] protected Transform FloorCollisionTolerance = null;

        /// <summary>
        /// If the player is moving.
        /// </summary>
        private bool _moving = false;

        /// <summary>
        /// Is a checkpoint transition underway.
        /// </summary>
        private Checkpoint _activeCheckpoint = null;

        private Rigidbody2D _rigidbody = null;

        protected override void OnAwake()
        {
            AudioSource[] audios = GetComponents<AudioSource>();
            runSource = audios[0];
            deathSource = audios[1];
        }

        /// <summary>
        /// Kills the player on collision. Also hides defautl behaviour (touch list upkeep).
        /// </summary>
        /// <param name="col">The collision.</param>
        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.gameObject.CompareTag("Movable") ||
                (col.collider.gameObject.CompareTag("MovableWalkable") && !CollisionBelow(col)))
            {
                SceneManager.Instance.PlayDistanceBasedSound(deathSource.PlayOneShot, deathSound, deathVolume, transform.position);
                //deathSource.PlayOneShot(deathSound, deathVolume);
                SceneManager.Instance.PlayerDeathReset();
            }
        }

        /// <summary>
        /// Checks if the collision was from below. This is pretty much a guess, not accurate.
        /// 
        /// TODO: Could use normals to get more accurate guess i think.
        /// </summary>
        /// <param name="col">the collision</param>
        /// <returns>true for below collision</returns>
        private bool CollisionBelow(Collision2D col)
        {
            if (FloorCollisionTolerance == null) return false;

            var footHeight = FloorCollisionTolerance.position.y;
            var colHeight = col.contacts[0].point.y;

            return colHeight < footHeight;
        }

        /// <summary>
        /// Hides default behaviour, because player has no reason to manage touchlist.
        /// </summary>
        /// <param name="col">not used</param>
        void OnCollisionExit2D(Collision2D col)
        {

        }

        /// <summary>
        /// Moves the player in the world once clicked.
        /// </summary>
        void Update()
        {
            var speedToSet = Mathf.Abs(PlayerSpeed.x);

            if (_moving)
            {
                MyTransform.Translate(PlayerSpeed*SceneManager.Instance.DeltaTime, Space.World);
            }
            else
            {
                speedToSet = 0f;
            }

            PlayerAnimation.Speed = speedToSet;
        }

        /// <summary>
        /// Overrides default behaviour that is used for other blocks to simply enable movement.
        /// </summary>
        void OnMouseDown()
        {
            if (!LockedFromPlayer
                && !_moving && _activeCheckpoint == null
                && !SceneManager.Instance.Pause)
            {
                SceneManager.Instance.PlayDistanceBasedSound(runSource.PlayOneShot, runSound, runVolume, transform.position);
                //runSource.PlayOneShot(runSound, runVolume);
                _moving = true;
            }
        }

        /// <summary>
        /// Resets the state of this block.
        /// </summary>
        public override void ResetBlock()
        {
            base.ResetBlock();
            _moving = false;
        }

        /// <summary>
        /// Sets variables when a checkpoint is reached.
        /// </summary>
        /// <param name="c">which chechpoint</param>
        public void Checkpoint(Checkpoint c)
        {
            StartLocation = c.Spawn.position;
            _moving = false;
            _activeCheckpoint = c;
            PlayerAnimation.Braking = true;
        }

        /// <summary>
        /// Releases control of the character after checkpoitn transition is done.
        /// </summary>
        public void CheckpointRelease()
        {
            _activeCheckpoint = null;
            PlayerAnimation.Braking = false;
        }
    }
}