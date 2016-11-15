using UnityEngine;
using System.Collections;
using fi.tamk.game.theone.phys;

namespace fi.tamk.game.theone.shader
{
    /// <summary>
    /// Controls desaturation shader.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    [RequireComponent(typeof(Renderer))]
    public class DesaturationShaderController : MonoBehaviour
    {
        /// <summary>
        /// How much to desaturate.
        /// </summary>
        public float Fade = 0f;

        /// <summary>
        /// Renderer material
        /// </summary>
        private Material _mat;

        /// <summary>
        /// Find reference to material.
        /// </summary>
        void Start()
        {
            _mat = GetComponent<Renderer>().material;
        }

        /// <summary>
        /// Pass information to shader.
        /// </summary>
        void Update()
        {
            Vector3 v = SceneManager.Instance.PlayerGameObject.transform.position;
            _mat.SetFloat("_Fade", Fade);
            _mat.SetFloat("_WorldX", v.x);
            _mat.SetFloat("_WorldY", v.y);
        }
    }
}
