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
                if (_quit)
                {
                    return null;
                }

                lock (_lock)
                {
                    if (_instance != null) return _instance;

                    _instance = (T)FindObjectOfType(typeof(T));

                    if (_instance == null)
                    {
                        var singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "Singleton instance of " + typeof(T).ToString();
                        /*
                            DontDestroyOnLoad(singleton);

                            Disabled because we need to have singletons derive from this class one scene lifetime.
                            It was an oversight to rely on singleton at the start of the development. Changes to
                            concept forced to this workaround or rewrite. Due lack of time/energy decided to settle
                            on workaround.
                        */
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

        /// <summary>
        /// Sets "flag" to signal application is quitting.
        /// </summary>
        public void OnDestroy()
        {
            _quit = true;
        }

        /// <summary>
        /// Turns of quitting flag, as it is mistakenly raised on scene change. This is due a workaround 
        /// discussed in comment block above (@ T Instance).
        /// </summary>
        public static void TurnOffQuitFlag()
        {
            _quit = false;
        }
    }
}
