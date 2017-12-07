using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController2 : MonoBehaviour {

	public static PlayerController2 instance;
	// Slider 값 변수
	public Slider sliderBar;
	//타이머 UI Text
	public Text counterText;
	//타이머 변수형태 변환을 위한 변수
	public float seconds;

	public float jumpForce = 0.0f;
	public float runningSpeed = 20.0f;
	public Animator animator;


	private Rigidbody2D rigidBody;
	//출발 지점 값(이동거리 계산), 
	private Vector3 startingPosition;
	//도착 지점 값(이동거리 계산)
	private Vector3 target;

	// 타이머 변수
	private float timer;
	// 점프 여부 판단
	private bool isJump = false;

	//현재 스코어
	public float nowScore;
	public float topScore;




	void Awake(){
		rigidBody = GetComponent<Rigidbody2D> ();
		//counterText = GetComponent<Text>() as Text;
		instance = this;
		startingPosition = this.transform.position;
		jumpForce = 10.0f;
		runningSpeed = 20.0f;
		//터치 시간 10초 설정
		timer = 10.0f;

	}
	void Start(){
		topScore = ScoreCalc.topScore;
	}
	public void StartGame(){
		animator.SetBool ("isAlive", true);
		this.transform.position = startingPosition;
	}
	// Update is called once per frame
	void Update () {
		
		sliderBar.value = runningSpeed;
		seconds = (int)timer+1;
		counterText.text = seconds.ToString("00");

		if(timer < 0.4f && timer > 0.0f){
			animator.SetBool("isReady", true);
		}
		if (timer >= 0) {
			Minor ();
			timer -= Time.deltaTime;
		} else if (timer < 0 && isJump == false) {
			
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
			StartCoroutine (Wait ());
		}


	}

	// 터치 시 점프 힘 추가
	public void Jump(){
		jumpForce += 0.3f;
		runningSpeed += 0.6f;
		
	}

	// 게이지 구간 별 줄어듦
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

	//이동한 거리만큼 계산
	public void Score(){
		nowScore = (target.x - startingPosition.x)/2;
		if(topScore < nowScore){
			topScore = nowScore;
		}
	}

	//Ground 판별을 위한 layer값 저장 변수
	public LayerMask groundLayer;


	// 캐릭터 기준 아래 땅 판별 True or False
	bool IsGrounded(){

		if(Physics2D.Raycast(this.transform.position, Vector2.down, 2.5f, groundLayer.value)){
			return true;
		}
		else {
			return false;
		}
	}
	// 착지후 기다림 및 씬 전환
	IEnumerator Wait(){
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene ("Score");

	}
	
	/*public void Kill(){
		Debug.Log ("kill");
		GameManager.instance.GameOver ();
		animator.SetBool ("isAlive", false);

	}*/

}
