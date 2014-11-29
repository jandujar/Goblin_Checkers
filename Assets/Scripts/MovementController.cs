using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
	[SerializeField] UILabel informationText;
	[SerializeField] GameController gameController;
	[SerializeField] OpponentAI opponentAI;
	[SerializeField] Collider touchBlocker;
	CheckerContainer checkerContainerScript;
	bool enableMovement = false;
	GameObject checkerObject;
	Transform newTransform;
	int positionValue;

	void Start()
	{
		touchBlocker.enabled = false;
	}

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
				if (!touchBlocker.enabled)
					touchBlocker.enabled = true;

				checkerObject.transform.position = Vector3.Lerp(checkerObject.transform.position, newTransform.position, Time.deltaTime * 4f);
			}
			else
			{
				enableMovement = false;
				touchBlocker.enabled = false;

				checkerObject.transform.position = newTransform.position;
				checkerContainerScript = checkerObject.GetComponent<CheckerContainer>();
				checkerContainerScript.BoardLocation = positionValue;

				if (!gameController.GameOver && gameController.CapturePerformed)
				{
					gameController.FindAdditionalCaptures(checkerObject);

					if (gameController.CanRecapture)
					{
						gameController.FindCaptures();

						if (opponentAI.playingAI)
						{
							if (gameController.WhiteTurn && opponentAI.aiCheckerColor == 1)
								opponentAI.RunAIChecklist();
							else if (!gameController.WhiteTurn && opponentAI.aiCheckerColor == 2)
								opponentAI.RunAIChecklist();
						}

					}
					else if (!gameController.RecaptureCheck)
					{
						if (gameController.WhiteTurn)
						{
							gameController.WhiteTurn = false;
							informationText.text = "Red Turn";

							if (opponentAI.playingAI && opponentAI.aiCheckerColor == 2)
								opponentAI.RunAIChecklist();

							gameController.FindCaptures();
						}
						else
						{
							gameController.WhiteTurn = true;
							informationText.text = "White Turn";

							if (opponentAI.playingAI && opponentAI.aiCheckerColor == 1)
								opponentAI.RunAIChecklist();

							gameController.FindCaptures();
						}
					}
				}
				else if (!gameController.GameOver)
				{
					gameController.CaptureRequired = false;

					if (gameController.WhiteTurn)
					{
						gameController.WhiteTurn = false;
						informationText.text = "Red Turn";

						if (opponentAI.playingAI && opponentAI.aiCheckerColor == 2)
							opponentAI.RunAIChecklist();

						gameController.FindCaptures();
					}
					else
					{
						gameController.WhiteTurn = true;
						informationText.text = "White Turn";

						if (opponentAI.playingAI && opponentAI.aiCheckerColor == 1)
							opponentAI.RunAIChecklist();

						gameController.FindCaptures();
					}
				}

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
