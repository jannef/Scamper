using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using fi.tamk.game.theone.menu;

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
            if (other.gameObject != SceneManager.Instance.PlayerGameObject) return;
            LevelLoadController.Instance.LevelCompletionTime = SceneManager.Instance.TimeSinceLevelLoaded;
            LevelLoadController.Instance.CompleteLevel(LevelLoadController.Instance.LastLevelStarted);
            LevelLoadController.Instance.ToScene(7);
        }
    }
}
