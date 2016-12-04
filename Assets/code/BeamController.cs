using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Manages a beam emitter object.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class BeamController : RemoteBase
    {

        public AudioClip deathSound;

        private AudioSource deathSource;

        private const float deathVolume = 0.6f;

        /// <summary>
        /// If this beam should kill the player.
        /// </summary>
        [SerializeField] private bool KillsPlayer = false;

        /// <summary>
        /// Has the current interruption of the beam already been handled.
        /// This resets when the beam has one uninterrupted update.
        /// </summary>
        private bool InterruptionHandled = false;

        /// <summary>
        /// Origin of the beam.
        /// </summary>
        [SerializeField] private Transform Origin;

        /// <summary>
        /// Endpoint of the beam.
        /// </summary>
        [SerializeField] private Transform End;

        /// <summary>
        /// Adjustable scale for the beam sprite.
        /// </summary>
        [SerializeField] private float ScaleModifier = 0.7f;

        /// <summary>
        /// Cached transform.
        /// </summary>
        private Transform _transform;

        /// <summary>
        /// Cached renderer material to mask parts of the beam in shader.
        /// </summary>
        //private Material _material;

        private ParticleSystem _particleSystem;

        [SerializeField] private bool IsActive = true;

        /// <summary>
        /// Gets references to objects.
        /// </summary>
        private void Awake()
        {
            deathSource = GetComponent<AudioSource>();
            _transform = transform;
            _particleSystem = GetComponent<ParticleSystem>();

            SceneManager.Instance.LevelResetEvent += ResetBeam;
        }

        /// <summary>
        /// Handles reset events.
        /// </summary>
        private void ResetBeam()
        {
            if (InterruptionHandled) EndInterruption();
            InterruptionHandled = false;
        }

        /// <summary>
        /// Finds center of the origin and end.
        /// </summary>
        /// <returns>The found center.</returns>
        private Vector3 FindCenterOfSprite(Vector3 loc)
        {
            return (Origin.position + loc + Vector3.back) / 2;
        }

        /// <summary>
        /// Finds correct rotation for the beam sprite.
        /// </summary>
        /// <param name="cutOff">What is coonsidered the end point for this calculation.</param>
        /// <returns></returns>
        private float FindRotationOfSprite(Vector3 cutOff)
        {
            var vec = new Vector2(cutOff.x - Origin.position.x, cutOff.y - Origin.position.y);
            return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Finds the obstacles (at Player or CatchRC layer) in this beam's path.
        /// </summary>
        /// <returns></returns>
        private Vector3 FindObstacle(out GameObject other, out float frac)
        {
            RaycastHit2D rv = Physics2D.Linecast(Origin.position, End.position, LayerMask.GetMask(new [] {"Player", "CatchRC"}));
            if (rv)
            {
                other = rv.collider.gameObject;
                frac = rv.fraction;
                return rv.point;
            }
            other = null;
            frac = 1;
            return End.position;
        }

        /// <summary>
        /// Updates particle system of the beam according to collisions.
        /// </summary>
        private void Update()
        { 
            _transform.rotation = Quaternion.AngleAxis(FindRotationOfSprite(End.position), Vector3.forward);

            GameObject other = null;

            if (IsActive)
            {
                float frac = 1;
                var col = FindObstacle(out other, out frac);

                _transform.position = FindCenterOfSprite(col);

                var scale = new Vector3((Origin.position - End.position).magnitude, 0, 0);
                var shape = _particleSystem.shape;
                shape.box = scale*frac;
            }

            if (!InterruptionHandled && other != null || other == SceneManager.Instance.PlayerGameObject)
            {
                // Setting boolean before the function call is mandatory. Do not change.
                InterruptionHandled = true;
                BeginInterruption(other);
            }

            if (InterruptionHandled && other == null)
            {
                EndInterruption();   
            }
            
        }

        /// <summary>
        /// Handling of the interruption of the beam, depending on what interrupted it and what behaviour is set.
        /// </summary>
        /// <param name="col">the interrupting event</param>
        private void BeginInterruption(GameObject col)
        {
            if (KillsPlayer && col.CompareTag("Player"))
            {
                deathSource.PlayOneShot(deathSound, deathVolume);
                SceneManager.Instance.PlayerDeathReset();
                return;
            }

            ActivateBlocks();
        }

        /// <summary>
        /// Sending end of activation event to blocks.
        /// </summary>
        private void EndInterruption()
        {
            DeactivateBlocks();
            InterruptionHandled = false;
        }

        public override void OnRemoteActivation()
        {
            IsActive = false;
            _particleSystem.Stop();
        }

        public override void OnRemoteActivationActionReset()
        {
            IsActive = true;
            _particleSystem.Play();
        }
    }
}
