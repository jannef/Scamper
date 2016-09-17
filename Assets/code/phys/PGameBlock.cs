using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PGameBlock : MonoBehaviour
    {
        protected Vector3 _startLocation;
        protected Transform _transform;
        protected Rigidbody2D _rb;

        void Start()
        {
            _transform = transform;
            _startLocation = _transform.position;
            _rb = GetComponent<Rigidbody2D>();
        }

        void ResetBlock()
        {
            _rb.velocity = Vector2.zero;
            _transform.position = _startLocation;
        }
    }
}
