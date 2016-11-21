using UnityEngine;
using System.Collections;

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
        /// Reverses direction the lever is pushed towards.
        /// </summary>
        [SerializeField]
        private bool ReverseDirection = false;

        /// <summary>
        /// Get reference to hinge joint.
        /// </summary>
        private void Awake()
        {
            _hinge = GetComponent<HingeJoint2D>();
            if (_hinge == null) return;

            if (ReverseDirection)
            {
                var motor = _hinge.motor;
                motor.motorSpeed *= -1f;
                _hinge.motor = motor;
            }

            _originaLimits2D = _hinge.limits;
            SceneManager.Instance.LevelResetEvent += ResetLimits;
        }

        /// <summary>
        /// Sets new limits for the hinge joint.
        /// </summary>
        private void Update()
        {
            if (_hinge == null || !LowerUpperLimitWhenPressed || ((!(_hinge.jointAngle < _hinge.limits.max) && !ReverseDirection) || (!(_hinge.jointAngle > _hinge.limits.min) && ReverseDirection))) return;

            var limits = _hinge.limits;
            if (ReverseDirection)
            {
                limits.min = Mathf.Min(_hinge.jointAngle, limits.max);
            }
            else
            {
                limits.max = Mathf.Max(_hinge.jointAngle, limits.min);
            }
            

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
