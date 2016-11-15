using UnityEngine;
using System.Collections;
namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Floor game object. Most of the functionality inherited is hidden.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class PGameFloor : PGameBlock
    {
        /// <summary>
        /// Is always considered to be in rest state.
        /// </summary>
        /// <returns>Always true.</returns>
        public override bool IsResting()
        {
            return true;
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            
        }

        void OnCollisionExit2D(Collision2D col)
        {

        }

        void OnMouseDown()
        {

        }
    }
}
