using UnityEngine;
using System.Collections;
using System;

public class CameraControl : MonoBehaviour {

	public struct BoxLimit
	{
		public float leftLimit;
		public float rightLimit;
		public float topLimit;
		public float bottomLimit;
	}

	public static BoxLimit controlLimits = new BoxLimit();
	public static BoxLimit mouseScrollLimits = new BoxLimit();
	public static CameraControl instance;

	private float cameraMoveSpeed = 40f;
	private float mouseBoundary = 5f;

	private bool runTacticalView;
	private bool runNormalView;

	void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start ()
	{
		runTacticalView = false;
		controlLimits.leftLimit = -10f;
		controlLimits.rightLimit = 84f;
		controlLimits.topLimit = 103f;
		controlLimits.bottomLimit = -40f;

		mouseScrollLimits.leftLimit = mouseBoundary;
		mouseScrollLimits.rightLimit = mouseBoundary;
		mouseScrollLimits.topLimit = mouseBoundary;
		mouseScrollLimits.bottomLimit = mouseBoundary;
	}

	// Update is called once per frame
	void Update ()
	{
		if (CheckIfUserCameraInput ()) {
			Vector3 cameraDesiredMove = GetDesiredTranslation ();

			if (!IsDesiredPositionOverBoundaries (cameraDesiredMove)) {
				this.transform.Translate (cameraDesiredMove);
			}
		}


		if (runTacticalView) {
			if (Camera.main.transform.eulerAngles.x >= 90) {
				runTacticalView = false;
			} else
			Camera.main.transform.rotation = Quaternion.Slerp (Camera.main.transform.rotation, Quaternion.Euler (90, 0, 0), Time.deltaTime * 5);
			transform.position = Vector3.Lerp (transform.position, new Vector3(transform.position.x,40,transform.position.z), Time.deltaTime * 5);
		}
		if (runNormalView) {
			if (Camera.main.transform.eulerAngles.x <= 60) {
				runNormalView = false;
			}
			Camera.main.transform.rotation = Quaternion.Slerp (Camera.main.transform.rotation, Quaternion.Euler (60, 0, 0), Time.deltaTime * 5);
			transform.position = Vector3.Lerp (transform.position, new Vector3(transform.position.x,20,transform.position.z), Time.deltaTime * 5);
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (Camera.main.transform.eulerAngles.x >= 89) {
				runNormalView = true;
				runTacticalView = false;
			}
			else if (Camera.main.transform.eulerAngles.x <= 61) {
				runTacticalView = true;
				runNormalView = false;
			}
		}
	}

	private bool CheckIfUserCameraInput()
	{
		bool keyboardMove;
		bool mouseMove;
		bool canMove;

		if (CameraControl.AreCameraKeyboardButtonPressed())
		{
			keyboardMove = true;
		}
		else
			keyboardMove = false;
		if (CameraControl.IsMousePositionWithinBoundaries())
		{
			mouseMove = true;
		} else mouseMove = false;


		if (keyboardMove || mouseMove)
		canMove = true;  else canMove = false;

		return canMove;
	}

	public Vector3 GetDesiredTranslation()
	{
		float moveSpeed = 0f;
		float desiredX = 0f;

		float desiredZ = 0f;
		moveSpeed = cameraMoveSpeed * Time.deltaTime;

		if (Input.GetKey(KeyCode.W))
		{
			desiredZ = moveSpeed;
		}
		if (Input.GetKey(KeyCode.S))
		{
			desiredZ = moveSpeed * -1;
		}
		if (Input.GetKey(KeyCode.A))
		{
			desiredX = moveSpeed * -1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			desiredX = moveSpeed;
		}

		if(Input.mousePosition.x < mouseScrollLimits.leftLimit)
		{
			desiredX = moveSpeed * -1;
		}
		if (Input.mousePosition.x > (Screen.width - mouseScrollLimits.rightLimit))
		{
			desiredX = moveSpeed;
		}
		if (Input.mousePosition.y < mouseScrollLimits.bottomLimit)
		{
			desiredZ = moveSpeed * -1;
		}
		if (Input.mousePosition.y > (Screen.height - mouseScrollLimits.topLimit))
		{
			desiredZ = moveSpeed;
		}

		return new Vector3(desiredX, 0, desiredZ);
	}

	public bool IsDesiredPositionOverBoundaries(Vector3 desiredPosition)
	{
		bool overBoundaries = false;

		if((this.transform.position.x + desiredPosition.x) < controlLimits.leftLimit)
		{
			overBoundaries = true;
		}
		if ((this.transform.position.x + desiredPosition.x) > controlLimits.rightLimit)
		{
			overBoundaries = true;
		}
		if ((this.transform.position.z + desiredPosition.z) > controlLimits.topLimit)
		{
			overBoundaries = true;
		}
		if ((this.transform.position.z + desiredPosition.z) < controlLimits.bottomLimit)
		{
			overBoundaries = true;
		}
		return overBoundaries;
	}
	public static bool AreCameraKeyboardButtonPressed()
	{
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
			return true;
		else return false;
	}
	public static bool IsMousePositionWithinBoundaries()
	{
		if (
			(Input.mousePosition.x < mouseScrollLimits.leftLimit && Input.mousePosition.x > -5) ||
			(Input.mousePosition.x > (Screen.width - mouseScrollLimits.rightLimit) && Input.mousePosition.x < (Screen.width + 5)) ||
			(Input.mousePosition.y < mouseScrollLimits.bottomLimit && Input.mousePosition.y > -5) ||
			(Input.mousePosition.y > (Screen.height - mouseScrollLimits.topLimit) && Input.mousePosition.y < (Screen.height + 5))
		)
		return true; else return false;
	}
}