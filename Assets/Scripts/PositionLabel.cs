using UnityEngine;
using System.Collections;

public class PositionLabel : MonoBehaviour
{
	[SerializeField] int positionValue = 0;
	[SerializeField] GameObject moveIndicator;
	[SerializeField] GameObject captureIndicator;
	int occupationValue = 0; // 0 = Empty, 1 = White, 2 = Red
	bool moveIndicatorEnabled = false;
	bool captureIndicatorEnabled = false;
	public int PositionValue { get { return positionValue; }}
	public int OccupationValue { get { return occupationValue; } set { occupationValue = value; }}
	public bool MoveIndicatorEnabled { get { return moveIndicatorEnabled; } set { moveIndicatorEnabled = value; }}
	public bool CaptureIndicatorEnabled { get { return captureIndicatorEnabled; } set { captureIndicatorEnabled = value; }}

	void Awake()
	{
		moveIndicator.SetActive(false);
		captureIndicator.SetActive(false);
	}

	public void EnableMoveIndicator()
	{
		moveIndicator.SetActive(true);
	}

	public void DisableMoveIndicator()
	{
		moveIndicator.SetActive(false);
	}

	public void EnableCaptureIndicator()
	{
		captureIndicator.SetActive(true);
	}

	public void DisableCaptureIndicator()
	{
		captureIndicator.SetActive(false);
	}
}