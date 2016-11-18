using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Manages a beam emitter object.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class BeamController : MonoBehaviour
    {
        /// <summary>
        /// Blocks this remote activator will affect.
        /// </summary>
        [SerializeField] private PGameBlock[] ActivatedBlocks;

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
        private Material _material;

        /// <summary>
        /// Gets references to objects.
        /// </summary>
        private void Awake()
        {
            _transform = transform;
            _material = GetComponent<Renderer>().material;
            SceneManager.Instance.LevelResetEvent += ResetBeam;
        }

        private void ResetBeam()
        {
            if (InterruptionHandled) EndInterruption();
            InterruptionHandled = false;
        }

        /// <summary>
        /// Finds center of the origin and end.
        /// </summary>
        /// <returns>The found center.</returns>
        private Vector3 FindCenterOfSprite()
        {
            return (Origin.position + End.position) / 2;
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
        private RaycastHit2D FindObstacle()
        {
            RaycastHit2D rv = Physics2D.Linecast(Origin.position, End.position, LayerMask.GetMask(new [] {"Player", "CatchRC"}));
            return rv;
        }

        /// <summary>
        /// Updates the beam it's shader.
        /// </summary>
        private void Update()
        {
            _transform.position = FindCenterOfSprite();
            _transform.rotation = Quaternion.AngleAxis(FindRotationOfSprite(End.position), Vector3.forward);

            float scale = (Origin.position - End.position).magnitude * ScaleModifier;
            _transform.localScale = new Vector3(scale, scale, 1);

            var col = FindObstacle();
            if (col)
            {
                _material.SetFloat("_WorldX", col.fraction);
                if (!InterruptionHandled)
                {
                    // Setting boolean before the function call is mandatory. Do not change.
                    InterruptionHandled = true;
                    BeginInterruption(col);
                }

            } else
            {
                _material.SetFloat("_WorldX", 1);
                if (InterruptionHandled)
                {
                    EndInterruption();
                    InterruptionHandled = false;
                }
            }
        }

        /// <summary>
        /// Handling of the interruption of the beam, depending on what interrupted it and what behaviour is set.
        /// </summary>
        /// <param name="col">the interrupting event</param>
        private void BeginInterruption(RaycastHit2D col)
        {
            if (KillsPlayer && col.collider.gameObject.CompareTag("Player"))
            {
                SceneManager.Instance.PlayerDeathReset();
                return;
            }

            foreach (var block in ActivatedBlocks)
            {
                if (block == null) continue;
                block.OnRemoteActivation();
            }
        }

        /// <summary>
        /// Sending end of activation event to blocks.
        /// </summary>
        private void EndInterruption()
        {
            foreach (var block in ActivatedBlocks)
            {
                if (block == null) continue;
                block.OnRemoteActivationActionReset();
            }
        }
    }
}
