using UnityEngine;
using System.Collections;

public class Celebrate : MonoBehaviour {
	
	public float cycleTime = 0.5f;
	
	private float switchTime = Mathf.Infinity;
	private bool handsUp = false;
	
	private Player player;
	
	// Use this for initialization
	void Start () {
		player = gameObject.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > switchTime) {
			switchTime = Time.time + cycleTime;
			if (handsUp) {
				PlayHandsDown();
				handsUp = false;
			} else {
				PlayHandsUp();
				handsUp = true;
			}
		}
	}
	
	public void Play() {	
		switchTime = Time.time + cycleTime;
		PlayHandsDown();
	}
	
	void PlayHandsDown(){
		player.PlayAnimation("Stand");
	}
	
	void PlayHandsUp(){
		player.PlayAnimation("StandCarry");
	}
}
