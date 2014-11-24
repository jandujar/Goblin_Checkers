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
		if (gesture.pickObject == gameObject && !gameController.CanRecapture)
		{
			if (!gameController.opponentAI.playingAI || ((gameController.WhiteTurn && gameController.opponentAI.aiCheckerColor != 1) || (!gameController.WhiteTurn && gameController.opponentAI.aiCheckerColor != 2)))
			{
				gameController.ClearPositionLabels();
				gameController.FindSelectedCheckerOptions(gameObject);
			}
		}
	}
}
