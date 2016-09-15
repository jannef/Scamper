using System;
using UnityEngine;

namespace fi.tamk.game.theone
{
    public class BasicUpDownBox : Interactable
    {
        public BasicUpDownBox(BoxMovement master) : base(master)
        {

        }

        public override void MoveBlock()
        {
            _transform.Translate(0, _speed * SceneManager.Instance.DeltaTime, 0);
        }

        public override void OnTopColl(Collision2D col)
        {
            _speed *= -Mathf.Abs(_speed);
        }

        public override void OnBelowColl(Collision2D col)
        {
            GameBlock collider = SceneManager.Instance.ColliderMap[col.collider.gameObject];
            if (collider.gameObject.CompareTag("Player")) return;

            if (collider != null && !collider.IsStationary())
            {
                _speed = Mathf.Abs(_speed);
            } else
            {
                _speed = 0;
            }
        }

        public override void OnBoxClicked()
        {
            foreach (var t in _master.touchList)
            {
                // Break and do nothing if this Interactable is in contact with another Interactable thats above this one.
                if (t.CompareTag("Movable") && t.transform.position.y - _master.transform.position.y > 0) return;
            }

            if (_speed == 0) _speed = _maxSpeed;
        }
    }
}
