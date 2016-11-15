using UnityEngine;
using System.Collections;
using fi.tamk.game.theone.phys;

namespace fi.tamk.game.theone.shader
{
    /// <summary>
    /// This shader was scrapped. Not used.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    [RequireComponent(typeof(Renderer))]
    public class TilingDesaturationController : MonoBehaviour
    {
        public float Fade = 0f;
        private Material _mat;

        public float tileX = 1f;
        public float tileY = 1f;

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
            _mat.SetFloat("_TileX", tileX);
            _mat.SetFloat("_TileY", tileY);
        }
    }
}
