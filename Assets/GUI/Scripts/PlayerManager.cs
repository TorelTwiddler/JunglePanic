using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {
	
	private MenuController MenuController;
	private PlayerSetup[] Players;
	private bool ShouldStartPlayerJoin = false;
	private KeyCode JoinKey = KeyCode.None;
	private List<string> LockedInputs = new List<string>();
	private List<KeyCode> LockedKeys = new List<KeyCode>();
	public bool KeyboardRebinding = false;
	public int NumberOfCharacters = 4;
	
	void Awake(){
		Players = GetComponentsInChildren<PlayerSetup>();
		MenuController = GameObject.Find("MenuController").GetComponent<MenuController>();
		enabled = false;
	}

	// Use this for initialization
	void Start () {
//		enabled = false;
		//print(Input.GetJoystickNames()[0]);
	}
	
	public void Initialize(){
		enabled = true;
		foreach(PlayerSetup player in Players){
			player.Initialize();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(ShouldStartPlayerJoin && !KeyboardRebinding){
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
			if(!KeyboardRebinding){
				//print(e.keyCode.ToString());
				JoinKey = e.keyCode;
				ShouldStartPlayerJoin = true;
			}
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
			LockInputSource(index, joystickNumber);
		}
		else{
			options.SetPlayerInputSource(index, "Keyboard");
		}
	}
	
	public void LockInputSource(int index, string source){
		GlobalOptions options = GlobalOptions.Instance;
		if(source == "Keyboard"){
			options.SetPlayerInputSource(index, source);
			return;
		}
		
		if(!LockedInputs.Contains(source)){
			LockedInputs.Add(source);
			options.SetPlayerInputSource(index, source);
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
		
		GlobalOptions options = GlobalOptions.Instance;
		options.SetAlreadyInitializedPlayers(true);
		StartGame();
	}
	
	public string GetInputSource(KeyCode key){
		return "keyboard";
	}
	
	public bool GetIsCharacterAvailable(int index){
		for(int i = 0; i < Players.Length; i++){
			if(index == Players[i].GetCurrentCharacter()){
				return false;
			}
		}
		
		return true;
	}
	
	public int GetNextAvailableCharacter(int index, int direction){
		int newIndex = (index + direction) % NumberOfCharacters;
		newIndex = newIndex < 0 ? NumberOfCharacters-1 : newIndex;
		if(GetIsCharacterAvailable(newIndex)){
			return newIndex;
		}
		else{
			return GetNextAvailableCharacter(newIndex, direction);
		}
	}
	
	public void LockKey(KeyCode key){
		if(!LockedKeys.Contains(key)){
			LockedKeys.Add(key);
		}
	}
	
	public void UnlockKey(KeyCode key){
		//foreach(KeyCode key in keys){
			if(LockedKeys.Contains(key)){
				LockedKeys.Remove(key);
			}
		//}
	}
	
	public bool GetIsKeyAvailable(KeyCode key){
		foreach(KeyCode lockedKey in LockedKeys){
			if(key == lockedKey){
				return false;
			}
		}
		
		return true;
	}
	
	public void StartGame(){
		MenuController.StartGame();
	}
}
