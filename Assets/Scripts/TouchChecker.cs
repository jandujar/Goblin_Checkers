using UnityEngine;
using System.Collections;

public class TouchChecker : MonoBehaviour
{	
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
		if (gesture.pickObject == gameObject)
			Debug.Log("Selected " + gameObject.name);
	}
}
