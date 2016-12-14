using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public GameObject screenParent;
	public GameObject scoreParent;
	public UnityEngine.UI.Text loseText;
	public UnityEngine.UI.Text scoreText;

	// Use this for initialization
	void Start () {
		screenParent.SetActive (false);
	}

	// Update is called once per frame
	void Update () {

	}

	public void Lose()
	{
		screenParent.SetActive (true);
		scoreParent.SetActive (false);

		Animator animator = GetComponent<Animator> ();

		if (animator) {
			animator.Play ("GameOverShow");
		}
	}

	public void Win(int score, int starCount)
	{
		screenParent.SetActive (true);
		loseText.enabled = false;

		scoreText.text = score.ToString ();
		scoreText.enabled = false;

		Animator animator = GetComponent<Animator> ();

		if (animator) {
			animator.Play ("GameOverShow");
		}

		StartCoroutine (ShowWinCoroutine (starCount));
	}

	private IEnumerator ShowWinCoroutine(int starCount)
	{
		yield return new WaitForSeconds (0.5f);


		scoreText.enabled = true;
	}

	public void replayClicked()
	{
		SceneManager.LoadScene (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name);
	}

	public void doneClicked()
	{
		SceneManager.LoadScene ("levelSelect");
	}
}
