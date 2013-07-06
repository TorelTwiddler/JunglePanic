using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GlobalOptions{
	
	private static GlobalOptions instance = new GlobalOptions();
	public static GlobalOptions Instance{
		get{
			return instance;
		}
	}
	
	GlobalOptions(){
		PlayerConfigs.Add(new Dictionary<string,KeyCode>(){{"MoveLeft",KeyCode.A},{"MoveRight",KeyCode.D},{"MoveDown",KeyCode.S},{"Jump",KeyCode.Space}});
		PlayerConfigs.Add(new Dictionary<string,KeyCode>(){{"MoveLeft",KeyCode.LeftArrow},{"MoveRight",KeyCode.RightArrow},{"MoveDown",KeyCode.DownArrow},{"Jump",KeyCode.KeypadEnter}});
		PlayerConfigs.Add(new Dictionary<string,KeyCode>(){{"MoveLeft",KeyCode.Z},{"MoveRight",KeyCode.C},{"MoveDown",KeyCode.X},{"Jump",KeyCode.V}});
		PlayerConfigs.Add(new Dictionary<string,KeyCode>(){{"MoveLeft",KeyCode.Q},{"MoveRight",KeyCode.E},{"MoveDown",KeyCode.W},{"Jump",KeyCode.R}});
		
		TeamsInGame[0] = true;	//Red team is automatically included
		//LoadPlayerConfig();
	}
	
	public struct ControllerConfig{
		public string MovementAxis, JumpButton;
	};
	
	public List<Dictionary<string,KeyCode>> PlayerConfigs = new List<Dictionary<string,KeyCode>>();
	//team order is Red, Blue, Green, White
	public bool[] TeamsInGame = new bool[4];
	public int[] PlayerTeamAssignment = new int[4]{-1, -1, -1, -1};	//default everyone to no team
	//this will be "Keyboard", "Joystick1", "Joystick2", "Joystick3", "Joystick4", or ""
	public string[] PlayerInputSources = new string[4];
	
	public void SetKeyConfig(int playerIndex, string action, KeyCode newKey){
		//Debug.Log("Player " + playerIndex + "s " + action + " changed to " + newKey.ToString());
		PlayerConfigs[playerIndex][action] = newKey;
	}
	
	public Dictionary<string,KeyCode> GetPlayerConfig(int playerIndex){
		return PlayerConfigs[playerIndex];
	}
	
	public void SetPlayerTeam(int index, int team){
		PlayerTeamAssignment[index] = team;
		int indexOf = -1;
		for(int i = 0; i < 4; i++){
			indexOf = Array.IndexOf(PlayerTeamAssignment, i);
			TeamsInGame[i] = indexOf >= 0;
		}
		/*foreach(bool element in TeamsInGame){
			Debug.Log(element.ToString());
		}*/
		//Debug.Log(string.Join(',', TeamsInGame));
	}
	
	public int[] GetPlayerTeams(){
		return PlayerTeamAssignment;
	}
	
	public void SetPlayerInputSource(int index, string source){
		PlayerInputSources[index] = source;
	}
	
	public string GetPlayerInputSource(int index){
		if(index < 0){
			return "";
		}
		return PlayerInputSources[index];
	}
	
	/*public void SavePlayerConfigs(){
		for(int i = 0; i < PlayerConfigs.Count; i++){
			Dictionary<string,KeyCode> config = PlayerConfigs[i];
			foreach(KeyValuePair<string,KeyCode> pair in config){
				string key = "player_" + i + "_" + pair.Key;
				Debug.Log("saving " + key);
				PlayerPrefs.SetString(key, pair.Value.ToString());
			}
		}
	}
	
	public void LoadPlayerConfig(){
		Dictionary<string,KeyCode> config = new Dictionary<string,KeyCode>(){{"MoveLeft",KeyCode.A},{"MoveRight",KeyCode.D},{"Jump",KeyCode.Space}};
		for(int i = 0; i < 4; i++){
			foreach(KeyValuePair<string,KeyCode> pair in config){
				string key = "player_" + i + "_" + pair.Key;
				if(PlayerPrefs.HasKey(key)){
					
				}
			}
		}
	}*/
}
