using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace fi.tamk.game.theone
{
    public class BoxMovement : GameBlock
    {
        [HideInInspector] public float speed
        {
            get
            {
                return _movementClass._speed;
            }

            set
            {
                _movementClass._maxSpeed = value;
            }
        }

        public BoxType TypeOfBox = BoxType.Basic;
        [SerializeField] protected float _maxSpeedConfig = 2f;
        [SerializeField] protected float _initialSpeedConfig = -2f;

        protected Interactable _movementClass;

        new void Awake()
        {
            base.Awake();

            switch (TypeOfBox) {
                case BoxType.Basic:
                    _movementClass = new BasicUpDownBox(this);
                    break;
                case BoxType.Accelerating:
                    _movementClass = new AcceleratingUpDownbox(this);
                    break;
                default:
                    _movementClass = new BasicUpDownBox(this);
                    break;
            }

            speed = _maxSpeedConfig;
            _movementClass._speed = _initialSpeedConfig;
        }

        override protected void CollisionBegin(Collision2D col)
        {
            var colYLoc = _movementClass._transform.position.y - col.transform.position.y;

            if (colYLoc > 0)
            {
                _movementClass.OnBelowColl(col);
            } else
            {
                _movementClass.OnTopColl(col);
            }
        }

        void Update()
        {
            _movementClass.MoveBlock();
        }

        private void OnMouseDown()
        {
            _movementClass.OnBoxClicked();
        }

        public override bool IsStationary()
        {
            return _movementClass.InRestState();
        }

        protected override void OnReset()
        {
            _movementClass._speed = _initialSpeedConfig;
        }

        public enum BoxType { Basic, Accelerating }
    }
}
