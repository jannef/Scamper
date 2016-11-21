using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class NeckLight : LightFlicker
    {
        private void NeckLightOn()
        {
            ActivateLight();
        }

        protected override void OnAwake()
        {
            SceneManager.Instance.ClickEvent += NeckLightOn;
        }
    }
}
