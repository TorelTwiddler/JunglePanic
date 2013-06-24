using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputToggleButton : ToggleButton {
	
	private ControlsFunctions ControlsFunctions;
	private TextMesh InputText;
	
	void Awake(){
		ControlsFunctions = GameObject.Find("ControlsMenu").GetComponent<ControlsFunctions>();
		InputText = transform.GetComponentInChildren<TextMesh>();
	}
	
	protected override void SetToggleStates(){
		List<string> inputSources = ControlsFunctions.GetInputSources();
		ToggleStates = inputSources;
	}

	protected override void NextToggleState(){
		SetToggleStates();
		base.NextToggleState();
		InputText.text = ToggleStates[CurrentToggleState];
	}
}
