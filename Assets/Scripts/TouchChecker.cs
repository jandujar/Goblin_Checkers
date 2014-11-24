using UnityEngine;
using System.Collections;

public class TouchChecker : MonoBehaviour
{	
	GameController gameController;

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

	void Awake()
	{
		var gameControllerObject = GameObject.FindGameObjectWithTag("GameController");

		if (gameControllerObject != null)
			gameController = gameControllerObject.GetComponent<GameController>();
	}

	public void On_TouchStart(Gesture gesture)
	{
		if (gesture.pickObject == gameObject)
		{
			if (!gameController.CanRecapture)
			{
				gameController.ClearPositionLabels();
				gameController.FindSelectedCheckerOptions(gameObject);
				gameController.CheckerOfInterest = gameObject;
			}
		}
	}
}
