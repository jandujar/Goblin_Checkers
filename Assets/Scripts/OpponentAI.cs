using UnityEngine;
using System.Collections;

public class OpponentAI : MonoBehaviour
{
	// AI Checklist
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

	bool pieceThreatened = false;

	public void RunAIChecklist()
	{
		if (CaptureAnalyzer())
		{
			Debug.Log("Random capture performed");
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
	}

	bool CaptureAnalyzer()
	{
		// 1. Check if any captures available
		// 2. Check if any multi-captures available
		// 3. Perform multi-capture moves and return true if available
		// 4. If no multi-captures but captures available, choose one capture at random and return true
		// 5. Else, return false

		return false;
	}

	void ThreatAnalyzer()
	{
		pieceThreatened = false;

		// 1. Do threat analysis for all pieces
		// 2. Store list of pieces in threat of capture

		pieceThreatened = true; // Placeholder for actual threat evaluation
	}

	bool MoveToBlock()
	{
		// 1. Retrieve list of pieces in threat of capture
		// 2. Find end position of enemy checker after capture for all end positions
		// 3. Find if any checkers can move to any of those positions
		// 4. If checker can move to any end positions, execute move and return true
		// 5. Else, return false

		return true;
	}

	bool MoveToAvoidCapture()
	{
		// 1. Retrieve list of pieces in threat of capture
		// 2. If any pieces can move to any position that wont result in capture, execute move and return true
		// 3. Else, return false

		return true;
	}

	bool KingMoveToCapture()
	{
		// 1. Check if any AI checkers are king pieces (return false if no king pieces)
		// 2. Choose closest enemy piece
		// 3. Choose move toward enemy piece that doesn't put king in threat of capture (if no moves exist, return false)
		// 4. Else, return false

		return true;
	}

	bool SafeRandomMove()
	{
		// 1. Create list of all moves available that don't put piece in threat of capture
		// 2. Check if any moves result in AI checker turning into king
		// 3. If moves exist to turn piece into king, execute random move and return true
		// 4. If no king move available but another move available, execute random move and return true
		// 5. Else, return false

		return true;
	}

	bool RandomMove()
	{
		// 1. Create list of all potential moves
		// 2. If move available, execute random move and return true
		// 3. Else, return false

		return true;
	}
}
