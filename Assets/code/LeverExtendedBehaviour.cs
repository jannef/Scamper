using UnityEngine;
using System.Collections;
using UnityEditor;

namespace fi.tamk.game.theone.phys
{

    /// <summary>
    /// Extends the functionality of a 2D hinge joint. Now the joint can be configured to only
    /// be able to turned into one direction.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    [RequireComponent(typeof(HingeJoint2D))]
    public class LeverExtendedBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Hinge joint functionality of which to extend.
        /// </summary>
        private HingeJoint2D _hinge = null;

        /// <summary>
        /// Stored original limits of this hinge joint. Used to restore defaults on level reset.
        /// </summary>
        private JointAngleLimits2D _originaLimits2D;

        [SerializeField] private bool LowerUpperLimitWhenPressed = false;

        /// <summary>
        /// Get reference to hinge joint.
        /// </summary>
        private void Awake()
        {
            _hinge = GetComponent<HingeJoint2D>();
            if (_hinge == null) return;

            _originaLimits2D = _hinge.limits;
            SceneManager.Instance.LevelResetEvent += ResetLimits;
        }

        /// <summary>
        /// Sets new limits for the hinge joint.
        /// </summary>
        private void Update()
        {
            if (_hinge == null || !LowerUpperLimitWhenPressed || !(_hinge.jointAngle < _hinge.limits.max)) return;

            var limits = _hinge.limits;
            limits.max = _hinge.jointAngle;

            _hinge.limits = limits;
        }

        /// <summary>
        /// Resets the limits to original ones.
        /// </summary>
        private void ResetLimits()
        {
            if (_hinge != null) _hinge.limits = _originaLimits2D;
        }
    }
}
