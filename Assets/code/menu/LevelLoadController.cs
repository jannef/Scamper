using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

namespace fi.tamk.game.theone.menu
{
    public class LevelLoadController : MonoBehaviour
    {

        /// <summary>
        /// Makes sure there are no duplicate load systems without using a full singleton pattern.
        /// </summary>
        private void Awake()
        {
            var found = FindObjectsOfType<LevelLoadController>();

            if (found.Any(f => f != this))
            {
                Destroy(this.gameObject);
                return;
            }

            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        }

        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            fi.tamk.game.theone.phys.SceneManager.TurnOffQuitFlag();
        }
    }
}
