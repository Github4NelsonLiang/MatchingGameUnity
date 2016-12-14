using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class highScoreController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //GameControl.Instance.updateSavedScores(3);
        //GameControl.Instance.updateSavedScores(1);
        //GameControl.Instance.updateSavedScores(5);
        for (int i = 0; i < 10; i++)
        {
            
            string tempName = "Score" + (i+1);
            GameObject temp = GameObject.Find(tempName);
            Text displayText = temp.GetComponent<Text>();
            displayText.text = "High Score " + (i+1) + " : "+ GameControl.Instance.highScores[i];
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
