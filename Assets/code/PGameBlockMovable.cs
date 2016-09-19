using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class PGameBlockMovable : PGameBlock
    {
        public Vector2 Locomotion = new Vector2(2f, 0f);
        public float LocomotionAcceleration = 1.3f;
        public bool LocomotionActive = false;

        /* These boxes are ment to have routes that are set up by triggers
         * initial values that can be changed during lifetime are stored to
         * be able to reset the block on player death */
        private Vector2 _initialLocomotion;
        private float _initialAcceleration;
        private bool _initialLocomotionActive;

        override protected void OnStart()
        {        
            // Store initial settings
            _initialAcceleration = LocomotionAcceleration;
            _initialLocomotion = Locomotion;
            _initialLocomotionActive = LocomotionActive;
        }

        void Update()
        {
            if (LocomotionActive)
            {
                _transform.Translate(Locomotion * SceneManager.Instance.DeltaTime, Space.World);
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
