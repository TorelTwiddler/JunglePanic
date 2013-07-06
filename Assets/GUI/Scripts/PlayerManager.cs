using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {
	
	private MenuController MenuController;
	private PlayerSetup[] Players;
	private bool ShouldStartPlayerJoin = false;
	private KeyCode JoinKey = KeyCode.None;
	private List<string> LockedInputs = new List<string>();
	
	void Awake(){
		Players = GetComponentsInChildren<PlayerSetup>();
		MenuController = GameObject.Find("MenuController").GetComponent<MenuController>();
	}

	// Use this for initialization
	void Start () {
		enabled = false;
		//print(Input.GetJoystickNames()[0]);
	}
	
	// Update is called once per frame
	void Update () {
		if(ShouldStartPlayerJoin){
			ShouldStartPlayerJoin = false;
			StartPlayerJoin(JoinKey);
			//JoinKey = KeyCode.None;
		}
		
		for(int i = 350; i < 430; i++){
			if(Input.GetKeyDown((KeyCode)i)){
				//print(((KeyCode)i).ToString());
				string joystickNumber = ((KeyCode)i).ToString().Substring(0, 9);
				if(!LockedInputs.Contains(joystickNumber)){
					StartPlayerJoin((KeyCode)i);
				}
			}
		}
	}
	
	void OnGUI(){
		Event e = Event.current;
		if(e.keyCode != KeyCode.None && e.keyCode != JoinKey){
			//print(e.keyCode.ToString());
			JoinKey = e.keyCode;
			ShouldStartPlayerJoin = true;
		}
		/*if(Input.inputString != ""){
			print(Input.inputString);
		}*/
	}
	
	public void StartPlayerJoin(KeyCode key){
		for(int i = 0; i < Players.Length; i++){
			if(Players[i].GetIsSlotAvailable()){
				Players[i].StartPlayerJoin(key);
				break;
			}
		}
	}
	
	public void LockInputSource(int index, KeyCode key){
		GlobalOptions options = GlobalOptions.Instance;
		if((int)key >= 350){
			string joystickNumber = key.ToString().Substring(0, 9);
			if(!LockedInputs.Contains(joystickNumber)){
				LockedInputs.Add(joystickNumber);
				options.SetPlayerInputSource(index, joystickNumber);
			}
		}
		else{
			options.SetPlayerInputSource(index, "Keyboard");
		}
	}
	
	public void ReleaseInputSource(int index, KeyCode key){
		GlobalOptions options = GlobalOptions.Instance;
		if((int)key >= 350){
			string joystickNumber = key.ToString().Substring(0, 9);
			if(LockedInputs.Contains(joystickNumber)){
				LockedInputs.Remove(joystickNumber);
			}
		}
		options.SetPlayerInputSource(index, "");
	}
	
	public void CheckIfAllPlayersReady(){
		for(int i = 0; i < Players.Length; i++){
			if(Players[i].GetIsPlayerJoined() && !Players[i].GetIsReady()){
				return;
			}
		}
		
		MenuController.StartGame();
	}
	
	public string GetInputSource(KeyCode key){
		return "keyboard";
	}
}
