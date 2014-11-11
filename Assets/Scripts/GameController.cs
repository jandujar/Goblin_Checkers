using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	[SerializeField] bool whiteStart = true;
	[SerializeField] PositionLabel[] positionLabels;
	GameObject checkerOfInterest;
	bool whiteTurn;
	PositionLabel potentialMoveLabel;
	CheckerContainer checkerContainerScript;
	int checkerPosition;
	public GameObject CheckerOfInterest { get { return checkerOfInterest; } set { checkerOfInterest = value; }}
	public bool WhiteTurn { get { return whiteTurn; } set { whiteTurn = value; }}

	void Start()
	{
		if (whiteStart)
			whiteTurn = true;
		else
			whiteTurn = false;
	}

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

		if (checkerContainerScript.PieceColor == 1 && WhiteTurn)
		{
			checkerPosition = checkerContainerScript.BoardLocation;
			SelectedCheckerMoves(selectedChecker);
		}
		else if (checkerContainerScript.PieceColor == 2 && !WhiteTurn)
		{
			checkerPosition = checkerContainerScript.BoardLocation;
			SelectedCheckerMoves(selectedChecker);
		}
	}

	void SelectedCheckerMoves(GameObject selectedChecker)
	{
		if (checkerContainerScript.PieceType == 1)
		{
			if (checkerContainerScript.PieceColor == 1)
			{
				CheckUpLeft();
				CheckUpRight();
			}
			else if (checkerContainerScript.PieceColor == 2)
			{
				CheckDownLeft();
				CheckDownRight();
			}
		}
		else if (checkerContainerScript.PieceType == 2)
			CheckAll();
	}

	void CheckUpLeft()
	{
		if(checkerPosition != 1 && (checkerPosition - 1) % 8 != 0)
		{
			if (checkerPosition < 5 || (checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
			{
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 3].GetComponent<PositionLabel>();

				if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
				{
					potentialMoveLabel = positionLabels[(checkerPosition - 1) + 7].GetComponent<PositionLabel>();

					if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
					potentialMoveLabel.EnableMoveIndicator();
			}
			else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25))
			{
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 4].GetComponent<PositionLabel>();

				if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
				{
					potentialMoveLabel = positionLabels[(checkerPosition - 1) + 7].GetComponent<PositionLabel>();
					
					if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
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

				if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
				{
					potentialMoveLabel = positionLabels[(checkerPosition - 1) + 9].GetComponent<PositionLabel>();
					
					if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
					potentialMoveLabel.EnableMoveIndicator();
			}
			else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25))
			{
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 5].GetComponent<PositionLabel>();

				if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
				{
					potentialMoveLabel = positionLabels[(checkerPosition - 1) + 9].GetComponent<PositionLabel>();
					
					if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
					potentialMoveLabel.EnableMoveIndicator();
			}
		}
	}

	void CheckDownLeft()
	{
		if(checkerPosition != 1 && (checkerPosition - 1) % 8 != 0)
		{
			if ((checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
			{
				potentialMoveLabel = positionLabels[(checkerPosition - 1) - 5].GetComponent<PositionLabel>();

				if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
				{
					potentialMoveLabel = positionLabels[(checkerPosition - 1) - 9].GetComponent<PositionLabel>();
					
					if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
					potentialMoveLabel.EnableMoveIndicator();
			}
			else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25) || checkerPosition >= 29)
			{
				potentialMoveLabel = positionLabels[(checkerPosition - 1) - 4].GetComponent<PositionLabel>();

				if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
				{
					potentialMoveLabel = positionLabels[(checkerPosition - 1) - 9].GetComponent<PositionLabel>();
					
					if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
					potentialMoveLabel.EnableMoveIndicator();
			}
		}
	}
	
	void CheckDownRight()
	{
		if(checkerPosition % 8 != 0)
		{
			if ((checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
			{
				potentialMoveLabel = positionLabels[(checkerPosition - 1) - 4].GetComponent<PositionLabel>();

				if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
				{
					potentialMoveLabel = positionLabels[(checkerPosition - 1) - 7].GetComponent<PositionLabel>();
					
					if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
					potentialMoveLabel.EnableMoveIndicator();
			}
			else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25) || checkerPosition >= 29)
			{
				potentialMoveLabel = positionLabels[(checkerPosition - 1) - 3].GetComponent<PositionLabel>();

				if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
				{
					potentialMoveLabel = positionLabels[(checkerPosition - 1) - 7].GetComponent<PositionLabel>();
					
					if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
					potentialMoveLabel.EnableMoveIndicator();
			}
		}
	}

	void CheckAll()
	{
		CheckUpLeft();
		CheckUpRight();
	}

	public void ResetOccupationValue(int positionValue)
	{
		positionLabels[positionValue - 1].OccupationValue = 0;
	}
}
