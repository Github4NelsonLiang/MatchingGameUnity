using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenController : MonoBehaviour {

	public void LoadScene(string sceneName){

		SceneManager.LoadScene (sceneName);

	}

	public void QuitGame(){

		Application.Quit ();

	}


/*
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
*/
}
