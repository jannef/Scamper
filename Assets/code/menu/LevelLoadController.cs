using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace fi.tamk.game.theone.menu
{
    public class LevelLoadController : MonoBehaviour
    {

        private const string _saveFile = "save.savedata";
        private SaveData _saveData = null;

        private bool SaveExists
        {
            get { return File.Exists(SaveFile); }
        }

        public static string SaveFile
        {
            get { return Path.Combine(Application.persistentDataPath, _saveFile); }
        }

        [Serializable]
        private class SaveData
        {
            public readonly Dictionary<int, bool> LevelsCompleted = null;
            public int LastLevelPlayed = 0;

            public SaveData(int howManyLevels)
            {
                LevelsCompleted = new Dictionary<int, bool>();

                for (int i = 0; i < howManyLevels; i++)
                {
                    LevelsCompleted.Add(i, false);
                }
            }
        }

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
            _saveData = LoadGameData();
        }

        private static void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            fi.tamk.game.theone.phys.SceneManager.TurnOffQuitFlag();
        }

        private SaveData LoadGameData()
        {
            // return fresh save data
            if (!SaveExists) return new SaveData(SceneManager.sceneCount);

            // read save data
            byte[] data = File.ReadAllBytes(SaveFile);
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream(data);

            Debug.Log(string.Format("Loaded successfully: {0}", SaveFile));
            return (SaveData)(formatter.Deserialize(stream));
        }

        private void SaveGameData()
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();

            formatter.Serialize(stream, _saveData);
            File.WriteAllBytes(SaveFile, stream.GetBuffer());

            Debug.Log(string.Format("Saved game data successfully: {0}", SaveFile));
        }

        public void CompleteLevel(int whichLevel)
        {
            if (_saveData.LevelsCompleted.ContainsKey(whichLevel))
            {
                _saveData.LevelsCompleted[whichLevel] = true;
            }
        }

        public void SetLastPlayed(int toWhichLevel)
        {
            _saveData.LastLevelPlayed = toWhichLevel;
        }

        private void OnApplicationQuit()
        {
            SaveGameData();
        }
    }
}
