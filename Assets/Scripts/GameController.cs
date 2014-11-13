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
	bool canCaptureUL = false;
	bool canCaptureUR = false;
	bool canCaptureDL = false;
	bool canCaptureDR = false;
	bool capturePrecheck = false;
	bool captureRequired = false;
	bool capturePerformed = false;
	bool recaptureCheck = false;
	bool canRecapture = false;
	GameObject captureObjectUL;
	GameObject captureObjectUR;
	GameObject captureObjectDL;
	GameObject captureObjectDR;
	PositionLabel potentialMoveLabel;
	CheckerContainer checkerContainerScript;
	int checkerPosition;
	public GameObject CheckerOfInterest { get { return checkerOfInterest; } set { checkerOfInterest = value; }}
	public bool WhiteTurn { get { return whiteTurn; } set { whiteTurn = value; }}
	public bool CanCaptureUL { get { return canCaptureUL; }}
	public bool CanCaptureUR { get { return canCaptureUR; }}
	public bool CanCaptureDL { get { return canCaptureDL; }}
	public bool CanCaptureDR { get { return canCaptureDR; }}
	public GameObject CaptureObjectUL { get { return captureObjectUL; } set { captureObjectUL = value; }}
	public GameObject CaptureObjectUR { get { return captureObjectUR; } set { captureObjectUR = value; }}
	public GameObject CaptureObjectDL { get { return captureObjectDL; } set { captureObjectDL = value; }}
	public GameObject CaptureObjectDR { get { return captureObjectDR; } set { captureObjectDR = value; }}
	public bool CapturePerformed { get { return capturePerformed; } set { capturePerformed = value; }}
	public bool RecaptureCheck { get { return recaptureCheck; }}
	public bool CanRecapture { get { return canRecapture; }}

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

		canCaptureUL = false;
		canCaptureUR = false;
		canCaptureDL = false;
		canCaptureDR = false;
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

	public void FindAdditionalCaptures(GameObject capturingChecker)
	{
		CheckerOfInterest = capturingChecker;
		checkerContainerScript = capturingChecker.GetComponent<CheckerContainer>();
		checkerPosition = checkerContainerScript.BoardLocation;
		recaptureCheck = true;
		canRecapture = false;

		if (checkerContainerScript.PieceColor == 1)
		{
			if (checkerContainerScript.PieceType == 1)
			{
				CheckUpLeftCapture();
				CheckUpRightCapture();
			}
			else
				CheckAllCapture();
		}
		else if (checkerContainerScript.PieceColor == 2 && !WhiteTurn)
		{
			if (checkerContainerScript.PieceType == 1)
			{
				CheckDownLeftCapture();
				CheckDownRightCapture();
			}
			else
				CheckAllCapture();
		}

		recaptureCheck = false;
	}

	public void FindSelectedCheckerOptions(GameObject selectedChecker)
	{
		CheckerOfInterest = selectedChecker;
		checkerContainerScript = selectedChecker.GetComponent<CheckerContainer>();
		checkerPosition = checkerContainerScript.BoardLocation;
		canCaptureUL = false;
		canCaptureUR = false;
		canCaptureDL = false;
		canCaptureDR = false;

		if (checkerContainerScript.PieceColor == 1 && WhiteTurn)
			SelectedCheckerMoves(selectedChecker);
		else if (checkerContainerScript.PieceColor == 2 && !WhiteTurn)
			SelectedCheckerMoves(selectedChecker);
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

		if (!captureRequired && !canCaptureUL && !canCaptureUR && !canCaptureDL && !canCaptureDR)
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
		else if (captureRequired && !canCaptureUL && !canCaptureUR && !canCaptureDL && !canCaptureDR)
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
					canCaptureUL = true;
					captureRequired = true;
					potentialMoveLabel.MoveDirection = "UL";
					CaptureObjectUL = potentialObject;

					if (!capturePrecheck)
						potentialMoveLabel.EnableCaptureIndicator();

					if (recaptureCheck)
						canRecapture = true;
				}
			}
			else
			{
				canCaptureUL = false;
				potentialMoveLabel.MoveDirection = null;
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
					canCaptureUL = true;
					captureRequired = true;
					potentialMoveLabel.MoveDirection = "UL";
					CaptureObjectUL = potentialObject;
					
					if (!capturePrecheck)
					{
						potentialMoveLabel.EnableCaptureIndicator();
					}

					if (recaptureCheck)
						canRecapture = true;
				}
			}
			else
			{
				canCaptureUL = false;
				potentialMoveLabel.MoveDirection = "UL";
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
					canCaptureUR = true;
					captureRequired = true;
					potentialMoveLabel.MoveDirection = "UR";
					CaptureObjectUR = potentialObject;
					
					if (!capturePrecheck)
					{
						potentialMoveLabel.EnableCaptureIndicator();
					}

					if (recaptureCheck)
						canRecapture = true;
				}
			}
			else
			{
				canCaptureUR = false;
				potentialMoveLabel.MoveDirection = null;
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
					canCaptureUR = true;
					captureRequired = true;
					potentialMoveLabel.MoveDirection = "UR";
					CaptureObjectUR = potentialObject;
					
					if (!capturePrecheck)
					{
						potentialMoveLabel.EnableCaptureIndicator();
					}

					if (recaptureCheck)
						canRecapture = true;
				}
			}
			else
			{
				canCaptureUR = false;
				potentialMoveLabel.MoveDirection = null;
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
					canCaptureDL = true;
					captureRequired = true;
					potentialMoveLabel.MoveDirection = "DL";
					CaptureObjectDL = potentialObject;
					Debug.Log("HERE :" + CaptureObjectDL.name);
					
					if (!capturePrecheck)
					{
						potentialMoveLabel.EnableCaptureIndicator();
					}

					if (recaptureCheck)
						canRecapture = true;
				}
			}
			else
			{
				canCaptureUL = false;
				potentialMoveLabel.MoveDirection = null;
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
					canCaptureDL = true;
					captureRequired = true;
					potentialMoveLabel.MoveDirection = "DL";
					CaptureObjectDL = potentialObject;
					
					if (!capturePrecheck)
					{
						potentialMoveLabel.EnableCaptureIndicator();
					}

					if (recaptureCheck)
						canRecapture = true;
				}
			}
			else
			{
				canCaptureUL = false;
				potentialMoveLabel.MoveDirection = null;
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
					canCaptureDR = true;
					captureRequired = true;
					potentialMoveLabel.MoveDirection = "DR";
					CaptureObjectDR = potentialObject;
					
					if (!capturePrecheck)
					{
						potentialMoveLabel.EnableCaptureIndicator();
					}

					if (recaptureCheck)
						canRecapture = true;
				}
			}
			else
			{
				canCaptureDR = false;
				potentialMoveLabel.MoveDirection = null;
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
					canCaptureDR = true;
					captureRequired = true;
					potentialMoveLabel.MoveDirection = "DR";
					CaptureObjectDR = potentialObject;
					
					if (!capturePrecheck)
					{
						potentialMoveLabel.EnableCaptureIndicator();
					}

					if (recaptureCheck)
						canRecapture = true;
				}
			}
			else
			{
				canCaptureDR = false;
				potentialMoveLabel.MoveDirection = null;
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
