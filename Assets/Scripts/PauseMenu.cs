using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			TogglePause();
		}
	}
	
	private void TogglePause(){
		if(Time.timeScale == 1.0f){
			print("pause");
			Time.timeScale = 0.0f;
			Time.fixedDeltaTime = 0.0f;
		}
		else{
			print("unpause");
			Time.timeScale = 1.0f;
			Time.fixedDeltaTime = 1.0f;
		}
	}
}
