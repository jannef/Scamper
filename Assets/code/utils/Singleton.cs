using UnityEngine;
using System.Collections;

namespace fi.tamk.game.theone.utils
{
    /// <summary>
    /// One scene lifetime singleton base class.
    /// </summary>
    /// <auth>Janne Forsell</auth>
    /// <typeparam name="T">Type of the class that inherits from this.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of the class.
        /// </summary>
        private static T _instance;

        /// <summary>
        /// Mutual-exclusion lock.
        /// </summary>
        private static object _lock = new object();

        /// <summary>
        /// Aplication quit flag.
        /// </summary>
        private static bool _quit = false;

        /// <summary>
        /// Returns, and creates if needed, reference to the singleten object.
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance != null) return _instance;

                    _instance = (T)FindObjectOfType(typeof(T));

                    if (_instance == null)
                    {
                        var singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "Singleton instance of " + typeof(T).ToString();
                        DontDestroyOnLoad(singleton);
                    }
                    else
                    {
                        // Somethign has broken, there has been floating instance of this singleton component floating
                        // around in the game world that was found by FindObjectOfType(). Am not sure how this can
                        // happen since using lock(object) should make this thread safe.
                        throw (new System.Exception(string.Format("Singleton instance broke. {0} is contained as _instance", _instance.GetType().ToString())));
                    }

                    return _instance;
                }
            }
        }
    }
}
