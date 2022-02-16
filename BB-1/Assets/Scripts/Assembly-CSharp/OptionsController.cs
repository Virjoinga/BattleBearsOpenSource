using System.Collections;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
	public GameObject style1Up;

	public GameObject style1Press;

	public GameObject style2Up;

	public GameObject style2Press;

	public GameObject effectsSlider;

	public GameObject musicSlider;

	public GameObject sensitivitySlider;

	public Transform effectsSliderTransform;

	public Transform musicSliderTransform;

	public Transform sensitivitySliderTransform;

	public GameObject confirmQuitMenu;

	public LayerMask sliderMask;

	public LayerMask sliderGrabMask;

	public float sliderMin = -83f;

	public float sliderMax = 120f;

	public float sensitivitySliderMax = 26f;

	private Camera sliderCamera;

	private bool skipOneFrame = true;

	private bool initialUpdateDone;

	private Transform currentSlider;

	public AudioClip changeEffectsSound;

	public GameObject quitUp;

	public GameObject quitPress;

	public GameObject scanlines;

	public TextMesh invertText;

	public TextMesh extraColorText;

	public TextMesh sliderValueText;

	public TextMesh windowText;

	public TextMesh resolutionText;

	public Controlls controlls;

	private Resolution[] resolution;

	private int screenCount;

	public GameObject KeyboardMap;

	public static OptionsController Instance;

	private void Awake()
	{
		Instance = this;
		setEffectsSlider();
		setMusicSlider();
		setSensitivitySlider();
		resolution = Screen.resolutions;
		screenCount = resolution.Length;
		KeyboardMap.SetActive(false);
	}

	private void Start()
	{
		if (Screen.fullScreen)
		{
			windowText.text = "OFF";
		}
		else
		{
			windowText.text = "ON";
		}
		screenCount = resolution.Length;
		resolutionText.text = Screen.width + " x " + Screen.height;
		controlls = GameObject.Find("Controlls").GetComponent<Controlls>();
		GameObject gameObject = GameObject.Find("MenuCamera");
		if (gameObject == null)
		{
			sliderCamera = base.transform.parent.Find("Camera").GetComponent<Camera>();
		}
		else
		{
			sliderCamera = gameObject.GetComponent<Camera>();
		}
		updateSelection();
	}

	private IEnumerator delayedUpdateSelection()
	{
		yield return new WaitForFixedUpdate();
		updateSelection();
	}

	private void updateSelection()
	{
		if (GameManager.Instance.currentGraphicsOption == GraphicsOption.ON)
		{
			extraColorText.text = "ON";
		}
		else
		{
			extraColorText.text = "OFF";
		}
		if (GameManager.Instance.currentInvertOption == InvertOption.ON)
		{
			invertText.text = "ON";
		}
		else
		{
			invertText.text = "OFF";
		}
	}

	private void style1()
	{
		GameManager.Instance.currentStyle = ControlStyle.STYLE1;
		updateSelection();
	}

	private void style2()
	{
		GameManager.Instance.currentStyle = ControlStyle.STYLE2;
		updateSelection();
	}

	private void extraColor()
	{
		if (GameManager.Instance.currentGraphicsOption == GraphicsOption.ON)
		{
			GameManager.Instance.currentGraphicsOption = GraphicsOption.OFF;
		}
		else
		{
			GameManager.Instance.currentGraphicsOption = GraphicsOption.ON;
		}
		updateSelection();
	}

	private void OpenFeint()
	{
	}

	public void Go()
	{
		SoundManager.Instance.OnTempMusicOn();
		SavedDataManager.Instance.savePreferences();
		Time.timeScale = 1f;
		SendMessageUpwards("OnResume", SendMessageOptions.DontRequireReceiver);
		if (GameManager.Instance.currentCharacter == Character.WIL)
		{
			RenderSettings.fog = true;
		}
		else
		{
			RenderSettings.fog = false;
		}
		Screen.lockCursor = true;
		Object.Destroy(base.gameObject);
	}

	private void Quit()
	{
		GameObject obj = Object.Instantiate(confirmQuitMenu);
		obj.transform.parent = base.transform.parent;
		obj.transform.position = new Vector3(0f, 150f, 0f);
		quitUp.SetActive(true);
		quitPress.SetActive(false);
	}

	private void MainMenu()
	{
		SavedDataManager.Instance.savePreferences();
	}

	private void Invert()
	{
		if (GameManager.Instance.currentInvertOption == InvertOption.ON)
		{
			GameManager.Instance.currentInvertOption = InvertOption.OFF;
		}
		else
		{
			GameManager.Instance.currentInvertOption = InvertOption.ON;
		}
		updateSelection();
		controlls.Initialize();
	}

	private void fullScreen()
	{
		if (Screen.fullScreen)
		{
			Screen.SetResolution(Screen.width, Screen.height, false);
		}
		else
		{
			Screen.SetResolution(Screen.width, Screen.height, true);
		}
		if (Screen.fullScreen)
		{
			windowText.text = "OFF";
		}
		else
		{
			windowText.text = "ON";
		}
	}

	private void screenShift(int dir)
	{
		int @int = PlayerPrefs.GetInt("ScreenResolution", 0);
		@int += dir;
		if (@int < 0)
		{
			@int = 0;
		}
		if (@int > screenCount - 1)
		{
			@int = screenCount - 1;
		}
		Screen.SetResolution(resolution[@int].width, resolution[@int].height, Screen.fullScreen);
		resolutionText.text = resolution[@int].width + "x" + resolution[@int].height;
		Debug.Log(@int);
		PlayerPrefs.SetInt("ScreenResolution", @int);
	}

	public void OnGUIButtonClicked(GUIButton b)
	{
		Debug.Log(b.gameObject.name);
		switch (b.gameObject.name)
		{
		case "keyboard_btn":
			break;
		case "style_1_btn":
			style1();
			break;
		case "style_2_btn":
			style2();
			break;
		case "extraColor_btn":
			extraColor();
			break;
		case "openfeint_btn":
			fullScreen();
			break;
		case "Invert_btn":
			Invert();
			break;
		case "fullscrn_btn":
			fullScreen();
			break;
		case "left_btn":
			screenShift(-1);
			break;
		case "right_btn":
			screenShift(1);
			break;
		case "Go_btn":
			Go();
			break;
		case "quit_btn":
			Quit();
			break;
		case "mainMenu_btn":
			MainMenu();
			break;
		}
	}

	private void Update()
	{
		bool flag = false;
		if (controlls.comp)
		{
			flag = true;
			if (Input.GetKeyDown("escape") && quitUp != null)
			{
				Go();
			}
		}
		else if (Input.touches.Length != 0)
		{
			flag = true;
		}
		if (skipOneFrame)
		{
			skipOneFrame = false;
		}
		else if (!initialUpdateDone)
		{
			updateSelection();
			initialUpdateDone = true;
		}
		if (!flag)
		{
			return;
		}
		Ray ray = sliderCamera.ScreenPointToRay(controlls.pos());
		RaycastHit hitInfo;
		if (currentSlider == null && controlls.began() && Physics.Raycast(ray.origin, ray.direction, out hitInfo, 2000f, sliderGrabMask))
		{
			Transform transform = hitInfo.transform;
			if (transform == sensitivitySliderTransform || transform == effectsSliderTransform || transform == musicSliderTransform)
			{
				currentSlider = hitInfo.transform;
			}
			else
			{
				currentSlider = null;
			}
		}
		if (currentSlider != null && controlls.moved() && Physics.Raycast(ray.origin, ray.direction, out hitInfo, 10000f, sliderMask))
		{
			GameObject gameObject = hitInfo.transform.gameObject;
			Vector3 point = hitInfo.point;
			if (gameObject == sensitivitySlider)
			{
				if (point.z > sensitivitySliderMax)
				{
					point.z = sensitivitySliderMax;
				}
				else if (point.z < sliderMin)
				{
					point.z = sliderMin;
				}
			}
			else if (point.z > sliderMax)
			{
				point.z = sliderMax;
			}
			else if (point.z < sliderMin)
			{
				point.z = sliderMin;
			}
			if (gameObject == effectsSlider && currentSlider == effectsSliderTransform)
			{
				Vector3 position = effectsSliderTransform.position;
				position.z = point.z;
				effectsSliderTransform.position = position;
				float num = (1f - (position.z - sliderMin) / (sliderMax - sliderMin)) / 1f;
				SoundManager.Instance.setEffectsVolume(num);
				SoundManager.Instance.setTempEffectsVolume(num);
			}
			else if (gameObject == musicSlider && currentSlider == musicSliderTransform)
			{
				Vector3 position2 = musicSliderTransform.position;
				position2.z = point.z;
				musicSliderTransform.position = position2;
				float num2 = (1f - (position2.z - sliderMin) / (sliderMax - sliderMin)) / 2f;
				SoundManager.Instance.setMusicVolume(num2);
				SoundManager.Instance.setTempMusicVolume(num2);
			}
			else if (gameObject == sensitivitySlider && currentSlider == sensitivitySliderTransform)
			{
				Vector3 position3 = sensitivitySliderTransform.position;
				position3.z = point.z;
				sensitivitySliderTransform.position = position3;
				float currentSensitivity = 1f - (position3.z - sliderMin) / (sensitivitySliderMax - sliderMin);
				GameManager.Instance.currentSensitivity = currentSensitivity;
				sliderValueText.text = ((int)(GameManager.Instance.currentSensitivity * 100f)).ToString();
				controlls.Initialize();
			}
		}
		else if (currentSlider != null && controlls.ended())
		{
			if (currentSlider == effectsSliderTransform)
			{
				SoundManager.Instance.playSound(changeEffectsSound);
			}
			GameObject gameObject2 = Object.Instantiate(currentSlider.gameObject);
			gameObject2.transform.parent = currentSlider.parent;
			gameObject2.transform.position = currentSlider.position;
			gameObject2.transform.localEulerAngles = currentSlider.localEulerAngles;
			gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject2.name = currentSlider.gameObject.name;
			if (currentSlider == effectsSliderTransform)
			{
				Object.Destroy(currentSlider.gameObject);
				effectsSliderTransform = gameObject2.transform;
			}
			else if (currentSlider == musicSliderTransform)
			{
				Object.Destroy(currentSlider.gameObject);
				musicSliderTransform = gameObject2.transform;
			}
			else if (currentSlider == sensitivitySliderTransform)
			{
				Object.Destroy(currentSlider.gameObject);
				sensitivitySliderTransform = gameObject2.transform;
				sliderValueText = gameObject2.transform.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
			}
			currentSlider = null;
		}
	}

	private void setEffectsSlider()
	{
		Vector3 position = effectsSliderTransform.position;
		position.z = (0f - (SoundManager.Instance.getEffectsVolume() - 1f)) * (sliderMax - sliderMin) + sliderMin;
		effectsSliderTransform.position = position;
	}

	private void setMusicSlider()
	{
		Vector3 position = musicSliderTransform.position;
		position.z = (0f - (SoundManager.Instance.getMusicVolume() * 2f - 1f)) * (sliderMax - sliderMin) + sliderMin;
		musicSliderTransform.position = position;
	}

	private void setSensitivitySlider()
	{
		Vector3 position = sensitivitySliderTransform.position;
		position.z = (0f - (GameManager.Instance.currentSensitivity - 1f)) * (sensitivitySliderMax - sliderMin) + sliderMin;
		sensitivitySliderTransform.position = position;
		sliderValueText.text = ((int)(GameManager.Instance.currentSensitivity * 100f)).ToString();
	}
}
