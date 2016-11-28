using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


namespace fi.tamk.game.theone.menu
{

    public class LoadOnClick : MonoBehaviour
    {

        public void LoadScene(int level)
        {
            SceneManager.LoadScene(level);
        }
    }
}