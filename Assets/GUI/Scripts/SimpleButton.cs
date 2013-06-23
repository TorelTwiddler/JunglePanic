using UnityEngine;
using System.Collections;

[System.Serializable]
public class ButtonParameter{
	public enum ParameterType{
		Boolean,
		Integer,
		Float,
		String
	};
	
	public ParameterType Type;
	public bool Boolean;
	public int Integer;
	public float Float;
	public string String;
	
	public object GetValue(){
		switch(Type){
			case ParameterType.Boolean:
				return Boolean;
			case ParameterType.Integer:
				return Integer;
			case ParameterType.Float:
				return Float;
			case ParameterType.String:
				return String;
			default:
				return null;
		}
	}
}

public class SimpleButton : MonoBehaviour {
	
	public enum ButtonStates{
		Normal = 0,
		Hover = 1,
		Active = 2,
		Disabled = 3
	};
	
	private ButtonStates CurrentState = ButtonStates.Normal;
	public Material[] ButtonMaterials = new Material[4];
	private Material _material = null;
	public Texture ButtonTexture = null;
	public GameObject CallbackGameObject = null;
	public string HoverFunctionName = "";
	public ButtonParameter HoverFunctionParameter = null;
	public string ClickedFunctionName = "";
	public ButtonParameter ClickedFunctionParameter = null;

	// Use this for initialization
	void Start(){
		if(CallbackGameObject == null){
			print("The button " + gameObject.name + " serves no purpose.");
			enabled = false;
		}
		UpdateMaterial(0);
	}
	
	void OnMouseEnter(){
		if(CurrentState != ButtonStates.Disabled){
			ChangeState(ButtonStates.Hover);
			if(HoverFunctionName != ""){
				CallbackGameObject.SendMessage(HoverFunctionName, HoverFunctionParameter.GetValue(), SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	void OnMouseExit(){
		if(CurrentState != ButtonStates.Disabled){
			ChangeState(ButtonStates.Normal);
		}
	}
	
	void OnMouseDown(){
		if(CurrentState != ButtonStates.Disabled){
			ChangeState(ButtonStates.Active);
		}
	}
	
	void OnMouseUpAsButton(){
		if(CurrentState != ButtonStates.Disabled){
			ChangeState(ButtonStates.Hover);
			if(ClickedFunctionName != ""){
				CallbackGameObject.SendMessage(ClickedFunctionName, ClickedFunctionParameter.GetValue(), SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	private void ChangeState(ButtonStates newState){
		CurrentState = newState;
		UpdateMaterial((int)newState);
	}
	
	private void UpdateMaterial(int matIndex){
		_material = new Material(ButtonMaterials[matIndex]);
		_material.mainTexture = ButtonTexture;
		renderer.sharedMaterial = _material;
	}
	
	public ButtonStates GetCurrentState(){
		return CurrentState;
	}
	
	public void SetButtonEnabled(bool value){
		if(value){
			ChangeState(ButtonStates.Normal);
		}
		else{
			ChangeState(ButtonStates.Disabled);
		}
	}
}