using UnityEngine;
using System.Collections;
namespace fi.tamk.game.theone.utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /**
         * Singleton instance of the class.
         */
        private static T _instance;

        /**
         * Mutual-exclusion lock.
         */
        private static object _lock = new object();

        /**
         * Aplication quit flag.
         */
        private static bool _quit = false;

        /**
         * Returns, and creates if needed, reference to the singleten object.
         */
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
                            Disabled
                            */
                    } else
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

        /**
         * Sets "flag" to signal application is quitting.
         */
        public void OnDestroy()
        {
            _quit = true;
        }

        public static void TurnOffQuitFlag()
        {
            _quit = false;
        }
    }
}
