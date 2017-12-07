using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour {

	public static PlayerController2 instance;
	public Slider sliderBar;
	public Text counterText;
	public float seconds;

	public float jumpForce = 0.0f;
	public float runningSpeed = 20.0f;
	public Animator animator;

	private Rigidbody2D rigidBody;
	private Vector3 startingPosition;
	private Vector3 target;

	private float timer;
	private bool isJump = false;

	void Awake(){
		rigidBody = GetComponent<Rigidbody2D> ();
		//counterText = GetComponent<Text>() as Text;
		instance = this;
		startingPosition = this.transform.position;
		jumpForce = 10.0f;
		runningSpeed = 20.0f;
		//터치 시간 10초 설정
		timer = 30.0f;

	}
	public void StartGame(){
		animator.SetBool ("isAlive", true);
		this.transform.position = startingPosition;
	}
	// Update is called once per frame
	void Update () {
		
		sliderBar.value = runningSpeed;
		timer -= Time.deltaTime;
		seconds = (int)timer+1;
		counterText.text = seconds.ToString("00");

		if(timer < 0.4f && timer > 0.0f){
			animator.SetBool("isReady", true);
		}
		if (timer < 0 && isJump == false) {
			Minor ();
			GameManager.instance.menuCanvas.enabled = false;
			animator.SetBool ("isJump", true);
			rigidBody.AddForce (Vector2.up * jumpForce, ForceMode2D.Impulse);
			rigidBody.AddForce (Vector2.right * runningSpeed, ForceMode2D.Impulse);
			isJump = true;

			//rigidBody.velocity = new Vector2 (runningSpeed, rigidBody.velocity.y);
		}
		if (IsGrounded()) {
			animator.SetBool ("isGround", true);
			Destroy (rigidBody);
			//runningSpeed = 0.0f;
			target = this.transform.position;
			this.transform.position = target;
			Score ();

		}

		/*if (GameManager.instance.currentGameState == GameState.inGame) {
			if (Input.GetMouseButtonDown (0)) {
				Jump ();
			}
		}*/
		/*if (timer > 0) {
			timer -= Time.deltaTime;
		} else if(timer<0 && isJump==false){
			GameManager.instance.menuCanvas.enabled = false;
			rigidBody.AddForce (Vector2.up * jumpForce, ForceMode2D.Impulse);
			rigidBody.AddForce (Vector2.right * runningSpeed, ForceMode2D.Impulse);
			//rigidBody.velocity = new Vector2 (runningSpeed, rigidBody.velocity.y);
			isJump = true;
		}*/

		//rigidBody.velocity = new Vector2 (runningSpeed, rigidBody.velocity.y);
	}

	public void Jump(){
		jumpForce += 0.3f;
		runningSpeed += 0.6f;
		
	}
	public void Minor(){
		
		if (runningSpeed > 20.0f && runningSpeed < 40.0f) {
			runningSpeed -= 0.01f;
			//yield return new WaitForSeconds (1000.0f);
		} else if (runningSpeed > 40.0f && runningSpeed < 70.0f) {
			runningSpeed -= 0.03f;
			//yield return new WaitForSeconds (1000.0f);
		} else if (runningSpeed > 70.0f) {
			runningSpeed -= 0.05f;
			//yield return new WaitForSeconds (1000.0f);
		} else if (runningSpeed > 80.0f) {
			runningSpeed = 80.0f;
		}
	}
			
	public void Score(){
		Debug.Log (target.x);
		Debug.Log (startingPosition.x);
		Debug.Log (target.x-startingPosition.x);
	}

	public LayerMask groundLayer;

	bool IsGrounded(){

		if(Physics2D.Raycast(this.transform.position, Vector2.down, 2.5f, groundLayer.value)){
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
