using UnityEngine;
using System.Collections;

public class PlayerSetup : MonoBehaviour {
	
	public TextMesh JoinText;
	public GameObject AllButtons;
	private PlayerManager PlayerManager;
	private bool PlayerJoined = false;
	private bool PlayerJoining = false;
	private bool ListeningForKey = false;
	private KeyCode KeyToHold = KeyCode.None;
	private float KeyHoldEnd = Mathf.Infinity;
	private bool IsReady = false;
	
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
					PlayerJoined = true;
					JoinText.renderer.enabled = false;
					AllButtons.SetActive(true);
				}
			}
			else{
				KeyToHold = KeyCode.None;
			}
		}
	}
	
	void OnGUI(){
		if(!ListeningForKey){
			return;
		}
		
		Event e = Event.current;
		if(e.keyCode != KeyCode.None){
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
	
	public void RemovePlayer(){
		AllButtons.SetActive(false);
		enabled = false;
		JoinText.renderer.enabled = true;
		PlayerJoined = false;
		PlayerJoining = false;
	}
	
	public void ToggleStateChanged(string newState){
		switch(newState){
			case "Ready!":
				IsReady = true;
				PlayerManager.CheckIfAllPlayersReady();
				break;
			case "Ready?":
				IsReady = false;
				break;
			default:
				break;
		}
	}
	
	public bool GetIsReady(){
		return IsReady;
	}
	
	/*public void BindKey(KeyCode key){
		GlobalOptions options = GlobalOptions.Instance;
		//print(ConfigListenerAction + " bound to " + key);
		string[] configAction = ConfigListenerAction.Split('_');
		if(configAction[0] == "player1"){
			options.SetKeyConfig(0, configAction[1], key);
		}
		else if(configAction[0] == "player2"){
			options.SetKeyConfig(1, configAction[1], key);
		}
		
		KeyText.text = key.ToString();
	}*/
}
