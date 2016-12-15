
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public enum PieceType
	{
		EMPTY,
		NORMAL,
		BUBBLE,
		ROW_CLEAR,
		COLUMN_CLEAR
	};

	[System.Serializable]
	public struct PiecePrefab
	{
		public PieceType type;
		public GameObject prefab;
	};

	[System.Serializable]
	public struct PiecePosition
	{
		public PieceType type;
		public int x;
		public int y;
	};

	public int xDim;
	public int yDim;
	public float fillTime;

	public simpleLevel level;

	public PiecePrefab[] piecePrefabs;
	public GameObject backgroundPrefab;

	private Dictionary<PieceType, GameObject> piecePrefabDict;

	private GamePiece[,] pieces;

	private bool inverse = false;

	private GamePiece pressedPiece;
	private GamePiece enteredPiece;

	private bool gameOver = false;

	private bool processing = false;

	public bool IsFilling
	{
		get { return processing; }
	}

	void Awake () {
		piecePrefabDict = new Dictionary<PieceType, GameObject> ();

		for (int i = 0; i < piecePrefabs.Length; i++) {
			if (!piecePrefabDict.ContainsKey (piecePrefabs [i].type)) {
				piecePrefabDict.Add (piecePrefabs [i].type, piecePrefabs [i].prefab);
			}
		}

		for (int x = 0; x < xDim; x++) {
			for (int y = 0; y < yDim; y++) {
				GameObject background = (GameObject)Instantiate (backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
				background.transform.parent = transform;
			}
		}

		pieces = new GamePiece[xDim, yDim];

		for (int x = 0; x < xDim; x++) {
			for (int y = 0; y < yDim; y++) {
				if (pieces [x, y] == null) {
					CreateNewP (x, y, PieceType.EMPTY);
				}
			}
		}

		StartCoroutine(Fill ());
	}

	// Update is called once per frame
	void Update () {

	}

	public IEnumerator Fill()
	{
		bool refillNeeded = true;
		processing = true;

		while (refillNeeded) {
			yield return new WaitForSeconds (fillTime);

			while (FillStep ()) {
				inverse = !inverse;
				yield return new WaitForSeconds (fillTime);
			}

			refillNeeded = ClearAllValidMatches ();
		}

		processing = false;
	}

	public bool FillStep()
	{
		bool pieceFlag = false;

		for (int y = yDim-2; y >= 0; y--)
		{
			for (int innerLoop = 0; innerLoop < xDim; innerLoop++)
			{
				int x = innerLoop;

				if (inverse) {
					x = xDim - 1 - innerLoop;
				}

				GamePiece piece = pieces [x, y];

				if (piece.IsMovable ())
				{
					GamePiece pieceBelow = pieces [x, y + 1];

					if (pieceBelow.Type == PieceType.EMPTY) {
						Destroy (pieceBelow.gameObject);
						piece.MovableComponent.Move (x, y + 1, fillTime);
						pieces [x, y + 1] = piece;
						CreateNewP (x, y, PieceType.EMPTY);
						pieceFlag = true;
					}
				}
			}
		}

		for (int x = 0; x < xDim; x++)
		{
			GamePiece pieceBelow = pieces [x, 0];

			if (pieceBelow.Type == PieceType.EMPTY)
			{
				Destroy (pieceBelow.gameObject);
				GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
				newPiece.transform.parent = transform;

				pieces [x, 0] = newPiece.GetComponent<GamePiece> ();
				pieces [x, 0].Init (x, -1, this, PieceType.NORMAL);
				pieces [x, 0].MovableComponent.Move (x, 0, fillTime);
				pieces [x, 0].ColorComponent.SetColor ((ColorPiece.ColorType)Random.Range (0, pieces [x, 0].ColorComponent.NumColors));
				pieceFlag = true;
			}
		}

		return pieceFlag;
	}

	public Vector2 GetWorldPosition(int x, int y)
	{
		return new Vector2 (transform.position.x - xDim / 2.0f + x,
			transform.position.y + yDim / 2.0f - y);
	}

	public GamePiece CreateNewP(int x, int y, PieceType type)
	{
		GameObject newPiece = (GameObject)Instantiate (piecePrefabDict [type], GetWorldPosition (x, y), Quaternion.identity);
		newPiece.transform.parent = transform;

		pieces [x, y] = newPiece.GetComponent<GamePiece> ();
		pieces [x, y].Init (x, y, this, type);

		return pieces [x, y];
	}

	public bool IsAdjacent(GamePiece firstP, GamePiece secondP)
	{
		return (firstP.X == secondP.X && (int)Mathf.Abs (firstP.Y - secondP.Y) == 1)
			|| (firstP.Y == secondP.Y && (int)Mathf.Abs (firstP.X - secondP.X) == 1);
	}

	public void Swap(GamePiece firstP, GamePiece secondP)
	{
		if (gameOver) {
			return;
		}

		if (firstP.IsMovable () && secondP.IsMovable ()) {
			pieces [firstP.X, firstP.Y] = secondP;
			pieces [secondP.X, secondP.Y] = firstP;

			if (findMatches (firstP, secondP.X, secondP.Y) != null || findMatches (secondP, firstP.X, firstP.Y) != null) {

				int piece1X = firstP.X;
				int piece1Y = firstP.Y;

				firstP.MovableComponent.Move (secondP.X, secondP.Y, fillTime);
				secondP.MovableComponent.Move (piece1X, piece1Y, fillTime);


				ClearAllValidMatches ();

				if (firstP.Type == PieceType.ROW_CLEAR || firstP.Type == PieceType.COLUMN_CLEAR) {
					ClearPiece (firstP.X, firstP.Y);
				}

				if (secondP.Type == PieceType.ROW_CLEAR || secondP.Type == PieceType.COLUMN_CLEAR) {
					ClearPiece (secondP.X, secondP.Y);
				}

				pressedPiece = null;
				enteredPiece = null;

				StartCoroutine (Fill ());

				level.OnMove ();
			} else {
				pieces [firstP.X, firstP.Y] = firstP;
				pieces [secondP.X, secondP.Y] = secondP;
			}
		}
	}

	public void PressPiece(GamePiece piece)
	{
		pressedPiece = piece;
	}

	public void EnterPiece(GamePiece piece)
	{
		enteredPiece = piece;
	}

	public void ReleasePiece()
	{
		if (IsAdjacent (pressedPiece, enteredPiece)) {
			Swap (pressedPiece, enteredPiece);
		}
	}

	public List<GamePiece> findMatches(GamePiece piece, int newX, int newY)
	{
		if (piece.IsColored ()) {
			ColorPiece.ColorType color = piece.ColorComponent.Color;
			List<GamePiece> horizontalPieces = new List<GamePiece> ();
			List<GamePiece> verticalPieces = new List<GamePiece> ();
			List<GamePiece> matchingPieces = new List<GamePiece> ();

			// First check horizontal
			horizontalPieces.Add(piece);

			for (int searchDirection = 0; searchDirection <= 1; searchDirection++) {
				for (int xOffset = 1; xOffset < xDim; xOffset++) {
					int x;

					if (searchDirection == 0) { // Left
						x = newX - xOffset;
					} else { // Right
						x = newX + xOffset;
					}

					if (x < 0 || x >= xDim) {
						break;
					}

					if (pieces [x, newY].IsColored () && pieces [x, newY].ColorComponent.Color == color) {
						horizontalPieces.Add (pieces [x, newY]);
					} else {
						break;
					}
				}
			}

			if (horizontalPieces.Count >= 3) {
				for (int i = 0; i < horizontalPieces.Count; i++) {
					matchingPieces.Add (horizontalPieces [i]);
				}
			}


			if (matchingPieces.Count >= 3) {
				return matchingPieces;
			}

			// Didn't find anything going horizontally first,
			// so now check vertically
			horizontalPieces.Clear();
			verticalPieces.Clear ();
			verticalPieces.Add(piece);

			for (int searchDirection = 0; searchDirection <= 1; searchDirection++) {
				for (int yOffset = 1; yOffset < yDim; yOffset++) {
					int y;

					if (searchDirection == 0) { // Up
						y = newY - yOffset;
					} else { // Down
						y = newY + yOffset;
					}

					if (y < 0 || y >= yDim) {
						break;
					}

					if (pieces [newX, y].IsColored () && pieces [newX, y].ColorComponent.Color == color) {
						verticalPieces.Add (pieces [newX, y]);
					} else {
						break;
					}
				}
			}

			if (verticalPieces.Count >= 3) {
				for (int i = 0; i < verticalPieces.Count; i++) {
					matchingPieces.Add (verticalPieces [i]);
				}
			}

			if (matchingPieces.Count >= 3) {
				return matchingPieces;
			}
		}

		return null;
	}

	public bool ClearAllValidMatches()
	{
		bool refillNeeded = false;

		for (int y = 0; y < yDim; y++) {
			for (int x = 0; x < xDim; x++) {
				if (pieces [x, y].IsClearable ()) {
					List<GamePiece> match = findMatches (pieces [x, y], x, y);

					if (match != null) {
						PieceType newPieceType = PieceType.NORMAL;
						GamePiece randomPiece = match [Random.Range (0, match.Count)];
						int specialPieceX = randomPiece.X;
						int specialPieceY = randomPiece.Y;

						if (match.Count >= 3) {
							if (pressedPiece == null || enteredPiece == null) {
								newPieceType = (PieceType)Random.Range ((int)PieceType.ROW_CLEAR, (int)PieceType.COLUMN_CLEAR);
							} else if (pressedPiece.Y == enteredPiece.Y) {
								newPieceType = PieceType.ROW_CLEAR;
							} else {
								newPieceType = PieceType.COLUMN_CLEAR;
							}
						}

						for (int i = 0; i < match.Count; i++) {
							if (ClearPiece (match [i].X, match [i].Y)) {
								refillNeeded = true;

								if (match [i] == pressedPiece || match [i] == enteredPiece) {
									specialPieceX = match [i].X;
									specialPieceY = match [i].Y;
								}
							}
						}

						if (newPieceType != PieceType.NORMAL) {
							Destroy (pieces [specialPieceX, specialPieceY]);
							GamePiece newPiece = CreateNewP (specialPieceX, specialPieceY, newPieceType);

							if ((newPieceType == PieceType.ROW_CLEAR || newPieceType == PieceType.COLUMN_CLEAR)
								&& newPiece.IsColored () && match [0].IsColored ()) {
								newPiece.ColorComponent.SetColor (match [0].ColorComponent.Color);
							}
						}
					}
				}
			}
		}

		return refillNeeded;
	}

	public bool ClearPiece(int x, int y)
	{
		if (pieces [x, y].IsClearable () && !pieces [x, y].ClearableComponent.IsBeingCleared) {
			pieces [x, y].ClearableComponent.Clear ();
			CreateNewP (x, y, PieceType.EMPTY);

			return true;
		}

		return false;
	}

	public void GameOver()
	{
		gameOver = true;
	}

}
