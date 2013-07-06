using UnityEngine;
using System.Collections;

public class PlayerSetup : MonoBehaviour {
	
	public TextMesh JoinText;
	public GameObject AllButtons;
	public int PlayerIndex;
	private PlayerManager PlayerManager;
	private bool PlayerJoined = false;
	private bool PlayerJoining = false;
	private bool ListeningForKey = false;
	private KeyCode KeyToHold = KeyCode.None;
	private float KeyHoldEnd = Mathf.Infinity;
	private bool IsReady = false;
	private string InputSource = "";
	private string[] KeybindActions = new string[4]{"MoveLeft", "MoveRight", "MoveDown", "Jump"};
	private int CurrentKeybindIndex = 0;
	private bool RebindingKeys = false;
	public TextMesh RebindButtonText;
	
	void Awake(){
		PlayerManager = transform.parent.GetComponent<PlayerManager>();
	}

	// Use this for initialization
	void Start(){
		RemovePlayer();
	}
	
	// Update is called once per frame
	void Update(){
		if(PlayerJoining){
			if(Input.GetKey(KeyToHold)){
				if(Time.time > KeyHoldEnd){
					AddPlayer();
				}
			}
			else{
				//KeyToHold = KeyCode.None;
			}
		}
		else if(RebindingKeys){
			if(Input.GetKey(KeyToHold)){
				if(Time.time > KeyHoldEnd){
					BindKey(KeyToHold);
				}
			}
			else{
				KeyToHold = KeyCode.None;
				ListeningForKey = true;
			}
		}
		
		if(ListeningForKey && InputSource != "Keyboard"){
			int joystickNumber = int.Parse(InputSource[8].ToString());
			for(int i = 0; i < 20; i++){
				int keyInt = 330 + (20 * joystickNumber) + i;
				if(Input.GetKeyDown((KeyCode)keyInt)){
					KeyToHold = (KeyCode)keyInt;
					KeyHoldEnd = Time.time + 1.0f;
					ListeningForKey = false;
				}
			}
			
			/*string keyString = InputSource + "Button";
			for(int i = 0; i < 20; i++){
				if(Input.GetKeyDown((keyString + i).ToLower())){
					KeyToHold = (KeyCode)System.Enum.Parse(typeof(KeyCode), (keyString + i).ToLower());
					KeyHoldEnd = Time.time + 1.0f;
					ListeningForKey = false;
				}
			}*/
		}
	}
	
	void OnGUI(){
		if(!ListeningForKey){
			return;
		}
		
		Event e = Event.current;
		if(e.keyCode != KeyCode.None){
			KeyToHold = e.keyCode;
			KeyHoldEnd = Time.time + 1.0f;
			ListeningForKey = false;
			//BindKey(e.keyCode);
		}
	}
	
	public void StartPlayerJoin(KeyCode key){
		enabled = true;
		PlayerJoining = true;
		KeyToHold = key;
		KeyHoldEnd = Time.time + 1.0f;
	}
	
	public bool GetIsSlotAvailable(){
		return !PlayerJoined && !PlayerJoining;
	}
	
	public bool GetIsPlayerJoined(){
		return PlayerJoined;
	}
	
	public void AddPlayer(){
		PlayerJoined = true;
		JoinText.renderer.enabled = false;
		AllButtons.SetActive(true);
		PlayerJoining = false;
		PlayerManager.LockInputSource(PlayerIndex, KeyToHold);
		GlobalOptions options = GlobalOptions.Instance;
		options.SetPlayerTeam(PlayerIndex, 0);
		if((int)KeyToHold >= 350){
			InputSource = KeyToHold.ToString().Substring(0, 9);
		}
		else{
			InputSource = "Keyboard";
		}
	}
	
	public void RemovePlayer(){
		AllButtons.SetActive(false);
		enabled = false;
		JoinText.renderer.enabled = true;
		PlayerJoined = false;
		PlayerJoining = false;
		//print("releasing input source " + KeyToHold.ToString());
		PlayerManager.ReleaseInputSource(PlayerIndex, KeyToHold);
		InputSource = "";
		GlobalOptions options = GlobalOptions.Instance;
		options.SetPlayerTeam(PlayerIndex, -1);
	}
	
	public void ToggleStateChanged(string newState){
		GlobalOptions options = GlobalOptions.Instance;
		switch(newState){
			case "Ready!":
				IsReady = true;
				PlayerManager.CheckIfAllPlayersReady();
				break;
			case "Ready?":
				IsReady = false;
				break;
			case "Red":
				options.SetPlayerTeam(PlayerIndex, 0);
				break;
			case "Blue":
				options.SetPlayerTeam(PlayerIndex, 1);
				break;
			case "Green":
				options.SetPlayerTeam(PlayerIndex, 2);
				break;
			case "White":
				options.SetPlayerTeam(PlayerIndex, 3);
				break;
			default:
				break;
		}
	}
	
	public bool GetIsReady(){
		return IsReady;
	}
	
	public void StartRebind(){
		if(InputSource == "Keyboard"){
			if(!PlayerManager.KeyboardRebinding){
				PlayerManager.KeyboardRebinding = true;
				RebindingKeys = true;
				ListeningForKey = true;
				CurrentKeybindIndex = 0;
				RebindButtonText.text = KeybindActions[CurrentKeybindIndex];
			}
		}
		else{
			RebindingKeys = true;
			ListeningForKey = true;
			CurrentKeybindIndex = KeybindActions.Length-1;
			RebindButtonText.text = KeybindActions[CurrentKeybindIndex];
		}
	}
	
	public void EndRebind(){
		PlayerManager.KeyboardRebinding = false;
		RebindingKeys = false;
		ListeningForKey = false;
	}
	
	public void BindKey(KeyCode key){
		GlobalOptions options = GlobalOptions.Instance;
		options.SetKeyConfig(PlayerIndex, KeybindActions[CurrentKeybindIndex], key);
		CurrentKeybindIndex++;
		KeyToHold = KeyCode.None;
		if(CurrentKeybindIndex < KeybindActions.Length){
			RebindButtonText.text = KeybindActions[CurrentKeybindIndex];
		}
		else{
			RebindButtonText.text = "Rebind";
			EndRebind();
		}
	}
}
