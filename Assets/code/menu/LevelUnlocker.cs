using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.menu
{
    public class LevelUnlocker : MonoBehaviour
    {
        private void Awake()
        {
            CheckLockedLevels();
        }

        private void Update()
        {
            if (Input.GetKey("p"))
            {
                LevelLoadController.Instance.ResetSave();
                CheckLockedLevels();
            }
        }

        public void CheckLockedLevels()
        {
            var canvas = GameObject.Find("Canvas");

            foreach (var b in LevelLoadController.Instance.LevelsLocked)
            {
                Debug.Log(b);
            }

            for (var j = 0; j < LevelLoadController.NumberOfLevels; j++)
            {
                var i = (j + 1);
                if (!LevelLoadController.Instance.LevelsLocked.ContainsKey(j)) continue;
                canvas.transform.GetChild(i).gameObject.SetActive(!LevelLoadController.Instance.LevelsLocked[j]);
            }
        }
    }
}
