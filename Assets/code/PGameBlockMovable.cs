using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class PGameBlockMovable : PGameBlock
    {
        public Vector2 Locomotion = new Vector2(2f, 0f);
        public float LocomotionAcceleration = 1.3f;
        public bool LocomotionActive = false;

        #region LocomotionVariables
        /* These boxes are ment to have routes that are set up by triggers
         * initial values that can be changed during lifetime are stored to
         * be able to reset the block on player death */
        private Vector2 _initialLocomotion;
        private float _initialAcceleration;
        private bool _initialLocomotionActive;
        #endregion

        #region RotationVaraibles
        /* 
         * This rotates the box anticipating, collision to make stuff look
         * smooth. These are used by SetRotation()
         */
        private bool _inRotation = false;
        private float _startRotation;
        private float _rotationTimer;
        private float _rotationEndTime;
        private float _rotationTarget;
        #endregion

        #region MonoBehaviourMethods
        void Update()
        {
            if (LocomotionActive)
            {
                _transform.Translate(Locomotion * SceneManager.Instance.DeltaTime, Space.World);
            }

            if (_inRotation)
            {
                _rotationTimer += SceneManager.Instance.DeltaTime;
                _rb.rotation = Mathf.Lerp(_startRotation, _rotationTarget, Mathf.Min(_rotationTimer / _rotationEndTime));

                if (_rotationTimer >= _rotationEndTime) _inRotation = false;
            }
        }
        #endregion

        #region OverrideMethods
        override protected void OnStart()
        {        
            // Store initial settings
            _initialAcceleration = LocomotionAcceleration;
            _initialLocomotion = Locomotion;
            _initialLocomotionActive = LocomotionActive;
        }
        #endregion

        public void SetRotation(float duration, float rotation)
        {
            if (duration <= 0)
            {
                _rb.rotation = rotation;
            }
            else
            {
                _inRotation = true;
                _rotationTimer = 0f;
                _rotationEndTime = duration;
                _startRotation = _rb.rotation;
            }
        }

        new public void ResetBlock()
        {
            base.ResetBlock();

            // Restore initial settings
            LocomotionAcceleration = _initialAcceleration;
            Locomotion = _initialLocomotion;
            LocomotionActive = _initialLocomotionActive;
        }

        public void StopBox()
        {
            _rb.velocity = Vector2.zero;
        }
    }
}
