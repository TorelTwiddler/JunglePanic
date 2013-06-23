using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour {
	
	public Brick[][] m_map;
	
	private GameObject brickPrefab;
	
	public Vector2 v2_brickBuffer = new Vector2(0.5f,0.2f);
	
	private Vector3 startingPos = new Vector3(0,0,0);
	
	public int percentBrickBlank = 90;

	private int m_totalWeights = 0;
	
	public int levelWidth = 10;
	public int levelHeight = 4;
	
	public float totalPoints = 0.0f;
	
	public Brick[] m_bricks = new Brick[0];
	
	public event Brick.BrickHitDelegate OnBrickHit;
	
	// Use this for initialization
	void Start () {
		foreach(Brick val in m_bricks)
		{
			m_totalWeights += val.m_weightPercent;
		}
		BuildNewRandomLevel(levelWidth,levelHeight);
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
		m_map[x][y].transform.eulerAngles = new Vector3(0, 0, 180);
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
		
		int randomNumber = Random.Range(0, m_totalWeights);

        Brick selectedBrick = null;
        for (int i = 0; i < m_bricks.Length; i++)
        {
			int brickweight = m_bricks[i].m_weightPercent;
           if (randomNumber < brickweight)
            {
                selectedBrick = m_bricks[i];
                break;
            }
           
            randomNumber = randomNumber - brickweight;
        }

        return selectedBrick.gameObject;	
		
	}
}
