using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Gives entering player a jump impulse.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class JumpTrigger : MonoBehaviour
    {
        /// <summary>
        /// How big an impulse.
        /// </summary>
        public float JumpPower = 5f;

        /// <summary>
        /// Gives the impulse.
        /// </summary>
        /// <param name="other">Not used.</param>
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, JumpPower), ForceMode2D.Impulse);
            }
        }
    }
}
