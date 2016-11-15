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
            if (rv) OnBeamInterruption(rv);

            return rv;
        }

        /// <summary>
        /// Fires when something blocks blocks the beams path.
        /// </summary>
        /// <param name="Interruption">The hit that interrupted the beam.</param>
        private void OnBeamInterruption(RaycastHit2D Interruption)
        {
            
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
                Debug.Log(col.fraction);
                _material.SetFloat("_WorldX", col.fraction);
            } else
            {
                _material.SetFloat("_WorldX", 1);
            }
        }
    }
}
