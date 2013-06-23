using UnityEngine;
using System.Collections;

public class OptionsFunctions : MonoBehaviour {
	
	private MenuController MenuController;
	
	void Awake(){
		MenuController = GameObject.Find("MenuController").GetComponent<MenuController>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
