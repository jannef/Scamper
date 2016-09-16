using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone
{
    public class JumpTrigger : MonoBehaviour
    {
        public float JumpPower = 5f;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().Jump(JumpPower);
            }
        }
    }
}
