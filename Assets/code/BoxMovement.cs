using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace fi.tamk.game.theone
{
    public class BoxMovement : MonoBehaviour
    {
        public float speed
        {
            get
            {
                return _movementClass._speed;
            }
        }

        [HideInInspector] public List<GameObject> touchList;

        protected Interactable _movementClass;

        void Awake()
        {
            _movementClass = new BasicUpAndDownBox(this);
            touchList = new List<GameObject>();
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            touchList.Add(col.collider.gameObject);

            var colYLoc = _movementClass._transform.position.y - col.transform.position.y;

            if (colYLoc > 0)
            {
                _movementClass.OnBelowColl(col);
            }
            else
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

    }
}
