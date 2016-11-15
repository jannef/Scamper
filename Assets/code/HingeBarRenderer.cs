using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Renders the rope in hanging objects.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    [RequireComponent(typeof(LineRenderer))]
    [ExecuteInEditMode]
    public class HingeBarRenderer : MonoBehaviour
    {
        /// <summary>
        /// Reference to anchoring transform.
        /// </summary>
        [SerializeField] private Transform AnchorPos;

        /// <summary>
        /// Reference to the transform the rope is attached on the object.
        /// </summary>
        [SerializeField] private Transform ObjectPos;

        /// <summary>
        /// Line renderer reference.
        /// </summary>
        private LineRenderer _lr;

        /// <summary>
        /// Finds the reference to line renderer.
        /// </summary>
        private void Awake()
        {
            _lr = GetComponent<LineRenderer>();
            UpdatePositions();
        }

        /// <summary>
        /// Repositions the end of the linerenderer.
        /// </summary>
        private void UpdatePositions()
        {
            _lr.SetPositions(new[] { AnchorPos.position, ObjectPos.position });
        }

        /// <summary>
        /// Calls to update the position of the linerenderer.
        /// </summary>
        private void Update()
        {
            UpdatePositions();
        }
    }
}
