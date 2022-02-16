using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	public Animation tiles;

	public Animation backBtn;

	public Material holomat;

	private bool hasPressed;

	private bool Survival_Tut;

	private bool TA_Tut;

	public GameObject[] levelObj;

	private int index;

	private GameObject left;

	private GameObject center;

	private GameObject right;

	private int wasRight;

	public GameObject tutPrefab;

	public GameObject joulesMenu;

	private GameObject myJoulesMenu;

	public MeshRenderer musicButton;

	public MeshRenderer allSoundButton;

	public GameObject[] levelArr = new GameObject[4];

	public MainMenuTextController sign;

	public void Start()
	{
		Debug.Log("I am working!!!!!");
		GameManager.Instance.isLevel = false;
		GameManager.Instance.currentGameMode = GameMode.MENU;
		GameManager.Instance.isOver = false;
		GameManager.Instance.UpdatePowerUps();
		Time.timeScale = 1f;
		index = 3;
		wasRight = -1;
		GameManager.Instance.controls.disabled = false;
		if (SoundController.Instance.isAllOn())
		{
			allSoundButton.enabled = true;
		}
		else
		{
			allSoundButton.enabled = false;
		}
		if (SoundController.Instance.isMusicOn())
		{
			musicButton.enabled = true;
		}
		else
		{
			musicButton.enabled = false;
		}
		(Resources.Load("OliverMat") as Material).SetColor("_Color", new Color(1f, 1f, 1f, 1f));
		(Resources.Load("Environment Tile") as Material).SetColor("_Color", new Color(1f, 1f, 1f, 1f));
		setLevelLocks();
		Texture[] array = new Texture[3]
		{
			Resources.Load("Oliver") as Texture,
			Resources.Load("Oco") as Texture,
			Resources.Load("darkOco") as Texture
		};
		Material material = Resources.Load("OliverMat") as Material;
		material.SetTexture("_MainTex", array[PlayerPrefs.GetInt("skin")]);
		Material material2 = Resources.Load("Enemies") as Material;
		material2.SetColor("_Color", Color.white);
		SoundController.Instance.FadeMusic(false);
		SoundController.Instance.soundTrack();
		StartCoroutine("waitForLoad");
		Object[] array2 = Resources.LoadAll("beem", typeof(Texture));
		Material material3 = Resources.Load("mwBlast") as Material;
		switch (PlayerPrefs.GetInt("beamColor"))
		{
		case 8:
			GameManager.Instance.beemColor = Enemy.NONE;
			material3.SetTexture("_MainTex", array2[2] as Texture);
			break;
		case 0:
			GameManager.Instance.beemColor = Enemy.BLU;
			material3.SetTexture("_MainTex", array2[0] as Texture);
			break;
		case 1:
			GameManager.Instance.beemColor = Enemy.GRN;
			material3.SetTexture("_MainTex", array2[1] as Texture);
			break;
		case 2:
			GameManager.Instance.beemColor = Enemy.ORG;
			material3.SetTexture("_MainTex", array2[3] as Texture);
			break;
		case 3:
			GameManager.Instance.beemColor = Enemy.PIN;
			material3.SetTexture("_MainTex", array2[4] as Texture);
			break;
		case 4:
			GameManager.Instance.beemColor = Enemy.RED;
			material3.SetTexture("_MainTex", array2[5] as Texture);
			break;
		case 5:
			GameManager.Instance.beemColor = Enemy.YEL;
			material3.SetTexture("_MainTex", array2[6] as Texture);
			break;
		case 6:
			GameManager.Instance.beemColor = Enemy.PUR;
			break;
		case 7:
			GameManager.Instance.beemColor = Enemy.WHI;
			break;
		default:
			GameManager.Instance.beemColor = Enemy.NONE;
			material3.SetTexture("_MainTex", array2[2] as Texture);
			break;
		}
		array2 = null;
		material3 = null;
		array = null;
		material = null;
		material2 = null;
		Resources.UnloadUnusedAssets();
	}

	public IEnumerator waitForLoad()
	{
		if (!GameManager.Instance.onStartup)
		{
			GameManager.Instance.onStartup = true;
			yield return new WaitForSeconds(0.12f);
			SoundController.Instance.playClip(Resources.Load("Abbi Sounds/abbi_welcome") as AudioClip);
		}
	}

	public void ButtonPress(string name)
	{
		if (GameManager.Instance.currentGameMode == GameMode.BACKPACK)
		{
			return;
		}
		switch (name)
		{
		case "Play":
			base.GetComponent<Animation>()["playHit"].speed = 1f;
			base.GetComponent<Animation>().Play("playHit");
			base.GetComponent<Animation>().Play("playHit");
			backBtn.Play("enter");
			GameManager.Instance.tronActive = true;
			GameManager.Instance.playerController.life = 3f;
			GameManager.Instance.isOver = false;
			GameManager.Instance.canHasWil = true;
			GameManager.Instance.canHasNuggs = true;
			GameManager.Instance.isWilOut = false;
			SoundController.Instance.playClip(Resources.Load("target_select_from_main") as AudioClip);
			StartCoroutine(GameManager.Instance.animC.scoot(-2.29f));
			StartCoroutine("holofade");
			StartCoroutine("introLevels");
			break;
		case "Classic":
			PlayerPrefs.SetString("UnityMessage", "60sec");
			GameManager.Instance.currentGameMode = GameMode.TIME;
			GameManager.Instance.playerController.life = 3f;
			GameManager.Instance.arcadeTime = 60f;
			GameManager.Instance.isOver = false;
			base.GetComponent<Animation>().Play("classicHit");
			GameManager.Instance.hudController.resetScore();
			GameManager.Instance.cashRetryData();
			StartCoroutine(GameManager.Instance.animC.scoot(-3.47f));
			StartCoroutine("exitLevels");
			StartCoroutine(startLevel("classicHit"));
			SoundController.Instance.playClip(Resources.Load("Abbi Sounds/abbi_60sec") as AudioClip);
			break;
		case "TA_Tutorial":
			PlayerPrefs.SetString("UnityMessage", "60sec");
			TA_Tut = true;
			GameManager.Instance.currentGameMode = GameMode.TIME;
			GameManager.Instance.playerController.life = 3f;
			GameManager.Instance.arcadeTime = 60f;
			GameManager.Instance.isOver = false;
			base.GetComponent<Animation>().Play("classicHit");
			GameManager.Instance.hudController.resetScore();
			GameManager.Instance.cashRetryData();
			SoundController.Instance.playClip(Resources.Load("target_select_from_main") as AudioClip);
			StartCoroutine(GameManager.Instance.animC.scoot(-3.47f));
			StartCoroutine("exitLevels");
			StartCoroutine(startLevel("classicHit"));
			break;
		case "Arcade":
			PlayerPrefs.SetString("UnityMessage", "survival");
			GameManager.Instance.currentGameMode = GameMode.SURVIVAL;
			base.GetComponent<Animation>().Play("arcadeHit");
			GameManager.Instance.playerController.life = 3f;
			GameManager.Instance.isOver = false;
			GameManager.Instance.hudController.resetScore();
			GameManager.Instance.cashRetryData();
			SoundController.Instance.playClip(Resources.Load("target_select_from_main") as AudioClip);
			StartCoroutine(GameManager.Instance.animC.scoot(-3.47f));
			StartCoroutine("exitLevels");
			StartCoroutine(startLevel("arcadeHit"));
			SoundController.Instance.playClip(Resources.Load("Abbi Sounds/abbi_survival") as AudioClip);
			break;
		case "Survival_Tutorial":
			PlayerPrefs.SetString("UnityMessage", "survival");
			Survival_Tut = true;
			GameManager.Instance.currentGameMode = GameMode.SURVIVAL;
			base.GetComponent<Animation>().Play("arcadeHit");
			GameManager.Instance.playerController.life = 3f;
			GameManager.Instance.isOver = false;
			GameManager.Instance.hudController.resetScore();
			GameManager.Instance.cashRetryData();
			SoundController.Instance.playClip(Resources.Load("target_select_from_main") as AudioClip);
			StartCoroutine(GameManager.Instance.animC.scoot(-3.47f));
			StartCoroutine("exitLevels");
			StartCoroutine(startLevel("arcadeHit"));
			break;
		case "Backpack":
			hasPressed = true;
			base.GetComponent<Animation>().Play("backpackHit");
			SoundController.Instance.playClip(Resources.Load("target_select_from_main") as AudioClip);
			StartCoroutine("backpack");
			SoundController.Instance.playClip(Resources.Load("Abbi Sounds/abbi_backpack") as AudioClip);
			break;
		case "Back":
			if (!hasPressed)
			{
				hasPressed = true;
				backBtn.Play("press");
				StartCoroutine("exitLevels");
				StopCoroutine("backPress");
				StartCoroutine("backPress", "press");
				StartCoroutine(GameManager.Instance.animC.scoot(-3.47f));
				base.GetComponent<Animation>()["playHit"].speed = -1f;
				base.GetComponent<Animation>()["playHit"].time = base.GetComponent<Animation>().GetClip("playHit").length;
				base.GetComponent<Animation>().Play("playHit");
				StartCoroutine("tempOff", "playHit");
			}
			break;
		case "Music":
			if (!hasPressed)
			{
				hasPressed = true;
				GameManager.Instance.animC.music();
				base.GetComponent<Animation>().Play("musicPress");
				SoundController.Instance.muteMusic();
				StopCoroutine("tempOff");
				StartCoroutine("tempOff", "musicPress");
				StartCoroutine("disableButton", "musicPress");
			}
			break;
		case "Mute":
			if (!hasPressed)
			{
				hasPressed = true;
				GameManager.Instance.animC.mute();
				base.GetComponent<Animation>().Play("mutePress");
				SoundController.Instance.muteAll();
				StopCoroutine("tempOff");
				StartCoroutine("tempOff", "mutePress");
				StartCoroutine("disableButton", "mutePress");
			}
			break;
		case "News":
			if (!hasPressed)
			{
				SoundController.Instance.playClip(Resources.Load("microwave_button1") as AudioClip);
				hasPressed = true;
				GameManager.Instance.animC.news();
				base.GetComponent<Animation>().Play("newsPress");
				StopCoroutine("tempOff");
				StartCoroutine("tempOff", "newsPress");
				StartCoroutine("LoadWebPage", "newsPress");
			}
			break;
		case "SkyVuStore":
			if (!hasPressed)
			{
				hasPressed = true;
				SoundController.Instance.playClip(Resources.Load("microwave_button2") as AudioClip);
				if (myJoulesMenu == null)
				{
					myJoulesMenu = Object.Instantiate(joulesMenu) as GameObject;
				}
			}
			hasPressed = false;
			break;
		case "Menu":
			if (!hasPressed)
			{
				hasPressed = true;
				GameManager.Instance.playerController.life = 3f;
				GameManager.Instance.arcadeTime = 60f;
				GameManager.Instance.isOver = false;
				StartCoroutine(boxes());
			}
			break;
		case "Achievement":
			GameManager.Instance.OFOpenDashboard();
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				if (!hasPressed)
				{
					hasPressed = true;
					StartCoroutine("tempOff", "storePress");
				}
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				GameManager.Instance.OFOpenAchievementList();
			}
			break;
		case "objBar":
			GameManager.Instance.OFOpenDashboard();
			break;
		case "Leaderboard":
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				if (!hasPressed)
				{
					hasPressed = true;
					StartCoroutine("tempOff", "storePress");
				}
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				TapJoyManager.Instance.ShowOffers();
			}
			break;
		case "MainMenuBanner":
			GameManager.Instance.hudController.mainMenuObjective();
			GameManager.Instance.controls.disabled = true;
			break;
		case "Cliff":
			SoundController.Instance.playClip(Resources.Load("microwave_button2") as AudioClip);
			if (!PlayerPrefs.GetString("unlockedItems").Contains("level3"))
			{
				Alerts alerts3 = base.gameObject.AddComponent<Alerts>();
				if (PlayerPrefs.GetInt("joules") < 1000)
				{
					alerts3.hasEnough = false;
				}
				else
				{
					alerts3.hasEnough = true;
				}
				alerts3.buyString = "level3";
				alerts3.caller = this;
				alerts3.callbackMessage = "setLevelLocks";
			}
			else
			{
				SoundController.Instance.playClip(Resources.Load("microwave_button2") as AudioClip);
				GameManager.Instance.currentLevel = Level.CLIFF;
				StopCoroutine("LevelSwitch");
				StartCoroutine("LevelSwitch", 0);
			}
			break;
		case "Jungle":
			SoundController.Instance.playClip(Resources.Load("microwave_button2") as AudioClip);
			StopCoroutine("LevelSwitch");
			StartCoroutine("LevelSwitch", 1);
			GameManager.Instance.currentLevel = Level.JUNGLE;
			break;
		case "Sewer":
			if (!PlayerPrefs.GetString("unlockedItems").Contains("level2"))
			{
				Alerts alerts2 = base.gameObject.AddComponent<Alerts>();
				if (PlayerPrefs.GetInt("joules") < 1000)
				{
					alerts2.hasEnough = false;
				}
				else
				{
					alerts2.hasEnough = true;
				}
				alerts2.buyString = "level2";
				alerts2.caller = this;
				alerts2.callbackMessage = "setLevelLocks";
			}
			else
			{
				SoundController.Instance.playClip(Resources.Load("microwave_button2") as AudioClip);
				StopCoroutine("LevelSwitch");
				StartCoroutine("LevelSwitch", 2);
				GameManager.Instance.currentLevel = Level.SEWER;
			}
			break;
		case "Pirate":
			if (!PlayerPrefs.GetString("unlockedItems").Contains("level4"))
			{
				Alerts alerts = base.gameObject.AddComponent<Alerts>();
				if (PlayerPrefs.GetInt("joules") < 1000)
				{
					alerts.hasEnough = false;
				}
				else
				{
					alerts.hasEnough = true;
				}
				alerts.buyString = "level4";
				alerts.caller = this;
				alerts.callbackMessage = "setLevelLocks";
			}
			else
			{
				SoundController.Instance.playClip(Resources.Load("microwave_button2") as AudioClip);
				StopCoroutine("LevelSwitch");
				StartCoroutine("LevelSwitch", 3);
				GameManager.Instance.currentLevel = Level.PIRATE;
			}
			break;
		case "addJoules":
			TapJoyManager.Instance.ShowOffers();
			break;
		case "OpenFeint":
			GameManager.Instance.OFOpenDashboard();
			break;
		}
	}

	public IEnumerator LevelSwitch(int index)
	{
		bool isFinished = true;
		for (int i = 0; i < levelArr.Length; i++)
		{
			if (index == i)
			{
				if (levelArr[i].transform.localScale.x < 0.99f || levelArr[i].transform.localScale.y < 0.99f || levelArr[i].transform.localScale.z < 0.99f)
				{
					isFinished = false;
					levelArr[i].transform.localScale = new Vector3(Mathf.Lerp(levelArr[i].transform.localScale.x, 1f, 0.3f), Mathf.Lerp(levelArr[i].transform.localScale.y, 1f, 0.3f), Mathf.Lerp(levelArr[i].transform.localScale.z, 1f, 0.3f));
				}
			}
			else if (levelArr[i].transform.localScale.x > 0.75f || levelArr[i].transform.localScale.y > 0.75f || levelArr[i].transform.localScale.z > 0.75f)
			{
				isFinished = false;
				levelArr[i].transform.localScale = new Vector3(Mathf.Lerp(levelArr[i].transform.localScale.x, 0.5f, 0.3f), Mathf.Lerp(levelArr[i].transform.localScale.y, 0.5f, 0.3f), Mathf.Lerp(levelArr[i].transform.localScale.z, 0.5f, 0.3f));
			}
		}
		if (!isFinished)
		{
			yield return new WaitForSeconds(0.05f);
			StartCoroutine("LevelSwitch", index);
		}
	}

	public IEnumerator startLevel(string aniName)
	{
		backBtn.Play("exit");
		yield return new WaitForSeconds(base.GetComponent<Animation>().GetClip(aniName).length);
		base.GetComponent<Animation>().Play("roomRetreat");
		yield return new WaitForSeconds(base.GetComponent<Animation>().GetClip("roomRetreat").length);
		if (!GameManager.Instance.tutorialComplete || !GameManager.Instance.tutorialScore || !GameManager.Instance.tutorialTime)
		{
			PlayerPrefs.SetString("UnityMessage", "tutorial");
			GameObject tut = Object.Instantiate(tutPrefab) as GameObject;
			tut.name = "Tutorial";
		}
		else if (Survival_Tut)
		{
			PlayerPrefs.SetString("UnityMessage", "tutorial");
			Survival_Tut = false;
			GameObject tut3 = Object.Instantiate(tutPrefab) as GameObject;
			tut3.name = "Tutorial";
		}
		else if (TA_Tut)
		{
			PlayerPrefs.SetString("UnityMessage", "tutorial");
			TA_Tut = false;
			GameObject tut2 = Object.Instantiate(tutPrefab) as GameObject;
			tut2.name = "Tutorial";
		}
		else
		{
			SoundController.Instance.FadeMusic(true);
			GameManager.Instance.isTutorial = false;
			StartCoroutine("tronRoom");
			yield return StartCoroutine(GameManager.Instance.hudController.scaleCrosshairs());
			GameManager.Instance.hudController.tacomOnline();
		}
	}

	public IEnumerator tronRoom()
	{
		//Application.LoadLevelAdditiveAsync("BlackRoom");
		SceneManager.LoadSceneAsync("BlackRoom");
		tiles["flip"].speed = 0.75f;
		tiles.Play("flip");
		yield return new WaitForSeconds(tiles.GetClip("flip").length * 2.1f);
		Object.Destroy(base.gameObject);
	}

	public IEnumerator backpack()
	{
		GameManager.Instance.currentGameMode = GameMode.BACKPACK;
		GameManager.Instance.animC.backpack();
		yield return new WaitForSeconds(2.136f);
		//Application.LoadLevel("Backpack");
		SceneManager.LoadSceneAsync("Backpack");
		GameManager.Instance.currentGameMode = GameMode.BACKPACK;
	}

	public IEnumerator tempOff(string anim)
	{
		yield return new WaitForSeconds(base.GetComponent<Animation>()[anim].length * 2f);
		hasPressed = false;
	}

	public IEnumerator backPress(string anim)
	{
		yield return new WaitForSeconds(0.05f);
		backBtn.Play("exit");
	}

	public void rezIn()
	{
		tiles.GetComponent<Animation>()["flip"].speed = -0.5f;
		tiles["flip"].time = tiles["flip"].length;
		tiles.Play("flip");
		base.GetComponent<Animation>()["inverse"].speed = 1f;
		base.GetComponent<Animation>().Play("inverse");
	}

	private IEnumerator boxes()
	{
		yield return new WaitForSeconds(3f);
		base.GetComponent<Animation>().Play("boxes");
		hasPressed = false;
	}

	private IEnumerator LoadWebPage(string anim)
	{
		if (anim == "storePress")
		{
			yield return new WaitForSeconds(base.GetComponent<Animation>()[anim].length * 2f);
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Application.OpenURL("http://www.battlebears.com/storage/blast_ios_web/index.html");
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				Application.OpenURL("http://www.battlebears.com/storage/blast_android_web/index.html");
			}
		}
		else if (anim == "newsPress")
		{
			PlayerPrefs.SetString("UnityMessage", "movie");
			yield return new WaitForSeconds(base.GetComponent<Animation>()[anim].length * 2f);
			if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform == RuntimePlatform.Android)
			{
				Application.OpenURL("http://www.youtube.com/watch?v=DLJCYp2OAx8");
			}
		}
	}

	public IEnumerator holofade()
	{
		holomat.SetColor("_Color", new Color(1f, 1f, 1f, 0f));
		yield return new WaitForSeconds(1.95f);
		holomat.SetColor("_Color", new Color(1f, 1f, 1f, 0.25f));
		yield return new WaitForSeconds(0.5f);
		holomat.SetColor("_Color", new Color(1f, 1f, 1f, 0f));
		yield return new WaitForSeconds(0.25f);
		while ((double)holomat.color.a < 0.98)
		{
			float alpha = Mathf.Lerp(holomat.color.a, 1f, 0.1f);
			holomat.SetColor("_Color", new Color(1f, 1f, 1f, alpha));
		}
	}

	public IEnumerator introLevels()
	{
		levelArr[0] = levelObj[1];
		levelArr[1] = levelObj[0];
		levelArr[2] = levelObj[2];
		levelArr[3] = levelObj[3];
		yield return new WaitForSeconds(1.6f);
		SoundController.Instance.playClip(Resources.Load("LRB_menu") as AudioClip);
		for (int i = 0; i < levelObj.Length; i++)
		{
			levelObj[i].transform.localScale = Vector3.zero;
		}
		yield return new WaitForSeconds(0.31f);
		byte dex = 64;
		left = levelObj[0];
		center = levelObj[2];
		right = levelObj[1];
		GameManager.Instance.generateLevel();
		GameObject[] array = levelArr;
		foreach (GameObject obj in array)
		{
			obj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
		}
		if (GameManager.Instance.currentLevel == Level.JUNGLE)
		{
			levelObj[0].transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (GameManager.Instance.currentLevel == Level.SEWER)
		{
			levelObj[2].transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (GameManager.Instance.currentLevel == Level.CLIFF)
		{
			levelObj[1].transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (GameManager.Instance.currentLevel == Level.PIRATE)
		{
			levelObj[3].transform.localScale = new Vector3(1f, 1f, 1f);
		}
		while (dex > 0)
		{
			float jungleX = Mathf.Lerp(levelObj[0].transform.localPosition.x, 0.44f, 0.2f);
			levelObj[0].transform.localPosition = new Vector3(jungleX, 0f, 0f);
			float sewerX = Mathf.Lerp(levelObj[2].transform.localPosition.x, 0.145f, 0.2f);
			levelObj[2].transform.localPosition = new Vector3(sewerX, 0f, 0f);
			float cliffX = Mathf.Lerp(levelObj[1].transform.localPosition.x, -0.44f, 0.2f);
			levelObj[1].transform.localPosition = new Vector3(cliffX, 0f, 0f);
			float pirateX = Mathf.Lerp(levelObj[3].transform.localPosition.x, -0.145f, 0.2f);
			levelObj[3].transform.localPosition = new Vector3(pirateX, 0f, 0f);
			yield return new WaitForSeconds(0.005f);
			dex = (byte)(dex - 1);
		}
	}

	public IEnumerator exitLevels()
	{
		yield return new WaitForSeconds(0.15f);
		SoundController.Instance.playClip(Resources.Load("LRB_menu_close") as AudioClip);
		for (byte dex = 64; dex > 0; dex = (byte)(dex - 1))
		{
			float jungleX = Mathf.Lerp(levelObj[0].transform.localPosition.x, 0f, 0.2f);
			levelObj[0].transform.localPosition = new Vector3(jungleX, 0f, 0f);
			float jungleScale = Mathf.Lerp(levelObj[0].transform.localScale.x, 0f, 0.2f);
			levelObj[0].transform.localScale = Vector3.one * jungleScale;
			float sewerX = Mathf.Lerp(levelObj[2].transform.localPosition.x, 0f, 0.2f);
			levelObj[2].transform.localPosition = new Vector3(sewerX, 0f, 0f);
			float sewerScale = Mathf.Lerp(levelObj[2].transform.localScale.x, 0f, 0.2f);
			levelObj[2].transform.localScale = Vector3.one * sewerScale;
			float cliffX = Mathf.Lerp(levelObj[1].transform.localPosition.x, 0f, 0.2f);
			levelObj[1].transform.localPosition = new Vector3(cliffX, 0f, 0f);
			float cliffScale = Mathf.Lerp(levelObj[1].transform.localScale.x, 0f, 0.2f);
			levelObj[1].transform.localScale = Vector3.one * cliffScale;
			float pirateX = Mathf.Lerp(levelObj[3].transform.localPosition.x, 0f, 0.2f);
			levelObj[3].transform.localPosition = new Vector3(pirateX, 0f, 0f);
			float pirateScale = Mathf.Lerp(levelObj[3].transform.localScale.x, 0f, 0.2f);
			levelObj[3].transform.localScale = Vector3.one * pirateScale;
			yield return new WaitForSeconds(0.005f);
		}
	}

	public IEnumerator disableButton(string anim)
	{
		yield return new WaitForSeconds(base.GetComponent<Animation>()[anim].length / 2f);
		if (anim == "mutePress")
		{
			if (SoundController.Instance.isAllOn())
			{
				allSoundButton.enabled = true;
				SoundController.Instance.playClip(Resources.Load("microwave_door_open") as AudioClip);
			}
			else
			{
				allSoundButton.enabled = false;
				SoundController.Instance.playClip(Resources.Load("microwave_door_close") as AudioClip);
			}
		}
		else if (anim == "musicPress")
		{
			if (SoundController.Instance.isMusicOn())
			{
				musicButton.enabled = true;
				SoundController.Instance.playClip(Resources.Load("microwave_door_open") as AudioClip);
			}
			else
			{
				musicButton.enabled = false;
				SoundController.Instance.playClip(Resources.Load("microwave_door_close") as AudioClip);
			}
		}
	}

	public void setLevelLocks()
	{
		if (!PlayerPrefs.GetString("unlockedItems").Contains("level3"))
		{
			//levelObj[1].SetActiveRecursively(true);
			levelObj[1].SetActive(true);
		}
		else
		{
			//levelObj[1].SetActiveRecursively(false);
			levelObj[1].SetActive(false);
			//levelObj[1].active = true;
			levelObj[1].SetActive(true);
		}
		if (!PlayerPrefs.GetString("unlockedItems").Contains("level2"))
		{
			//levelObj[2].SetActiveRecursively(true);
			levelObj[2].SetActive(true);
		}
		else
		{
			//levelObj[2].SetActiveRecursively(false);
			levelObj[2].SetActive(false);
			//levelObj[2].active = true;
			levelObj[2].SetActive(true);
		}
		if (!PlayerPrefs.GetString("unlockedItems").Contains("level4"))
		{
			//levelObj[3].SetActiveRecursively(true);
			levelObj[3].SetActive(true);
			return;
		}
		//levelObj[3].SetActiveRecursively(false);
		levelObj[3].SetActive(false);
		//levelObj[3].active = true;
		levelObj[3].SetActive(true);
	}
}
