using UnityEngine;
using System.Collections;

public class TeamText : MonoBehaviour {

	GlobalOptions options;
	
	// Use this for initialization
	void Start () {
		
		options = GlobalOptions.Instance;
		Team winningTeam = options.GetMostRecentWinningTeam();
		GetComponent<TextMesh>().text = winningTeam.teamName;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
