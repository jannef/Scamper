using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace fi.tamk.game.theone.menu
{
    public class UnlockLevels : MonoBehaviour
    {

        protected string currentLevel;

        protected int _levelIndex;

        private LevelLoadController _levelLoad;

        private void Start()
        {
            _levelLoad = FindObjectOfType<LevelLoadController>();
            currentLevel = SceneManager.GetActiveScene().name;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                Unlock();
            }
        }

        protected void Unlock()
        {
            for (int i = 0; i < LockLevel.levels; i++)
            {
                if (currentLevel == "Day" + (i + 1).ToString() + "_playerfriendly")
                {
                    _levelIndex = (i + 1);
                    _levelLoad.CompleteLevel(_levelIndex);

                    if (_levelIndex < LockLevel.levels)
                    {
                        _levelLoad.LevelLocks(_levelIndex + 1, false);
                    }
                }
            }
        }
    }
}
