using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToggleButton : SimpleButton {
	
	public List<string> ToggleStates = new List<string>();
	public int CurrentToggleState = 0;
	
	protected override void Start(){
		UpdateMaterial(0);
	}
	
	protected override void OnMouseUpAsButton(){
		if(CurrentState != ButtonStates.Disabled){
			ChangeState(ButtonStates.Hover);
			NextToggleState();
		}
	}
	
	protected virtual void SetToggleStates(){
		
	}
	
	protected virtual string GetCurrentState(){
		return ToggleStates[CurrentToggleState];
	}
	
	protected virtual void NextToggleState(){
		//print("Go to the next toggle state");
		CurrentToggleState = CurrentToggleState >= ToggleStates.Count - 1 ? 0 : CurrentToggleState + 1;
	}
}
