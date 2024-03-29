﻿#define DEBUG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class Orienter : MonoBehaviour {

	private GameObject _currentLevel;
	private string _levelName;
	private Quaternion _rotationTarget;
#if DEBUG
	private Quaternion DEBUG_lastRotation = Quaternion.identity;
#endif

	// Use this for initialization
	void Start () {
		HideDummies ();
	}

	public void LoadLevel(string level, Quaternion targetRotation) {
		if (_currentLevel != null) {
			Destroy (_currentLevel);
		}

		this.transform.localRotation = Quaternion.identity;
		_rotationTarget = targetRotation;

		var levelObject = Resources.Load<GameObject>("Levels/" + level);
		_currentLevel = (GameObject)Instantiate(levelObject, Vector3.zero, Quaternion.identity);
		_currentLevel.transform.parent = this.transform;
		_currentLevel.transform.localPosition = Vector3.zero;
		_currentLevel.transform.localRotation = Quaternion.identity;
		_levelName = level;
	}

	static void HideDummies() {
		foreach (var dummy in GameObject.FindGameObjectsWithTag ("Dummy")) {
			dummy.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update () {
		var movement = GetTouchMovement ();

		float h = -0.7f * movement.Drag.x + Input.GetAxis ("Horizontal");
		float v = 0.7f * movement.Drag.y + Input.GetAxis ("Vertical");
		float t = Input.GetAxis ("Tilt");
		float angle = (Math.Abs (h) + Math.Abs(v) + Math.Abs(t)) * 3.0f;
		this.transform.RotateAround (this.transform.position, new Vector3 () { x = v, y = h, z = t }, angle);
		this.transform.RotateAround (this.transform.position, new Vector3 () { z = 1 }, movement.Tilt * 200f);
	}

	public bool IsNearTarget()
	{
		#if DEBUG
		var r = this.transform.localRotation;
		if (r != DEBUG_lastRotation)
			Debug.Log(_levelName + "\t" + AsString(r));
			DEBUG_lastRotation = r;
		#endif

		return IsClose (_rotationTarget, this.transform.localRotation);
		//return IsClose (_orientationTarget, this.transform.localToWorldMatrix);
	}

	private static string AsString(Quaternion q)
	{
		return String.Format ("{0}\t{1}\t{2}\t{3}",	q.x, q.y, q.z, q.w);
	}

	static bool IsClose(Quaternion target, Quaternion source)
	{
		float errorSum = Math.Abs (target.w - source.w);
		errorSum += Math.Abs (target.x - source.x);
		errorSum += Math.Abs (target.y - source.y);
		errorSum += Math.Abs (target.z - source.z);

		Debug.Log (errorSum);
		return (errorSum < 0.08f);
	}

	static bool IsClose(Matrix4x4 target, Matrix4x4 source)
	{
		float errorSum = Math.Abs (target.m00 - source.m00);
		errorSum += Math.Abs (target.m01 - source.m01);
		errorSum += Math.Abs (target.m02 - source.m02);
		errorSum += Math.Abs (target.m03 - source.m03);
		
		errorSum += Math.Abs (target.m10 - source.m10);
		errorSum += Math.Abs (target.m11 - source.m11);
		errorSum += Math.Abs (target.m12 - source.m12);
		errorSum += Math.Abs (target.m13 - source.m13);
		
		errorSum += Math.Abs (target.m20 - source.m20);
		errorSum += Math.Abs (target.m21 - source.m21);
		errorSum += Math.Abs (target.m22 - source.m22);
		errorSum += Math.Abs (target.m23 - source.m23);
		
		errorSum += Math.Abs (target.m30 - source.m30);
		errorSum += Math.Abs (target.m31 - source.m31);
		errorSum += Math.Abs (target.m32 - source.m32);
		errorSum += Math.Abs (target.m33 - source.m33);
//		Debug.Log (errorSum);

		return (errorSum < 0.5f);
	}
	
	TouchMovement GetTouchMovement()
	{
		TouchMovement m = new TouchMovement ();
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

				float currAngle = (float)Math.Atan2((touch1.position.y - touch0.position.y), (touch1.position.x - touch0.position.x));
				float oldAngle = (float)Math.Atan2(((old1Y) - (old0Y)), ((old1X) - (old0X)));
				m.Tilt = currAngle - oldAngle;
				if (Math.Abs(m.Tilt) > 1) m.Tilt = 0;
			}
			else if (touch0.phase == TouchPhase.Moved)
			{
				// Get movement of the finger since last frame
				m.Drag = touch0.deltaPosition;
			}
		}

		return m;
	}

	struct TouchMovement
	{
		public Vector2 Drag { get; set; }
		public float Tilt { get; set; }
		public float Zoom { get; set; }
	}
}
