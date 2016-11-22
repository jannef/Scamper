using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Light on the players neck. Turns on when telekinetic powers are used.
    /// </summary>
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
