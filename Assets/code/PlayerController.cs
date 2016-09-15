using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone
{
    public class PlayerController : GameBlock
    {
        [SerializeField] protected Vector2 _speed = new Vector2(0, 0);
        [SerializeField] protected float _targetSpeed = 2.1f;
        [SerializeField] protected float _acceleration = 1f;
        [SerializeField] protected bool _stationary = true;
        [SerializeField] protected bool _falling = true;
        protected Transform _transform;

        protected Vector3 _lastCheckpoint;

        new public void Awake()
        {
            base.Awake();
            _transform = transform;

            _lastCheckpoint = transform.position;
        }

        public void Checkpoint(Checkpoint c)
        {
            _speed.x = 0;
            _stationary = true;
            _lastCheckpoint = c.Spawn.position;
        }

        private void OnMouseDown()
        {
            if (_stationary && !_falling) _stationary = false;
        }

        void Update()
        {
            if (_falling)
            {
                _speed.y += SceneManager.Instance.GlobalGravity * SceneManager.Instance.DeltaTime;
            }

            if (!_stationary)
            {
                _speed.x = Mathf.Min(_speed.x + _acceleration * SceneManager.Instance.DeltaTime, _targetSpeed);
            }

            _transform.Translate(_speed.x * SceneManager.Instance.DeltaTime, _speed.y * SceneManager.Instance.DeltaTime, 0);
        }

        public override bool IsStationary()
        {
            return _stationary;
        }

        protected override void CollisionBegin(Collision2D col)
        {
            if (SceneManager.Instance.ColliderMap[col.collider.gameObject].CompareTag("Floor"))
            {
                _falling = false;
                _speed.y = 0f;
            } else if (SceneManager.Instance.ColliderMap[col.collider.gameObject].CompareTag("Movable"))
            {
                _transform.position = _lastCheckpoint;
                _stationary = true;
                _falling = true;
                _speed = Vector2.zero;
                SceneManager.Instance.PlayerDeathReset();
            }
        }

        protected override void CollisionEnd(Collision2D col)
        {
            if (SceneManager.Instance.ColliderMap[col.collider.gameObject].CompareTag("Floor"))
            {
                _falling = true;
            }
        }
    }
}
