using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.menu
{
    public class LevelUnlocker : MonoBehaviour
    {
        public GameObject LockedIcon;
        private const string LockName = "Locked";

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
            DestroyOldLocks(canvas);

            for (var j = 0; j < LevelLoadController.NumberOfLevels; j++)
            {
                var i = (j + 1);
                if (!LevelLoadController.Instance.LevelsLocked.ContainsKey(j)) continue;
                var locked = LevelLoadController.Instance.LevelsLocked[j];
                canvas.transform.GetChild(i).gameObject.SetActive(!locked);

                if (!locked) continue;
                var lockIcon = Instantiate(LockedIcon, canvas.transform) as GameObject;
                lockIcon.name = LockName;

                ((RectTransform) lockIcon.transform).anchoredPosition =
                    ((RectTransform) canvas.transform.GetChild(i)).anchoredPosition;
            }
        }

        private static void DestroyOldLocks(GameObject parent)
        {
            for (var i = 0; i < parent.transform.childCount; i++)
            {
                var child = parent.transform.GetChild(i).gameObject;
                if (child.name.Equals(LockName)) DestroyImmediate(child);
            }
        }
    }
}
