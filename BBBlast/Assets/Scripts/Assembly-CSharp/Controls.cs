using System.Collections;
using UnityEngine;

public class Controls : MonoBehaviour
{
	private float x;

	private float y;

	private float width;

	private float height;

	[HideInInspector]
	public bool disabled;

	private float myTime;

	private Vector3 touchToRay;

	private Transform myTransform;

	public Texture safe;

	public Texture fire;

	public RangedWeaponController tlc;

	public LayerMask aimMask;

	private GameObject prevSelection;

	private GameObject curSelection;

	private bool isShoot;

	public GameObject myCamera;

	private Transform myCamTrans;

	private float cameraIndex;

	public Animation mwBlast;

	private bool blast;

	private Ray touch;

	private RaycastHit hit;

	private bool isObjectHit;

	private RaycastHit lastObjectHit;

	private float degreesPerPixel = -0.04f;

	private float pixelIndex;

	private float difference;

	private AudioSource myAudio;

	private AudioClip start;

	private AudioClip loop;

	private AudioClip end;

	public float sensitivity = 64f;

	private Vector2 lastTouch;

	private Vector2 curTouch;

	private Color defaultBGColor;

	private Color defaultColor;

	private Rect window = new Rect((Screen.width - 400) / 2, (Screen.height - 250) / 2, 400f, 250f);

	private Color alertColor = Color.red;

	private GUISkin gskin;

	private bool showQuitMenu;

	private bool wasPressed;

	public void Start()
	{
		x = (float)Screen.width * 0.5f;
		y = (float)Screen.height * 0.5f;
		curTouch = new Vector2(x, y);
		lastTouch = curTouch;
		myAudio = base.GetComponent<AudioSource>();
		isShoot = false;
		disabled = false;
		cameraIndex = -1f;
		mwBlast.Play("idleStart");
		pixelIndex = x;
		GameManager.Instance.hudController.turnCrosshairsOff();
		difference = 0f;
		myCamTrans = myCamera.transform;
		myTransform = base.transform;
		start = Resources.Load("blast_start") as AudioClip;
		loop = Resources.Load("blast_loop") as AudioClip;
		end = Resources.Load("blast_stop") as AudioClip;
	}

	private void OnGUI()
	{
		if (showQuitMenu)
		{
			gskin = GUI.skin;
			gskin.button.fontSize = 18;
			gskin.label.fontSize = 30;
			gskin.window.fontSize = 18;
			defaultBGColor = GUI.backgroundColor;
			if (GUI.Button(new Rect(0f, 0f, Screen.width, Screen.height), string.Empty, gskin.box))
			{
				showQuitMenu = false;
			}
			GUI.backgroundColor = alertColor;
			window = GUI.Window(0, window, OnPositive, "Quit?", gskin.window);
			GUI.backgroundColor = defaultBGColor;
		}
	}

	private void OnPositive(int windowID)
	{
		gskin.label.fontSize = 18;
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Would you like to quit playing?", gskin.label);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		if (GUI.Button(new Rect(window.width / 2f - 170f, window.height - 100f, 150f, 60f), "Yes", gskin.button))
		{
			Application.Quit();
		}
		if (GUI.Button(new Rect(window.width / 2f + 20f, window.height - 100f, 150f, 60f), "No", gskin.button))
		{
			showQuitMenu = false;
		}
	}

	private void Update()
	{
		if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.currentGameMode == GameMode.MENU)
		{
			showQuitMenu = true;
		}
		if (GameManager.Instance.isOver)
		{
			GameManager.Instance.hudController.turnCrosshairsOff();
			lastObjectHit = default(RaycastHit);
			myAudio.Stop();
			return;
		}
		if (disabled)
		{
			GameManager.Instance.hudController.turnCrosshairsOff();
			myAudio.Stop();
			return;
		}
		if (GameManager.Instance.currentGameMode == GameMode.MENU)
		{
			menuControls();
		}
		else if (GameManager.Instance.currentGameMode == GameMode.BACKPACK)
		{
			GameManager.Instance.hudController.turnCrosshairsOff();
		}
		else if (!GameManager.Instance.isTutorial)
		{
			gameControls();
		}
		if (!GameManager.Instance.isLevel)
		{
			return;
		}
		if (Time.timeScale != 0f)
		{
			if (x < (float)Screen.width * 0.35f)
			{
				StartCoroutine(GameManager.Instance.animC.move(4f, isShoot));
			}
			else if (x > (float)Screen.width * 0.65f)
			{
				StartCoroutine(GameManager.Instance.animC.move(-4f, isShoot));
			}
			else if (x > (float)Screen.width * 0.35f && x < (float)Screen.width * 0.65f)
			{
				StartCoroutine(GameManager.Instance.animC.move(0f, isShoot));
			}
			else
			{
				GameManager.Instance.animC.idle(isShoot);
			}
		}
		difference = pixelIndex - x;
		if (difference != 0f)
		{
			myCamTrans.RotateAround(myTransform.position, Vector3.up, difference * degreesPerPixel);
			pixelIndex = x;
		}
	}

	private void menuControls()
	{
		if (!Application.isEditor)
		{
			if (Input.touchCount <= 0)
			{
				return;
			}
			touchToRay = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0f);
			Ray ray = Camera.main.ScreenPointToRay(touchToRay);
			RaycastHit hitInfo;
			bool flag = Physics.Raycast(ray.origin, ray.direction, out hitInfo, 150f, aimMask);
			Touch touch = Input.GetTouch(0);
			if (lastObjectHit.collider != null && lastObjectHit.collider != hitInfo.collider)
			{
				lastObjectHit.collider.SendMessage("unSelected", SendMessageOptions.DontRequireReceiver);
			}
			lastObjectHit = hitInfo;
			if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
			{
				if (flag)
				{
					hitInfo.collider.SendMessage("selected", SendMessageOptions.DontRequireReceiver);
				}
			}
			else if (flag)
			{
				hitInfo.collider.SendMessage("unSelected", SendMessageOptions.DontRequireReceiver);
				hitInfo.collider.SendMessage("clicked", SendMessageOptions.DontRequireReceiver);
			}
		}
		else if (Input.GetMouseButton(0))
		{
			touchToRay = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
			Ray ray2 = Camera.main.ScreenPointToRay(touchToRay);
			RaycastHit hitInfo2;
			if (Physics.Raycast(ray2.origin, ray2.direction, out hitInfo2, 150f, aimMask))
			{
				hitInfo2.collider.SendMessage("clicked", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private void gameControls()
	{
		if (Input.touchCount > 0)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
				curTouch = Input.GetTouch(0).position;
				lastTouch = curTouch;
			}
			if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
			{
				curTouch = Input.GetTouch(0).position;
			}
			wasPressed = true;
			if (/*GameManager.Instance.hudController.crosshairsSafe.active*/ GameManager.Instance.hudController.crosshairsSafe)
			{
				GameManager.Instance.hudController.turnCrosshairsFire();
			}
			isShoot = true;
			StopCoroutine("stopBlast");
			if (!blast)
			{
				StartCoroutine("startBlast");
			}
			touchToRay = new Vector3(x, y, 0f);
			touch = Camera.main.ScreenPointToRay(touchToRay);
			isObjectHit = Physics.Raycast(touch.origin, touch.direction, out hit, 150f, aimMask);
			if (isObjectHit)
			{
				GameManager.Instance.hitPos = hit.point;
			}
			else
			{
				GameManager.Instance.hitPos = Vector3.zero;
			}
			x += (curTouch.x - lastTouch.x) * sensitivity;
			y += (curTouch.y - lastTouch.y) * sensitivity;
			if (x > (float)Screen.width)
			{
				x = Screen.width;
			}
			if (x < 0f)
			{
				x = 0f;
			}
			if (y > (float)(Screen.height + 5))
			{
				y = Screen.height + 5;
			}
			if (y < (float)Screen.height * 0.25f)
			{
				y = (float)Screen.height * 0.25f;
			}
			float num = (float)Screen.width / 8f;
			float num2 = 0.721f * (float)Screen.height / 8.5f;
			GameManager.Instance.hudController.moveCrosshairs(new Vector2(x / num - (float)Screen.width * 0.5f / num, y / num2 - (float)Screen.height * 0.5f / num2));
			lastTouch = curTouch;
		}
		else
		{
			wasPressed = false;
			GameManager.Instance.hudController.turnCrosshairsSafe();
			isShoot = false;
			StopCoroutine("startBlast");
			if (blast)
			{
				StartCoroutine("stopBlast");
			}
		}
	}

	private IEnumerator startBlast()
	{
		blast = true;
		myAudio.Stop();
		myAudio.PlayOneShot(start);
		mwBlast.Play("introCharge");
		yield return new WaitForSeconds(mwBlast.GetClip("introCharge").length);
		mwBlast.CrossFade("idleCharge");
		myAudio.loop = true;
		myAudio.clip = loop;
		myAudio.Play();
	}

	private IEnumerator stopBlast()
	{
		blast = false;
		mwBlast.CrossFade("outroCharge");
		myAudio.Stop();
		myAudio.PlayOneShot(end);
		yield return new WaitForSeconds(mwBlast.GetClip("outroCharge").length * 0.5f);
		yield return new WaitForSeconds(mwBlast.GetClip("outroCharge").length * 0.5f);
	}

	public void moveCam(float dir)
	{
		if (cameraIndex == dir)
		{
			return;
		}
		if (dir == 0f)
		{
			myCamTrans.RotateAround(myTransform.position, Vector3.up, -0.2f);
			if (myCamTrans.localEulerAngles.y <= 174f)
			{
				cameraIndex = dir;
			}
		}
		if (dir == 1f)
		{
			myCamTrans.RotateAround(myTransform.position, Vector3.up, 0.2f);
			if (myCamTrans.localEulerAngles.y >= 186f)
			{
				cameraIndex = dir;
			}
		}
		if (dir == 2f)
		{
			cameraIndex = -1f;
		}
	}

	public void resetCrosshairs(bool reEnable)
	{
	}
}
