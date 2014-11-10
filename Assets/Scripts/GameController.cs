using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	[SerializeField] PositionLabel[] positionLabels;
	GameObject checkerOfInterest;
	PositionLabel potentialMoveLabel;
	CheckerContainer checkerContainerScript;
	int checkerPosition;
	public GameObject CheckerOfInterest { get { return checkerOfInterest; } set { checkerOfInterest = value; }}

	public void ClearPositionLabels()
	{
		foreach (PositionLabel positionLabel in positionLabels)
		{
			positionLabel.DisableMoveIndicator();
			positionLabel.DisableCaptureIndicator();
		}
	}

	public void FindSelectedCheckerOptions(GameObject selectedChecker)
	{
		CheckerOfInterest = selectedChecker;
		checkerContainerScript = selectedChecker.GetComponent<CheckerContainer>();
		checkerPosition = checkerContainerScript.BoardLocation;
		SelectedCheckerMoves(selectedChecker);
	}

	void SelectedCheckerMoves(GameObject selectedChecker)
	{
		if (checkerContainerScript.PieceColor == 1)
		{
			if (checkerContainerScript.PieceType == 1)
			{
				CheckUpLeft();
				CheckUpRight();
			}
			else
				CheckAll();
		}
	}

	void CheckUpLeft()
	{
		if(checkerPosition != 1 && (checkerPosition - 1) % 8 != 0)
		{
			if (checkerPosition < 5 || (checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
			{
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 3].GetComponent<PositionLabel>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
					potentialMoveLabel.EnableMoveIndicator();
			}
			else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25) || checkerPosition >= 29)
			{
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 4].GetComponent<PositionLabel>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
					potentialMoveLabel.EnableMoveIndicator();
			}
		}
	}

	void CheckUpRight()
	{
		if(checkerPosition % 8 != 0)
		{
			if (checkerPosition < 5 || (checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
			{
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 4].GetComponent<PositionLabel>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
					potentialMoveLabel.EnableMoveIndicator();
			}
			else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25) || checkerPosition >= 29)
			{
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 5].GetComponent<PositionLabel>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
					potentialMoveLabel.EnableMoveIndicator();
			}
		}
	}

	void CheckAll()
	{
		CheckUpLeft();
		CheckUpRight();
	}
}
