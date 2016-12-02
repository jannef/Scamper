using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.menu
{

    public class LockLevel : MonoBehaviour
    {

        public const int levels = 4;

        private LevelLoadController _levelLoad;

        private int levelIndex;

        private void Start()
        {
            _levelLoad = FindObjectOfType<LevelLoadController>();
            // PlayerPrefs.DeleteAll();
            LockLevels();
        }

        void LockLevels()
        {
            for (int i = 0; i < levels; i++)
            {
                levelIndex = (i + 1);

                _levelLoad.LevelLocks(levelIndex, true);

            }
        }
    }
}
