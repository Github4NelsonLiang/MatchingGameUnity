using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class simpleLevel : MonoBehaviour {

	public int timeInSeconds;
	public int targetScore;

	private float timer;
	private bool timeOut = false;

	public Grid grid;
	public UIDisplay scoreDisplay;

	private int currentScore;

	private bool didWin;

	private GameControl dataObject;

	// Use this for initialization


	void Start () {

		scoreDisplay.SetScore (currentScore);
		scoreDisplay.SetTarget (targetScore);
		scoreDisplay.SetRemaining (string.Format ("{0}:{1:00}", timeInSeconds / 60, timeInSeconds % 60));
//		GameControl.Instance = new GameControl ();
		dataObject = GameControl.Instance;
//		dataObject = new GameControl();
	}

	// Update is called once per frame
	void Update () {
		if (!timeOut) {
			timer += Time.deltaTime;
			scoreDisplay.SetRemaining (string.Format ("{0}:{1:00}", (int)Mathf.Max((timeInSeconds - timer) / 60, 0), (int)Mathf.Max((timeInSeconds - timer) % 60, 0)));

			if (timeInSeconds - timer <= 0) {
				if (currentScore >= targetScore) {
					GameWin ();
				} else {
					GameLose ();
				}

				timeOut = true;
			}
		}
	}

	public void GameWin()
	{
		grid.GameOver ();
		didWin = true;
		StartCoroutine (WaitForGridFill ());
	}

	public void GameLose()
	{
		grid.GameOver ();
		didWin = false;
		StartCoroutine (WaitForGridFill ());
	}

	public void OnMove()
	{

	}

	public void OnPieceCleared(GamePiece piece)
	{
		currentScore += piece.score;
		scoreDisplay.SetScore (currentScore);
	}

	private IEnumerator WaitForGridFill()
	{
		while (grid.IsFilling) {
			yield return 0;
		}

		if (didWin) {
			// Go to highscore scene
			dataObject.updateScore(currentScore);
			Debug.Log ("Score is: " + currentScore);
			SceneManager.LoadScene("HighScore");
		} else {
			// go to game over scene
			dataObject.updateScore(currentScore);
			dataObject.currentScore = currentScore;
//			GameControl.Instance.updateScore (currentScore);
//			GameControl.Instance.currentScore = currentScore;
//			Debug.Log ("Score is: " + currentScore);

//			Debug.Log ("Score is: " + GameControl.Instance.currentScore);
			SceneManager.LoadScene("GameOver");
		}
	}
}
