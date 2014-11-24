using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpponentAI : MonoBehaviour
{
	// AI Checklist
	//
	// Check if any captures available
	// 
	// Piece in danger of being captured
	// 1. Check if piece is in danger of being captured
	// 2. If piece is in danger, move another checker to block space to prevent piece from being captured
	// 3. If capture move cant be blocked by moving other piece, move piece in danger away from capture position
	//
	// No threat of capture
	// 4. If king piece available, find closest enemy piece advance to capture position (only if move doesn't put king in danger)
	// 5. Randomly choose checker move that will not put checker in danger of being captured
	// 6. Choose random move

	[SerializeField] GameController gameController;
	public bool playingAI = false;
	public int aiCheckerColor = 2; //1 = White, 2 = Red
	bool pieceThreatened = false;
	GameObject[] aiCheckers;
	//GameObject[] aiOpponentCheckers;
	List<GameObject> aiCaptureCheckers = new List<GameObject>();
	public List<PositionContainer> aiMovePositions = new List<PositionContainer>();
	public List<PositionContainer> aiCapturePositions = new List<PositionContainer>();
	public List<PositionContainer> aiThreatenedPositions = new List<PositionContainer>();

	public void RunAIChecklist()
	{
		StartCoroutine(RunChecklist());
	}

	IEnumerator RunChecklist()
	{
		gameController.CaptureRequired = false;
		aiCaptureCheckers.Clear();
		aiMovePositions.Clear();
		aiCapturePositions.Clear();
		aiThreatenedPositions.Clear();

		// Wait time to give checkers enough time to spawn
		yield return new WaitForSeconds(0.1f);

		if (aiCheckerColor == 1)
		{
			aiCheckers = GameObject.FindGameObjectsWithTag("White");
			//aiOpponentCheckers = GameObject.FindGameObjectsWithTag("Red");
		}
		else
		{
			aiCheckers = GameObject.FindGameObjectsWithTag("Red");
			//aiOpponentCheckers = GameObject.FindGameObjectsWithTag("White");
		}

		if (CaptureAnalyzer())
		{
			gameController.ClearPositionLabels();
			int randomCheckerInt = Random.Range(0, aiCaptureCheckers.Count);
			GameObject randomChecker = aiCaptureCheckers[randomCheckerInt];

			aiCapturePositions.Clear();
			gameController.FindSelectedCheckerOptions(randomChecker);

			int randomPositionContainer = Random.Range(0, aiCapturePositions.Count);
			aiCapturePositions[randomPositionContainer].EnableCaptureIndicator();
			aiCapturePositions[randomPositionContainer].TriggerContainer();

			StartCoroutine(CheckRecapture(randomChecker));
		}
		else
		{
			ThreatAnalyzer();

			if (pieceThreatened)
			{
				if (MoveToBlock())
					Debug.Log("Capture prevented by block");
				else if (MoveToAvoidCapture())
					Debug.Log("Capture prevented by move");
			}
			else if (KingMoveToCapture())
				Debug.Log("King piece hunting opponent piece");
			else if (SafeRandomMove())
				Debug.Log("Safe random move performed");
			else if (RandomMove())
				Debug.Log("Random move performed");
			else
				Debug.Log("No moves available (Player Wins)");
		}
	}

	bool CaptureAnalyzer()
	{
		gameController.ClearPositionLabels();

		// 1. Check if any captures available
		if (aiCheckers != null)
		{
			gameController.CaptureRequired = false;

			foreach (GameObject aiChecker in aiCheckers)
			{
				gameController.FindSelectedCheckerOptions(aiChecker);

				if (gameController.CanCaptureUL || gameController.CanCaptureUR || gameController.CanCaptureDL || gameController.CanCaptureDR)
					aiCaptureCheckers.Add(aiChecker);
			}
		}

		if (aiCaptureCheckers.Count > 0)
			return true;
		else
		{
			// Always white turn...
			//gameController.WhiteTurn = true;

			return false;
		}

		// 2. Check if any multi-captures available
		// 3. Perform multi-capture moves and return true if available
		// 4. If no multi-captures but captures available, choose one capture at random and return true
		// 5. Else, return false


		//return false;
	}

	void ThreatAnalyzer()
	{
		gameController.ClearPositionLabels();

		pieceThreatened = false;
		gameController.FindThreats();
		int index = 0;

		if (aiThreatenedPositions.Count > 0)
		{
			pieceThreatened = true;

			while (index < aiThreatenedPositions.Count - 1)
			{
				if (aiThreatenedPositions[index] == aiThreatenedPositions[index + 1])
					aiThreatenedPositions.RemoveAt(index);
				else
					index++;
			}
		}
		else
			pieceThreatened = false;
	}

	bool MoveToBlock()
	{
		gameController.ClearPositionLabels();

		// 1. Retrieve list of pieces in threat of capture
		// 2. Find end position of enemy checker after capture for all end positions
		// 3. Find if any checkers can move to any of those positions
		// 4. If checker can move to any end positions, execute move and return true
		// 5. Else, return false

		return false;
	}

	bool MoveToAvoidCapture()
	{
		gameController.ClearPositionLabels();

		// 1. Retrieve list of pieces in threat of capture
		// 2. If any pieces can move to any position that wont result in capture, execute move and return true
		// 3. Else, return false

		return false;
	}

	bool KingMoveToCapture()
	{
		gameController.ClearPositionLabels();

		// 1. Check if any AI checkers are king pieces (return false if no king pieces)
		// 2. Choose closest enemy piece
		// 3. Choose move toward enemy piece that doesn't put king in threat of capture (if no moves exist, return false)
		// 4. Else, return false

		return false;
	}

	bool SafeRandomMove()
	{
		gameController.ClearPositionLabels();

		// 1. Create list of all moves available that don't put piece in threat of capture
		// 2. Check if any moves result in AI checker turning into king
		// 3. If moves exist to turn piece into king, execute random move and return true
		// 4. If no king move available but another move available, execute random move and return true
		// 5. Else, return false

		return false;
	}

	bool RandomMove()
	{
		gameController.ClearPositionLabels();

		// 1. Create list of all potential moves
		gameController.FindMoves();

		// 2. If move available, execute random move and return true
		if (aiMovePositions.Count > 0)
		{
			int randomInt = Random.Range(0, aiMovePositions.Count);
			aiMovePositions[randomInt].TriggerContainer();
			return true;
		}
		else if (aiCheckers.Length > 0)
		{
			RandomMove();
			return true;
		}
		else
			return false;
	}

	IEnumerator CheckRecapture(GameObject selectedChecker)
	{
		gameController.ClearPositionLabels();

		yield return new WaitForSeconds(2.0f);
		gameController.FindAdditionalCaptures(selectedChecker);

		if (gameController.CanRecapture)
		{
			aiCapturePositions.Clear();
			gameController.FindSelectedCheckerOptions(selectedChecker);
			
			int randomPositionContainer = Random.Range(0, aiCapturePositions.Count);
			aiCapturePositions[randomPositionContainer].EnableCaptureIndicator();
			aiCapturePositions[randomPositionContainer].TriggerContainer();

			CheckRecapture(selectedChecker);
		}
	}
}
