using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	public OpponentAI opponentAI;
	[SerializeField] UILabel informationText;
	[SerializeField] UILabel checkerCounter;
	public PositionContainer[] positionLabels;
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
	bool checkerPrecheck = false;
	bool captureRequired = false;
	bool capturePerformed = false;
	bool recaptureCheck = false;
	bool canRecapture = false;
	bool movePrecheck = false;
	bool threatCheck = false;
	GameObject captureObjectUL;
	GameObject captureObjectUR;
	GameObject captureObjectDL;
	GameObject captureObjectDR;
	PositionContainer potentialMoveLabel;
	CheckerContainer checkerContainerScript;
	int checkerPosition;
	bool gameOver = false;
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
	public bool CaptureRequired { get { return captureRequired; } set { captureRequired = value; }}
	public bool CapturePerformed { get { return capturePerformed; } set { capturePerformed = value; }}
	public bool RecaptureCheck { get { return recaptureCheck; }}
	public bool CanRecapture { get { return canRecapture; }}
	public List<GameObject> whiteCheckers = new List<GameObject>();
	public List<GameObject> redCheckers = new List<GameObject>();
	public UILabel InformationText { get { return informationText; } set { informationText = value; }}
	public UILabel CheckerCounter { get { return checkerCounter; } set { checkerCounter = value; }}
	public bool GameOver { get { return gameOver; } set { gameOver = value; }}
	int randomInt;

	void Start()
	{
		randomInt = Random.Range(0, 2);

		if (randomInt == 0)
			WhiteTurn = true;
		else
			WhiteTurn = false;

		if (WhiteTurn)
		{
			whiteTurn = true;
			InformationText.text = "White Turn";

			if (opponentAI.playingAI && opponentAI.aiCheckerColor == 1)
				opponentAI.RunAIChecklist();
		}
		else
		{
			whiteTurn = false;
			InformationText.text = "Red Turn";

			if (opponentAI.playingAI && opponentAI.aiCheckerColor == 2)
				opponentAI.RunAIChecklist();
		}

	}

	public void ClearPositionLabels()
	{
		foreach (PositionContainer positionLabel in positionLabels)
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
		checkerPrecheck = true;
		CaptureRequired = false;

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

		checkerPrecheck = false;
	}

	public void FindMoves()
	{
		CaptureRequired = false;
		opponentAI.aiMovePositions.Clear();
		movePrecheck = true;
		
		if (WhiteTurn)
		{
			GameObject[] whiteCheckers = GameObject.FindGameObjectsWithTag("White");
			
			if (whiteCheckers != null)
			{
				randomInt = Random.Range(0, whiteCheckers.Length);
				CheckerOfInterest = whiteCheckers[randomInt];
				checkerContainerScript = whiteCheckers[randomInt].GetComponent<CheckerContainer>();
				checkerPosition = checkerContainerScript.BoardLocation;
			}
		}
		else
		{
			GameObject[] redCheckers = GameObject.FindGameObjectsWithTag("Red");
			
			if (redCheckers != null)
			{
				randomInt = Random.Range(0, redCheckers.Length);
				CheckerOfInterest = redCheckers[randomInt];
				checkerContainerScript = redCheckers[randomInt].GetComponent<CheckerContainer>();
				checkerPosition = checkerContainerScript.BoardLocation;
			}
		}

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

		movePrecheck = false;
	}

	public void FindThreats()
	{
		CaptureRequired = false;
		checkerPrecheck = true;
		threatCheck = true;
		
		if (!WhiteTurn)
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
		
		checkerPrecheck = false;
		threatCheck = false;
	}

	public void FindAdditionalCaptures(GameObject capturingChecker)
	{
		CaptureRequired = false;
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

		if (!CaptureRequired && !canCaptureUL && !canCaptureUR && !canCaptureDL && !canCaptureDR)
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
		else if (CaptureRequired && !canCaptureUL && !canCaptureUR && !canCaptureDL && !canCaptureDR)
		{
			if (WhiteTurn)
				InformationText.text = "White Turn: Capture Required";
			else
				InformationText.text = "Red Turn: Capture Required";
		}
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
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 3].GetComponent<PositionContainer>();

			if (!checkerPrecheck && potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
			{
				if (!checkerPrecheck)
					potentialMoveLabel.EnableMoveIndicator();
				
				if (movePrecheck)
					opponentAI.aiMovePositions.Add(potentialMoveLabel);
			}
		}
		else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 4].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
			{
				if (!checkerPrecheck)
					potentialMoveLabel.EnableMoveIndicator();

				if (movePrecheck)
					opponentAI.aiMovePositions.Add(potentialMoveLabel);
			}
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
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 3].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				var potentialThreatened = potentialMoveLabel;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 7].GetComponent<PositionContainer>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCaptureUL = true;
					CaptureRequired = true;
					potentialMoveLabel.MoveDirection = "UL";
					CaptureObjectUL = potentialObject;

					if (!checkerPrecheck)
						potentialMoveLabel.EnableCaptureIndicator();

					if (recaptureCheck)
						canRecapture = true;

					if (opponentAI.playingAI && checkerContainerScript.PieceColor == opponentAI.aiCheckerColor)
						opponentAI.aiCapturePositions.Add(potentialMoveLabel);

					if (threatCheck)
						opponentAI.aiThreatenedPositions.Add(potentialThreatened);
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
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 4].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				var potentialThreatened = potentialMoveLabel;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 7].GetComponent<PositionContainer>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCaptureUL = true;
					CaptureRequired = true;
					potentialMoveLabel.MoveDirection = "UL";
					CaptureObjectUL = potentialObject;
					
					if (!checkerPrecheck)
						potentialMoveLabel.EnableCaptureIndicator();

					if (recaptureCheck)
						canRecapture = true;

					if (opponentAI.playingAI && checkerContainerScript.PieceColor == opponentAI.aiCheckerColor)
						opponentAI.aiCapturePositions.Add(potentialMoveLabel);

					if (threatCheck)
						opponentAI.aiThreatenedPositions.Add(potentialThreatened);
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
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 4].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
			{
				if (!checkerPrecheck)
					potentialMoveLabel.EnableMoveIndicator();
				
				if (movePrecheck)
					opponentAI.aiMovePositions.Add(potentialMoveLabel);
			}
		}
		else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25))
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 5].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
			{
				if (!checkerPrecheck)
					potentialMoveLabel.EnableMoveIndicator();
				
				if (movePrecheck)
					opponentAI.aiMovePositions.Add(potentialMoveLabel);
			}
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
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 4].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				var potentialThreatened = potentialMoveLabel;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 9].GetComponent<PositionContainer>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCaptureUR = true;
					CaptureRequired = true;
					potentialMoveLabel.MoveDirection = "UR";
					CaptureObjectUR = potentialObject;
					
					if (!checkerPrecheck)
						potentialMoveLabel.EnableCaptureIndicator();

					if (recaptureCheck)
						canRecapture = true;

					if (opponentAI.playingAI && checkerContainerScript.PieceColor == opponentAI.aiCheckerColor)
						opponentAI.aiCapturePositions.Add(potentialMoveLabel);

					if (threatCheck)
						opponentAI.aiThreatenedPositions.Add(potentialThreatened);
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
			potentialMoveLabel = positionLabels[(checkerPosition - 1) + 5].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				var potentialThreatened = potentialMoveLabel;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) + 9].GetComponent<PositionContainer>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCaptureUR = true;
					CaptureRequired = true;
					potentialMoveLabel.MoveDirection = "UR";
					CaptureObjectUR = potentialObject;
					
					if (!checkerPrecheck)
						potentialMoveLabel.EnableCaptureIndicator();

					if (recaptureCheck)
						canRecapture = true;

					if (opponentAI.playingAI && checkerContainerScript.PieceColor == opponentAI.aiCheckerColor)
						opponentAI.aiCapturePositions.Add(potentialMoveLabel);

					if (threatCheck)
						opponentAI.aiThreatenedPositions.Add(potentialThreatened);
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
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 5].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
			{
				if (!checkerPrecheck)
					potentialMoveLabel.EnableMoveIndicator();
				
				if (movePrecheck)
					opponentAI.aiMovePositions.Add(potentialMoveLabel);
			}
		}
		else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25) || checkerPosition >= 29)
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 4].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
			{
				if (!checkerPrecheck)
					potentialMoveLabel.EnableMoveIndicator();
				
				if (movePrecheck)
					opponentAI.aiMovePositions.Add(potentialMoveLabel);
			}
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
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 5].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				var potentialThreatened = potentialMoveLabel;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) - 9].GetComponent<PositionContainer>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCaptureDL = true;
					CaptureRequired = true;
					potentialMoveLabel.MoveDirection = "DL";
					CaptureObjectDL = potentialObject;
					
					if (!checkerPrecheck)
						potentialMoveLabel.EnableCaptureIndicator();

					if (recaptureCheck)
						canRecapture = true;

					if (opponentAI.playingAI && checkerContainerScript.PieceColor == opponentAI.aiCheckerColor)
						opponentAI.aiCapturePositions.Add(potentialMoveLabel);

					if (threatCheck)
						opponentAI.aiThreatenedPositions.Add(potentialThreatened);
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
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 4].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				var potentialThreatened = potentialMoveLabel;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) - 9].GetComponent<PositionContainer>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCaptureDL = true;
					CaptureRequired = true;
					potentialMoveLabel.MoveDirection = "DL";
					CaptureObjectDL = potentialObject;
					
					if (!checkerPrecheck)
						potentialMoveLabel.EnableCaptureIndicator();

					if (recaptureCheck)
						canRecapture = true;

					if (opponentAI.playingAI && checkerContainerScript.PieceColor == opponentAI.aiCheckerColor)
						opponentAI.aiCapturePositions.Add(potentialMoveLabel);

					if (threatCheck)
						opponentAI.aiThreatenedPositions.Add(potentialThreatened);
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
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 4].GetComponent<PositionContainer>();

			if (checkerPosition % 8 != 0 && potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
			{
				if (!checkerPrecheck)
					potentialMoveLabel.EnableMoveIndicator();
				
				if (movePrecheck)
					opponentAI.aiMovePositions.Add(potentialMoveLabel);
			}
		}
		else if ((checkerPosition >= 5 && checkerPosition < 9) || (checkerPosition >= 13 && checkerPosition < 17) || (checkerPosition >= 21 && checkerPosition < 25) || checkerPosition >= 29)
		{
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 3].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.MoveIndicatorEnabled)
			{
				if (!checkerPrecheck)
					potentialMoveLabel.EnableMoveIndicator();
				
				if (movePrecheck)
					opponentAI.aiMovePositions.Add(potentialMoveLabel);
			}
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
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 4].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				var potentialThreatened = potentialMoveLabel;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) - 7].GetComponent<PositionContainer>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCaptureDR = true;
					CaptureRequired = true;
					potentialMoveLabel.MoveDirection = "DR";
					CaptureObjectDR = potentialObject;
					
					if (!checkerPrecheck)
						potentialMoveLabel.EnableCaptureIndicator();

					if (recaptureCheck)
						canRecapture = true;

					if (opponentAI.playingAI && checkerContainerScript.PieceColor == opponentAI.aiCheckerColor)
						opponentAI.aiCapturePositions.Add(potentialMoveLabel);

					if (threatCheck)
						opponentAI.aiThreatenedPositions.Add(potentialThreatened);
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
			potentialMoveLabel = positionLabels[(checkerPosition - 1) - 3].GetComponent<PositionContainer>();

			if (potentialMoveLabel.OccupationValue != 0 && potentialMoveLabel.OccupationValue != checkerContainerScript.PieceColor)
			{
				var potentialObject = potentialMoveLabel.OccupyingChecker.gameObject;
				var potentialThreatened = potentialMoveLabel;
				potentialMoveLabel = positionLabels[(checkerPosition - 1) - 7].GetComponent<PositionContainer>();
				
				if (potentialMoveLabel.OccupationValue == 0 && !potentialMoveLabel.CaptureIndicatorEnabled)
				{
					canCaptureDR = true;
					CaptureRequired = true;
					potentialMoveLabel.MoveDirection = "DR";
					CaptureObjectDR = potentialObject;
					
					if (!checkerPrecheck)
						potentialMoveLabel.EnableCaptureIndicator();

					if (recaptureCheck)
						canRecapture = true;

					if (opponentAI.playingAI && checkerContainerScript.PieceColor == opponentAI.aiCheckerColor)
						opponentAI.aiCapturePositions.Add(potentialMoveLabel);

					if (threatCheck)
						opponentAI.aiThreatenedPositions.Add(potentialThreatened);
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
