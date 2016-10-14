using UnityEngine;
using System.Collections;
namespace fi.tamk.game.theone.phys
{
    public class PGameFloor : PGameBlock
    {
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
