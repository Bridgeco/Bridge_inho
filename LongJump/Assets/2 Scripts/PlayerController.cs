using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public static PlayerController instance;

	public float jumpForce = 0.0f;
	public float runningSpeed = 0.0f;
	public Animator animator;

	private Rigidbody2D rigidBody;
	private Vector3 startingPosition;

	private float timer;


	void Awake(){
		rigidBody = GetComponent<Rigidbody2D> ();
		instance = this;
		startingPosition = this.transform.position;

		//터치 시간 10초 설정
		timer = 10.0f;
	}

	public void StartGame(){
		animator.SetBool ("isAlive", true);
		this.transform.position = startingPosition;
	}

	// Update is called once per frame
	void Update () {
		if (GameManager.instance.currentGameState == GameState.inGame) {
			if (Input.GetMouseButtonDown (0)) {
				Jump ();
			}
		}
		if (timer > 0) {
			timer -= Time.deltaTime;
		} else {
			rigidBody.AddForce (Vector2.up * jumpForce, ForceMode2D.Impulse);
			FixedUpdate ();
			jumpForce = 0.0f;
			runningSpeed = 0.0f;
		}
		animator.SetBool ("isGrounded", IsGrounded ());
	}

	public void FixedUpdate(){
		if (GameManager.instance.currentGameState == GameState.inGame) {
			if (timer < 0){
				if (rigidBody.velocity.x < runningSpeed) {
					rigidBody.velocity = new Vector2 (runningSpeed, rigidBody.velocity.y);
				}
			}
		}
	}

	void Jump(){
		if (IsGrounded ()) {
			jumpForce += 0.5f;
			runningSpeed += 1.0f;
			//rigidBody.AddForce (Vector2.up * jumpForce, ForceMode2D.Impulse);
		}
	}
	public LayerMask groundLayer;

	bool IsGrounded(){
		
		if(Physics2D.Raycast(this.transform.position, Vector2.down, 0.2f, groundLayer.value)){
			return true;
		}
		else {
			return false;
		}
	}
	public void Kill(){
		Debug.Log ("kill");
		GameManager.instance.GameOver ();
		animator.SetBool ("isAlive", false);

	}

}
