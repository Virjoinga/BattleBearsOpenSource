using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public GameMode currentGameMode = GameMode.MENU;

	public GameMode lastGameMode;

	public InvertOption currentInvertOption = InvertOption.OFF;

	public Level currentLevel;

	public Level lastLevel;

	public Style controlStyle;

	public Controls controls;

	public AnimationController animC;

	public PlayerController playerController;

	public PowerupManager powerupManager;

	public HUDController hudController;

	public bool canHasWil;

	public bool isWilOut;

	public bool canHasNuggs;

	public bool isOver;

	public bool whiteOut;

	public float arcadeTime;

	public int maxEnemies;

	public GameObject menu;

	public bool allMute;

	public Enemy prev;

	public int colorComboAmt;

	public bool tronActive;

	public Enemy beemColor;

	public bool completedTutorial;

	public bool isLevel;

	public bool hasSkinnedUVs;

	public float enemySpeed = 1f;

	public float multiplier;

	public bool isHighEnd;

	public float statSets;

	public float statPowerups;

	public float statRatio;

	public float statBombDetonations;

	public float statBombKills;

	public float statMultiplier;

	public float statPinkCombo;

	public float statPinkKills;

	public float statBlueCombo;

	public float statBlueKills;

	public float statGreenCombo;

	public float statGreenKills;

	public float statOrangeCombo;

	public float statOrangeKills;

	public float statRedCombo;

	public float statRedKills;

	public float statYellowCombo;

	public float statYellowKills;

	public float statPurpleCombo;

	public float statPurpleKills;

	[HideInInspector]
	public Vector3[] vertices;

	[HideInInspector]
	public Vector3[] normals;

	[HideInInspector]
	public Vector3[] nuggsVert;

	[HideInInspector]
	public Vector3[] nuggsNorm;

	[HideInInspector]
	public Vector3[] bombVert;

	[HideInInspector]
	public Vector3[] bombNorm;

	[HideInInspector]
	public Vector3[] wilVert;

	[HideInInspector]
	public Vector3[] wilNorm;

	[HideInInspector]
	public Vector3[] pointNuggsVert;

	[HideInInspector]
	public Vector3[] pointNuggsNorm;

	public int firingSpeed = 1;

	private int levelIndex;

	private static GameManager instance;

	public bool hasGameCenter;

	public int gcState;

	public Vector2 crosshairPosition;

	public bool canVibrate;

	public bool isPaused;

	public bool clearedEntranceExam;

	public bool isTutorial = true;

	public bool inTutorial;

	public bool tutorialComplete;

	public bool tutorialScore;

	public bool tutorialTime;

	[HideInInspector]
	public Touch myTouch;

	public AsyncOperation ao;

	[HideInInspector]
	public Vector3 hitPos;

	public bool onStartup;

	public int[] objectiveItemUnlocks;

	public Helm helmLoadOut = Helm.NONE;

	public Chest chestLoadOut = Chest.NONE;

	public Skin skinLoadOut;

	private byte frame = 240;

	[HideInInspector]
	public int currencyMultiplier = 1;

	public bool isOnObjective;

	public int joulesToNext = 500;

	public WWW itemServer;

	private OpenFeintFacade openFeintPlugin;

	private bool leaderboardsOpen;

	private bool achievementsOpen;

	public static GameManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		if (!PlayerPrefs.HasKey("isFirstTime"))
		{
			PlayerPrefs.SetInt("isFirstTime", 1);
		}
		base.useGUILayout = false;
		Application.targetFrameRate = 120;
		//iPhoneSettings.screenCanDarken = false;
		if (PlayerPrefs.HasKey("clearedEntry"))
		{
			clearedEntranceExam = true;
		}
		else
		{
			clearedEntranceExam = false;
		}
		if (Application.isEditor)
		{
			canVibrate = true;
		}
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		openFeintPlugin = new OpenFeintFacade();
		leaderboardsOpen = false;
		achievementsOpen = false;
		string text = "BLAST";
		string key = "cH69JJQDmWG2CIhtWSXQw";
		string secret = "qgOuu3n6V4HorfqLW32UWO2halQv7j52m1op2pNR5o";
		string id = "277902";
		string text2 = "717937";
		int num = 42;
		int num2 = 1337;
		openFeintPlugin.Init(text, key, secret, id);
		if (SystemInfo.processorType == "ARMv6")
		{
			isHighEnd = false;
			maxEnemies = 8;
		}
		else
		{
			isHighEnd = true;
			maxEnemies = 12;
		}
		canHasNuggs = true;
		arcadeTime = 60f;
		if (!PlayerPrefs.HasKey("tutorial"))
		{
			PlayerPrefs.SetInt("tutorial", 0);
		}
		if (!PlayerPrefs.HasKey("best"))
		{
			PlayerPrefs.SetString("best", "0000000");
		}
		if (!PlayerPrefs.HasKey("bestTime"))
		{
			PlayerPrefs.SetString("bestTime", "0000000");
		}
		if (!PlayerPrefs.HasKey("Survival_BestTime"))
		{
			PlayerPrefs.SetString("Survival_BestTime", "0000000");
		}
		if (!PlayerPrefs.HasKey("helm") || !PlayerPrefs.HasKey("armor"))
		{
			PlayerPrefs.SetInt("helm", 2);
			PlayerPrefs.SetInt("armor", 2);
		}
		if (!PlayerPrefs.HasKey("music"))
		{
			PlayerPrefs.SetInt("music", 0);
		}
		if (!PlayerPrefs.HasKey("sound"))
		{
			PlayerPrefs.SetInt("sound", 0);
		}
		if (!PlayerPrefs.HasKey("joules"))
		{
			PlayerPrefs.SetInt("joules", 0);
		}
		isLevel = false;
		hasSkinnedUVs = false;
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
		}
		generateLevel();
		//Application.LoadLevel("MainMenu");
		SceneManager.LoadScene("MainMenu");
		if (Application.platform == RuntimePlatform.Android)
		{
			if (PlayerPrefs.GetInt("needToSend") == 0)
			{
				StartCoroutine("getpurchaseInfo");
			}
			else
			{
				StartCoroutine("sendPurchaseInfo");
			}
		}
	}

	public IEnumerator getpurchaseInfo()
	{
		yield return new WaitForSeconds(2f);
		yield return new WaitForEndOfFrame();
		WWWForm myForm = new WWWForm();
		myForm.AddField("gameID", "blast");
		myForm.AddField("method", "getItems");
		try
		{
			myForm.AddField("ofID", OFGetOFID());
		}
		catch
		{
			PlayerPrefs.SetInt("needToSend", 1);
			yield break;
		}
		itemServer = new WWW("http://www.baoloc9.com/android_blast/zzBlastItems/index.php", myForm);
		yield return itemServer;
		if (itemServer.error != null)
		{
			PlayerPrefs.SetInt("needToSend", 1);
			yield break;
		}
		PlayerPrefs.SetString("unlockedItems", itemServer.text);
		PlayerPrefs.SetInt("needToSend", 0);
	}

	public IEnumerator sendPurchaseInfo()
	{
		yield return new WaitForSeconds(2f);
		yield return new WaitForEndOfFrame();
		try
		{
			string test = Instance.OFGetOFID();
		}
		catch
		{
			PlayerPrefs.SetInt("needToSend", 1);
			yield break;
		}
		string[] myItems = PlayerPrefs.GetString("needSendingItems").Split(' ');
		for (int i = 0; i < myItems.Length; i++)
		{
			if (myItems[i] != string.Empty || myItems[i] != null)
			{
				WWWForm spend = new WWWForm();
				spend.AddField("gameID", "blast");
				spend.AddField("method", "sendItem");
				spend.AddField("ofID", Instance.OFGetOFID());
				spend.AddField("item", myItems[i]);
				itemServer = new WWW("http://www.baoloc9.com/android_blast/zzBlastItems/index.php", spend);
				yield return itemServer;
			}
		}
		StartCoroutine("getpurchaseInfo");
		PlayerPrefs.SetInt("needToSend", 0);
	}

	public void generateLevel()
	{
		float value = Random.value;
		if (value < 0.25f)
		{
			if (!PlayerPrefs.GetString("unlockedItems").Contains("level2"))
			{
				generateLevel();
			}
			else
			{
				currentLevel = Level.SEWER;
			}
		}
		else if (value < 0.5f)
		{
			if (!PlayerPrefs.GetString("unlockedItems").Contains("level3"))
			{
				generateLevel();
			}
			else
			{
				currentLevel = Level.CLIFF;
			}
		}
		else if (value < 0.75f)
		{
			if (!PlayerPrefs.GetString("unlockedItems").Contains("level4"))
			{
				generateLevel();
			}
			else
			{
				currentLevel = Level.PIRATE;
			}
		}
		else
		{
			currentLevel = Level.JUNGLE;
		}
	}

	public void storeSurvivalHighScore(int score)
	{
		int num = score;
		string empty = string.Empty;
		empty = ((num < 10) ? ("000000" + num) : ((num < 100) ? ("00000" + num) : ((num < 1000) ? ("0000" + num) : ((num < 10000) ? ("000" + num) : ((num < 100000) ? ("00" + num) : ((num >= 1000000) ? num.ToString() : ("0" + num)))))));
		PlayerPrefs.SetString("best", empty);
		PlayerPrefs.SetString("Survival_BestScore", empty);
	}

	public void storeTimeHighScore(int score)
	{
		int num = score;
		string empty = string.Empty;
		empty = ((num < 10) ? ("000000" + num) : ((num < 100) ? ("00000" + num) : ((num < 1000) ? ("0000" + num) : ((num < 10000) ? ("000" + num) : ((num < 100000) ? ("00" + num) : ((num >= 1000000) ? num.ToString() : ("0" + num)))))));
		PlayerPrefs.SetString("bestTime", empty);
	}

	private void OnLevelWasLoaded()
	{
		if (Application.loadedLevelName == "MainMenu")
		{
			controls = GameObject.Find("Oliver").GetComponent<Controls>();
			animC = GameObject.Find("Oliver/oliverAnim").GetComponent<AnimationController>();
			playerController = GameObject.Find("Oliver").GetComponent<PlayerController>();
			hudController = GameObject.Find("HUD").GetComponent<HUDController>();
		}
	}

	public IEnumerator startTime()
	{
		while (arcadeTime > 0f)
		{
			yield return new WaitForSeconds(1f / enemySpeed);
			arcadeTime -= 1f;
		}
	}

	public void cleanNSpawn(bool spawn)
	{
		if (spawn)
		{
			GameObject gameObject = Object.Instantiate(menu, Vector3.zero, base.transform.rotation) as GameObject;
			gameObject.name = menu.name;
			gameObject.SendMessage("rezIn");
		}
		GameObject[] array = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject gameObject2 in array)
		{
			if (!(gameObject2.transform.root.name == "_TheBoss") && !(gameObject2.transform.root.name == "MainMenu") && !(gameObject2.transform.root.name == "Oliver") && !(gameObject2.transform.root.name == "HUD") && !(gameObject2.transform.root.name == "tronRoom"))
			{
				Object.Destroy(gameObject2);
			}
		}
		Resources.UnloadUnusedAssets();
	}

	public void setLevel(Level level)
	{
		currentLevel = level;
		switch (level)
		{
		case Level.RANDOM:
			levelIndex = 0;
			break;
		case Level.JUNGLE:
			levelIndex = 1;
			break;
		case Level.CLIFF:
			levelIndex = 2;
			break;
		case Level.SEWER:
			levelIndex = 3;
			break;
		case Level.PIRATE:
			levelIndex = 4;
			break;
		}
	}

	public void nextLevel(bool right)
	{
		if (right)
		{
			levelIndex++;
		}
		else
		{
			levelIndex--;
		}
		if (levelIndex > 3)
		{
			levelIndex = 0;
		}
		if (levelIndex < 0)
		{
			levelIndex = 3;
		}
		switch (levelIndex)
		{
		case 0:
			currentLevel = Level.RANDOM;
			break;
		case 1:
			currentLevel = Level.JUNGLE;
			break;
		case 2:
			currentLevel = Level.CLIFF;
			break;
		case 3:
			currentLevel = Level.SEWER;
			break;
		case 4:
			currentLevel = Level.PIRATE;
			break;
		}
	}

	public void cashRetryData()
	{
		lastLevel = currentLevel;
		lastGameMode = currentGameMode;
	}

	public void loadRetryData()
	{
		currentLevel = lastLevel;
		currentGameMode = lastGameMode;
	}

	public void PowerUp(Nuggs power)
	{
		switch (power)
		{
		case Nuggs.FREEZE:
			enemySpeed = 0.25f;
			StartCoroutine("enemyColor", true);
			break;
		case Nuggs.WHITE:
		{
			whiteOut = true;
			GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
			GameObject[] array2 = array;
			foreach (GameObject gameObject in array2)
			{
				WhiteOut componentInChildren = gameObject.GetComponentInChildren<WhiteOut>();
				if ((bool)componentInChildren)
				{
					componentInChildren.TurnWhite();
				}
			}
			break;
		}
		}
		hudController.powerCounter(power);
	}

	public void NuggsDeath(Nuggs type)
	{
		PowerupManager.Instance.create(type);
	}

	public void PowerDown(Nuggs power)
	{
		switch (power)
		{
		case Nuggs.FREEZE:
			enemySpeed = 1f;
			StartCoroutine("enemyColor", false);
			break;
		case Nuggs.MULTIPLIER:
			break;
		case Nuggs.WHITE:
			whiteOut = false;
			break;
		case Nuggs.POINTS:
		case Nuggs.SCREEN:
			break;
		}
	}

	public IEnumerator enemyColor(bool toBlue)
	{
		Material enemy = Resources.Load("Enemies") as Material;
		for (int loops = 32; loops > 0; loops--)
		{
			float newRed;
			float newGreen;
			if (toBlue)
			{
				newRed = Mathf.Lerp(enemy.color.r, 0.65f, 0.1f);
				newGreen = Mathf.Lerp(enemy.color.g, 0.8f, 0.1f);
			}
			else
			{
				newRed = Mathf.Lerp(enemy.color.r, 1f, 0.1f);
				newGreen = Mathf.Lerp(enemy.color.g, 1f, 0.1f);
			}
			enemy.SetColor("_Color", new Color(newRed, newGreen, 1f, enemy.color.a));
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void resetPowerUps()
	{
		enemySpeed = 1f;
		multiplier = 0f;
	}

	public void saveStats()
	{
		PlayerPrefs.SetFloat("statBombDetonations", statBombDetonations);
		PlayerPrefs.SetFloat("statBombKills", statBombKills);
		PlayerPrefs.SetFloat("statMultiplier", statMultiplier);
		PlayerPrefs.SetFloat("statRatio", statRatio);
		PlayerPrefs.SetFloat("statPowerups", statPowerups);
		if (currentGameMode == GameMode.SCORE)
		{
			PlayerPrefs.SetFloat("statScore", hudController.scoreVal);
		}
		else if (currentGameMode == GameMode.TIME)
		{
			PlayerPrefs.SetFloat("statTimeScore", hudController.scoreVal);
			PlayerPrefs.SetFloat("statSets", hudController.currentSet);
		}
		else if (currentGameMode == GameMode.SURVIVAL)
		{
			PlayerPrefs.SetFloat("statSurvivalScore", hudController.scoreVal);
		}
		StartCoroutine("resetStats");
	}

	public IEnumerator resetStats()
	{
		yield return new WaitForSeconds(1f);
		statBlueCombo = 0f;
		statBlueKills = 0f;
		statBombDetonations = 0f;
		statBombKills = 0f;
		statGreenCombo = 0f;
		statGreenKills = 0f;
		statMultiplier = 0f;
		statOrangeCombo = 0f;
		statOrangeKills = 0f;
		statPinkCombo = 0f;
		statPinkKills = 0f;
		statPowerups = 0f;
		statPurpleCombo = 0f;
		statPurpleKills = 0f;
		statRatio = 0f;
		statRedCombo = 0f;
		statRedKills = 0f;
		statYellowCombo = 0f;
		statYellowKills = 0f;
	}

	public void getAchievement(string achievementName, float percent)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer && !PlayerPrefs.HasKey(achievementName))
		{
			PlayerPrefs.SetString(achievementName, "meow");
		}
	}

	public void OFOpenDashboard()
	{
		openFeintPlugin.OpenDashboard();
	}

	public void OFSubmitScore(string leaderboard, int score)
	{
		openFeintPlugin.SubmitScore(leaderboard, score);
	}

	public void OFSubmitAchievement(int achievementID)
	{
		openFeintPlugin.SubmitAchievement(achievementID);
	}

	public void OFOpenAchievementList()
	{
		if (leaderboardsOpen)
		{
			openFeintPlugin.CloseDashboard();
			leaderboardsOpen = false;
		}
		achievementsOpen = true;
		openFeintPlugin.OpenAchievements();
	}

	public void OFOpenLeaderboards()
	{
		if (achievementsOpen)
		{
			openFeintPlugin.CloseDashboard();
			achievementsOpen = false;
		}
		leaderboardsOpen = true;
		openFeintPlugin.OpenLeaderboards();
	}

	public string OFGetOFID()
	{
		return openFeintPlugin.GetUserID().ToString();
	}

	public IEnumerator SubmitFacebookCallback()
	{
		FacebookManager.Instance.PostImage(Application.persistentDataPath.ToString() + "/screenshot1.jpg");
		yield break;
	}

	public void checkVibrate(string enable)
	{
		if (enable == "YES")
		{
			canVibrate = true;
		}
		else if (enable == "NO")
		{
			canVibrate = false;
		}
		else
		{
			canVibrate = false;
		}
	}

	public void UpdatePowerUps()
	{
		helmLoadOut = (Helm)PlayerPrefs.GetInt("helm");
		chestLoadOut = (Chest)PlayerPrefs.GetInt("armor");
		skinLoadOut = (Skin)PlayerPrefs.GetInt("skin");
		if (helmLoadOut == Helm.SAMURAI)
		{
			playerController.isSamuraiHat = true;
		}
	}
}
