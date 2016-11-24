using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
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

        private int _changeToOnLoad = -1;

        /// <summary>
        /// Persistent data trough play sessions.
        /// </summary>
        public SaveData _saveData = null;

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
        public class SaveData
        {
            /// <summary>
            /// Dictionary of levels and their completion status.
            /// </summary>
            public readonly Dictionary<int, bool> LevelsCompleted = null;

            /// <summary>
            /// Which level was last played. Continue and next level rely on this.
            /// </summary>
            public int LastLevelPlayed = 0;
            public int RandomTest = 0;

            /// <summary>
            /// Constructor for the persistent data struct.
            /// </summary>
            /// <param name="howManyLevels"></param>
            public SaveData(int howManyLevels)
            {
                LevelsCompleted = new Dictionary<int, bool>();

                // Populates the dictionary based on how many scenes there are in the project.
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
            _saveData = LoadGameData();
        }

        /// <summary>
        /// When a scene is loaded unity fires event this is subscribed (done in Awake()) to.
        /// </summary>
        /// <param name="arg0">Not used.</param>
        /// <param name="loadSceneMode">Not used.</param>
        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            fi.tamk.game.theone.phys.SceneManager.TurnOffQuitFlag();

            if (_changeToOnLoad >= 0)
            {
                var to = _changeToOnLoad;
                _changeToOnLoad = -1;
                SceneManager.LoadScene(to);
            }

            _saveData.RandomTest++;
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
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream(data);

            Debug.Log(string.Format("Loaded successfully: {0}", SaveFile));
            return (SaveData)(formatter.Deserialize(stream));
        }

        /// <summary>
        /// Writes the persistent data into savefile.
        /// </summary>
        private void SaveGameData()
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();

            formatter.Serialize(stream, _saveData);
            File.WriteAllBytes(SaveFile, stream.GetBuffer());

            Debug.Log(string.Format("Saved game data successfully: {0}", SaveFile));
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
        /// Calls SaveGameData() to save persistent data when the app is closing.
        /// </summary>
        private void OnApplicationQuit()
        {
            SaveGameData();
        }

        public void ToScene(int whichScene)
        {
            _changeToOnLoad = whichScene;
            SceneManager.LoadScene(0);
        }
    }
}
