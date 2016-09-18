using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class PPlayerBlock : PGameBlock
    {
        protected Vector2 _playerSpeed = new Vector2(2.2f, 0);
        // protected float _playerAcceleration = 1.1f;
        private bool _moving = false;

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.gameObject.CompareTag("Movable"))
            {
                SceneManager.Instance.PlayerDeathReset();
            }
        }

        void OnCollisionExit2D(Collision2D col)
        {

        }

        void Update()
        {
            if (_moving)
            {
                _transform.Translate(_playerSpeed * SceneManager.Instance.DeltaTime);
            }
        }

        void OnMouseDown()
        {
            if (!_moving)
            {
                _moving = true;
            }
        }

        override public void ResetBlock()
        {
            base.ResetBlock();
            _moving = false;
        }

        public void Checkpoint(Checkpoint c)
        {
            _startLocation = c.Spawn.position;
            _moving = false;
        }
    }
}