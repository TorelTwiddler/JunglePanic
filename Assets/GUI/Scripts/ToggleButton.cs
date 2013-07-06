using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToggleButton : ButtonBase {
	
	public List<string> ToggleStates = new List<string>();
	public int CurrentToggleState = 0;
	
	protected override void OnButtonClicked(){
		NextToggleState();
	}
	
	protected virtual void SetToggleStates(){
		
	}
	
	protected virtual string GetCurrentState(){
		return ToggleStates[CurrentToggleState];
	}
	
	protected virtual void NextToggleState(){
		//print("Go to the next toggle state");
		CurrentToggleState = CurrentToggleState >= ToggleStates.Count - 1 ? 0 : CurrentToggleState + 1;
		CallbackGameObject.SendMessage("ToggleStateChanged", ToggleStates[CurrentToggleState], SendMessageOptions.DontRequireReceiver);
		if(ButtonText != null){
			ButtonText.text = ToggleStates[CurrentToggleState];
		}
	}
}
