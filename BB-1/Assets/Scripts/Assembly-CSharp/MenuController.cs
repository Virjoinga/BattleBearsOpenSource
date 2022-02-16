using System;
using System.Collections;
using UnityEngine;

public class MenuController : MonoBehaviour
{
	public GameObject mainMenu;

	public GameObject startNewGameMenu;

	public GameObject characterSelectMenu;

	public GameObject modeSelectMenu;

	public GameObject bossSelectMenu;

	public GameObject difficultySelectMenu;

	public GameObject optionsMenu;

	public GameObject riggsBuyMenu;

	public GameObject creditsMenu;

	public GameObject stageSelectMenu;

	public GameObject confirmQuit;

	public GameObject horizontalAnimator;

	public GameObject verticalAnimator;

	private GameObject currentMenu;

	public GameObject scanlinesCamera;

	public AudioClip menuClick;

	public AudioClip startupSound;

	public AudioClip playButtonSound;

	private void Start()
	{
		GameManager.Instance.inTutorialRoom = true;
		GameManager.Instance.inOliverBossRoom = false;
		GameManager.Instance.inRiggsBossRoom = false;
		GameManager.Instance.inUdderMode = false;
		GameManager.Instance.isLoading = false;
		GameManager.Instance.currentStage = 1;
		GameManager.Instance.isGoingUpElevator = false;
		GameManager.Instance.hasAcquiredSpecial = false;
		GameManager.Instance.ocoLives = 0;
		GameManager.Instance.startingStage = 1;
		Time.timeScale = 1f;
		scanlinesCamera.SetActive(false);
		StartCoroutine(delayedPlayMusic());
		GameManager.Instance.currentCharacter = Character.NONE;
		switchToMenu(mainMenu, true, false);
	}

	private IEnumerator delayedPlayMusic()
	{
		if (LoadingCanvas.Instance != null)
		{
			yield return new WaitUntil(() => LoadingCanvas.Instance.LoadingDone);
		}
		else
		{
			yield return new WaitForSeconds(0.5f);
		}
		SoundManager.Instance.playMusic(GameManager.Instance.menuMusic, true);
	}

	private void switchToMenu(GameObject newMenu, bool instantPop, bool goForward)
	{
		GUIController.isActive = false;
		StartCoroutine(delayedSwitch(newMenu, instantPop, goForward));
	}

	private IEnumerator delayedSwitch(GameObject newMenu, bool instantPop, bool goForward)
	{
		if (!instantPop)
		{
			GameObject outgoingTopSlider = UnityEngine.Object.Instantiate(verticalAnimator);
			GameObject outgoingBottomSlider = UnityEngine.Object.Instantiate(verticalAnimator);
			GameObject outgoingHorizontalSlider = UnityEngine.Object.Instantiate(horizontalAnimator);
			if (currentMenu != null)
			{
				currentMenu.transform.Find("MiddleRow").parent = outgoingHorizontalSlider.transform;
				currentMenu.transform.Find("TopRow").parent = outgoingTopSlider.transform;
				currentMenu.transform.Find("BottomRow").parent = outgoingBottomSlider.transform;
				outgoingTopSlider.GetComponent<Animation>().Play("middleToTop");
				outgoingBottomSlider.GetComponent<Animation>().Play("middleToBottom");
				if (goForward)
				{
					outgoingHorizontalSlider.GetComponent<Animation>().Play("middleToLeft");
				}
				else
				{
					outgoingHorizontalSlider.GetComponent<Animation>().Play("middleToRight");
				}
			}
			GameObject incomingTopSlider = UnityEngine.Object.Instantiate(verticalAnimator);
			GameObject incomingBottomSlider = UnityEngine.Object.Instantiate(verticalAnimator);
			GameObject incomingHorizontalSlider = UnityEngine.Object.Instantiate(horizontalAnimator);
			if (newMenu != null)
			{
				GameObject incomingMenu = UnityEngine.Object.Instantiate(newMenu);
				incomingMenu.transform.Find("MiddleRow").parent = incomingHorizontalSlider.transform;
				incomingMenu.transform.Find("TopRow").parent = incomingTopSlider.transform;
				incomingMenu.transform.Find("BottomRow").parent = incomingBottomSlider.transform;
				incomingTopSlider.GetComponent<Animation>().Play("topToMiddle");
				incomingBottomSlider.GetComponent<Animation>().Play("bottomToMiddle");
				if (goForward)
				{
					incomingHorizontalSlider.GetComponent<Animation>().Play("rightToMiddle");
					yield return new WaitForSeconds(incomingHorizontalSlider.GetComponent<Animation>()["rightToMiddle"].length);
				}
				else
				{
					incomingHorizontalSlider.GetComponent<Animation>().Play("leftToMiddle");
					yield return new WaitForSeconds(incomingHorizontalSlider.GetComponent<Animation>()["leftToMiddle"].length);
				}
				incomingHorizontalSlider.transform.Find("MiddleRow").parent = incomingMenu.transform;
				incomingTopSlider.transform.Find("TopRow").parent = incomingMenu.transform;
				incomingBottomSlider.transform.Find("BottomRow").parent = incomingMenu.transform;
				incomingMenu.transform.parent = base.transform;
				if (currentMenu != null)
				{
					UnityEngine.Object.Destroy(currentMenu);
				}
				currentMenu = incomingMenu;
			}
			else
			{
				yield return new WaitForSeconds(outgoingHorizontalSlider.GetComponent<Animation>()["leftToMiddle"].length);
			}
			incomingTopSlider.GetComponent<Animation>()["topToMiddle"].normalizedTime = 1f;
			incomingTopSlider.GetComponent<Animation>().Sample();
			incomingBottomSlider.GetComponent<Animation>()["bottomToMiddle"].normalizedTime = 1f;
			incomingBottomSlider.GetComponent<Animation>().Sample();
			if (goForward)
			{
				incomingHorizontalSlider.GetComponent<Animation>()["rightToMiddle"].normalizedTime = 1f;
			}
			else
			{
				incomingHorizontalSlider.GetComponent<Animation>()["leftToMiddle"].normalizedTime = 1f;
			}
			incomingHorizontalSlider.GetComponent<Animation>().Sample();
			yield return new WaitForSeconds(0.1f);
			UnityEngine.Object.Destroy(incomingHorizontalSlider);
			UnityEngine.Object.Destroy(outgoingHorizontalSlider);
			UnityEngine.Object.Destroy(outgoingTopSlider);
			UnityEngine.Object.Destroy(outgoingBottomSlider);
			UnityEngine.Object.Destroy(incomingTopSlider);
			UnityEngine.Object.Destroy(incomingBottomSlider);
		}
		else
		{
			yield return new WaitForSeconds(0.25f);
			if (currentMenu != null)
			{
				UnityEngine.Object.Destroy(currentMenu);
			}
			currentMenu = UnityEngine.Object.Instantiate(newMenu);
			currentMenu.transform.parent = base.transform;
		}
		if (newMenu == mainMenu || newMenu == optionsMenu)
		{
			scanlinesCamera.SetActive(false);
		}
		else
		{
			scanlinesCamera.SetActive(true);
		}
		GUIController.isActive = true;
	}

	public void OnShowBuyScreen()
	{
		switchToMenu(riggsBuyMenu, false, true);
	}

	public void OnGUIButtonClicked(GUIButton b)
	{
		if (b.name != "Oliver_btn" && b.name != "Riggs_btn" && b.name != "PlayButton")
		{
			SoundManager.Instance.playSound(menuClick);
		}
		switch (b.name)
		{
		case "CreditsButton":
			switchToMenu(creditsMenu, true, true);
			break;
		case "OptionsButton":
			switchToMenu(optionsMenu, true, true);
			break;
		case "ResumeButton":
			if (!PlayerPrefs.HasKey("character"))
			{
				break;
			}
			SavedDataManager.Instance.loadState();
			if (GameManager.Instance.currentGameMode == GameMode.CAMPAIGN)
			{
				if (GameManager.Instance.currentCharacter == Character.OLIVER)
				{
					StartCoroutine(delayedLoadLevel("OliverCampaignLevel" + GameManager.Instance.currentStage));
				}
				else if (GameManager.Instance.currentCharacter == Character.RIGGS)
				{
					StartCoroutine(delayedLoadLevel("RiggsCampaignLevel" + GameManager.Instance.currentStage));
				}
				else if (GameManager.Instance.currentCharacter == Character.WIL)
				{
					StartCoroutine(delayedLoadLevel("WilCampaignLevel" + GameManager.Instance.currentStage));
				}
			}
			else
			{
				GameManager.Instance.hasAcquiredSpecial = true;
				StartCoroutine(delayedLoadLevel((GameManager.Instance.currentCharacter == Character.WIL) ? "SurvivalZombies" : "Survival"));
			}
			break;
		case "OpenFeintButton":
			switchToMenu(confirmQuit, true, true);
			break;
		case "PlayButton":
			SoundManager.Instance.playSound(playButtonSound);
			if (PlayerPrefs.HasKey("character") && GameManager.Instance.version == PlayerPrefs.GetString("version", ""))
			{
				switchToMenu(startNewGameMenu, true, true);
			}
			else
			{
				switchToMenu(characterSelectMenu, true, true);
			}
			break;
		case "cancelnew_btn":
			switchToMenu(mainMenu, true, false);
			break;
		case "confirmnew_btn":
			switchToMenu(characterSelectMenu, true, true);
			break;
		case "mainMenu_btn":
			switchToMenu(mainMenu, true, false);
			break;
		case "backToCharacterChoose_btn":
			switchToMenu(characterSelectMenu, false, false);
			break;
		case "stageSelect_btn":
			switchToMenu(stageSelectMenu, false, true);
			break;
		case "backToDifficulty_btn":
			switchToMenu(difficultySelectMenu, false, false);
			break;
		case "GoFromCharacterChoose_btn":
			if (GameManager.Instance.currentCharacter != Character.NONE)
			{
				switchToMenu(modeSelectMenu, false, true);
			}
			break;
		case "GoFromModeSelect_btn":
			switch (GameManager.Instance.currentGameMode)
			{
			case GameMode.SURVIVAL:
				GameManager.Instance.currentDifficulty = GameDifficulty.MEDIUM;
				GameManager.Instance.currentBossTrial = BossMode.NONE;
				PlayerPrefs.DeleteKey("character");
				StatsManager.Instance.OnReset();
				GameManager.Instance.hasAcquiredSpecial = true;
				StartCoroutine(delayedLoadLevel((GameManager.Instance.currentCharacter == Character.WIL) ? "SurvivalZombies" : "Survival"));
				break;
			case GameMode.CAMPAIGN:
				switchToMenu(difficultySelectMenu, false, true);
				break;
			case GameMode.BOSSTRIAL:
				switchToMenu(bossSelectMenu, false, true);
				break;
			}
			break;
		case "backToModeSelect_btn":
			switchToMenu(modeSelectMenu, false, false);
			break;
		case "GoBossTrial_btn":
			GameManager.Instance.currentDifficulty = GameDifficulty.MEDIUM;
			switch (GameManager.Instance.currentBossTrial)
			{
			case BossMode.MECHABEARZERKER:
				StatsManager.Instance.OnReset();
				GameManager.Instance.inTutorialRoom = false;
				GameManager.Instance.hasAcquiredSpecial = true;
				StartCoroutine(delayedLoadLevel("MechaBossFight"));
				break;
			case BossMode.TENTACLEESE:
				StatsManager.Instance.OnReset();
				GameManager.Instance.inTutorialRoom = false;
				GameManager.Instance.hasAcquiredSpecial = true;
				StartCoroutine(delayedLoadLevel("TentacleeseFight"));
				break;
			case BossMode.SPACEBOSS:
				GameManager.Instance.ocoLives = 1;
				StatsManager.Instance.OnReset();
				GameManager.Instance.inTutorialRoom = false;
				GameManager.Instance.hasAcquiredSpecial = true;
				StartCoroutine(delayedLoadLevel("SpacebossFight"));
				break;
			}
			break;
		case "StartGame_btn":
			if (GameManager.Instance.currentDifficulty != GameDifficulty.NONE)
			{
				GameManager.Instance.currentBossTrial = BossMode.NONE;
				GameManager.Instance.currentStage = GameManager.Instance.startingStage;
				if (GameManager.Instance.startingStage > 1)
				{
					GameManager.Instance.hasAcquiredSpecial = true;
				}
				else
				{
					GameManager.Instance.hasAcquiredSpecial = false;
				}
				switch (GameManager.Instance.currentCharacter)
				{
				case Character.OLIVER:
					PlayerPrefs.DeleteKey("character");
					StatsManager.Instance.OnReset();
					StartCoroutine(delayedLoadLevel("OliverCampaignLevel" + GameManager.Instance.currentStage));
					break;
				case Character.RIGGS:
					PlayerPrefs.DeleteKey("character");
					StatsManager.Instance.OnReset();
					GameManager.Instance.inTutorialRoom = false;
					StartCoroutine(delayedLoadLevel("RiggsCampaignLevel" + GameManager.Instance.currentStage));
					break;
				case Character.WIL:
					PlayerPrefs.DeleteKey("character");
					StatsManager.Instance.OnReset();
					GameManager.Instance.inTutorialRoom = false;
					StartCoroutine(delayedLoadLevel("WilCampaignLevel" + GameManager.Instance.currentStage));
					PlayerPrefs.SetInt("hasShotgunFired", 0);
					PlayerPrefs.SetInt("isWil", 1);
					break;
				}
			}
			break;
		case "confirmquit_btn":
			Application.Quit();
			break;
		}
	}

	private IEnumerator delayedLoadLevel(string level)
	{
		currentMenu = null;
		mainMenu = null;
		startNewGameMenu = null;
		characterSelectMenu = null;
		modeSelectMenu = null;
		bossSelectMenu = null;
		difficultySelectMenu = null;
		optionsMenu = null;
		SoundManager.Instance.stopAll();
		switchToMenu(null, false, true);
		yield return new WaitForSeconds(1f);
		GameManager.Instance.levelToLoad = level;
		GC.Collect();
		Application.LoadLevel("LoadingScene");
	}
}
