using UnityEngine;
using System.Collections;

public class PositionLabel : MonoBehaviour
{
	[SerializeField] GameController gameController;
	[SerializeField] MovementController movementController;
	[SerializeField] int positionValue = 0;
	[SerializeField] GameObject moveIndicator;
	[SerializeField] GameObject captureIndicator;
	[SerializeField] Collider positionCollider;
	int occupationValue = 0; // 0 = Empty, 1 = White, 2 = Red
	bool moveIndicatorEnabled = false;
	bool captureIndicatorEnabled = false;
	CheckerContainer occupyingChecker;
	CheckerContainer captureContainerScript;
	string moveDirection;
	public int PositionValue { get { return positionValue; }}
	public int OccupationValue { get { return occupationValue; } set { occupationValue = value; }}
	public bool MoveIndicatorEnabled { get { return moveIndicatorEnabled; } set { moveIndicatorEnabled = value; }}
	public bool CaptureIndicatorEnabled { get { return captureIndicatorEnabled; } set { captureIndicatorEnabled = value; }}
	public CheckerContainer OccupyingChecker { get { return occupyingChecker; } set { occupyingChecker = value; }}
	public string MoveDirection { get { return moveDirection; } set { moveDirection = value; }}

	void OnEnable()
	{
		EasyTouch.On_TouchStart += On_TouchStart;
	}
	
	void OnDisable()
	{
		UnsubscribeEvent();
	}
	
	void OnDestroy()
	{
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
	}
	
	public void On_TouchStart(Gesture gesture)
	{
		if (gesture.pickObject == gameObject && CaptureIndicatorEnabled)
		{
			CheckerContainer checkerContainerScript = gameController.CheckerOfInterest.GetComponent<CheckerContainer>();
			gameController.ClearPositionLabels();
			gameController.ResetOccupationValue(checkerContainerScript.BoardLocation);

			if (MoveDirection == "UL")
			{
				captureContainerScript = gameController.CaptureObjectUL.GetComponent<CheckerContainer>();
				gameController.ResetOccupationValue(captureContainerScript.BoardLocation);
			}
			else if (MoveDirection == "UR")
			{
				captureContainerScript = gameController.CaptureObjectUR.GetComponent<CheckerContainer>();
				gameController.ResetOccupationValue(captureContainerScript.BoardLocation);
			}
			else if (MoveDirection == "DL")
			{
				captureContainerScript = gameController.CaptureObjectDL.GetComponent<CheckerContainer>();
				gameController.ResetOccupationValue(captureContainerScript.BoardLocation);
			}
			else if (MoveDirection == "DR")
			{
				captureContainerScript = gameController.CaptureObjectDR.GetComponent<CheckerContainer>();
				gameController.ResetOccupationValue(captureContainerScript.BoardLocation);
			}

			gameController.CapturePerformed = true;
			OccupyingChecker = checkerContainerScript;
			OccupationValue = checkerContainerScript.PieceColor;
			movementController.TriggerMovement(gameController.CheckerOfInterest, transform, positionValue);

			if (MoveDirection == "UL")
				Destroy(gameController.CaptureObjectUL, 1.0f);
			else if (MoveDirection == "UR")
				Destroy(gameController.CaptureObjectUR, 1.0f);
			else if (MoveDirection == "DL")
				Destroy(gameController.CaptureObjectDL, 1.0f);
			else if (MoveDirection == "DR")
				Destroy(gameController.CaptureObjectDR, 1.0f);
			else
				Debug.LogError("MoveDirection is null");
		}
		else if (gesture.pickObject == gameObject && MoveIndicatorEnabled)
		{
			gameController.CapturePerformed = false;
			CheckerContainer checkerContainerScript = gameController.CheckerOfInterest.GetComponent<CheckerContainer>();
			gameController.ClearPositionLabels();
			gameController.ResetOccupationValue(checkerContainerScript.BoardLocation);
			OccupyingChecker = checkerContainerScript;
			OccupationValue = checkerContainerScript.PieceColor;
			movementController.TriggerMovement(gameController.CheckerOfInterest, transform, positionValue);
		}
	}

	void Awake()
	{
		moveIndicator.SetActive(false);
		captureIndicator.SetActive(false);
		positionCollider.enabled = false;
	}

	public void EnableMoveIndicator()
	{
		moveIndicator.SetActive(true);
		MoveIndicatorEnabled = true;
		positionCollider.enabled = true;
	}

	public void DisableMoveIndicator()
	{
		moveIndicator.SetActive(false);
		MoveIndicatorEnabled = false;
		positionCollider.enabled = false;
	}

	public void EnableCaptureIndicator()
	{
		captureIndicator.SetActive(true);
		CaptureIndicatorEnabled = true;
		positionCollider.enabled = true;
	}

	public void DisableCaptureIndicator()
	{
		captureIndicator.SetActive(false);
		CaptureIndicatorEnabled = false;
		positionCollider.enabled = false;
	}
}