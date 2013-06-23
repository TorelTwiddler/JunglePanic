using UnityEngine;
using System.Collections;

public class inputTests : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//if(Input.anyKey && Input.inputString.Length > 0){
		//	print('"'+Input.inputString+'"');
		//}
	}
	
	void OnGUI(){
		Event e = Event.current;
		if(e.keyCode != KeyCode.None){
			print(e.keyCode);
		}
		if(Input.inputString != ""){
			print(Input.inputString);
		}
	}
}
