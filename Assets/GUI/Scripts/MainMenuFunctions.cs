using UnityEngine;
using System.Collections;

public class MainMenuFunctions : MonoBehaviour {
	
	private MenuController MenuController;
	
	public Vector3[] MenuCameraPositions;
	
	void Awake(){
		MenuController = GameObject.Find("MenuController").GetComponent<MenuController>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ButtonHover(string name = "default"){
		//print("hovering over " + name);
	}
	
	public void StartGame(){
		MenuController.StartGame();
	}
	
	public void ShowMainMenu(){
		Transform t = Camera.main.transform;
		t.position = MenuCameraPositions[0];
	}
	
	public void ShowOptionsMenu(string direction){
		Transform t = Camera.main.transform;
		t.position = MenuCameraPositions[1];
		
		//This probably doesn't need to be a string
		/*switch(direction){
			case "Forward":
				break;
			case "Backward":
				GlobalOptions options = GlobalOptions.Instance;
				options.SavePlayerConfigs();
				break;
			default:
				break;
		}*/
	}
	
	public void ShowControlsMenu(){
		Transform t = Camera.main.transform;
		t.position = MenuCameraPositions[2];
	}
	
	public void QuitGame(){
		print("quitting");
		Application.Quit();
	}
}
