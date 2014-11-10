using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
	[SerializeField] GameController gameController;
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
				CheckerContainer checkerContainerScript = checkerObject.GetComponent<CheckerContainer>();
				checkerContainerScript.BoardLocation = positionValue;
			}
			else
			{
				checkerObject.transform.position = newTransform.position;
				enableMovement = false;
			}
		}
	}
}
