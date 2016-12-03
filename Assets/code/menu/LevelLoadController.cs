using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization;
using System.IO;
using System.Linq.Expressions;
using fi.tamk.game.theone.utils;

namespace fi.tamk.game.theone.menu
{
    /// <summary>
    /// Handles persistent data and saving and loading of scenes.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    public class LevelLoadController : Singleton<LevelLoadController>
    {
        public const int NumberOfLevels = 4;

        public int LastLevelStarted
        {
            get { return _saveData.LastLevelPlayed; }

            set { _saveData.LastLevelPlayed = value; }
        }

        public float LevelCompletionTime = 0f;
        public int RatsDeadPerLevel = 0;

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

        public void ResetSave()
        {
            _saveData = new SaveData(NumberOfLevels);
            SaveGameData();
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
                for (int i = 0; i < howManyLevels; i++)
                {
                    LevelsLocked.Add(i, i != 0);
                }
            }
        }

        /// <summary>
        /// Makes sure there are no duplicate load systems without using a full singleton pattern.
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            try
            {
                _saveData = LoadGameData();
                if (_saveData.LevelsLocked == null) throw new Exception("Dun goofed with dictionaries again!");

            }
            catch (Exception e)
            {
                Debug.Log(e);
                _saveData = new SaveData(LevelLoadController.NumberOfLevels);
                SaveGameData();
            }
        }

        /// <summary>
        /// Loads the persistent data from savefile.
        /// </summary>
        /// <returns></returns>
        private SaveData LoadGameData()
        {
            if (!SaveExists) return new SaveData(SceneManager.sceneCount);

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

        /// <summary>
        /// Marks a given level completed.
        /// </summary>
        /// <param name="whichLevel">Which level to mark completed. This is a unity project scene index number.</param>
        public void CompleteLevel(int whichLevel)
        {
            if (_saveData.LevelsLocked.ContainsKey(whichLevel))
            {
                _saveData.LevelsLocked[whichLevel] = false;
            }
        }

        /// <summary>
        /// Returns a dictionary holding data on levels locked.
        /// </summary>
        public Dictionary<int, bool> LevelsLocked
        {
            get { return _saveData.LevelsLocked; }
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

        public void ToLevel(int whichLevel)
        {
            LastLevelStarted = whichLevel;
            ToScene(whichLevel + 1);
        }

        public void ToLevelSelect()
        {
            ToScene(1);
        }
    }
}
