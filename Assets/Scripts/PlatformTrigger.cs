using UnityEngine;
using System.Collections;

public class PlatformTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider jumper){
		Transform platform = transform.parent;
		Physics.IgnoreCollision(jumper.GetComponent<BoxCollider>(), platform.GetComponent<BoxCollider>());
	}
	
	void OnTriggerExit(Collider jumper){
		jumper.gameObject.layer = 10;
		
		Transform platform = transform.parent;
		Physics.IgnoreCollision(jumper.GetComponent<BoxCollider>(), platform.GetComponent<BoxCollider>(), false);
	}
}
