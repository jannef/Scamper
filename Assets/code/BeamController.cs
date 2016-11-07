using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class BeamController : MonoBehaviour
    {
        [SerializeField] private Transform Origin;
        [SerializeField] private Transform End;
        [SerializeField] private float ScaleModifier = 0.7f;
        private Transform _transform;
        private Material _material;

        private void Awake()
        {
            _transform = transform;
            _material = GetComponent<Renderer>().material;
        }

        private Vector3 FindCenterOfSprite()
        {
            return (Origin.position + End.position) / 2;
        }

        private float FindRotationOfSprite(Vector3 cutOff)
        {
            var vec = new Vector2(cutOff.x - Origin.position.x, cutOff.y - Origin.position.y);
            return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
        }

        private RaycastHit2D FindObstacle()
        {
            // 9 is CatchRC
            RaycastHit2D rv = Physics2D.Linecast(Origin.position, End.position, LayerMask.GetMask(new [] {"Player", "CatchRC"}));
            if (rv) OnBeamInterruption(rv);

            return rv;
        }

        private void OnBeamInterruption(RaycastHit2D Interruption)
        {
            Debug.Log(Interruption);
        }

        private void Update()
        {
            _transform.position = FindCenterOfSprite();
            _transform.rotation = Quaternion.AngleAxis(FindRotationOfSprite(End.position), Vector3.forward);

            float scale = (Origin.position - End.position).magnitude * ScaleModifier;
            _transform.localScale = new Vector3(scale, scale, 1);

            var col = FindObstacle();
            if (col)
            {
                _material.SetFloat("_WorldX", col.fraction);
            } else
            {
                _material.SetFloat("_WorldX", 1);
            }
        }
    }
}
