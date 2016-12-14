using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    public int currentWave;
    public int[] highScores;
    // Use this for initialization
    public static GameControl Instance;

    void Awake()
    {
        highScores = new int[10];
        //highScores = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1};
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateSavedScores(int wave)
    {
        int legitSpot = -1;
        for (int i = 0; i < 10; i++)
        {
            if (wave > highScores[i])
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
            highScores[legitSpot] = wave;
            //saveData();
        }
    }
}
