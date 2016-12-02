using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Level end checkpoint.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class LevelEndCheckpoint : MonoBehaviour {

        /// <summary>
        /// Changes to ToWhichLevel scene (via a menu screen)
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == SceneManager.Instance.PlayerGameObject)
            {
                SceneManager.Instance.PersistentData.ToScene(7);
            }
        }

    }
}
