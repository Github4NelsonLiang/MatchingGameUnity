using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		GameControl.Instance = new GameControl ();
//		GameControl.Instance.updateScore(3);
//		GameControl.Instance.updateScore(1);
//		GameControl.Instance.updateScore(5);
		Debug.Log("HIGHSCORE:::::"+GameControl.Instance.currentScore);
        for (int i = 0; i < 10; i++)
        {
			
            string tempName = "Score" + (i+1);
            GameObject temp = GameObject.Find(tempName);
            Text displayText = temp.GetComponent<Text>();
			displayText.text = "High Score " + (i+1) + " : "+ GameControl.Instance.highScores[i];

//			for(int i = 0; i < 10; i++){
			Debug.Log (tempName+" HIGHSCORE:::Printing::::"+GameControl.Instance.highScores [i]);
//			}
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
