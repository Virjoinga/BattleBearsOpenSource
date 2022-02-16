using System;
using System.Collections;
using UnityEngine;

public class HUDController : MonoBehaviour
{
	private PlayerController playerController;

	private Transform playerTransform;

	public TextMesh scoreText;

	public TextMesh livesText;

	private DamageReceiver dmgReceiver;

	public Renderer healthRenderer;

	public Renderer ammoRenderer;

	public Animation lifeBarAnimation;

	public TextMesh roomText;

	private Transform myTransform;

	private bool livesShowing;

	private float lastLifeshowTime;

	private GameObject minimap;

	private Transform minimapArrow;

	private bool minimapButtonIn;

	private bool minimapFullIn;

	private bool minimapAnimating;

	private static HUDController instance;

	public GameObject safetyOnButton;

	public GameObject safetyOffButton;

	public Animation safetyAnimation;

	public Renderer style2AmmoRenderer;

	public Renderer fireRendererPlate;

	public Material greyMaterial;

	private float ammoOffset;

	private Material colorMaterial;

	public GameObject pauseButton;

	public GameObject ingameOptionsMenu;

	public Transform reticleOrange;

	public Transform reticleGreen;

	public Transform reticleOCO;

	private Transform currentReticle;

	public GameObject rightControlStyle1;

	public GameObject rightControlStyle2;

	public GameObject leftReticle;

	public GameObject rightReticle;

	public AudioClip safetyOn;

	public AudioClip safetyOff;

	public AudioClip hideMinimap;

	public AudioClip showMinimap;

	public AudioClip roomClearSound;

	public AudioClip roomClearSoundWil;

	public AudioClip weaponSwitchSound;

	public bool isSafetyOn;

	private HUDGUIController hudGUIController;

	public SliderControl sliderControl;

	public GameObject roomBar;

	public GameObject chargeBarObject;

	public Renderer[] chargeBars;

	public GameObject[] weaponToggles;

	private int currentWeaponIndex;

	private float[] maxAmmoAmounts;

	public float[] currentAmmoAmounts;

	public bool[] specialModes;

	public Animation weaponToggleAnimation;

	public GameObject iPadWeaponSwitcher;

	public Material yellowChargebar;

	public Material greenChargebar;

	private GenericCharacterController genericCharacterController;

	public Camera hudCamera;

	public static HUDController Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		myTransform = base.transform;
		int length = Enum.GetValues(typeof(WeaponType)).Length;
		maxAmmoAmounts = new float[length];
		currentAmmoAmounts = new float[length];
		currentAmmoAmounts[0] = 1f;
		currentAmmoAmounts[6] = 1f;
		for (int i = 1; i < currentAmmoAmounts.Length; i++)
		{
			currentAmmoAmounts[i] = 0f;
		}
		specialModes = new bool[length];
		for (int j = 0; j < specialModes.Length; j++)
		{
			specialModes[j] = false;
		}
		hudGUIController = GetComponent(typeof(HUDGUIController)) as HUDGUIController;
		if (GameManager.Instance.currentCharacter == Character.WIL)
		{
			currentWeaponIndex = 6;
		}
		colorMaterial = ammoRenderer.sharedMaterial;
	}

	private void Start()
	{
		if (GameManager.Instance.currentGameMode != GameMode.SURVIVAL)
		{
			scoreText.gameObject.active = false;
		}
	}

	public void updateScore()
	{
		if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			if (StatsManager.Instance.currentScore == 0)
			{
				scoreText.text = "Score: 0";
			}
			else
			{
				scoreText.text = string.Format("Score: {0:#,#}", StatsManager.Instance.currentScore);
			}
		}
	}

	public void linkupGenericPlayer()
	{
		genericCharacterController = UnityEngine.Object.FindObjectOfType(typeof(GenericCharacterController)) as GenericCharacterController;
		playerTransform = genericCharacterController.transform;
		if (GameManager.Instance.currentStyle == ControlStyle.STYLE1)
		{
			changeToStyle1();
		}
		else
		{
			changeToStyle2();
		}
		if (pauseButton != null)
		{
			pauseButton.active = true;
		}
		OnSetNonFireReticle();
	}

	public Vector2 getRetPos()
	{
		return new Vector2(0f, -1f * (currentReticle.localPosition.z / 320f) + 0.5f);
	}

	public void linkupPlayer()
	{
		playerController = UnityEngine.Object.FindObjectOfType(typeof(PlayerController)) as PlayerController;
		maxAmmoAmounts[0] = 1f;
		maxAmmoAmounts[1] = playerController.spreadshotAmmoGains[GameManager.Instance.getDifficulty()] * 3;
		maxAmmoAmounts[2] = playerController.bearzookaAmmoGains[GameManager.Instance.getDifficulty()] * 3;
		maxAmmoAmounts[3] = playerController.getSpecialAmmoGains() * 3f;
		maxAmmoAmounts[4] = playerController.getSpecialAmmoGains() * 3f;
		maxAmmoAmounts[5] = playerController.getSpecialAmmoGains2() * 3f;
		if (GameManager.Instance.currentCharacter == Character.WIL)
		{
			maxAmmoAmounts[6] = 1f;
			maxAmmoAmounts[7] = 1f;
		}
		if (GameManager.Instance.currentCharacter == Character.WIL)
		{
			currentAmmoAmounts[6] = 1f;
		}
		dmgReceiver = GameObject.FindWithTag("Player").GetComponent(typeof(DamageReceiver)) as DamageReceiver;
		if (ModeManager.Instance != null && !ModeManager.Instance.survivalMode)
		{
			roomBar.SetActive(false);
			if (GameManager.Instance.currentGameMode == GameMode.CAMPAIGN)
			{
				if (GameManager.Instance.currentCharacter == Character.OLIVER)
				{
					minimap = UnityEngine.Object.Instantiate(Resources.Load("MinimapOliverLevel" + GameManager.Instance.currentStage)) as GameObject;
				}
				else if (GameManager.Instance.currentCharacter == Character.RIGGS)
				{
					minimap = UnityEngine.Object.Instantiate(Resources.Load("MinimapRiggsLevel" + GameManager.Instance.currentStage)) as GameObject;
				}
				else if (GameManager.Instance.currentCharacter == Character.WIL)
				{
					minimap = UnityEngine.Object.Instantiate(Resources.Load("MinimapWilLevel" + GameManager.Instance.currentStage)) as GameObject;
				}
				minimap.transform.parent = myTransform;
				(GetComponent(typeof(hudAdjuster)) as hudAdjuster).getMiniMap(minimap);
				minimapArrow = minimap.transform.Find("Minimap/initialArrowOffset/arrow");
				if (GameManager.Instance.isLoading)
				{
					float @float = PlayerPrefs.GetFloat("miniArrowX");
					float float2 = PlayerPrefs.GetFloat("miniArrowY");
					float float3 = PlayerPrefs.GetFloat("miniArrowZ");
					minimapArrow.localPosition = new Vector3(@float, float2, float3);
				}
				OnRoomCleared();
			}
		}
		dmgReceiver.listeners.Add(base.gameObject);
		updateScore();
		updateLives();
	}

	private void updateAmmoMaterials()
	{
		ammoOffset = ammoRenderer.sharedMaterial.mainTextureOffset.x;
		ammoRenderer.sharedMaterial = colorMaterial;
		colorMaterial.mainTextureOffset = new Vector2(ammoOffset, 0f);
	}

	public void OnSetReticlePos(Vector3 pos)
	{
		float num = 160f;
		currentReticle.localPosition = new Vector3(0f, 0f, 0f - pos.y + num);
	}

	public void OnSetOCOReticle()
	{
		currentReticle = reticleOCO;
		reticleOrange.gameObject.active = false;
		reticleGreen.gameObject.active = false;
		reticleOCO.gameObject.active = true;
	}

	public void OnSetFireReticle()
	{
		currentReticle = reticleOrange;
		reticleOrange.gameObject.active = true;
		reticleGreen.gameObject.active = false;
		reticleOCO.gameObject.active = false;
	}

	public void OnSetNonFireReticle()
	{
		currentReticle = reticleGreen;
		reticleOrange.gameObject.active = false;
		reticleGreen.gameObject.active = true;
		reticleOCO.gameObject.active = false;
	}

	public void OnRoomCleared()
	{
		if (!minimapButtonIn && !minimapFullIn && !(minimap == null))
		{
			minimap.SetActive(true);
			minimap.GetComponent<Animation>().CrossFade("buttonIn");
			minimapButtonIn = true;
			StartCoroutine("arrowRotator");
		}
	}

	public void OnRoomClearFirstTime()
	{
		if (!GameManager.Instance.vent)
		{
			SoundManager.Instance.playSound((GameManager.Instance.currentCharacter != Character.WIL) ? roomClearSound : roomClearSoundWil);
		}
	}

	public void OnMoveRooms(ExitDirection dir)
	{
		Vector3 localPosition = minimapArrow.localPosition;
		switch (dir)
		{
		case ExitDirection.NORTH:
			localPosition.z -= 30f;
			break;
		case ExitDirection.EAST:
			localPosition.x += 30f;
			break;
		case ExitDirection.SOUTH:
			localPosition.z += 30f;
			break;
		case ExitDirection.WEST:
			localPosition.x -= 30f;
			break;
		}
		minimapArrow.localPosition = localPosition;
		if (!minimap.activeSelf)
		{
			minimap.SetActive(true);
		}
		minimap.SendMessage("OnMove", dir, SendMessageOptions.DontRequireReceiver);
	}

	public void OnSaveMinimap()
	{
		if (minimap != null)
		{
			minimap.SendMessage("OnSaveData", SendMessageOptions.DontRequireReceiver);
		}
		if (minimapArrow != null)
		{
			PlayerPrefs.SetFloat("miniArrowX", minimapArrow.localPosition.x);
			PlayerPrefs.SetFloat("miniArrowY", minimapArrow.localPosition.y);
			PlayerPrefs.SetFloat("miniArrowZ", minimapArrow.localPosition.z);
		}
	}

	public void OnHideMinimap()
	{
		if (minimapFullIn)
		{
			string text = "mapFullOut";
			if (GameManager.Instance.currentCharacter == Character.RIGGS)
			{
				text = "big" + text;
			}
			if (GameManager.Instance.currentCharacter == Character.WIL && GameManager.Instance.currentStage == 2)
			{
				text = "big" + text;
			}
			StartCoroutine(animateMinimapOut(text));
		}
		else if (minimapButtonIn)
		{
			StartCoroutine(animateMinimapOut("buttonOut"));
		}
	}

	private IEnumerator animateMinimapOut(string animationType)
	{
		minimapAnimating = true;
		minimap.GetComponent<Animation>().CrossFade(animationType);
		yield return new WaitForSeconds(minimap.GetComponent<Animation>()[animationType].length);
		StopCoroutine("arrowRotator");
		minimap.SetActive(false);
		minimapButtonIn = false;
		minimapFullIn = false;
		minimapAnimating = false;
	}

	private IEnumerator arrowRotator()
	{
		while (true)
		{
			minimapArrow.rotation = playerTransform.rotation;
			yield return new WaitForSeconds(0.08f);
		}
	}

	private IEnumerator expandMinimap()
	{
		if (minimapButtonIn)
		{
			SoundManager.Instance.playSound(showMinimap);
			minimapAnimating = true;
			string text = "mapIn";
			if (GameManager.Instance.currentCharacter == Character.RIGGS)
			{
				text = "big" + text;
			}
			if (GameManager.Instance.currentCharacter == Character.WIL && GameManager.Instance.currentStage == 2)
			{
				text = "big" + text;
			}
			minimap.GetComponent<Animation>().CrossFade(text);
			yield return new WaitForSeconds(minimap.GetComponent<Animation>()[text].length);
			minimapFullIn = true;
			minimapButtonIn = false;
			minimapAnimating = false;
		}
	}

	private IEnumerator shrinkMinimap()
	{
		SoundManager.Instance.playSound(hideMinimap);
		minimapAnimating = true;
		string text = "mapToButton";
		if (GameManager.Instance.currentCharacter == Character.RIGGS)
		{
			text = "big" + text;
		}
		if (GameManager.Instance.currentCharacter == Character.WIL && GameManager.Instance.currentStage == 2)
		{
			text = "big" + text;
		}
		minimap.GetComponent<Animation>().CrossFade(text);
		yield return new WaitForSeconds(minimap.GetComponent<Animation>()[text].length);
		minimapButtonIn = true;
		minimapFullIn = false;
		minimapAnimating = false;
	}

	private void changeToStyle1()
	{
		genericCharacterController.updateInputScheme();
		rightControlStyle1.SetActive(true);
		updateAmmoMaterials();
	}

	private void changeToStyle2()
	{
		genericCharacterController.updateInputScheme();
		rightControlStyle1.SetActive(false);
		rightControlStyle2.SetActive(true);
		rightReticle.SetActive(false);
		updateAmmoMaterials();
	}

	private void OnResume()
	{
		hudGUIController.enabled = true;
	}

	public void pause()
	{
		if (Time.timeScale > 0f)
		{
			GameObject obj = UnityEngine.Object.Instantiate(ingameOptionsMenu);
			obj.transform.parent = myTransform;
			obj.transform.localPosition = new Vector3(0f, 30f, 0f);
			RenderSettings.fog = false;
			SoundManager.Instance.OnTempMusicOff();
			Time.timeScale = 0f;
			hudGUIController.enabled = false;
		}
		else if (OptionsController.Instance != null)
		{
			OptionsController.Instance.Go();
		}
	}

	public void mapToggle()
	{
		if (!minimapFullIn && !minimapAnimating)
		{
			StartCoroutine(expandMinimap());
		}
		else if (minimapFullIn && !minimapAnimating)
		{
			StartCoroutine(shrinkMinimap());
		}
	}

	public void WeaponToggle(int inputVal)
	{
		if (hasOnlyMachinegun())
		{
			return;
		}
		if (inputVal == -1)
		{
			do
			{
				currentWeaponIndex++;
				if (currentWeaponIndex >= weaponToggles.Length)
				{
					currentWeaponIndex = 0;
				}
			}
			while (currentAmmoAmounts[currentWeaponIndex] <= 0f);
			if (currentWeaponIndex == 0 && GameManager.Instance.currentCharacter == Character.WIL)
			{
				currentWeaponIndex = 6;
			}
			switchToWeapon(currentWeaponIndex);
		}
		else if (currentWeaponIndex != inputVal && inputVal != -1 && currentAmmoAmounts[inputVal] > 0f)
		{
			currentWeaponIndex = inputVal;
			switchToWeapon(currentWeaponIndex);
		}
	}

	private void OnGUIButtonClicked(GUIButton b)
	{
		switch (b.name)
		{
		case "Pause":
			pause();
			break;
		case "style_1_btn":
			changeToStyle1();
			break;
		case "style_2_btn":
			changeToStyle2();
			break;
		case "mapToggleButton":
			mapToggle();
			break;
		case "yes_btn":
			SoundManager.Instance.OnTempMusicOn();
			Application.LoadLevel("MainMenu");
			break;
		case "no_btn":
			UnityEngine.Object.Destroy(b.transform.parent.gameObject);
			break;
		case "WeaponToggles":
			WeaponToggle(-1);
			break;
		case "iPadSwitcher":
			WeaponToggle(-1);
			break;
		}
	}

	public void resetWeapons()
	{
		for (int i = 1; i < currentAmmoAmounts.Length; i++)
		{
			currentAmmoAmounts[i] = 0f;
		}
		for (int j = 0; j < specialModes.Length; j++)
		{
			specialModes[j] = false;
		}
		if (GameManager.Instance.currentCharacter == Character.WIL)
		{
			switchToWeapon(6);
		}
		else
		{
			switchToWeapon(0);
		}
	}

	public void switchToWeapon(int index)
	{
		currentWeaponIndex = index;
		chargeBarObject.SetActive(maxAmmoAmounts[index] != 1f);
		for (int i = 0; i < 3; i++)
		{
			Vector2 mainTextureOffset = chargeBars[i].material.mainTextureOffset;
			chargeBars[i].sharedMaterial = (specialModes[index] ? yellowChargebar : greenChargebar);
			chargeBars[i].sharedMaterial.mainTextureOffset = mainTextureOffset;
		}
		for (int j = 0; j < weaponToggles.Length; j++)
		{
			weaponToggles[j].SetActive(j == index);
		}
		playerController.switchToWeaponType((WeaponType)index, specialModes[index]);
		if (index == 0 && hasOnlyMachinegun())
		{
			if (iPadWeaponSwitcher != null)
			{
				StartCoroutine(delayediPadSwitcher());
			}
			StartCoroutine(delayedWeaponToggleOut());
		}
		else
		{
			SoundManager.Instance.playSound(weaponSwitchSound);
		}
		updateAmmoBars();
	}

	public void forceWeaponToggleHide()
	{
		weaponToggleAnimation.gameObject.SetActive(false);
		if (iPadWeaponSwitcher != null)
		{
			iPadWeaponSwitcher.SetActive(false);
		}
	}

	private IEnumerator delayedWeaponToggleOut()
	{
		weaponToggleAnimation.Play("out");
		yield return new WaitForSeconds(weaponToggleAnimation["out"].length);
		weaponToggleAnimation.gameObject.SetActive(false);
	}

	private IEnumerator delayediPadSwitcher()
	{
		if (iPadWeaponSwitcher != null)
		{
			iPadWeaponSwitcher.GetComponent<Animation>().Play("out");
			yield return new WaitForSeconds(iPadWeaponSwitcher.GetComponent<Animation>()["out"].length);
		}
		if (iPadWeaponSwitcher != null)
		{
			iPadWeaponSwitcher.SetActive(false);
		}
	}

	private bool hasOnlyMachinegun()
	{
		for (int i = 1; i < currentAmmoAmounts.Length; i++)
		{
			if (currentAmmoAmounts[i] > 0f)
			{
				return false;
			}
		}
		return true;
	}

	public void addAmmo(WeaponType weaponType, float amount)
	{
		if (specialModes[(int)weaponType] && amount > 0f)
		{
			return;
		}
		if (specialModes[(int)weaponType] && amount < 0f)
		{
			amount *= 3f;
		}
		bool num = amount > 0f && hasOnlyMachinegun();
		currentAmmoAmounts[(int)weaponType] += amount;
		float num2 = maxAmmoAmounts[(int)weaponType];
		if (currentAmmoAmounts[(int)weaponType] >= num2)
		{
			specialModes[(int)weaponType] = weaponType != WeaponType.SHOTGUN;
			currentAmmoAmounts[(int)weaponType] = num2;
			if (currentWeaponIndex == (int)weaponType)
			{
				switchToWeapon((int)weaponType);
			}
		}
		else if (currentAmmoAmounts[(int)weaponType] <= 0f)
		{
			currentAmmoAmounts[(int)weaponType] = 0f;
			if (specialModes[(int)weaponType])
			{
				specialModes[(int)weaponType] = false;
			}
			if (GameManager.Instance.currentCharacter == Character.WIL)
			{
				if (GameManager.Instance.vent && GameManager.Instance.currentStage <= 2)
				{
					switchToWeapon(6);
					forceWeaponToggleHide();
				}
			}
			else
			{
				switchToWeapon(0);
			}
		}
		if (num)
		{
			if (iPadWeaponSwitcher != null)
			{
				iPadWeaponSwitcher.SetActive(true);
				iPadWeaponSwitcher.GetComponent<Animation>().Play("in");
			}
			weaponToggleAnimation.gameObject.SetActive(true);
			weaponToggleAnimation.Play("in");
			StartCoroutine(weaponAnimationFix());
			switchToWeapon((int)weaponType);
		}
		updateAmmoBars();
	}

	private IEnumerator weaponAnimationFix()
	{
		yield return new WaitForSeconds(weaponToggleAnimation["in"].length);
		weaponToggleAnimation["in"].normalizedTime = 1f;
		weaponToggleAnimation.Sample();
	}

	public void updateAmmoBars()
	{
		if (!chargeBarObject.active)
		{
			updateBottomAmmo(0f);
			return;
		}
		float num = 1f;
		switch (currentWeaponIndex)
		{
		case 1:
			num = playerController.spreadshotAmmoGains[GameManager.Instance.getDifficulty()];
			break;
		case 2:
			num = playerController.bearzookaAmmoGains[GameManager.Instance.getDifficulty()];
			break;
		case 3:
			num = playerController.getSpecialAmmoGains();
			break;
		case 4:
			num = playerController.getSpecialAmmoGains();
			break;
		case 5:
			num = playerController.getSpecialAmmoGains2();
			break;
		}
		float num2 = currentAmmoAmounts[currentWeaponIndex];
		for (int i = 0; i < 3; i++)
		{
			if (num2 > num)
			{
				chargeBars[i].material.mainTextureOffset = new Vector2(0f, 0f);
			}
			else
			{
				float num3 = 0.5f - num2 / num * 0.5f;
				chargeBars[i].material.mainTextureOffset = new Vector2(num3, 0f);
				if (num2 > 0f)
				{
					updateBottomAmmo(num3);
				}
			}
			num2 -= num;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
		}
	}

	public void updateRoom(int roomNumber)
	{
		roomText.text = roomNumber.ToString();
		StartCoroutine(showRoomNumber());
	}

	private IEnumerator showRoomNumber()
	{
		roomBar.active = true;
		roomBar.GetComponent<Animation>()["in"].wrapMode = WrapMode.ClampForever;
		roomBar.GetComponent<Animation>().Play("in");
		yield return new WaitForSeconds(5f);
		roomBar.GetComponent<Animation>()["out"].wrapMode = WrapMode.Once;
		roomBar.GetComponent<Animation>().Play("out");
		yield return new WaitForSeconds(roomBar.GetComponent<Animation>()["out"].length);
		roomBar.GetComponent<Animation>()["out"].normalizedTime = 1f;
		roomBar.GetComponent<Animation>().Sample();
		roomBar.active = false;
	}

	public void updateLives()
	{
		if (playerController.numLives > 0)
		{
			livesText.text = (playerController.numLives - 1).ToString();
			lastLifeshowTime = Time.time;
			if (!livesShowing)
			{
				StartCoroutine(showLifebar());
			}
		}
	}

	public void updateLives(int numLives)
	{
		livesText.text = (numLives - 1).ToString();
		lastLifeshowTime = Time.time;
		if (!livesShowing)
		{
			StartCoroutine(showLifebar());
		}
	}

	private IEnumerator showLifebar()
	{
		livesShowing = true;
		lifeBarAnimation.gameObject.active = true;
		lifeBarAnimation.Play("in");
		while (Time.time < lastLifeshowTime + 5f)
		{
			yield return new WaitForSeconds(1f);
		}
		lifeBarAnimation.Play("out");
		yield return new WaitForSeconds(lifeBarAnimation.GetComponent<Animation>()["out"].length);
		lifeBarAnimation.GetComponent<Animation>()["out"].normalizedTime = 1f;
		lifeBarAnimation.Sample();
		lifeBarAnimation.gameObject.active = false;
		livesShowing = false;
	}

	public void updateBottomAmmo(float amount)
	{
		ammoRenderer.sharedMaterial.mainTextureOffset = new Vector2(amount, 0f);
	}

	public void displayMaxHealth()
	{
		healthRenderer.sharedMaterial.mainTextureOffset = new Vector2(0.12f, 0f);
	}

	public void OnNewHealth(float newHealth)
	{
		healthRenderer.sharedMaterial.mainTextureOffset = new Vector2(0.35f - newHealth / (float)playerController.maxHealth * 0.33f, 0f);
	}
}
