using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour {
	
	public AudioClip m_levelMusic;
	
	public GameObject[] playerPrefabs;
	public GameObject ballPrefab;
	
	public int numberOfTeams = 4;
	public GameObject teamScorePrefab;
	public Color[] teamColors;
	
	public int[] playerTeams;
	public int[] playerCharacters;
	
	private List<Player> players = new List<Player>();
	
	private List<Team> teams = new List<Team>();
	
	private Transform[] teamScoreStarts = new Transform[4];
	
	private Transform[] playerStarts = new Transform[4];
	private Transform ballStart;
	
	private OTAnimation ballAnimation;
	
	void Awake () {
		GlobalOptions options = GlobalOptions.Instance;
		playerTeams = options.GetPlayerTeams();
		playerCharacters = options.GetPlayerCharacters();
		// This is hardcoded to 4 players and 4 teams.
		for (int i = 0; i < 4; i++) {
			GameObject player = playerPrefabs[i];
			playerStarts[i] = GameObject.Find("Player" + (i + 1).ToString() + " Start").GetComponent<Transform>();
			teamScoreStarts[i] = GameObject.Find("TeamScore" + (i + 1).ToString() + " Start").GetComponent<Transform>();
		}
		ballStart = GameObject.Find("Ball Start").GetComponent<Transform>();
		ballAnimation = GameObject.Find("Ball Animation").GetComponent<OTAnimation>();
	}
	
	// Use this for initialization
	void Start () {
		//AudioManager.Get().PlaySound(m_levelMusic);
		AddTeams(numberOfTeams);
		AddPlayers(playerTeams);
		AddBall();
		
		StartGame();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void AddPlayers(int[] playerTeams)
	{
		//players = new Player[playerTeams.Length];
		for (int i = 0; i < playerTeams.Length; i++) {
			if(playerTeams[i] < 0){
				continue;
			}
			GameObject player = (Instantiate(playerPrefabs[playerCharacters[i]],
					playerStarts[i].position, new Quaternion(0,0,0,0)) as GameObject);
			player.name = player.name.Replace("(Clone)", "");
			player.GetComponent<Player>().team = teams[playerTeams[i]];
			player.GetComponent<Player>().playerIndex = i;
			OTAnimatingSprite playerSprite = player.GetComponentInChildren<OTAnimatingSprite>();
			OTAnimation playerAnimation = GameObject.Find(player.name + " Animation").GetComponent<OTAnimation>();
			playerSprite.animation = playerAnimation;
			players.Add(player.GetComponent<Player>());
		}
	}
	
	private void AddBall()
	{
		GameObject ball = (Instantiate(ballPrefab, ballStart.position, new Quaternion(0,0,0,0)) as GameObject);
		ball.name = ball.name.Replace("(Clone)", "");
		OTAnimatingSprite ballSprite = ball.GetComponentInChildren<OTAnimatingSprite>();
		ballSprite.animation = ballAnimation;
	}
	
	private void AddTeams(int numberOfTeams)
	{
		GlobalOptions options = GlobalOptions.Instance;
		//teams = new Team[numberOfTeams];
		for (int i = 0; i < numberOfTeams; i++) {
			if(!options.TeamsInGame[i]){
				teams.Add(null);
				continue;
			}
			GameObject teamScore = (Instantiate(teamScorePrefab, teamScoreStarts[i].position, new Quaternion(0,0,0,0)) as GameObject);
			teamScore.name = teamScore.name.Replace("(Clone)", "");
			teams.Add(teamScore.GetComponent<Team>());
			teams[i].SetColor(teamColors[i]);
			teams[i].SetTeamName(options.GetTeamName(i));
		}
	}
	
	public int GetNumberOfTeams(){
		int count = 0;
		for(int i = 0; i < teams.Count; i++){
			if(teams[i] != null){
				count++;
			}
		}
		return count;
	}
	
	private void StartGame() {
		GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
		gameController.StartGame(players.ToArray());
	}
}
