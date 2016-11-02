using UnityEngine;
using System.Collections;
using fi.tamk.game.theone.phys;

namespace fi.tamk.game.theone.shader
{
    public class CameraEffectController : MonoBehaviour
    {
        public Material Effect;
        private Camera _cam;

        private void Awake()
        {
            _cam = GetComponent<Camera>();
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (Effect != null)
            {
                var v = _cam.WorldToScreenPoint(SceneManager.Instance.PlayerGameObject.transform.position);
                Effect.SetFloat("_Fade", 1);
                Effect.SetFloat("_WorldX", v.x / _cam.pixelWidth);
                Effect.SetFloat("_WorldY", v.y / _cam.pixelHeight);
                Graphics.Blit(source, destination, Effect);
            }
        }
    }
}
