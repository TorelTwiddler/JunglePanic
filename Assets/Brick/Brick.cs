using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {
	
	#region Public Properties
	
	public int points
	{
		get
		{
			return i_points;
		}
		set
		{
			i_points = value;
		}
	}
	
	public int hitsLeft
	{
		get
		{
			return i_hitsLeft;
		}
		set
		{
			i_hitsLeft = value;
		}
	}
	
	#endregion
	
	#region Private Fields
	
	private int i_hitsLeft = 1;
	
	#endregion
	
	public string typeName;
	public int i_points;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Damage(Player owningPlayer)
	{
		hitsLeft -= 1;
		
		if(hitsLeft <= 0)
		{
			TextMesh scoreText = (Instantiate(owningPlayer.ScoreText, transform.position, new Quaternion(0,0,0,0)) as GameObject).GetComponent<TextMesh>();
			scoreText.text = points.ToString();
			owningPlayer.GivePoints(points);
			Destroy(gameObject);
		}
	}
	
}
