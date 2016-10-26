using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    [ExecuteInEditMode]
    public class HingeBarRenderer : MonoBehaviour
    {
        [SerializeField] private Transform AnchorPos;
        [SerializeField] private Transform ObjectPos;

        private LineRenderer _lr;

        private void Awake()
        {
            _lr = GetComponent<LineRenderer>();
            UpdatePositions();
        }

        private void UpdatePositions()
        {
            _lr.SetPositions(new[] { AnchorPos.position, ObjectPos.position });
        }

        private void Update()
        {
            UpdatePositions();
        }
    }
}
