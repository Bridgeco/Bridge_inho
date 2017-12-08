using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScoreCalc : MonoBehaviour {
	public static ScoreCalc instance;
	public Text nowScoreText;
	public float nowScore;

	public Text topScoreText;
	public static float topScore;

	void Awake(){
		instance = this;
	}
	// Use this for initialization
	void Start () {
		nowScore = PlayerController.instance.nowScore;
		topScore = PlayerController.instance.topScore;

		nowScoreText.text = nowScore.ToString("0000.0"+ "M");
		topScoreText.text = topScore.ToString ("0000.0" + "M");

	}
	
	// Update is called once per frame
	void Update () {
		//nowScoreText.text = PlayerController2.instance.nowScore;
	}

	public void Retry(){
		SceneManager.LoadScene ("InGame");
	}
}
