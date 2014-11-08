using UnityEngine;
using System.Collections;

public class BoardSetup : MonoBehaviour
{
	[SerializeField] GameObject whitePrefab;
	[SerializeField] GameObject redPrefab;
	[SerializeField] Transform[] whiteStartPositions;
	[SerializeField] Transform[] redStartPositions;
	int whiteSpawnCount = 1;
	int redSpawnCount = 1;

	void Start ()
	{
		foreach (Transform whiteStartPosition in whiteStartPositions)
		{
			var clone = Instantiate(whitePrefab, whiteStartPosition.position, whiteStartPosition.rotation);
			clone.name = "White_" + whiteSpawnCount;
			whiteSpawnCount++;
		}

		foreach (Transform redStartPosition in redStartPositions)
		{
			var clone = Instantiate(redPrefab, redStartPosition.position, redStartPosition.rotation);
			clone.name = "Red_" + redSpawnCount;
			redSpawnCount++;
		}
	}
}
