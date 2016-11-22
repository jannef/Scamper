using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.phys
{
    /// <summary>
    /// Level end checkpoint.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class LevelEndCheckpoin : MonoBehaviour {
        /// <summary>
        /// To which level to jump into.
        /// </summary>
        [SerializeField] private int ToWhichLevel = 0;

        /// <summary>
        /// Changes to ToWhichLevel scene (via a menu screen)
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == SceneManager.Instance.PlayerGameObject)
            {
                SceneManager.Instance.PersistentData.ToScene(ToWhichLevel);
            }
        }
    }
}
