using UnityEngine;
using System.Collections;

public class PlayerToggleButton : ToggleButton {
	
	private TextMesh PlayerText;
	
	void Awake(){
		PlayerText = transform.GetComponentInChildren<TextMesh>();
	}

	protected override void NextToggleState(){
		base.NextToggleState();
		PlayerText.text = ToggleStates[CurrentToggleState];
	}
}
