using UnityEngine;
using System.Collections;
using UnityEditor;

namespace fi.tamk.game.theone.phys
{
    [RequireComponent(typeof(HingeJoint2D))]
    public class LeverExtendedBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Hinge joint functionality of which to extend.
        /// </summary>
        private HingeJoint2D _hinge = null;

        private JointAngleLimits2D _originaLimits2D;

        [SerializeField] private bool LowerUpperLimitWhenPressed = false;

        /// <summary>
        /// Get reference to hinge joint.
        /// </summary>
        private void Awake()
        {
            _hinge = GetComponent<HingeJoint2D>();
            if (_hinge != null)
            {
                _originaLimits2D = _hinge.limits;
                SceneManager.Instance.LevelResetEvent += ResetLimits;
            }
        }

        private void Update()
        {
            if (_hinge == null || !LowerUpperLimitWhenPressed || !(_hinge.jointAngle < _hinge.limits.max)) return;

            var limits = _hinge.limits;
            limits.max = _hinge.jointAngle;

            _hinge.limits = limits;
        }

        private void ResetLimits()
        {
            if (_hinge != null) _hinge.limits = _originaLimits2D;
        }
    }
}
