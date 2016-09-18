using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class BoxScriptTrigger : MonoBehaviour
    {
        public bool StopBoxOnEnter = false;

        public bool SetNewLocomotion = false;
        public Vector2 NewLocomotion = Vector2.zero;
        //public bool SetNewAcceleration = false;
        //public float NewAcceleration = 0f;
        public bool ChangeLocomotionActiveStatus = false;
        public bool NewLocomotionActive = false;
        public bool SetNewGravityScale = false;
        public float NewGravityScale = 1f;
        public bool SetNewLockedStatus = false;
        public bool NewLockedStatus = false;

        [SerializeField] protected GameObject WhichBox;

        private PGameBlockMovable _box;

        void Start()
        {
            _box = WhichBox.GetComponent<PGameBlockMovable>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == WhichBox)
            {
                if (StopBoxOnEnter) _box.StopBox();

                if (SetNewLocomotion) _box.Locomotion = NewLocomotion;
                //if (SetNewAcceleration) _box.LocomotionAcceleration = NewAcceleration;
                if (ChangeLocomotionActiveStatus) _box.LocomotionActive = NewLocomotionActive;
                if (SetNewGravityScale) _box.SetGravity(NewGravityScale);
                if (SetNewLockedStatus) _box.LockedFromPlayer = NewLockedStatus;
            }
        }
    }
}
