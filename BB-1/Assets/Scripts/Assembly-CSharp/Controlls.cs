using BSCore;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controlls : MonoBehaviour
{
	public bool comp;

	public HUDController HUD;

	protected Joystick leftStick;

	protected Joystick rightStick;

	public bool leftTouch;

	public bool rightTouch;

	private Vector2 rightVec;

	private Vector2 leftVec;

	public BoxCollider hitBox;

	public GameManager GM;

	public float sensitivity;

	private OCOHUDController OCOHUD;

	private float invert;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		if (Application.platform != RuntimePlatform.Android)
		{
			comp = true;
		}
		else
		{
			comp = false;
		}
		hitBox.size = new Vector3(Screen.width, 1f, Screen.height);
	}

	private void OnLevelWasLoaded()
	{
		if (comp && Application.loadedLevelName != "MainMenu")
		{
			//Screen.lockCursor = true;
			Cursor.lockState = CursorLockMode.Locked;
		}
		GameObject gameObject = GameObject.Find("OCO_HUD");
		if (gameObject != null)
		{
			OCOHUD = gameObject.GetComponent<OCOHUDController>();
		}
	}

	private void OnMouseDown()
	{
		if ((bool)HUD && comp)
		{
			//Screen.lockCursor = true;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	public void Initialize()
	{
		sensitivity = GM.currentSensitivity;
		if (GameManager.Instance.currentInvertOption == InvertOption.ON)
		{
			invert = -1f;
		}
		else
		{
			invert = 1f;
		}
		GameObject gameObject = GameObject.Find("HUD");
		if (gameObject != null)
		{
			HUD = gameObject.GetComponent(typeof(HUDController)) as HUDController;
		}
	}

	private void Update()
	{
		if ((bool)leftStick)
		{
			leftTouch = leftStick.hasTouch;
			rightTouch = rightStick.hasTouch;
		}
		if (BSCoreInput.GetButton(Option.Settings))
		{
			Screen.lockCursor = false;
		}
		if (HUD != null)
		{
			if (BSCoreInput.GetButtonDown(Option.SwitchWeapon))
			{
				HUD.WeaponToggle(-1);
			}
			if (BSCoreInput.GetButton(Option.WeaponSlot1))
			{
				HUD.WeaponToggle((GameManager.Instance.currentCharacter == Character.WIL) ? 6 : 0);
			}
			if (BSCoreInput.GetButton(Option.WeaponSlot2))
			{
				HUD.WeaponToggle((GameManager.Instance.currentCharacter != Character.WIL) ? 1 : 7);
			}
			if (BSCoreInput.GetButton(Option.WeaponSlot3))
			{
				HUD.WeaponToggle(2);
			}
			if (BSCoreInput.GetButton(Option.WeaponSlot4))
			{
				HUD.WeaponToggle(3);
			}
			if (BSCoreInput.GetButton(Option.WeaponSlot5))
			{
				HUD.WeaponToggle(4);
			}
		}
	}

	public bool began()
	{
		bool flag = false;
		if (comp)
		{
			return Input.GetMouseButtonDown(0);
		}
		return Input.touches[0].phase == TouchPhase.Began;
	}

	public bool moved()
	{
		bool flag = false;
		if (comp)
		{
			return Input.GetMouseButton(0);
		}
		return Input.touches[0].phase == TouchPhase.Moved;
	}

	public bool ended()
	{
		bool flag = false;
		if (comp)
		{
			return Input.GetMouseButtonUp(0);
		}
		return Input.touches[0].phase == TouchPhase.Ended;
	}

	public Vector2 pos()
	{
		if (comp)
		{
			return Input.mousePosition;
		}
		return Input.touches[0].position;
	}

	public Vector2 move()
	{
		if (comp)
		{
			leftVec.x = BSCoreInput.GetAxis(Option.Horizontal);
			leftVec.y = BSCoreInput.GetAxis(Option.Vertical);
			if (BSCoreInput.GetButtonDown(Option.Scoreboard))
			{
				HUD.mapToggle();
			}
			if (BSCoreInput.GetButtonDown(Option.Settings))
			{
				HUD.pause();
			}
		}
		else
		{
			leftVec = leftStick.position;
		}
		if (leftVec.x != 0f && leftVec.y != 0f)
		{
			return leftVec * 0.75f;
		}
		return leftVec;
	}

	public Vector2 aim()
	{
		if (comp)
		{
			rightVec.x = BSCoreInput.GetAxis(Option.CameraHorizontal) * sensitivity;
			rightVec.y += BSCoreInput.GetAxis(Option.CameraVertical) * sensitivity * invert;
			if (rightVec.y > 310f)
			{
				rightVec.y = 310f;
			}
			if (rightVec.y < 120f)
			{
				rightVec.y = 120f;
			}
		}
		else
		{
			rightVec.x = rightStick.position.x;
			rightVec.y = rightStick.position.y;
		}
		return rightVec;
	}

	public bool fire()
	{
		bool flag = false;
		if (comp)
		{
			if (BSCoreInput.GetButton(Option.Fire) && OCOHUD == null && Time.timeScale != 0f)
			{
				flag = true;
			}
			else if (BSCoreInput.GetButton(Option.Fire) && OCOHUD != null)
			{
				if (OCOHUD.FireSelection() == 3)
				{
					flag = true;
				}
			}
			else
			{
				flag = false;
			}
		}
		else if ((flag && HUDController.Instance.isSafetyOn) || Input.touches.Length == 0 || (Input.touches.Length == 1 && leftTouch))
		{
			flag = false;
		}
		else if (!flag && !HUDController.Instance.safetyOnButton.active && rightTouch)
		{
			flag = true;
		}
		return flag;
	}
}
