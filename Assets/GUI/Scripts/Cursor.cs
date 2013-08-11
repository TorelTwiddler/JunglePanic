using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {

	public string InputSource;
	public KeyCode AcceptKeyCode;
	public Material[] CursorMaterials = new Material[4];
	
	public float cursorSpeed = 1.0f;
	
	public float screenHClamp = 14.2f;
	public float screenVClamp = 8.0f;
	
	string[] hAxes = new string[3]{"LeftX", "DpadX", "RightX"};
	string[] vAxes = new string[3]{"LeftY", "DpadY", "RightY"};
	
	private Collider currentMouseOver = null;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Move();
		HandleButton();
	}
	
	public void Setup(string inputSource, KeyCode acceptKeyCode, int playerIndex){
		InputSource = inputSource;
		AcceptKeyCode = acceptKeyCode;
		transform.parent = Camera.main.transform;
		transform.Translate(Vector3.forward * -5);
		renderer.material = CursorMaterials[playerIndex];
	}
	
	public void SetAcceptKey(KeyCode acceptKey){
		AcceptKeyCode = acceptKey;
	}
	
	void Move() {
		float horizontal = 0.0f;
		foreach(string axis in hAxes){
			float value = Input.GetAxis(InputSource + axis);
			if(Mathf.Abs(value) > 0.1f){
				horizontal = value;
				break;
			}
		}
		
		float vertical = 0.0f;
		foreach(string axis in vAxes){
			float value = Input.GetAxis(InputSource + axis);
			if(Mathf.Abs(value) > 0.1f){
				vertical = value;
				break;
			}
		}
		transform.Translate(horizontal * cursorSpeed * Time.deltaTime, vertical * cursorSpeed * Time.deltaTime, 0 );
		
		float x = Mathf.Clamp(transform.localPosition.x, -screenHClamp, screenHClamp);
		float y = Mathf.Clamp(transform.localPosition.y, -screenVClamp, screenVClamp);
		transform.localPosition = new Vector3(x, y, transform.localPosition.z);
		
	}
	
	void HandleButton() {
		if (Input.GetKeyDown(AcceptKeyCode)) {
			Click();
		}
		else {
			MouseOver();
		}
	}
	
	void Click() {
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position, Vector3.forward,
				out hitInfo, 100.0f, 1<<9)) {
			hitInfo.collider.SendMessage("OnMouseUpAsButton",
					SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void MouseOver() {
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position, Vector3.forward,
				out hitInfo, 100.0f, 1<<9)) {
			currentMouseOver = hitInfo.collider;
			currentMouseOver.SendMessage("OnMouseEnter",
					SendMessageOptions.DontRequireReceiver);
			
		}
		else if (currentMouseOver != null) {
			currentMouseOver.SendMessage("OnMouseExit",
					SendMessageOptions.DontRequireReceiver);
			currentMouseOver = null;
		}
	}
	
	public void Remove() {
		Destroy(gameObject);
	}
}
