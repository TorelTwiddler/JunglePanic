using UnityEngine;
using System.Collections;

public class ScoreText : MonoBehaviour {
	
	public float TimeAlive = 100.0f;
	
	private float endTime = Mathf.Infinity;

	// Use this for initialization
	void Start () {
		endTime = Time.time + TimeAlive;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > endTime)
		{
			Destroy(gameObject);
		}
	}
}
