using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    public int currentScore;
    public int[] highScores;
    // Use this for initialization
	public static GameControl Instance;

	public GameControl(){
		highScores = new int[10];


	}

    void Awake()
    {
        highScores = new int[10];
//        highScores = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1};
        if (Instance == null)
        {
			DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

//		DontDestroyOnLoad(gameObject);
//		Instance = this;
    }

    void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    public void updateScore(int newScore)
    {
        int legitSpot = -1;
        for (int i = 0; i < 10; i++)
        {
            if (newScore > highScores[i])
            {
                legitSpot = i;
                break;
            }
        }

        if (legitSpot > -1)
        {
            for (int j = 9; j > legitSpot; j--)
            {
                highScores[j] = highScores[j - 1];
            }
            highScores[legitSpot] = newScore;
            //saveData();
        }

//		for (int i = 0; i < 10; i++)
//		{
//			Debug.Log (i+"Printing::::"+highScores [i]);
//		}
    }
}
