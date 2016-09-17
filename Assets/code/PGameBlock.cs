using UnityEngine;
using System.Collections.Generic;

namespace fi.tamk.game.theone.phys
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PGameBlock : MonoBehaviour
    {
        struct Touch
        {
            GameObject collider;
        }

        public Vector2 ForceOnClick = new Vector2(0, 7.8f);
        public bool DampenInertia = true;

        protected Vector3 _startLocation;
        protected Transform _transform;
        protected Rigidbody2D _rb;

        protected Dictionary<GameObject, Collision2D> _touchList;

        void OnCollisionEnter2D(Collision2D col)
        {
            _touchList.Add(col.collider.gameObject, col);

            if (DampenInertia && IsResting())
            {
                _rb.velocity = Vector2.zero;
            }
        }

        virtual public bool IsResting()
        {
            foreach (var t in _touchList)
            {
                if (t.Value.contacts[0].point.y < _transform.position.y)
                {
                    // found solid ground or somethign in touch with ground below
                    if (SceneManager.Instance.GameObjectMap[t.Key].IsResting()) return true;
                }
            }

            // didn't find solid ground below
            return false;
        }

        public bool IsTopmost()
        {
            foreach (var t in _touchList)
            {
                if (t.Value.contacts[0].point.y > _transform.position.y)
                {
                    return false;
                }
            }

            return true;
        }

        void OnCollisionExit2D(Collision2D col)
        {
            _touchList.Remove(col.collider.gameObject);
        }

        void Start()
        {
            SceneManager.Instance.GameObjectMap.Add(gameObject, this);

            _touchList = new Dictionary<GameObject, Collision2D>();
            _transform = transform;
            _startLocation = _transform.position;
            _rb = GetComponent<Rigidbody2D>();
        }

        virtual public void ResetBlock()
        {
            _rb.velocity = Vector2.zero;
            _transform.position = _startLocation;
        }

        void OnMouseDown()
        {
            if (IsResting() && IsTopmost())
            {
                _rb.AddForce(ForceOnClick, ForceMode2D.Impulse);
            }
        }
    }
}
