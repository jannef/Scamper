using UnityEngine;
using System.Collections;
using fi.tamk.game.theone.phys;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Not currently used. Was used from a remote activation test/proto.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class TriggerBase : MonoBehaviour
    {
        [SerializeField] private PGameBlock[] ActivatedBlocks;
        [SerializeField] private float duration = 2f; 

        public void ActivateBlocks()
        {
            if (ActivatedBlocks != null && ActivatedBlocks.Length > 0)
            {
                foreach(var a in ActivatedBlocks)
                {
                    a.OnRemoteActivation(duration);
                }
            }
        }
    }
}
