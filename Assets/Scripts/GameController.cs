using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject readyGui;
	public float readySeconds = 1.0f;
	public GameObject goGui;
	public float goSeconds = 1.0f;
	
	public GameObject teamGui;
	public GameObject winsGui;
	
	public float teamWinsSeconds = 5.0f;
	
	public float endTime = Mathf.Infinity;
	public bool didReadyGui = false;
	public bool didGoGui = false;
	public bool didGameEnd = false;
	
	private Player[] players;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > endTime) {
			if (didGameEnd) {
				GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>().LoadMainMenu();
			}
			else if (didGoGui) {
				endTime = Mathf.Infinity;
			}
			else if (didReadyGui) {
				didGoGui = true;
				HideGo();
				UnfreezePlayers();
			}
			else {
				didReadyGui = true;
				HideReady();
				DisplayGo();
				endTime = Time.time + goSeconds;
			}
		}
	}
	
	public void StartGame(Player[] newPlayers) {
		players = newPlayers;
		FreezePlayers();
		
		// Turn on ready
		DisplayReady();
		
		// Start countdown to GO!!
		endTime = Time.time + readySeconds;
	}
	
	public void DisplayReady() {
		readyGui.SetActive(true);
	}
	
	public void HideReady() {
		readyGui.SetActive(false);
	}
	
	public void DisplayGo() {
		goGui.SetActive(true);
	}
	
	public void HideGo() {
		goGui.SetActive(false);
	}
	
	public void DisplayTeamWins(Team team) {
		teamGui.SetActive(true);
		teamGui.GetComponent<TextMesh>().text = team.teamName;
		winsGui.SetActive(true);
	}
	
	public void FreezePlayers() {
		foreach (Player player in players) {
			player.canMove = false;
		}
	}
	
	public void UnfreezePlayers() {
		foreach (Player player in players) {
			player.canMove = true;
		}
	}
	
	public void EndGame(Team winningTeam) {
		didGameEnd = true;
		FreezePlayers();
		DisplayTeamWins(winningTeam);
		endTime = Time.time + teamWinsSeconds;
	}
}
