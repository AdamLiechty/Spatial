using System;
using UnityEngine;
using System.Collections;

public class Zoomer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	const float maxZ = -1.5f;
	// Update is called once per frame
	void Update () {
		float zoom = Input.GetAxis ("Zoom") * 0.5f;
		float touchZoom = GetTouchZoom () * 0.1f;
		this.transform.Translate (new Vector3 (0, 0, touchZoom + zoom));
		if (this.transform.position.z > maxZ) {
			this.transform.Translate(new Vector3(0,0, maxZ - this.transform.position.z));
		}

	}

	float GetTouchZoom()
	{
		if (Input.touchCount > 0)
		{
			var touch0 = Input.GetTouch (0);
			Vector2 touch0Delta = touch0.phase == TouchPhase.Moved ? touch0.deltaPosition : Vector2.zero;
			
			if (Input.touchCount > 1)
			{
				var touch1 = Input.GetTouch (1);
				Vector2 touch1Delta = touch1.phase == TouchPhase.Moved ? touch1.deltaPosition : Vector2.zero;
				
				float old0Y = touch0.position.y - touch0Delta.y;
				float old1Y = touch1.position.y - touch1Delta.y;
				float old0X = touch0.position.x - touch0Delta.x;
				float old1X = touch1.position.x - touch1Delta.x;
				
				float distX = touch1.position.x - touch0.position.x;
				float distY = touch1.position.y - touch0.position.y;
				float oldDistX = old1X - old0X;
				float oldDistY = old1Y - old0Y;
				return (float)Math.Sqrt(distX * distX + distY * distY) -
					(float)Math.Sqrt(oldDistX * oldDistX + oldDistY * oldDistY);
			}
		}
		
		return 0;
	}
}
