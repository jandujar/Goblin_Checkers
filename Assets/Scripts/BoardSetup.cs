using UnityEngine;
using System.Collections;

public class BoardSetup : MonoBehaviour
{
	[SerializeField] GameController gameController;
	[SerializeField] GameObject whitePrefab;
	[SerializeField] GameObject redPrefab;
	[SerializeField] PositionContainer[] whiteStartPositions;
	[SerializeField] PositionContainer[] redStartPositions;
	int whiteSpawnCount = 1;
	int redSpawnCount = 1;

	void Start ()
	{
		gameController.whiteCheckers.Clear();
		gameController.redCheckers.Clear();

		foreach (PositionContainer whiteStartPosition in whiteStartPositions)
		{
			whiteStartPosition.OccupationValue = 1;
			var clone = Instantiate(whitePrefab, whiteStartPosition.transform.position, whiteStartPosition.transform.rotation) as GameObject;
			clone.name = "White_" + whiteSpawnCount;
			whiteSpawnCount++;

			CheckerContainer checkerContainerScript = clone.GetComponent<CheckerContainer>();
			whiteStartPosition.OccupyingChecker = checkerContainerScript;
			checkerContainerScript.BoardLocation = whiteStartPosition.PositionValue;

			gameController.whiteCheckers.Add(clone);
		}

		foreach (PositionContainer redStartPosition in redStartPositions)
		{
			redStartPosition.OccupationValue = 2;
			var clone = Instantiate(redPrefab, redStartPosition.transform.position, redStartPosition.transform.rotation) as GameObject;
			clone.name = "Red_" + redSpawnCount;
			redSpawnCount++;

			CheckerContainer checkerContainerScript = clone.GetComponent<CheckerContainer>();
			redStartPosition.OccupyingChecker = checkerContainerScript;
			checkerContainerScript.BoardLocation = redStartPosition.PositionValue;

			gameController.redCheckers.Add(clone);
		}

		gameController.CheckerCounter.text = "White: " + gameController.whiteCheckers.Count + " Red: " + gameController.redCheckers.Count;
	}
}
