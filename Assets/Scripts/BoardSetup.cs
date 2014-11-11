using UnityEngine;
using System.Collections;

public class BoardSetup : MonoBehaviour
{
	[SerializeField] GameObject whitePrefab;
	[SerializeField] GameObject redPrefab;
	[SerializeField] PositionLabel[] whiteStartPositions;
	[SerializeField] PositionLabel[] redStartPositions;
	int whiteSpawnCount = 1;
	int redSpawnCount = 1;

	void Start ()
	{
		foreach (PositionLabel whiteStartPosition in whiteStartPositions)
		{
			whiteStartPosition.OccupationValue = 1;
			var clone = Instantiate(whitePrefab, whiteStartPosition.transform.position, whiteStartPosition.transform.rotation) as GameObject;
			clone.name = "White_" + whiteSpawnCount;
			whiteSpawnCount++;

			CheckerContainer checkerContainerScript = clone.GetComponent<CheckerContainer>();
			whiteStartPosition.OccupyingChecker = checkerContainerScript;
			checkerContainerScript.BoardLocation = whiteStartPosition.PositionValue;
		}

		foreach (PositionLabel redStartPosition in redStartPositions)
		{
			redStartPosition.OccupationValue = 2;
			var clone = Instantiate(redPrefab, redStartPosition.transform.position, redStartPosition.transform.rotation) as GameObject;
			clone.name = "Red_" + redSpawnCount;
			redSpawnCount++;

			CheckerContainer checkerContainerScript = clone.GetComponent<CheckerContainer>();
			redStartPosition.OccupyingChecker = checkerContainerScript;
			checkerContainerScript.BoardLocation = redStartPosition.PositionValue;
		}
	}
}
