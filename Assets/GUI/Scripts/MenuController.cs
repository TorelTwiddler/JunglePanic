using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
	
	private SceneManager SceneManager;
	
	void Awake(){
		GameObject sceneManagerObject = GameObject.FindWithTag("SceneManager");
		if(sceneManagerObject == null){
			Debug.LogWarning("No scene manager found");
		}
		else{
			SceneManager = sceneManagerObject.GetComponent<SceneManager>();
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void StartGame(){
		SceneManager.StartGame();
	}
}
