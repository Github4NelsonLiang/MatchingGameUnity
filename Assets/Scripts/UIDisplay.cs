using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour {

	public simpleLevel level;
	public Text remainingText;
	public Text remainingSubtext;
	public Text targetText;
	public Text targetSubtext;
	public Text scoreText;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetScore(int score)
	{
		scoreText.text = score.ToString ();

			
	}

	public void SetTarget(int target)
	{
		targetText.text = target.ToString ();
	}

	public void SetRemaining(int remaining)
	{
		remainingText.text = remaining.ToString ();
	}

	public void SetRemaining(string remaining)
	{
		remainingText.text = remaining;
	}

	public void SetLevelType()
	{
			remainingSubtext.text = "time remaining";
			targetSubtext.text = "target score";
	}
		
}
