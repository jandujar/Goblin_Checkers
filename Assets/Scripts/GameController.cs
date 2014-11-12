using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	[SerializeField] bool whiteStart = true;
	[SerializeField] PositionLabel[] positionLabels;
	[SerializeField] int[] upLeftMoveExclusions;
	[SerializeField] int[] upRightMoveExclusions;
	[SerializeField] int[] downLeftMoveExclusions;
	[SerializeField] int[] downRightMoveExclusions;
	[SerializeField] int[] upLeftCaptureExclusions;
	[SerializeField] int[] upRightCaptureExclusions;
	[SerializeField] int[] downLeftCaptureExclusions;
	[SerializeField] int[] downRightCaptureExclusions;
	GameObject checkerOfInterest;
	bool whiteTurn;
	bool canCapture = false;
	bool capturePrecheck = false;
	bool captureRequired = false;
	bool capturePerformed = false;
	GameObject captureObject;
	PositionLabel potentialMoveLabel;
	CheckerContainer checkerContainerScript;
	int checkerPosition;
	public GameObject CheckerOfInterest { get { return checkerOfInterest; } set { checkerOfInterest = value; }}
	public bool WhiteTurn { get { return whiteTurn; } set { whiteTurn = value; }}
	public GameObject CaptureObject { get { return captureObject; } set { captureObject = value; }}
	public bool CanCapture { get { return canCapture; }}
	public bool CapturePerformed { get { return capturePerformed; } set { capturePerformed = value; }}

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

		canCapture = false;
	}

	public void FindCaptures()
	{
		capturePrecheck = true;
		captureRequired = false;

		if (WhiteTurn)
		{
			GameObject[] whiteCheckers = GameObject.FindGameObjectsWithTag("White");

			if (whiteCheckers != null)
			{
				foreach (GameObject whiteChecker in whiteCheckers)
				{
					checkerContainerScript = whiteChecker.GetComponent<CheckerContainer>();
					checkerPosition = checkerContainerScript.BoardLocation;

					if (checkerContainerScript.PieceColor == 1)
					{
						CheckUpLeftCapture();
						CheckUpRightCapture();
					}
					else
						CheckAllCapture();
				}
			}
		}
		else
		{
			GameObject[] redCheckers = GameObject.FindGameObjectsWithTag("Red");
			
			if (redCheckers != null)
			{
				foreach (GameObject redChecker in redCheckers)
				{
					checkerContainerScript = redChecker.GetComponent<CheckerContainer>();
					checkerPosition = checkerContainerScript.BoardLocation;
					
					if (checkerContainerScript.PieceColor == 2)
					{
						CheckDownLeftCapture();
						CheckDownRightCapture();
					}
					else
						CheckAllCapture();
				}
			}
		}

		capturePrecheck = false;
	}

	public void FindSelectedCheckerOptions(GameObject selectedChecker)
	{
		CheckerOfInterest = selectedChecker;
		checkerContainerScript = selectedChecker.GetComponent<CheckerContainer>();
		canCapture = false;

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
				CheckUpLeftCapture();
				CheckUpRightCapture();
			}
			else if (checkerContainerScript.PieceColor == 2)
			{
				CheckDownLeftCapture();
				CheckDownRightCapture();
			}
		}
		else if (checkerContainerScript.PieceType == 2)
			CheckAllCapture();

		if (!captureRequired && !CanCapture)
		{
			if (checkerContainerScript.PieceType == 1)
			{
				if (checkerContainerScript.PieceColor == 1)
				{
					CheckUpLeftMove();
					CheckUpRightMove();
				}
				else if (checkerContainerScript.PieceColor == 2)
				{
					CheckDownLeftMove();
					CheckDownRightMove();
				}
			}
			else if (checkerContainerScript.PieceType == 2)
				CheckAllMove();
		}
		else if (captureRequired && !CanCapture)
			Debug.Log("Capture Required");
	}

	void CheckUpLeftMove()
	{
		foreach (int exclusionNumber in upLeftMoveExclusions)
		{
			if (exclusionNumber == checkerPosition)
				return;
		}

		if (checkerPosition < 5 || (checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 3].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
				potentialMoveLabel.EnableMoveIndicator();
		}
		else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 4].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
				potentialMoveLabel.EnableMoveIndicator();
		}
	}

	void CheckUpLeftCapture()
	{
		foreach (int exclusionNumber in upLeftCaptureExclusions)
		{
			if (exclusionNumber == checkerPosition)
				return;
		}

		if (checkerPosition < 5 || (checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 3].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 7].GetComponent<PositionLabel>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCapture = true;
					captureRequired = true;
					CaptureObject = potentialObject;

					if (!capturePrecheck)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else
					canCapture = false;
			}
		}
		else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 4].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 7].GetComponent<PositionLabel>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCapture = true;
					captureRequired = true;
					CaptureObject = potentialObject;
					
					if (!capturePrecheck)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else
					canCapture = false;
			}
		}
	}

	void CheckUpRightMove()
	{
		foreach (int exclusionNumber in upRightMoveExclusions)
		{
			if (exclusionNumber == checkerPosition)
				return;
		}

		if (checkerPosition < 5 || (checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 4].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
				potentialMoveLabel.EnableMoveIndicator();
		}
		else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 5].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
				potentialMoveLabel.EnableMoveIndicator();
		}
	}

	void CheckUpRightCapture()
	{
		foreach (int exclusionNumber in upRightCaptureExclusions)
		{
			if (exclusionNumber == checkerPosition)
				return;
		}

		if (checkerPosition < 5 || (checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 4].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 9].GetComponent<PositionLabel>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCapture = true;
					captureRequired = true;
					CaptureObject = potentialObject;
					
					if (!capturePrecheck)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else
					canCapture = false;
			}
		}
		else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 5].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 9].GetComponent<PositionLabel>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCapture = true;
					captureRequired = true;
					CaptureObject = potentialObject;
					
					if (!capturePrecheck)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else
					canCapture = false;
			}
		}
	}

	void CheckDownLeftMove()
	{
		foreach (int exclusionNumber in downLeftMoveExclusions)
		{
			if (exclusionNumber == checkerPosition)
				return;
		}

		if ((checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 5].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
				potentialMoveLabel.EnableMoveIndicator();
		}
		else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25) || checkerPosition >= 29)
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 4].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
				potentialMoveLabel.EnableMoveIndicator();
		}
	}

	void CheckDownLeftCapture()
	{
		foreach (int exclusionNumber in downLeftCaptureExclusions)
		{
			if (exclusionNumber == checkerPosition)
				return;
		}

		if ((checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 5].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) - 9].GetComponent<PositionLabel>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCapture = true;
					captureRequired = true;
					CaptureObject = potentialObject;
					
					if (!capturePrecheck)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else
					canCapture = false;
			}
		}
		else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25) || checkerPosition >= 29)
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 4].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) - 9].GetComponent<PositionLabel>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCapture = true;
					captureRequired = true;
					CaptureObject = potentialObject;
					
					if (!capturePrecheck)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else
					canCapture = false;
			}
		}
	}
	
	void CheckDownRightMove()
	{
		foreach (int exclusionNumber in downRightMoveExclusions)
		{
			if (exclusionNumber == checkerPosition)
				return;
		}

		if ((checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 4].GetComponent<PositionLabel>();

			if (checkerPosition % 8 != 0 && potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
				potentialMoveLabel.EnableMoveIndicator();
		}
		else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25) || checkerPosition >= 29)
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 3].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
				potentialMoveLabel.EnableMoveIndicator();
		}
	}

	void CheckDownRightCapture()
	{
		foreach (int exclusionNumber in downRightCaptureExclusions)
		{
			if (exclusionNumber == checkerPosition)
				return;
		}

		if ((checkerPosition >= 9 && checkerPosition < 13) || (checkerPosition >= 17 && checkerPosition < 21) || (checkerPosition >= 25 && checkerPosition < 29))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 4].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) - 7].GetComponent<PositionLabel>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCapture = true;
					captureRequired = true;
					CaptureObject = potentialObject;
					
					if (!capturePrecheck)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else
					canCapture = false;
			}
		}
		else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25) || checkerPosition >= 29)
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 3].GetComponent<PositionLabel>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) - 7].GetComponent<PositionLabel>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCapture = true;
					captureRequired = true;
					CaptureObject = potentialObject;
					
					if (!capturePrecheck)
						potentialMoveLabel.EnableCaptureIndicator();
				}
				else
					canCapture = false;
			}
		}
	}

	void CheckAllMove()
	{
		CheckUpLeftMove();
		CheckUpRightMove();
		CheckDownLeftMove();
		CheckDownRightMove();
	}

	void CheckAllCapture()
	{
		CheckUpLeftCapture();
		CheckUpRightCapture();
		CheckDownLeftCapture();
		CheckDownRightCapture();
	}

	public void ResetOccupationValue(int positionValue)
	{
		positionLabels[positionValue - 1].OccupyingChecker = null;
		positionLabels[positionValue - 1].OccupationValue = 0;
	}
}
