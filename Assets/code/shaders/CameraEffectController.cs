using UnityEngine;
using System.Collections;
using fi.tamk.game.theone.phys;

namespace fi.tamk.game.theone.shader
{
    /// <summary>
    /// Camera post effect shader controller.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    [RequireComponent(typeof(Camera))]
    public class CameraEffectController : MonoBehaviour
    {
        /// <summary>
        /// Reference to shader material.
        /// </summary>
        public Material Effect;

        /// <summary>
        /// Player light intensity.
        /// </summary>
        public float Intensity;

        /// <summary>
        /// Reference to camra.
        /// </summary>
        private Camera _cam;

        /// <summary>
        /// Finds references.
        /// </summary>
        private void Awake()
        {
            if (!SystemInfo.supportsImageEffects || Effect == null)
            {
                this.enabled = false;
                return;
            }

            _cam = GetComponent<Camera>();
        }

        /// <summary>
        /// Applies the post effect from Material to camera.
        /// </summary>
        /// <param name="source">unity render thing things</param>
        /// <param name="destination">unity render thing things</param>
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (Effect == null) return;

            var v = _cam.WorldToScreenPoint(SceneManager.Instance.PlayerGameObject.transform.position);
            Effect.SetFloat("_Fade", 1);
            Effect.SetFloat("_WorldX", v.x / _cam.pixelWidth);
            Effect.SetFloat("_WorldY", v.y / _cam.pixelHeight);
            Effect.SetFloat("_Overlay", Intensity);
            Graphics.Blit(source, destination, Effect);
        }
    }
}
