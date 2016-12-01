using UnityEngine;
using System.Collections;

public class LockLevel : MonoBehaviour {

    public static int levels = 4;

    private int levelIndex;
    
    void Start()
    {
        PlayerPrefs.DeleteAll();
        LockLevels();
    }

    void LockLevels()
    {
        for (int i = 0; i < levels; i++)
        {
            levelIndex = (i + 1);

            if (!PlayerPrefs.HasKey("level" + levelIndex.ToString()))
            {
                PlayerPrefs.SetInt("level" + levelIndex.ToString(), 0);
            }
        }
    }
}
