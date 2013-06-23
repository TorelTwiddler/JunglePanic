using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	
	public float f_speed = 10.0f;
	public float f_rotate_speed = 10.0f;
	public Player m_lastPlayer;
	public bool IsHeldByPlayer = false;
	public Player CarryingPlayer;
	public ParticleSystem Particles;

	// Use this for initialization
	void Start () {
		// rigidbody.AddForce(Vector3.down * f_speed * Time.deltaTime, ForceMode.VelocityChange);
	
	}
	
	// Update is called once per frame
	void Update () {
		if(IsHeldByPlayer){
			transform.position = CarryingPlayer.transform.position + CarryingPlayer.BallOffset;
		}
	}
	
	void FixedUpdate () {
		if (rigidbody.velocity.magnitude > f_speed) {
			rigidbody.velocity = rigidbody.velocity.normalized * f_speed;
		}
	}
	
	void OnCollisionEnter (Collision collision) {
		if (IsHeldByPlayer) {
			return;
		}
		switch (collision.gameObject.tag) {
			case "Player":
				Player player = collision.gameObject.GetComponent<Player>();
				if (player.CanCatch) {
					player.CatchBall(this);
				}
				break;
			case "Floor":
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
				break;
			case "Brick":
				collision.gameObject.GetComponent<Brick>().Damage(this.m_lastPlayer);
				RotateBall();
				break;
			default:
				RotateBall();
				break;
		}
	}
	
	void RotateBall () {
		rigidbody.angularVelocity = Vector3.forward * f_rotate_speed;
	}
	
		
	public void GrabBall (Player player) {
		rigidbody.angularVelocity = Vector3.zero;
		rigidbody.velocity = Vector3.zero;
		m_lastPlayer = player;
		IsHeldByPlayer = true;
		CarryingPlayer = player;
		Particles.startColor = player.PlayerColor;
	}
	
	
	public void ReleaseBall(Vector3 velocity, Player player){
		velocity.y = f_speed;
		rigidbody.AddForce(velocity, ForceMode.VelocityChange);
		
		IsHeldByPlayer = false;
		CarryingPlayer = null;
	}
}
