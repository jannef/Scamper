using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization;
using System.IO;

namespace fi.tamk.game.theone.menu
{
    /// <summary>
    /// Handles persistent data and saving and loading of scenes.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class LevelLoadController : MonoBehaviour
    {
        /// <summary>
        /// Save file filename.
        /// </summary>
        private const string _saveFile = "save.savedata";

        /// <summary>
        /// Persistent data trough play sessions.
        /// </summary>
        [SerializeField] private SaveData _saveData = null;

        /// <summary>
        /// If the save file exists or not.
        /// </summary>
        private bool SaveExists
        {
            get { return File.Exists(SaveFile); }
        }

        /// <summary>
        /// Full path to save file.
        /// </summary>
        public static string SaveFile
        {
            get { return Path.Combine(Application.persistentDataPath, _saveFile); }
        }

        /// <summary>
        /// Persistent data datatype.
        /// </summary>
        [Serializable]
        private class SaveData
        {
            /// <summary>
            /// Dictionary of levels and their completion status.
            /// </summary>
            public Dictionary<int, bool> LevelsCompleted = new Dictionary<int, bool>();

            /// <summary>
            /// Which level was last played. Continue and next level rely on this.
            /// </summary>
            public int LastLevelPlayed = 0;

            public Dictionary<int, bool> LevelsLocked = new Dictionary<int, bool>();

            /// <summary>
            /// Constructor for the persistent data struct.
            /// </summary>
            /// <param name="howManyLevels"></param>
            public SaveData(int howManyLevels)
            {
                // TODO: rethink this. Title screen, menu, level select and scoring screen all eat up scenes
                // so might need to be adjusted a little.
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
            
            try
            {
                _saveData = LoadGameData();

            }
            catch (Exception e)
            {
                Debug.Log(e);
                _saveData = new SaveData(SceneManager.sceneCount);
                SaveGameData();

            }

        }

        /// <summary>
        /// When a scene is loaded unity fires event this is subscribed (done in Awake()) to.
        /// </summary>
        /// <param name="arg0">Not used.</param>
        /// <param name="loadSceneMode">Not used.</param>
        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            fi.tamk.game.theone.phys.SceneManager.TurnOffQuitFlag();
        }

        /// <summary>
        /// Loads the persistent data from savefile.
        /// </summary>
        /// <returns></returns>
        private SaveData LoadGameData()
        {
            // return fresh save data
            if (!SaveExists) return new SaveData(SceneManager.sceneCount);

            // read save data
            byte[] data = File.ReadAllBytes(SaveFile);
            var formatter = new DataContractSerializer(typeof(SaveData));
            var stream = new MemoryStream(data);

            return (SaveData)(formatter.ReadObject(stream));
        }

        /// <summary>
        /// Writes the persistent data into savefile.
        /// </summary>
        private void SaveGameData()
        {
            var formatter = new DataContractSerializer(typeof(SaveData));
            var stream = new MemoryStream();

            formatter.WriteObject(stream, _saveData);
            File.WriteAllBytes(SaveFile, stream.GetBuffer());
        }

        public void InitializeLevelLocks()
        {
            for (int j = 0; j < LockLevel.levels; j++)
            {
                _saveData.LevelsLocked.Add(j, true);
            }
        }

        /// <summary>
        /// Marks a given level completed.
        /// </summary>
        /// <param name="whichLevel">Which level to mark completed. This is a unity project scene index number.</param>
        public void CompleteLevel(int whichLevel)
        {
            if (_saveData.LevelsCompleted.ContainsKey(whichLevel))
            {
                _saveData.LevelsCompleted[whichLevel] = true;
            }
        }

        /// <summary>
        /// Sets last played scene.
        /// </summary>
        /// <param name="toWhichLevel">To what to set the last played scene.</param>
        public void SetLastPlayed(int toWhichLevel)
        {
            _saveData.LastLevelPlayed = toWhichLevel;
        }

        /// <summary>
        /// Sets locked status of the given level.
        /// </summary>
        /// <param name="levelIndex">Which level's lock status to modify.</param>
        /// <param name="locked">If the level should be locked or not.</param>
        public void LevelLocks(int levelIndex, bool locked)
        {
            for (int j = 0; j < LockLevel.levels; j++)
            {
                if (j == levelIndex)
                {
                    _saveData.LevelsLocked.Add(j, locked);
                }
            }
        }

        /// <summary>
        /// Returns a dictionary holding data on levels locked.
        /// </summary>
        /// <returns>Dictionary holding data on levels locked.</returns>
        public Dictionary<int, bool> GetLevelsLocked()
        {
            return _saveData.LevelsLocked;
        }
        
        /// <summary>
        /// Calls SaveGameData() to save persistent data when the app is closing.
        /// </summary>
        private void OnApplicationQuit()
        {
            SaveGameData();
        }

        public void ToScene(int whichScene)
        {
            SceneManager.LoadScene(whichScene);
        }

        public void ToLevelSelect()
        {
            ToScene(1);
        }
    }
}
