using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Lights up the box's indicator arrow when the box is clicked.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class BoxArrowLight : LightFlicker
    {
        /// <summary>
        /// Box to click event of which to bind into.
        /// </summary>
        [SerializeField] private PGameBlock ParentBlock;

        /// <summary>
        /// Subscribe to click event.
        /// </summary>
        protected override void OnAwake()
        {
            if (ParentBlock != null) ParentBlock.BlockClickedEvent += ActivateLight;
        }
    }
}
