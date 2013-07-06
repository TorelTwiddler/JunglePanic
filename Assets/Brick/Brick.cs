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
	
	public int m_weightPercent = 25;
	
	public delegate void BrickHitDelegate(Brick hitBrick, int hitsLeft);
	
	public event BrickHitDelegate OnHit;
	
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
	
	public int i_hitsLeft = 1;
	
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
		
		if(OnHit != null)
		{
			OnHit(this, hitsLeft);
		}
		
		if(hitsLeft <= 0)
		{
			SpawnText(owningPlayer);
			owningPlayer.GivePoints(points);
			Destroy(gameObject);
		}
	}
	
	public void SpawnText (Player player) {
		TextMesh scoreText = (Instantiate(player.GetScoreText(), transform.position, new Quaternion(0,0,0,0)) as GameObject).GetComponent<TextMesh>();
		Color color = player.GetColor();
		Material material = new Material(scoreText.renderer.material);
		material.color = new Color(color.r, color.g, color.b, scoreText.renderer.material.color.a);
		scoreText.renderer.sharedMaterial = material;
		scoreText.text = points.ToString();
	}
}
