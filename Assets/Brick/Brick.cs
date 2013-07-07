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
	
	public float m_weightPercent = 25.0f;
	
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
	
	public Material crumbMaterial;
	
	// Use this for initialization
	void Start () {
		GetComponentInChildren<OTAnimatingSprite>().onAnimationFinish = OnAnimationFinish;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Damage(Player owningPlayer)
	{
		SpawnCrumbs(owningPlayer);
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

	public void OnAnimationFinish(OTObject owner) {
		GetComponentInChildren<OTAnimatingSprite>().PlayLoop("Idle");
	}
	
	public void SpawnCrumbs (Player owningPlayer) {
		GameObject particles = Instantiate(Resources.Load("Crumb Particle System"), transform.position, new Quaternion(0,0,0,0)) as GameObject;
		particles.GetComponent<ParticleSystem>().renderer.material = crumbMaterial;
		
		particles.GetComponent<ParticleDirector>().target = owningPlayer.GetParticlePoint();
	}
}