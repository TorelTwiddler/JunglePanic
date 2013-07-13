using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour {
	
	public Brick[][] m_map;
	
	private GameObject brickPrefab;
	
	public Vector2 v2_brickBuffer = new Vector2(0.5f,0.2f);
	
	private Vector3 startingPos = new Vector3(0,0,0);
	
	public int percentBrickBlank = 90;

	private float m_totalWeights = 0.0f;
	
	public int levelWidth = 10;
	public int levelHeight = 4;
	
	public float totalPoints = 0.0f;
	
	public Brick[] m_bricks = new Brick[0];
	
	public event Brick.BrickHitDelegate OnBrickHit;
	
	public AudioClip m_levelMusic;
	
	public GameObject[] playerPrefabs;
	public GameObject ballPrefab;
	
	public int numberOfTeams = 4;
	public GameObject teamScorePrefab;
	public string[] teamNames;
	public Color[] teamColors;
	
	public int[] playerTeams;
	
	private List<Player> players = new List<Player>();
	
	private List<Team> teams = new List<Team>();
	
	private Transform[] teamScoreStarts = new Transform[4];
	
	private Transform[] playerStarts = new Transform[4];
	private Transform ballStart;
	
	private OTAnimation[] playerAnimations = new OTAnimation[4];
	private OTAnimation ballAnimation;
	
	void Awake () {
		GlobalOptions options = GlobalOptions.Instance;
		playerTeams = options.GetPlayerTeams();
		// This is hardcoded to 4 players and 4 teams.
		for (int i = 0; i < 4; i++) {
			GameObject player = playerPrefabs[i];
			playerStarts[i] = GameObject.Find(player.name + " Start").GetComponent<Transform>();
			playerAnimations[i] = GameObject.Find(player.name + " Animation").GetComponent<OTAnimation>();
			teamScoreStarts[i] = GameObject.Find("TeamScore" + (i + 1).ToString() + " Start").GetComponent<Transform>();
		}
		ballStart = GameObject.Find("Ball Start").GetComponent<Transform>();
		ballAnimation = GameObject.Find("Ball Animation").GetComponent<OTAnimation>();
	}
	
	// Use this for initialization
	void Start () {
		foreach(Brick val in m_bricks)
		{
			m_totalWeights += val.m_weightPercent;
		}
		BuildNewRandomLevel(levelWidth,levelHeight);
		//AudioManager.Get().PlaySound(m_levelMusic);
		AddTeams(numberOfTeams);
		AddPlayers(playerTeams);
		AddBall();
		
		StartGame();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("p"))
		{
			ClearBricks();
			BuildNewRandomLevel(levelWidth,levelHeight);
		}
	}
	
	public void ClearBricks()
	{	
		if(m_map == null)
		{
			return;
		}
		
		for(int i = 0; i< m_map.Length; i++)
		{
			for(int j = 0; j < m_map[i].Length; j++)
			{
				if(m_map[i][j] != null)
				{
					Destroy(m_map[i][j].gameObject);
				}
			}
		}
	}
	
	public void BuildNewRandomLevel(int width, int height, int MinBlocks = -1)
	{
		ClearBricks();
		m_map = new Brick[width][];
		for(int i = 0; i < width; i++)
		{
			m_map[i] = new Brick[height];
		}
		
		int numBlocks = 0;
		while (true)
		{
			for(int x = 0; x < width/2; x++)
			{
				for(int y = 0; y < height/2; y++)
				{
					int numrand = Random.Range(0,100);
					if(numrand > percentBrickBlank)
					{
						brickPrefab = GetRandomBrick();
						totalPoints += brickPrefab.GetComponent<Brick>().points * 4;
						CreateBrick(x, y, brickPrefab);
						numBlocks++;
					}
				}
			}
			numBlocks *= 4;
			if(MinBlocks == -1 || numBlocks > MinBlocks)
			{
				break;
			}
		}
		
		for(int x = 0; x < width/2; x++)
		{
			for(int y = 0; y < height/2; y++)
			{
				if(m_map[x][y] != null)
				{
					CreateBrick(x, height-y-1, m_map[x][y].gameObject);
				}
			}
		}
		for(int x = 0; x < width/2; x++)
		{
			for(int y = 0; y < height/2; y++)
			{
				if(m_map[x][y] != null)
				{
					CreateBrick(width-x-1, y, m_map[x][y].gameObject);
				}
			}
		}
		for(int x = 0; x < width/2; x++)
		{
			for(int y = 0; y < height/2; y++)
			{
				if(m_map[x][y] != null)
				{
					CreateBrick(width-x-1, height-y-1, m_map[x][y].gameObject);
				}
			}
		}
	
		PlaceLevel();
	}
	
	public void CreateBrick(int x, int y, GameObject brick)
	{
		m_map[x][y] = (Instantiate(brick, startingPos, new Quaternion(0,0,0,0)) as GameObject).GetComponent<Brick>();
		m_map[x][y].name = string.Format("Brick({2}): [{0}][{1}]", x,y, m_map[x][y].typeName);
		m_map[x][y].gameObject.transform.parent = gameObject.transform;
		m_map[x][y].OnHit += onBrickHit;
	}
	
	private void onBrickHit(Brick sender, int hitsLeft)
	{
		if(OnBrickHit != null)
		{
			OnBrickHit(sender, hitsLeft);
		}
	}
	
	public void PlaceLevel()
	{
		Vector3 currentPos = startingPos;
		
		for(int x = 0; x < m_map.Length; x++)
		{
			currentPos.x += brickPrefab.transform.localScale.x + v2_brickBuffer.x;
			for(int y = 0; y < m_map[x].Length; y++)
			{
				currentPos.y += brickPrefab.transform.localScale.y + v2_brickBuffer.y;
				if(m_map[x][y] != null)
				{
					m_map[x][y].gameObject.transform.localPosition = currentPos;
				}
			}
			currentPos.y = 0;
		}
	}
	
	private GameObject GetRandomBrick()
	{
		if(m_bricks.Length == 0 )
		{
			return null;
		}
		
		float randomNumber = Random.Range(0.0f, 100.0f);

        Brick selectedBrick = null;
        for (int i = 0; i < m_bricks.Length; i++)
        {
			float brickweight = m_bricks[i].m_weightPercent;
			if (randomNumber < brickweight)
            {
                selectedBrick = m_bricks[i];
                break;
            }
           
            randomNumber = randomNumber - brickweight;
        }

        return selectedBrick.gameObject;	
		
	}
	
	private void AddPlayers(int[] playerTeams)
	{
		//players = new Player[playerTeams.Length];
		for (int i = 0; i < playerTeams.Length; i++) {
			if(playerTeams[i] < 0){
				continue;
			}
			GameObject player = (Instantiate(playerPrefabs[i], playerStarts[i].position, new Quaternion(0,0,0,0)) as GameObject);
			player.name = player.name.Replace("(Clone)", "");
			player.GetComponent<Player>().team = teams[playerTeams[i]];
			OTAnimatingSprite playerSprite = player.GetComponentInChildren<OTAnimatingSprite>();
			playerSprite.animation = playerAnimations[i];
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
				return;
			}
			GameObject teamScore = (Instantiate(teamScorePrefab, teamScoreStarts[i].position, new Quaternion(0,0,0,0)) as GameObject);
			teamScore.name = teamScore.name.Replace("(Clone)", "");
			teams.Add(teamScore.GetComponent<Team>());
			teams[i].SetColor(teamColors[i]);
			teams[i].SetTeamName(teamNames[i]);
		}
	}
	
	public int GetNumberOfTeams(){
		return teams.Count;
	}
	
	private void StartGame() {
		GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
		gameController.StartGame(players.ToArray());
	}
}
