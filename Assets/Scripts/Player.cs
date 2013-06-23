using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	public enum InputTypes{
		Keyboard,
		Controller
	};
	
	public InputTypes InputType = InputTypes.Keyboard;
	private KeyCode LeftKey, RightKey, DownKey, JumpKey;
	public float PlayerSpeed = 25.0f;
	public float PlayerAcceleration = 300.0f;
	public float JumpHeight = 20.0f;
	public bool IsInvulnerable = false;
	public float f_InvulnerableCooldown = 1.0f;
	public bool CanJump = true;
	public bool HasBall = false;
	public bool CanCatch = true;
	public float f_CatchCooldown = 0.5f;
	public Ball CarriedBall = null;
	public Vector3 BallOffset = new Vector3(0, 1.0f, 0);
	public float Points = 0.0f;
	public GameObject ScoreBar;
	public GameObject ScoreText;
	public Color PlayerColor = Color.white;
	
	private OTAnimatingSprite sprite;
	private string playingFrameset = "";
	
	// Use this for initialization
	void Start () {
		GlobalOptions options = GlobalOptions.Instance;
		Dictionary<string,KeyCode> playerConfig = new Dictionary<string,KeyCode>();
		switch (name){
			case "Player1":
				playerConfig = options.PlayerConfigs[0];
				break;
			case "Player2":
				playerConfig = options.PlayerConfigs[1];
				break;
			case "Player3":
				playerConfig = options.PlayerConfigs[2];
				break;
			case "Player4":
				playerConfig = options.PlayerConfigs[3];
				break;
		}
		LeftKey = playerConfig["MoveLeft"];
		RightKey = playerConfig["MoveRight"];
		DownKey = playerConfig["MoveDown"];
		JumpKey = playerConfig["Jump"];
		
		sprite = GetComponentInChildren<OTAnimatingSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		HandleHorizontal();
		HandleVertical();
		
		HandleJump();
		
		HandleAnimation();
	}
	
	public void HandleHorizontal(){
		float horizontal = 0.0f;
		if(Input.GetKey(LeftKey)){
			horizontal = -1.0f;
		}
		else if(Input.GetKey(RightKey)){
			horizontal = 1.0f;
		}
		Vector3 velocity = rigidbody.velocity;
		if(horizontal == 0){
			velocity.x = 0;
			rigidbody.velocity = velocity;
			return;
		}
		if(horizontal > 0){
			transform.eulerAngles = new Vector3(0, 180, 0);
		}
		else if(horizontal < 0){
			transform.eulerAngles = Vector3.zero;
		}
		velocity.x += horizontal * PlayerAcceleration * Time.deltaTime;
		velocity.x = Mathf.Clamp(velocity.x, -PlayerSpeed, PlayerSpeed);
		rigidbody.velocity = velocity;
	}
	
	public void HandleVertical(){
		if(Input.GetKeyDown(DownKey)){
			gameObject.layer = 12;
			CanJump = false;
		}
	}
	
	public void HandleJump(){
		if(CanJump){
			if(Input.GetKeyDown(JumpKey)){
				CanJump = false;
				Vector3 velocity = rigidbody.velocity;
				rigidbody.velocity = velocity + new Vector3(0, JumpHeight, 0);
				if(HasBall){
					ReleaseBall(velocity + new Vector3(0, JumpHeight, 0));
				}
			}
		}
	}
	
	public bool IsJumping(){
		if(CanJump){
			return false;
		}
		else {
			return true;
		}
	}
	
	void OnCollisionEnter(Collision collision){
		switch (collision.gameObject.tag) {
		case "Floor":
			CanJump = true;
			break;
		case "Platform":
			CanJump = true;
			break;
		case "Player":
			Player otherPlayer = collision.gameObject.GetComponent<Player>();
			if(otherPlayer.HasBall && !otherPlayer.IsInvulnerable && CanCatch){
				Ball theBall = otherPlayer.CarriedBall;
				otherPlayer.LoseBall();
				CatchBall(theBall);
				IsInvulnerable = true;
				StartCoroutine(InvulnerableCooldown(f_InvulnerableCooldown));
			}
			break;
		}
	}
	
	public void GivePoints(int points){
		float totalPoints = GameObject.Find("LevelBuilder").GetComponent<LevelBuilder>().totalPoints;
		Points += points;
		ScoreBar.transform.localScale = new Vector3((Points / totalPoints) * 20, 1, 1);
		if (Points >= totalPoints / 2) {
			GameObject.Find ("SceneManager").GetComponent<SceneManager>().LoadMainMenu();
		}
	}
	
	public void CatchBall(Ball ball){
		if(CanCatch){
			HasBall = true;
			CarriedBall = ball;
			CarriedBall.rigidbody.velocity = Vector3.zero;
			CarriedBall.GrabBall(this);
		}
	}
	
	public void LoseBall(){
		HasBall = false;
		CanCatch = false;
		CarriedBall = null;
		StartCoroutine(CatchCooldown(f_CatchCooldown));
	}
	
	public void ReleaseBall(Vector3 velocity){
		CarriedBall.ReleaseBall(velocity, this);
		LoseBall();
	}
	
	IEnumerator CatchCooldown(float cooldown){
		yield return new WaitForSeconds(cooldown);
		CanCatch = true;
	}
	
	IEnumerator InvulnerableCooldown(float cooldown){
		yield return new WaitForSeconds(cooldown);
		IsInvulnerable = false;
	}
	
	public void HandleAnimation(){
		if (HasBall) {
			if (IsJumping()) {
				// has ball, jumping
				PlayAnimation("JumpCarry");
			}
			else if (rigidbody.velocity.magnitude > 1) {
				// has ball, walking
				PlayAnimation("WalkCarry");
			}
			else {
				// has ball, standing
				PlayAnimation("StandCarry");
			}
		}
		else {
			if (IsJumping()) {
				// jumping, no ball
				PlayAnimation("Jump");
			}
			else if (rigidbody.velocity.magnitude > 1) {
				// walking, no ball
				PlayAnimation("Walk");
			}
			else {
				// standing, no ball
				PlayAnimation("Stand");
			}
		}
	}
	
	public void PlayAnimation(string frameset) {
		if (playingFrameset != frameset) {
			sprite.PlayLoop(frameset);
		}
	}
}
