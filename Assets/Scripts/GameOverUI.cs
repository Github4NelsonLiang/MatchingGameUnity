using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
	public Text current;
	public Text highest;

	// Use this for initialization
	void Start () {
		current.text = "Current High Score: " + GameControl.Instance.currentScore;
		highest.text = "Highest Score: " + GameControl.Instance.highScores [0];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
