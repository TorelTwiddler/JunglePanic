using UnityEngine;
using System.Collections;

public class PlayerToggleButton : ToggleButton {

	protected override void NextToggleState(){
		base.NextToggleState();
		ButtonText.text = ToggleStates[CurrentToggleState];
	}
}
