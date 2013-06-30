using UnityEngine;
using System.Collections;

public class SimpleButton : ButtonBase {
	
	public string HoverFunctionName = "";
	public ButtonParameter HoverFunctionParameter = null;
	public string ClickedFunctionName = "";
	public ButtonParameter ClickedFunctionParameter = null;
	
	protected override void OnButtonHover(){
		if(HoverFunctionName != "" && CallbackGameObject != null){
			CallbackGameObject.SendMessage(HoverFunctionName, HoverFunctionParameter.GetValue(), SendMessageOptions.DontRequireReceiver);
		}
	}
	
	protected override void OnButtonClicked(){
		if(ClickedFunctionName != ""){
			CallbackGameObject.SendMessage(ClickedFunctionName, ClickedFunctionParameter.GetValue(), SendMessageOptions.DontRequireReceiver);
		}
	}
}