using UnityEngine;
using System.Collections;

public class ShineTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Brick") {
			OTAnimatingSprite sprite = collider.GetComponentInChildren<OTAnimatingSprite>();
			sprite.PlayOnce("Shine");
		}
	}
}
