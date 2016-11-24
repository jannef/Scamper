using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using fi.tamk.game.theone.menu;

public class test : MonoBehaviour {
    public LevelLoadController Control;
    private Text _text;

	// Use this for initialization
	void Start () {
        _text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        _text.text = string.Format("{0}", Control._saveData.RandomTest);
	}
}
