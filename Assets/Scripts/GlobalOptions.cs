using UnityEngine;
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
		//LoadPlayerConfig();
	}
	
	public struct ControllerConfig{
		public string MovementAxis, JumpButton;
	};
	
	public List<Dictionary<string,KeyCode>> PlayerConfigs = new List<Dictionary<string,KeyCode>>();
	
	public void SetKeyConfig(int playerIndex, string action, KeyCode newKey){
		//Debug.Log("Player " + playerIndex + "s " + action + " changed to " + newKey.ToString());
		PlayerConfigs[playerIndex][action] = newKey;
	}
	
	public Dictionary<string,KeyCode> GetKeyConfig(int playerIndex){
		return PlayerConfigs[playerIndex];
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
