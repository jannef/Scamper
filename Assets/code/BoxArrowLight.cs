using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class BoxArrowLight : LightFlicker
    {
        [SerializeField] private PGameBlock ParentBlock;

        protected override void OnAwake()
        {
            if (ParentBlock != null) ParentBlock.BlockClickedEvent += ActivateLight;
        }
    }
}
