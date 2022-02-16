using System.Collections;
using UnityEngine;

public class Joystick : MonoBehaviour
{
	private static Joystick[] joysticks;

	private static bool enumeratedJoysticks;

	public Vector2 position;

	public bool holdingDoubleTap;

	public bool hasTouch;

	private int touchID = -1;

	private Vector3 inputTransPos;

	private Transform objTransform;

	private float colliderRadius;

	private Vector2 homePos;

	private Camera hudCamera;

	public bool captureStrayTouches = true;

	public bool captureTouches = true;

	private Vector2 unboundTouch;

	private int unboundID;

	public LayerMask layerMask;

	private Transform directionIndicator;

	public bool isLeft;

	public void setUnboundTouch(Vector2 v)
	{
		unboundTouch = v;
	}

	private void Awake()
	{
		enumeratedJoysticks = false;
		hudCamera = base.transform.root.Find("Camera").GetComponent<Camera>();
		objTransform = base.transform;
		unboundTouch = new Vector2(0f, 0f);
		unboundID = -1;
		directionIndicator = objTransform.Find("indicator");
		if (captureStrayTouches)
		{
			StartCoroutine(captureStrayTouchesDelayedActivation());
		}
	}

	private void Start()
	{
		inputTransPos = objTransform.position;
		colliderRadius = (GetComponent(typeof(SphereCollider)) as SphereCollider).radius;
		position = default(Vector2);
		Vector3 vector = hudCamera.WorldToScreenPoint(inputTransPos);
		homePos = new Vector2(vector.x, vector.y);
	}

	private IEnumerator captureStrayTouchesDelayedActivation()
	{
		captureStrayTouches = false;
		yield return new WaitForSeconds(1f);
		captureStrayTouches = true;
	}

	public Vector2 getUnboundTouchPos()
	{
		return unboundTouch;
	}

	public bool hasUnboundTouch()
	{
		return unboundID != -1;
	}

	public bool touchIsBound(int ID)
	{
		return ID == touchID;
	}

	private void Update()
	{
		if (!enumeratedJoysticks)
		{
			joysticks = Object.FindObjectsOfType(typeof(Joystick)) as Joystick[];
			enumeratedJoysticks = true;
			return;
		}
		if (isLeft)
		{
			position.y = 0f;
		}
		position.x = 0f;
		for (int i = 0; i < Input.touchCount; i++)
		{
			if (i > 1)
			{
				continue;
			}
			Touch touch = Input.GetTouch(i);
			bool flag = false;
			Joystick[] array = joysticks;
			foreach (Joystick joystick in array)
			{
				if (joystick != this && joystick.touchIsBound(touch.fingerId))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			if (touch.fingerId != touchID && captureStrayTouches && touch.phase == TouchPhase.Began)
			{
				Ray ray = hudCamera.ScreenPointToRay(new Vector3(Input.touches[i].position.x, Input.touches[i].position.y, 0f));
				RaycastHit hitInfo;
				if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, 2000f, layerMask) && hitInfo.transform.gameObject == base.gameObject)
				{
					if (touch.fingerId == unboundID)
					{
						unboundID = -1;
						unboundTouch = new Vector2(0f, 0f);
					}
					if (captureTouches)
					{
						hasTouch = true;
						touchID = touch.fingerId;
					}
					if (touch.tapCount >= 2)
					{
						holdingDoubleTap = true;
					}
				}
			}
			if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
			{
				if (touchID == touch.fingerId)
				{
					hasTouch = false;
					touchID = -1;
					holdingDoubleTap = false;
					continue;
				}
				if (touch.fingerId == unboundID)
				{
					unboundID = -1;
					unboundTouch = new Vector2(0f, 0f);
					if (!hasTouch && holdingDoubleTap)
					{
						holdingDoubleTap = false;
					}
				}
			}
			if (touchID != touch.fingerId)
			{
				if (touch.phase == TouchPhase.Began)
				{
					unboundID = touch.fingerId;
					if (touch.tapCount >= 2)
					{
						holdingDoubleTap = true;
					}
				}
				if (touch.fingerId == unboundID)
				{
					unboundTouch = touch.position;
				}
				continue;
			}
			Vector2 vector = touch.position;
			Vector3 vector2 = new Vector3(vector.x - homePos.x, vector.y - homePos.y, 0f);
			if (vector2.magnitude > colliderRadius)
			{
				vector2.Normalize();
			}
			else
			{
				vector2 = vector2.normalized * (vector2.magnitude / colliderRadius);
			}
			position.x = vector2.x;
			position.y = vector2.y;
			if (isLeft)
			{
				directionIndicator.localPosition = new Vector3(-2f + vector2.x * -28f, -1f, vector2.y * -28f);
			}
			else
			{
				directionIndicator.localPosition = new Vector3(2f + vector2.x * -28f, -1f, vector2.y * -28f);
			}
		}
	}
}
