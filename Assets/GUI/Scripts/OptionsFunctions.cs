using UnityEngine;
using System.Collections;

public class OptionsFunctions : MonoBehaviour {
	
	private MainMenuFunctions MainMenuFunctions;
	
	void Awake(){
		MainMenuFunctions = GameObject.Find("MainMenu").GetComponent<MainMenuFunctions>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
