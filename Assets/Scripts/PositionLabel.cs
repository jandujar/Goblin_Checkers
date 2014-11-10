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
	public int PositionValue { get { return positionValue; }}
	public int OccupationValue { get { return occupationValue; } set { occupationValue = value; }}
	public bool MoveIndicatorEnabled { get { return moveIndicatorEnabled; } set { moveIndicatorEnabled = value; }}
	public bool CaptureIndicatorEnabled { get { return captureIndicatorEnabled; } set { captureIndicatorEnabled = value; }}

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
		if (gesture.pickObject == gameObject && MoveIndicatorEnabled)
		{
			CheckerContainer checkerContainerScript = gameController.CheckerOfInterest.GetComponent<CheckerContainer>();
			gameController.ClearPositionLabels();
			gameController.ResetOccupationValue(checkerContainerScript.BoardLocation);
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