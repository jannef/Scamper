using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class RemoteBase : RemoteBehaviour
    {
        /// <summary>
        /// Blocks this remote activator will affect.
        /// </summary>
        [SerializeField] private RemoteBehaviour[] ActivatedBlocks;

        protected void ActivateBlocks()
        {
            foreach (var block in ActivatedBlocks)
            {
                if (block == null) continue;
                block.OnRemoteActivation();
            }
        }

        protected void DeactivateBlocks()
        {
            foreach (var block in ActivatedBlocks)
            {
                if (block == null) continue;
                block.OnRemoteActivationActionReset();
            }
        }
    }
}
