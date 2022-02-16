using UnityEngine;

public class SliderControl : MonoBehaviour
{
	public Camera hudCamera;

	public LayerMask layerMask;

	public bool isSlider;

	private Vector2 lastTouchPosition;

	private int currFingerID;

	private Vector3 deltaTouch;

	private Vector3 angleCrossHair = Vector3.zero;

	private Vector3 startTouchPosition;

	private Vector3[] deltaTouchArray = new Vector3[3];

	private int arraySize = 3;

	private int counter;

	public Vector3 position;

	private Vector3 crossPos = Vector3.zero;

	public Transform rotator;

	public float sensitivitySlider;

	private bool starting;

	private bool contact;

	private void Start()
	{
		if (hudCamera == null)
		{
			hudCamera = GameObject.Find("HUD/Camera").GetComponent<Camera>();
		}
		if (GameManager.Instance.currentCharacter == Character.RIGGS)
		{
			rotator = base.transform.Find("/RiggsFps/Rotator");
		}
		for (int i = 0; i < arraySize; i++)
		{
			deltaTouchArray[i] = new Vector3(0f, 0f, 0f);
		}
		deltaTouch = new Vector3(0f, 0f, 0f);
		position = new Vector3(0f, 0f, 0f);
		lastTouchPosition = new Vector2(0f, 0f);
		startTouchPosition = new Vector3(0f, 0f, 0f);
		crossPos = new Vector3(0f, 0f, 0f);
	}

	private void Update()
	{
		if (!isSlider)
		{
			position.x = 0f;
			RecordTouch(position.x);
		}
		for (int i = 0; i < Input.touchCount; i++)
		{
			if (i > 1)
			{
				continue;
			}
			Touch touch = Input.GetTouch(i);
			if (currFingerID == touch.fingerId)
			{
				if (!starting)
				{
					starting = true;
					lastTouchPosition.x = touch.position.x;
					lastTouchPosition.y = touch.position.y;
				}
				deltaTouch.x = touch.position.x - lastTouchPosition.x;
				deltaTouch.y = touch.position.y - lastTouchPosition.y;
				RecordTouch(deltaTouch.x);
				RecordTouchy(deltaTouch.y);
				deltaTouch = touch.deltaPosition;
				lastTouchPosition = touch.position;
			}
			Ray ray = hudCamera.ScreenPointToRay(new Vector3(Input.touches[i].position.x, Input.touches[i].position.y, 0f));
			RaycastHit hitInfo;
			if (touch.phase == TouchPhase.Began && Physics.Raycast(ray.origin, ray.direction, out hitInfo, 2000f, layerMask) && hitInfo.collider.name == "Slider")
			{
				isSlider = true;
				lastTouchPosition = touch.position;
				startTouchPosition = touch.position;
				currFingerID = touch.fingerId;
				if (GameManager.Instance.currentInvertOption == InvertOption.OFF)
				{
					crossPos.y = position.y;
				}
				else
				{
					crossPos.y = 0f - position.y;
				}
			}
			if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && currFingerID == touch.fingerId)
			{
				isSlider = false;
				currFingerID = -1;
			}
		}
		if (isSlider)
		{
			angleCrossHair.x = lastTouchPosition.y - startTouchPosition.y;
			deltaTouch.x = 0f;
			deltaTouch.y = 0f;
			for (int j = 0; j < 3; j++)
			{
				deltaTouch.x += deltaTouchArray[j].x;
				deltaTouch.y += deltaTouchArray[j].y;
			}
			if (GameManager.Instance.currentSensitivity > 0.3f)
			{
				sensitivitySlider = GameManager.Instance.currentSensitivity * 0.16f;
			}
			else
			{
				sensitivitySlider = 0.048f;
			}
			position.x = deltaTouch.x / 3f * sensitivitySlider;
			position.y = deltaTouch.y / 2f * sensitivitySlider;
			float num = 100f;
			if (GameManager.Instance.isIpad)
			{
				num = 350f;
			}
			if (crossPos.y + angleCrossHair.x / num > 0.89f)
			{
				crossPos.y = 0.89f - angleCrossHair.x / num;
			}
			else if (crossPos.y + angleCrossHair.x / num < -0.6f)
			{
				crossPos.y = -0.6f - angleCrossHair.x / num;
			}
			if (GameManager.Instance.currentInvertOption == InvertOption.OFF)
			{
				position.y = crossPos.y + angleCrossHair.x / num;
			}
			else
			{
				position.y = 0f - (crossPos.y + angleCrossHair.x / num);
			}
		}
		deltaTouch.x = 0f;
		deltaTouch.y = 0f;
	}

	private void RecordTouch(float f)
	{
		deltaTouchArray[counter].x = deltaTouch.x;
		if (++counter >= arraySize)
		{
			counter = 0;
		}
	}

	private void RecordTouchy(float f)
	{
		deltaTouchArray[counter].y = deltaTouch.y;
		if (++counter >= arraySize)
		{
			counter = 0;
		}
	}
}
