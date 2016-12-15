using UnityEngine;
using System.Collections;

public class MovablePiece : MonoBehaviour {

	private GamePiece piece;
	private IEnumerator moveCoroutine;

	void Awake() {
		piece = GetComponent<GamePiece> ();
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void Move(int xVal, int yVal, float tVal)
	{
		if (moveCoroutine != null) {
			StopCoroutine (moveCoroutine);
		}

		moveCoroutine = MoveCoroutine (xVal, yVal, tVal);
		StartCoroutine (moveCoroutine);
	}

	public IEnumerator MoveCoroutine(int xVal, int yVal, float tVal)
	{
		piece.X = xVal;
		piece.Y = yVal;

		Vector3 startPos = transform.position;
		Vector3 endPos = piece.GridRef.GetWorldPosition (xVal, yVal);

		for (float t = 0; t <= 1 * tVal; t += Time.deltaTime) {
			piece.transform.position = Vector3.Lerp (startPos, endPos, t / tVal);
			yield return 0;
		}

		piece.transform.position = piece.GridRef.GetWorldPosition (xVal, yVal);
	}
}
