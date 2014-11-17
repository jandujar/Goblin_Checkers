using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
	[SerializeField] UILabel informationText;
	[SerializeField] GameController gameController;
	[SerializeField] OpponentAI opponentAI;
	CheckerContainer checkerContainerScript;
	bool enableMovement = false;
	GameObject checkerObject;
	Transform newTransform;
	int positionValue;

	public void TriggerMovement(GameObject selectedChecker, Transform positionTransform, int boardLocation)
	{
		checkerObject = selectedChecker;
		newTransform = positionTransform;
		positionValue = boardLocation;
		enableMovement = true;
	}

	void Update()
	{
		if (enableMovement)
		{
			if (Vector3.Distance(checkerObject.transform.position, newTransform.position) > 0.001f)
			{
				checkerObject.transform.position = Vector3.Lerp(checkerObject.transform.position, newTransform.position, Time.deltaTime * 4f);
				checkerContainerScript = checkerObject.GetComponent<CheckerContainer>();
				checkerContainerScript.BoardLocation = positionValue;
			}
			else
			{
				checkerObject.transform.position = newTransform.position;
				enableMovement = false;

				if (!gameController.GameOver && gameController.CapturePerformed)
				{
					gameController.FindAdditionalCaptures(checkerObject);

					if (gameController.CanRecapture)
						gameController.FindCaptures();
					else if (!gameController.RecaptureCheck)
					{
						if (gameController.WhiteTurn)
						{
							gameController.WhiteTurn = false;
							informationText.text = "Red Turn";

							if (opponentAI.playingAI && opponentAI.aiCheckerColor == 2)
								opponentAI.RunAIChecklist();
						}
						else
						{
							gameController.WhiteTurn = true;
							informationText.text = "White Turn";

							if (opponentAI.playingAI && opponentAI.aiCheckerColor == 1)
								opponentAI.RunAIChecklist();
						}
					}
				}
				else if (!gameController.GameOver && (!gameController.RecaptureCheck || !gameController.CapturePerformed || (!gameController.CanCaptureUL && !gameController.CanCaptureUR && !gameController.CanCaptureDL && !gameController.CanCaptureDR)))
				{
					if (gameController.WhiteTurn)
					{
						gameController.WhiteTurn = false;
						informationText.text = "Red Turn";

						if (opponentAI.playingAI && opponentAI.aiCheckerColor == 2)
							opponentAI.RunAIChecklist();
					}
					else
					{
						gameController.WhiteTurn = true;
						informationText.text = "White Turn";

						if (opponentAI.playingAI && opponentAI.aiCheckerColor == 1)
							opponentAI.RunAIChecklist();
					}
				}

				gameController.FindCaptures();

				if (checkerContainerScript.BoardLocation >= 29 && checkerContainerScript.PieceColor == 1 && checkerContainerScript.PieceType != 2)
				{
					checkerContainerScript.PieceType = 2;
					Debug.Log("White Checker Kinged");
				}
				else if (checkerContainerScript.BoardLocation <= 4 && checkerContainerScript.PieceColor == 2 && checkerContainerScript.PieceType != 2)
				{
					checkerContainerScript.PieceType = 2;
					Debug.Log("Red Checker Kinged");
				}
			}
		}
	}
}
