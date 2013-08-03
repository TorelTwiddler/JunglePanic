using UnityEngine;
using System.Collections;

public class MainMenuFunctions : MonoBehaviour {
	
	public Vector3[] MenuCameraPositions;
	public PlayerManager PlayerManager;
	private SceneManager SceneManager;
	public GameObject SceneManagerPrefab;
	
	void Awake(){
		
	}

	// Use this for initialization
	void Start () {
		GlobalOptions options = GlobalOptions.Instance;
		if(options.GetAlreadyInitializedPlayers()){
			ShowPlayerSetup();
		}
		
		GameObject obj = GameObject.FindWithTag("SceneManager");
		if(obj == null){
			obj = Instantiate(SceneManagerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		}
		SceneManager = obj.GetComponent<SceneManager>();
	}
	
	// Update is called once per frame
	void Update () {
		/*GlobalOptions options = GlobalOptions.Instance;
		if(options.GetAlreadyInitializedPlayers()){
			ShowPlayerSetup();
		}*/
		//enabled = false;
	}
	
	public void ButtonHover(string name = "default"){
		//print("hovering over " + name);
	}
	
	public void StartGame(){
		SceneManager.StartGame();
	}
	
	public void ShowPlayerSetup(){
		Transform t = Camera.main.transform;
		t.position = MenuCameraPositions[3];
		PlayerManager.Initialize();
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
