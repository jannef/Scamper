using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class NeckLight : LightFlicker
    {
        private void NeckLightOn()
        {
            StopAllCoroutines();
            ActivateLight();
        }

        protected override void OnAwake()
        {
            SceneManager.Instance.ClickEvent += NeckLightOn;
        }
    }
}
