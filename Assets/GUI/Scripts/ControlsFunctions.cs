using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlsFunctions : MonoBehaviour {
	
	private MenuController MenuController;
	private string ConfigListenerAction;
	
	void Awake(){
		MenuController = GameObject.Find("MenuController").GetComponent<MenuController>();
	}

	// Use this for initialization
	void Start () {
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		Event e = Event.current;
		if(e.keyCode != KeyCode.None){
			BindKey(e.keyCode);
			enabled = false;
		}
	}
	
	public List<string> GetInputSources(){
		List<string> inputSources = new List<string>(Input.GetJoystickNames());
		inputSources.Insert(0, "Keyboard");
		return inputSources;
	}
	
	public void StartKeybindListener(string action){
		ConfigListenerAction = action;
		enabled = true;
	}
	
	public void BindKey(KeyCode key){
		GlobalOptions options = GlobalOptions.Instance;
		//print(ConfigListenerAction + " bound to " + key);
		string[] configAction = ConfigListenerAction.Split('_');
		if(configAction[0] == "player1"){
			options.SetKeyConfig(0, configAction[1], key);
		}
		else if(configAction[0] == "player2"){
			options.SetKeyConfig(1, configAction[1], key);
		}
	}
}
