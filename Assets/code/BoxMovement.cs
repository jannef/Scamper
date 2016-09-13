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
        [HideInInspector] public List<GameObject> touchList;

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

            touchList = new List<GameObject>();
            speed = _maxSpeedConfig;
            _movementClass._speed = _initialSpeedConfig;
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            touchList.Add(col.collider.gameObject);
            var colYLoc = _movementClass._transform.position.y - col.transform.position.y;

            if (colYLoc > 0)
            {
                _movementClass.OnBelowColl(col);
            } else
            {
                _movementClass.OnTopColl(col);
            }
        }

        void OnCollisionExit2D(Collision2D col)
        {
            touchList.Remove(col.collider.gameObject);
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

        public void OnDestroy()
        {
            SceneManager.Instance.ColliderMap.Remove(GetComponent<Collider2D>());
        }

        public enum BoxType { Basic, Accelerating }
    }
}
