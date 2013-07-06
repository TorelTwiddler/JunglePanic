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

public class ButtonBase : MonoBehaviour {

	public enum ButtonStates{
		Normal = 0,
		Hover = 1,
		Active = 2,
		Disabled = 3
	};
	
	protected ButtonStates CurrentState = ButtonStates.Normal;
	public Material[] ButtonMaterials = new Material[4];
	private Material _material = null;
	public Texture ButtonTexture = null;
	public GameObject CallbackGameObject = null;
	public TextMesh ButtonText;
	
	void Awake(){
		TextMesh textMesh = GetComponentInChildren<TextMesh>();
		if(textMesh){
			ButtonText = textMesh;
		}
	}

	// Use this for initialization
	protected virtual void Start(){
		if(CallbackGameObject == null){
			print("The button " + gameObject.name + " serves no purpose.");
			enabled = false;
		}
		UpdateMaterial(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseEnter(){
		if(CurrentState != ButtonStates.Disabled){
			ChangeState(ButtonStates.Hover);
			OnButtonHover();
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
			OnButtonClicked();
		}
	}
	
	protected virtual void OnButtonHover(){
		
	}
	
	protected virtual void OnButtonClicked(){
		
	}
	
	protected virtual void ChangeState(ButtonStates newState){
		CurrentState = newState;
		UpdateMaterial((int)newState);
	}
	
	protected void UpdateMaterial(int matIndex){
		_material = new Material(ButtonMaterials[matIndex]);
		if(ButtonTexture != null){
			_material.mainTexture = ButtonTexture;
		}
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
