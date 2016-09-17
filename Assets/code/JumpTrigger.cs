using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    public class JumpTrigger : MonoBehaviour
    {
        public float JumpPower = 5f;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, JumpPower), ForceMode2D.Impulse);
            }
        }
    }
}
