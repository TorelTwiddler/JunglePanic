using UnityEngine;
using System.Collections;

public class TeamToggle : ToggleButton {
	
	
	protected override void ChangeState(ButtonStates newState){
	}
	
	/*protected override void NextToggleState(){
		//print("Go to the next toggle state");
		base.NextToggleState();
		UpdateMaterial(CurrentToggleState);
	}*/
	
	public override void SetToggleState(int index){
		base.SetToggleState(index);
		UpdateMaterial(index);
	}
}
