using UnityEngine;
using System.Collections;
using fi.tamk.game.theone.phys;

namespace fi.tamk.game.theone.shader
{
    [RequireComponent(typeof(Renderer))]
    public class AlertShadeController : MonoBehaviour
    {
        public float Fade = 0f;
        private Material _mat;

        void Start()
        {
            _mat = GetComponent<Renderer>().material;
        }

        void Update()
        {
            Vector3 v = SceneManager.Instance.PlayerGameObject.transform.position;
            _mat.SetFloat("_Fade", Fade);
            _mat.SetFloat("_WorldX", v.x);
            _mat.SetFloat("_WorldY", v.y);
        }
    }
}
