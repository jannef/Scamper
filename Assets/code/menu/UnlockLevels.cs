using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UnlockLevels : MonoBehaviour {

    protected string currentLevel;
    protected int _levelIndex;

	void Start()
    {
        currentLevel = SceneManager.GetActiveScene().name;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Unlock();
        }
    }

    protected void Unlock()
    {
        for (int i = 0; i < LockLevel.levels; i++)
        {
            if (currentLevel == "Day" + (i + 1).ToString() + "_playerfriendly")
            {
                _levelIndex = (i + 1);
                PlayerPrefs.SetInt("level" + _levelIndex.ToString(), 1);
            }
        }
    }
}
