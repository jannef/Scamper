using UnityEngine;

namespace fi.tamk.game.theone
{
    abstract public class Interactable
    {
        public Transform _transform;
        public float _speed;
        public float _maxSpeed;
        protected BoxMovement _master;
        protected Collider2D _collider;

        public Interactable(BoxMovement master)
        {
            _master = master;
            _transform = _master.transform;
            _collider = _master.gameObject.GetComponent<Collider2D>();
        }

        void Awake()
        {
            _transform = _master.transform;
        }


        public virtual void OnBelowColl(Collision2D col)
        {
        }

        public virtual void OnTopColl(Collision2D col)
        {
        }

        void Update()
        {
            MoveBlock();
        }

        public virtual void MoveBlock()
        {

        }

        private void OnMouseDown()
        {
            OnBoxClicked();
        }

        public virtual void OnBoxClicked()
        {
        }

        public virtual bool InRestState()
        {
            if (_speed != 0f) return false;

            // Invoke recursive search on blocks below to find possible connection to ground
            foreach (var t in _master.touchList)
            {
                if (t.transform.position.y >= _transform.transform.position.y)
                {
                    continue;
                } else
                {
                    if (SceneManager.Instance.ColliderMap[t].IsStationary()) return true;
                }
            }
            return false;
        }
    }
}
