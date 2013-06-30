using UnityEngine;
using System.Collections;

public class KeyConfigButton : SimpleButton {

	private ControlsFunctions ControlsFunctions;
	private TextMesh KeyText;
	private string ConfigListenerAction;
	
	void Awake(){
		ControlsFunctions = GameObject.Find("ControlsMenu").GetComponent<ControlsFunctions>();
		KeyText = transform.GetComponentInChildren<TextMesh>();
	}
	
	protected override void Start(){
		base.Start();
		enabled = false;
		//print(Input.GetKey(KeyCode.Joystick1Button0));
		//print(Input.GetAxis("Joystick 1 X Axis"));
	}
	
	void OnGUI(){
		Event e = Event.current;
		if(e.keyCode != KeyCode.None){
			BindKey(e.keyCode);
			enabled = false;
		}
	}
	
	protected override void OnButtonClicked(){
		if(ClickedFunctionName != ""){
			//CallbackGameObject.SendMessage(ClickedFunctionName, ClickedFunctionParameter.GetValue(), SendMessageOptions.DontRequireReceiver);
			StartKeybindListener(ClickedFunctionParameter.String);
		}
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
		
		KeyText.text = key.ToString();
	}
	
}
