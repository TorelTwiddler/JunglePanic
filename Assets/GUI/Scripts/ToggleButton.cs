using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToggleButton : SimpleButton {
	
	public List<string> ToggleStates = new List<string>();
	public string CurrentToggleState;
	
	protected override void Start(){
		UpdateMaterial(0);
	}
	
	protected override void OnMouseUpAsButton(){
		if(CurrentState != ButtonStates.Disabled){
			ChangeState(ButtonStates.Hover);
			if(ClickedFunctionName != ""){
				//CallbackGameObject.SendMessage(ClickedFunctionName, ClickedFunctionParameter.GetValue(), SendMessageOptions.DontRequireReceiver);
				NextToggleState();
			}
		}
	}
	
	protected virtual void NextToggleState(){
		
	}
}
