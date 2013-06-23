using UnityEngine;
using System.Collections;

public class LevelBuilder : MonoBehaviour {
	
	public Brick[][] m_map;
	
	public GameObject brickPrefab;
	
	public Vector2 v2_brickBuffer = new Vector2(0.5f,0.2f);
	
	private Vector3 startingPos = new Vector3(0,0,0);
	
	public int percentBrickBlank = 90;
	public System.Collections.Generic.List<int> brickTypeWeights = new System.Collections.Generic.List<int>();
	
	public int levelWidth = 10;
	public int levelHeight = 4;
	
	public float totalPoints = 0.0f;
	
	public GameObject[] bricks;
	
	// Use this for initialization
	void Start () {
		BuildNewRandomLevel(levelWidth,levelHeight);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void BuildNewRandomLevel(int width, int height, int MinBlocks = -1)
	{
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
		
		return bricks[Random.Range(0, bricks.Length)];
		/*
		System.Collections.Generic.List<int> WeightedAmounts = new System.Collections.Generic.List<int>();
		int maxValue = 0;
		for(int i = 0; i < brickTypeWeights.Count; i++)
		{
			
				if(brickTypeWeights[i] > maxValue)
				{
					maxValue = brickTypeWeights[i];
				}
				WeightedAmounts.Add(brickTypeWeights[i]);
			
		}
		for(int i = 0; i < WeightedAmounts.Count; i++)
		{
			WeightedAmounts[i] = (WeightedAmounts[i]/maxValue) * 100;
		}
		System.Collections.Generic.List<int> SortedWeights = new System.Collections.Generic.List<int>(WeightedAmounts);
		
		SortedWeights.Sort();
		
		int num = rand.Next(0, 100);
		for(int i = 0; i < SortedWeights.Count; i++)
		{
			if(num <= SortedWeights[i])
			{
				System.Collections.Generic.List<int> sameWeights = new System.Collections.Generic.List<int>();
				sameWeights.Add(WeightedAmounts.IndexOf(SortedWeights[i]));
				
				while(WeightedAmounts.IndexOf(SortedWeights[i],sameWeights[sameWeights.Count-1]) > -1)
				{
					sameWeights.Add(WeightedAmounts.IndexOf(SortedWeights[i],sameWeights[sameWeights.Count-1]));
				}
				if(sameWeights.Count > 1)
				{
					return sameWeights[rand.Next(0, sameWeights.Count)];
				}
				else
				{
					return sameWeights[0];
				}
			}
		}
		
		return 0;
		*/
		
	}
}
