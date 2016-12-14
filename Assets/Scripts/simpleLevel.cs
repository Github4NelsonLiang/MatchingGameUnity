using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class simpleLevel : MonoBehaviour {

	public int timeInSeconds;
	public int targetScore;

	private float timer;
	private bool timeOut = false;

	public Grid grid;
	public HUD hud;

	private int currentScore;

	private bool didWin;

	// Use this for initialization
	void Start () {
		
		hud.SetScore (currentScore);
		hud.SetTarget (targetScore);
		hud.SetRemaining (string.Format ("{0}:{1:00}", timeInSeconds / 60, timeInSeconds % 60));
	}

	// Update is called once per frame
	void Update () {
		if (!timeOut) {
			timer += Time.deltaTime;
			hud.SetRemaining (string.Format ("{0}:{1:00}", (int)Mathf.Max((timeInSeconds - timer) / 60, 0), (int)Mathf.Max((timeInSeconds - timer) % 60, 0)));

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
		hud.SetScore (currentScore);
	}

	private IEnumerator WaitForGridFill()
	{
		while (grid.IsFilling) {
			yield return 0;
		}

		if (didWin) {
			// Go to highscore scene
			SceneManager.LoadScene("HighScore");
		} else {
			// go to game over scene
			SceneManager.LoadScene("GameOver");
		}
	}
}
