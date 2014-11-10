using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	[SerializeField] PositionLabel[] positionLabels;
	PositionLabel potentialMoveLabel;

	public void ClearPositionLabels()
	{
		foreach (PositionLabel positionLabel in positionLabels)
		{
			positionLabel.DisableMoveIndicator();
			positionLabel.DisableCaptureIndicator();
		}
	}

	public void FindSelectedCheckerMoves(GameObject selectedChecker)
	{
		CheckerContainer checkerContainerScript = selectedChecker.GetComponent<CheckerContainer>();
		int checkerPosition = checkerContainerScript.BoardLocation;

		if (checkerContainerScript.PieceColor == 1)
		{
			if (checkerContainerScript.PieceType == 1)
			{
				// Check if selected checker can move up and right
				if(checkerPosition % 8 != 0)
				{
					if (checkerPosition < 5 || (checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
					{
						potentialMoveLabel = positionLabels[(checkerPosition - 1) + 4].GetComponent<PositionLabel>();
						Debug.Log(checkerPosition);
						Debug.Log(potentialMoveLabel.PositionValue);

						if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
							potentialMoveLabel.EnableMoveIndicator();
					}
					else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25) || checkerPosition >= 29)
					{
						potentialMoveLabel = positionLabels[(checkerPosition - 1) + 5].GetComponent<PositionLabel>();
						Debug.Log(checkerPosition);
						Debug.Log(potentialMoveLabel.PositionValue);
						
						if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
							potentialMoveLabel.EnableMoveIndicator();
					}
				}
			}
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
			Debug.Log(positionLabels[11].OccupationValue);
		}
	}
}
