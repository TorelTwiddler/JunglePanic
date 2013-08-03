using UnityEngine;
using System.Collections;

public class WinnerSceneController : MonoBehaviour {
	
	public float delay = 3.0f;
	
	private float endTime = Mathf.Infinity;
	
	// Use this for initialization
	void Start () {
		endTime = Time.time + delay;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= endTime) {
			if (Input.anyKeyDown) {
				print ("CHANGE SCENE!!!");
				GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>().LoadMainMenu();
			}
		}
	}
}
