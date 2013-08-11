using UnityEngine;
using System.Collections;

public class PlayerSetup : MonoBehaviour {
	
	private GlobalOptions options;
	//private TextMesh JoinText;
	//private TextMesh RebindText;
	private GameObject RebindBackground;
	private Renderer RebindInstructionsRenderer;
	private GameObject AllButtons;
	public int PlayerIndex;
	private PlayerManager PlayerManager;
	private bool PlayerJoined = false;
	private bool PlayerJoining = false;
	private bool ListeningForKey = false;
	private KeyCode KeyToHold = KeyCode.None;
	private KeyCode AcceptKey = KeyCode.None;
	private float KeyHoldEnd = Mathf.Infinity;
	private bool IsReady = false;
	private string InputSource = "";
	private string[] KeybindActions = new string[4]{"MoveLeft", "MoveRight", "MoveDown", "Jump"};
	private int CurrentKeybindIndex = 0;
	private bool RebindingKeys = false;
	private bool InitialRebindDone = false;
	private Cursor cursor = null;
	//public TextMesh RebindButtonText;
	private int CurrentCharacter = -1;
	private MeshRenderer CharacterPortrait;
	public Texture[] CharacterPortraits = new Texture[4];
	public Texture[] RebindInstructions = new Texture[4];
	private TeamToggle TeamToggleButton;
	private GameObject ProgressMeter;
	
	
	void Awake(){
		PlayerManager = transform.parent.GetComponent<PlayerManager>();
		AllButtons = transform.Find("Buttons").gameObject;
		RebindBackground = transform.Find("RebindBackground").gameObject;
		RebindInstructionsRenderer = transform.Find("RebindBackground/Instructions").renderer;
		ProgressMeter = transform.Find("ProgressMeter").gameObject;
		TeamToggleButton = transform.Find("Buttons/TeamToggle").gameObject.GetComponentInChildren<TeamToggle>();
		//JoinText = transform.Find("JoinText").GetComponent<TextMesh>();
		//RebindText = transform.Find("RebindText").GetComponent<TextMesh>();
		CharacterPortrait = transform.Find("Buttons/CharacterPortrait").GetComponent<MeshRenderer>();
		options = GlobalOptions.Instance;
	}

	// Use this for initialization
	void Start(){
//		RemovePlayer();
	}
	
	public void Initialize(){
		//GlobalOptions options = GlobalOptions.Instance;
		int team = options.GetPlayerTeams()[PlayerIndex];
		if(team >= 0){
			InitPlayerSettings();
			//RebindText.renderer.enabled = false;
			RebindBackground.SetActive(false);
			InputSource = options.GetPlayerInputSource(PlayerIndex);
			AcceptKey = options.GetPlayerConfig(PlayerIndex)["Jump"];
			PlayerManager.LockInputSource(PlayerIndex, InputSource);
			TeamToggleButton.SetToggleState(team);
			CurrentCharacter = options.GetPlayerCharacters()[PlayerIndex];
			SetCharacterPortrait(CurrentCharacter);
			OnPlayerAdded();
			SetMeterProgress(0.0f);
		}
		else{
			RemovePlayer();
		}
	}
	
	// Update is called once per frame
	void Update(){
		if(PlayerJoining){
			if(Input.GetKey(KeyToHold)){
				if(Time.time > KeyHoldEnd){
					AddPlayer();
					KeyToHold = KeyCode.None;
				}
				else{
					SetMeterProgress(1 - (KeyHoldEnd - Time.time));
				}
			}
			else{
				PlayerManager.KeyboardRebinding = false;
				PlayerJoining = false;
				SetMeterProgress(0);
				//KeyToHold = KeyCode.None;
			}
		}
		else if(RebindingKeys){
			if(Input.GetKey(KeyToHold)){
				if(Time.time > KeyHoldEnd){
					BindKey(KeyToHold);
				}
				else{
					SetMeterProgress(1 - (KeyHoldEnd - Time.time));
				}
			}
			else{
				KeyToHold = KeyCode.None;
				ListeningForKey = true;
				SetMeterProgress(0);
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
		}
	}
	
	void OnGUI(){
		if(!ListeningForKey){
			return;
		}
		
		Event e = Event.current;
		if(e.keyCode != KeyCode.None && InputSource == "Keyboard"){
			if(PlayerManager.GetIsKeyAvailable(e.keyCode)){
				KeyToHold = e.keyCode;
				KeyHoldEnd = Time.time + 1.0f;
				ListeningForKey = false;
				//BindKey(e.keyCode);
			}
		}
	}
	
	public void StartPlayerJoin(KeyCode key){
		if(PlayerManager.GetIsKeyAvailable(key)){
			if((int)key < 350){
				PlayerManager.KeyboardRebinding = true;
			}
			enabled = true;
			PlayerJoining = true;
			KeyToHold = key;
			KeyHoldEnd = Time.time + 1.0f;
		}
	}
	
	public bool GetIsSlotAvailable(){
		return !PlayerJoined && !PlayerJoining;
	}
	
	public bool GetIsPlayerJoined(){
		return PlayerJoined;
	}
	
	private void InitPlayerSettings(){
		PlayerJoined = true;
		//JoinText.renderer.enabled = false;
		//RebindText.renderer.enabled = true;
		RebindBackground.SetActive(true);
		//AllButtons.SetActive(true);
		PlayerJoining = false;
	}
	
	public void AddPlayer(){
		//options.SetPlayerTeam(PlayerIndex, PlayerIndex);
		TeamToggleButton.SetToggleState(PlayerIndex);
		InitPlayerSettings();
		PlayerManager.LockInputSource(PlayerIndex, KeyToHold);
		//GlobalOptions options = GlobalOptions.Instance;
		if((int)KeyToHold >= 350){
			InputSource = KeyToHold.ToString().Substring(0, 9);
		}
		else{
			InputSource = "Keyboard";
		}
		CycleCharacter(1);
		StartRebind();
	}
	
	public void OnPlayerAdded(){
		//InputSource is "Joystick1", "Joystick2", etc.
		//AcceptKey is the KeyCode for accepting stuff
		if (InputSource != "Keyboard") {
			cursor = ((Instantiate(Resources.Load("Cursor"), Camera.main.transform.position, Quaternion.identity))
					as GameObject).GetComponent<Cursor>();
			cursor.Setup(InputSource, AcceptKey);
		}
	}
	
	public void RemovePlayer(){
		AllButtons.SetActive(false);
		ProgressMeter.SetActive(true);
		SetMeterProgress(0.0f);
		enabled = false;
		//RebindText.renderer.enabled = false;
		RebindBackground.SetActive(false);
		//JoinText.renderer.enabled = true;
		PlayerJoined = false;
		PlayerJoining = false;
		//print("releasing input source " + KeyToHold.ToString());
		PlayerManager.ReleaseInputSource(PlayerIndex, AcceptKey);
		//GlobalOptions options = GlobalOptions.Instance;
		foreach(KeyCode key in options.GetPlayerConfig(PlayerIndex).Values){
			PlayerManager.UnlockKey(key);
		}
		//PlayerManager.UnlockKeys(options.GetPlayerConfig(PlayerIndex).Values.ToList<KeyCode>());
		InputSource = "";
		options.SetPlayerTeam(PlayerIndex, -1);
		InitialRebindDone = false;
		OnPlayerRemoved();
	}
	
	public void OnPlayerRemoved(){
		if (cursor != null) {
			cursor.Remove();
		}
	}
	
	public void ToggleStateChanged(string newState){
		//GlobalOptions options = GlobalOptions.Instance;
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
			//if(!PlayerManager.KeyboardRebinding){
				//GlobalOptions options = GlobalOptions.Instance;
				foreach(KeyCode key in options.GetPlayerConfig(PlayerIndex).Values){
					PlayerManager.UnlockKey(key);
				}
				PlayerManager.KeyboardRebinding = true;
				RebindingKeys = true;
				AllButtons.SetActive(false);
				ProgressMeter.SetActive(true);
				SetMeterProgress(0.0f);
				//RebindText.renderer.enabled = true;
				RebindBackground.SetActive(true);
				ListeningForKey = true;
				CurrentKeybindIndex = 0;
				//RebindText.text = InputSource + "\n" + KeybindActions[CurrentKeybindIndex];
				SetRebindInstructions(CurrentKeybindIndex, InputSource);
			//}
		}
		else{
			//GlobalOptions options = GlobalOptions.Instance;
			foreach(KeyCode key in options.GetPlayerConfig(PlayerIndex).Values){
				PlayerManager.UnlockKey(key);
			}
			RebindingKeys = true;
			AllButtons.SetActive(false);
			ProgressMeter.SetActive(true);
			SetMeterProgress(0.0f);
			//RebindText.renderer.enabled = true;
			RebindBackground.SetActive(true);
			ListeningForKey = true;
			CurrentKeybindIndex = KeybindActions.Length-1;
			//RebindText.text = InputSource + "\n" + KeybindActions[CurrentKeybindIndex];
			SetRebindInstructions(CurrentKeybindIndex, InputSource);
		}
	}
	
	public void EndRebind(){
		if(InputSource == "Keyboard"){
			PlayerManager.KeyboardRebinding = false;
		}
		RebindingKeys = false;
		AllButtons.SetActive(true);
		ProgressMeter.SetActive(false);
		//RebindText.renderer.enabled = false;
		RebindBackground.SetActive(false);
		ListeningForKey = false;
		if(!InitialRebindDone){
			InitialRebindDone = true;
			OnPlayerAdded();
		}
		else if(InputSource != "Keyboard"){
			cursor.SetAcceptKey(AcceptKey);
		}
	}
	
	public void BindKey(KeyCode key){
		//GlobalOptions options = GlobalOptions.Instance;
		options.SetKeyConfig(PlayerIndex, KeybindActions[CurrentKeybindIndex], key);
		PlayerManager.LockKey(key);
		CurrentKeybindIndex++;
		if(CurrentKeybindIndex < KeybindActions.Length){
			//RebindText.text = InputSource + "\n" + KeybindActions[CurrentKeybindIndex];
			SetRebindInstructions(CurrentKeybindIndex, InputSource);
		}
		else{
			AcceptKey = KeyToHold;
			//RebindButtonText.text = "Rebind";
			EndRebind();
		}
		KeyToHold = KeyCode.None;
	}
	
	private void SetRebindInstructions(int index, string inputSource){
		TextMesh inputText = RebindBackground.GetComponentInChildren<TextMesh>();
		inputText.text = inputSource;
		Material mat = new Material(RebindInstructionsRenderer.material);
		mat.mainTexture = RebindInstructions[index];
		RebindInstructionsRenderer.sharedMaterial = mat;
	}
	
	public int GetCurrentCharacter(){
		return CurrentCharacter;
	}
	
	public void CycleCharacter(int direction){
		int index = CurrentCharacter;
		CurrentCharacter = -1;
		CurrentCharacter = PlayerManager.GetNextAvailableCharacter(index, direction);
		//GlobalOptions options = GlobalOptions.Instance;
		options.SetPlayerCharacter(PlayerIndex, CurrentCharacter);
		SetCharacterPortrait(CurrentCharacter);
	}
	
	public void SetCharacterPortrait(int character){
		Material newMat = new Material(CharacterPortrait.material);
		newMat.mainTexture = CharacterPortraits[character];
		CharacterPortrait.sharedMaterial = newMat;
	}
	
	public void SetMeterProgress(float value){
		ProgressMeter.transform.localScale = new Vector3(value * 7, 1, 1);
	}
}
