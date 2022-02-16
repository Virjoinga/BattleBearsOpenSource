using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
	public Camera hudCam;

	public GameObject HUD;

	public GameObject pauseAar;

	public TextMesh score;

	public TextMesh best;

	public TextMesh survival_Score;

	public TextMesh survival_BestScore;

	public GameObject[] strikesLit;

	public GameObject timeAttack;

	public GameObject scoreAttack;

	public GameObject survival;

	public TextMesh[] pairOne;

	public TextMesh[] pairTwo;

	public TextMesh[] pairThree;

	public TextMesh[] pairFour;

	public TextMesh[] pairFive;

	public TextMesh time;

	public TextMesh challengeSet;

	public float scoreVal;

	public GameObject retryBtn;

	public GameObject pauseResumeBtn;

	public GameObject reheatBtn;

	public GameObject gameCenterBtn;

	public GameObject faceBookBtn;

	private int strikeIndex;

	public bool isScrolling;

	[HideInInspector]
	public float realTime;

	public TextMesh[] textFields;

	public TextMesh musicLight;

	public TextMesh soundLight;

	public Material tutorialMat;

	public TextMesh[] powerUps;

	public Vector3[] powerUpPos;

	private int powerUpIndex;

	private Ray touchToRay;

	private RaycastHit outHit;

	private bool hasHitObj;

	private AudioClip[] buttonPressArr;

	public float spawnDelay;

	[HideInInspector]
	public bool canStart;

	private int pairIndex;

	private bool retryMenu;

	public GameObject aarText;

	private Display currDisplay;

	private bool aarScroll;

	public int currentSet;

	private float scoreStartBest;

	private float ratio;

	private bool setCompleteScroll;

	public GameObject timeUp;

	public GameObject timeUpActive;

	private int objectiveStart;

	public GameObject clearedAnimation;

	public GameObject clearedAnimationActive;

	public ClearedAnimationHandler clearScript;

	public bool pauseTimer;

	public bool isRetry;

	public Animation hudAnimation;

	private int formatIndex;

	public GameObject negScore;

	public GameObject gameOverText;

	public GameObject crosshairsSafe;

	public GameObject crosshairsFire;

	public GameObject pauseBtn;

	private GameObject firstObjectTouched;

	private bool playedTutAnim;

	public Vector3 iconScale;

	public bool didBeatHighScore;

	private bool didGetPair;

	public GameObject sensitivityBar;

	private Vector3 raycastPosition;

	public void Awake()
	{
		GameManager.Instance.hudController = this;
		hudAnimation = base.GetComponent<Animation>();
		firstObjectTouched = null;
	}

	public void Start()
	{
		if (!PlayerPrefs.HasKey("sensitivity"))
		{
			PlayerPrefs.SetFloat("sensitivity", 2f);
		}
		GameManager.Instance.controls.sensitivity = PlayerPrefs.GetFloat("sensitivity");
		sensitivityBar.transform.position = new Vector3((GameManager.Instance.controls.sensitivity - 1f) * 5f + 103.4332f, sensitivityBar.transform.position.y, sensitivityBar.transform.position.z);
		pairIndex = 0;
		//HUD.SetActiveRecursively(false);
		HUD.SetActive(false);
		strikeIndex = 0;
		for (int i = 0; i < powerUps.Length; i++)
		{
			powerUps[i].text = string.Empty;
			powerUps[i].transform.localScale = Vector3.zero;
			//powerUps[i].gameObject.active = false;
			powerUps[i].gameObject.SetActive(false);
		}
		powerUpIndex = -1;
		if (SoundController.Instance.isAllOn())
		{
			soundLight.text = "\"";
		}
		else
		{
			soundLight.text = " ";
		}
		if (SoundController.Instance.isMusicOn())
		{
			musicLight.text = "\"";
		}
		else
		{
			musicLight.text = " ";
		}
		buttonPressArr = new AudioClip[3];
		buttonPressArr[0] = Resources.Load("microwave_button1") as AudioClip;
		buttonPressArr[1] = Resources.Load("microwave_button2") as AudioClip;
		buttonPressArr[2] = Resources.Load("microwave_button3") as AudioClip;
		//faceBookBtn.gameObject.active = false;
		faceBookBtn.gameObject.SetActive(false);
		//gameCenterBtn.gameObject.active = false;
		gameCenterBtn.gameObject.SetActive(false);
		currDisplay = Display.NONE;
		iconScale = pairOne[0].gameObject.transform.localScale;
	}

	public void sexRobots(string anim)
	{
		if (GameManager.Instance.currentGameMode == GameMode.SCORE)
		{
			//scoreAttack.SetActiveRecursively(true);
			scoreAttack.SetActive(true);
			hudAnimation.Play(anim);
		}
		if (GameManager.Instance.currentGameMode == GameMode.TIME)
		{
			pairOne[0].text = ":";
			pairTwo[1].text = ":";
			//timeAttack.SetActiveRecursively(true);
			timeAttack.SetActive(true);
			hudAnimation.Play(anim);
		}
		if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			//survival.SetActiveRecursively(true);
			survival.SetActive(true);
			hudAnimation.Play(anim);
		}
		playedTutAnim = true;
	}

	public void tacomOnline()
	{
		//pauseBtn.active = true;
		pauseBtn.SetActive(true);
		if (GameManager.Instance.currentGameMode == GameMode.SCORE)
		{
			//scoreAttack.SetActiveRecursively(true);
			scoreAttack.SetActive(true);
			//score.gameObject.active = true;
			score.gameObject.SetActive(true);
			//best.gameObject.active = true;
			best.gameObject.SetActive(true);
			if (hudAnimation["ScoreAttack"].time != hudAnimation["ScoreAttack"].length)
			{
				if (playedTutAnim)
				{
					playedTutAnim = false;
				}
				else
				{
					hudAnimation.Play("ScoreAttack");
				}
			}
			MWPowerBarManager.Instance.InitializeRedBlocks();
			MWPowerBarManager.Instance.InitializeGreenBlocks();
		}
		if (GameManager.Instance.currentGameMode == GameMode.TIME)
		{
			//timeAttack.SetActiveRecursively(true);
			timeAttack.SetActive(true);
			//score.gameObject.active = true;
			score.gameObject.SetActive(true);
			//best.gameObject.active = true;
			best.gameObject.SetActive(true);
			pairIndex = 1;
			for (int i = 0; i < pairOne.Length; i++)
			{
				if (pairOne.Length != 2 || pairTwo.Length != 2 || pairThree.Length != 2 || pairFour.Length != 2)
				{
					return;
				}
				//pairOne[i].gameObject.active = false;
				pairOne[i].gameObject.SetActive(false);
			}
			challengeSet.text = "01";
			time.text = "60";
			if (hudAnimation["TimeAttack"].time != hudAnimation["TimeAttack"].length)
			{
				if (playedTutAnim)
				{
					playedTutAnim = false;
				}
				else
				{
					hudAnimation.Play("TimeAttack");
				}
			}
		}
		if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			//survival.SetActiveRecursively(true);
			survival.SetActive(true);
			GameObject[] array = strikesLit;
			foreach (GameObject gameObject in array)
			{
				//gameObject.active = false;
				gameObject.SetActive(false);
			}
			strikeIndex = 0;
			if (hudAnimation["Survival_Slide"].time != hudAnimation["Survival_Slide"].length)
			{
				if (playedTutAnim)
				{
					playedTutAnim = false;
				}
				else
				{
					hudAnimation.Play("Survival_Slide");
				}
			}
		}
		scoreVal = 0f;
		score.text = "xxxxxxx";
		if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			survival_Score.text = "xxxxxxx";
		}
		StartCoroutine("introMessage", "HELLO");
		if (GameManager.Instance.currentGameMode == GameMode.SCORE)
		{
			best.text = PlayerPrefs.GetString("best");
		}
		else if (GameManager.Instance.currentGameMode == GameMode.TIME)
		{
			best.text = PlayerPrefs.GetString("bestTime");
		}
		else if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			survival_BestScore.text = PlayerPrefs.GetString("Survival_BestTime");
		}
		Objectives.Instance.objectiveTen();
		objectiveStart = PlayerPrefs.GetInt("objectives");
	}

	public IEnumerator introMessage(string text)
	{
		for (int i = 0; i < Objectives.Instance.setList.Length; i++)
		{
			Objectives.Instance.setList[i] = false;
		}
		Objectives.Instance.pairIndex = 0;
		Objectives.Instance.setCompletions = 1;
		GameManager.Instance.multiplier = 0f;
		GameManager.Instance.colorComboAmt = 1;
		PlayerPrefs.SetFloat("statSurvivalScore", float.Parse(PlayerPrefs.GetString("Survival_BestTime")));
		PlayerPrefs.SetFloat("statTimeScore", float.Parse(PlayerPrefs.GetString("bestTime")));
		yield return StartCoroutine("scrollingText", text);
		yield return StartCoroutine(resetScoreHUD());
	}

	public IEnumerator scrollingText(string text)
	{
		isScrolling = true;
		text.ToUpper();
		string initialTest = score.text;
		string[] prePostPend = new string[4] { "xxxxxxx", "yyyyyyy", "vvvvvvv", "wwwwwww" };
		string textBuf = prePostPend[0] + text + prePostPend[0];
		score.text = string.Empty;
		if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			survival_Score.text = string.Empty;
		}
		yield return new WaitForSeconds(0.5f);
		int rotatorStrPos2 = 0;
		int strIndex = 0;
		for (int i = 0; i < textBuf.Length - 7; i++)
		{
			if (score != null)
			{
				score.text = textBuf.Substring(strIndex, 7);
				if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
				{
					survival_Score.text = textBuf.Substring(strIndex, 7);
				}
				yield return new WaitForSeconds(0.25f);
				strIndex++;
				rotatorStrPos2++;
				rotatorStrPos2 %= 4;
				textBuf = prePostPend[rotatorStrPos2] + text + prePostPend[rotatorStrPos2];
			}
		}
		yield return new WaitForSeconds(0.2f);
		if (score != null)
		{
			score.text = initialTest;
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				survival_Score.text = initialTest;
			}
		}
		yield return new WaitForSeconds(0.25f);
		isScrolling = false;
	}

	private IEnumerator resetScoreHUD()
	{
		challengeSet.text = "01";
		currentSet = 0;
		score.text = "0000000";
		survival_Score.text = "0000000";
		yield return new WaitForSeconds(0.25f);
		score.text = "///////";
		survival_Score.text = "///////";
		yield return new WaitForSeconds(0.25f);
		score.text = "0000000";
		survival_Score.text = "0000000";
		yield return new WaitForSeconds(0.25f);
	}

	public void dynamicText(Display display, string text)
	{
		aarText.transform.localPosition = new Vector3(0f, -54f, 4.5f);
		currDisplay = display;
		int num = PlayerPrefs.GetInt("objectives") - objectiveStart;
		formatIndex = 0;
		bool flag = false;
		if (display == Display.AAR)
		{
			Format(1.82f, 0.65f, 0f);
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				if (scoreVal < PlayerPrefs.GetFloat("statSurvivalScore"))
				{
					textFields[formatIndex - 1].text = "\"SCORE.";
				}
				else if (scoreVal == PlayerPrefs.GetFloat("statSurvivalScore"))
				{
					textFields[formatIndex - 1].text = "#SCORE.";
				}
				else
				{
					textFields[formatIndex - 1].text = "$SCORE.";
					flag = true;
				}
			}
			else if (GameManager.Instance.currentGameMode == GameMode.TIME)
			{
				if (scoreVal < PlayerPrefs.GetFloat("statTimeScore"))
				{
					textFields[formatIndex - 1].text = "\"SCORE.";
				}
				else if (scoreVal == PlayerPrefs.GetFloat("statTimeScore"))
				{
					textFields[formatIndex - 1].text = "#SCORE.";
				}
				else
				{
					textFields[formatIndex - 1].text = "$SCORE.";
					flag = true;
				}
			}
			Format(3f, 0.65f, 0.9674193f);
			textFields[formatIndex - 1].text = numToDigit(scoreVal, 7, true);
			if (flag)
			{
				Format(1.5f, 0.65f, 0f);
				textFields[formatIndex - 1].text = "NEW/HIGH/SCOREz";
				SoundController.Instance.PlayMySound("Abbi Sounds/abbi_highscore");
			}
			if (GameManager.Instance.currentGameMode == GameMode.TIME)
			{
				Format(2f, 0.65f, 0f);
				if ((float)Objectives.Instance.pairIndex < PlayerPrefs.GetFloat("statSets"))
				{
					textFields[formatIndex - 1].text = "\"MULTI." + Objectives.Instance.pairIndex;
				}
				else if ((float)currentSet == PlayerPrefs.GetFloat("statSets"))
				{
					textFields[formatIndex - 1].text = "#MULTI." + Objectives.Instance.pairIndex;
				}
				else
				{
					textFields[formatIndex - 1].text = "$MULTI." + Objectives.Instance.pairIndex;
				}
			}
			Format(1.15f, 0.68f, 0f);
			if (ratio < PlayerPrefs.GetFloat("statRatio"))
			{
				textFields[formatIndex - 1].text = "\"POINT/RATIO./" + ratioDisplay();
			}
			else if (ratio == PlayerPrefs.GetFloat("statRatio"))
			{
				textFields[formatIndex - 1].text = "#POINT/RATIO./" + ratioDisplay();
			}
			else
			{
				textFields[formatIndex - 1].text = "$POINT/RATIO./" + ratioDisplay();
			}
			if (GameManager.Instance.arcadeTime < 1f || GameManager.Instance.isOver)
			{
				Format(1.3f, 0.68f, 0f);
				int num2 = Mathf.RoundToInt(scoreVal / 1000f) * GameManager.Instance.currencyMultiplier / 2;
				TapJoyManager.Instance.AwardJoules(num2);
				textFields[formatIndex - 1].text = "JOULES/EARNED. " + num2;
			}
			Format(1.15f, 0.68f, 0f);
			textFields[formatIndex - 1].text = "//BAKE///COMBOS";
			Format(1.15f, 0.65f, 0f);
			textFields[formatIndex - 1].text = ":/" + numToDigit(GameManager.Instance.statPinkKills, 4, false) + "///" + numToDigit(GameManager.Instance.statPinkCombo, 6, false) + "\n;/" + numToDigit(GameManager.Instance.statGreenKills, 4, false) + "///" + numToDigit(GameManager.Instance.statGreenCombo, 6, false) + "\n</" + numToDigit(GameManager.Instance.statRedKills, 4, false) + "///" + numToDigit(GameManager.Instance.statRedCombo, 6, false) + "\n=/" + numToDigit(GameManager.Instance.statYellowKills, 4, false) + "///" + numToDigit(GameManager.Instance.statYellowCombo, 6, false) + "\n>/" + numToDigit(GameManager.Instance.statBlueKills, 4, false) + "///" + numToDigit(GameManager.Instance.statBlueCombo, 6, false) + "\n?/" + numToDigit(GameManager.Instance.statOrangeKills, 4, false) + "///" + numToDigit(GameManager.Instance.statOrangeCombo, 6, false) + "\n+/" + numToDigit(GameManager.Instance.statPurpleKills, 4, false) + "///" + numToDigit(GameManager.Instance.statPurpleCombo, 6, false);
			Format(1.1f, 0.65f, 0f);
			textFields[formatIndex - 1].text = "HIGHEST/MULTIPLIER./" + GameManager.Instance.statMultiplier;
			Format(1.15f, 0.65f, 0f);
			if (GameManager.Instance.statBombDetonations < PlayerPrefs.GetFloat("statBombDetonations"))
			{
				textFields[formatIndex - 1].text = "\"BOMBS/DETONATED./" + GameManager.Instance.statBombDetonations;
			}
			else if (GameManager.Instance.statBombDetonations == PlayerPrefs.GetFloat("statBombDetonations"))
			{
				textFields[formatIndex - 1].text = "#BOMBS/DETONATED./" + GameManager.Instance.statBombDetonations;
			}
			else
			{
				textFields[formatIndex - 1].text = "$BOMBS/DETONATED./" + GameManager.Instance.statBombDetonations;
			}
			Format(1.15f, 0.65f, 0f);
			if (GameManager.Instance.statBombKills < PlayerPrefs.GetFloat("statBombKills"))
			{
				textFields[formatIndex - 1].text = "\"BOMB/KILLS./" + GameManager.Instance.statBombKills;
			}
			else if (GameManager.Instance.statBombKills == PlayerPrefs.GetFloat("statBombKills"))
			{
				textFields[formatIndex - 1].text = "#BOMB/KILLS./" + GameManager.Instance.statBombKills;
			}
			else
			{
				textFields[formatIndex - 1].text = "$BOMB/KILLS./" + GameManager.Instance.statBombKills;
			}
			Format(1.15f, 0.65f, 0f);
			if (GameManager.Instance.statPowerups < PlayerPrefs.GetFloat("statPowerups"))
			{
				textFields[formatIndex - 1].text = "\"POWERUP/COUNT./" + GameManager.Instance.statPowerups;
			}
			else if (GameManager.Instance.statPowerups == PlayerPrefs.GetFloat("statPowerups"))
			{
				textFields[formatIndex - 1].text = "#POWERUP/COUNT./" + GameManager.Instance.statPowerups;
			}
			else
			{
				textFields[formatIndex - 1].text = "$POWERUP/COUNT./" + GameManager.Instance.statPowerups;
			}
			Format(1.15f, 0.65f, 0f);
			textFields[formatIndex - 1].text = "OBJECTIVES/CLEARED./" + num;
			if (isRetry)
			{
				GameManager.Instance.statPinkKills = 0f;
				GameManager.Instance.statPinkCombo = 0f;
				GameManager.Instance.statGreenKills = 0f;
				GameManager.Instance.statGreenCombo = 0f;
				GameManager.Instance.statRedKills = 0f;
				GameManager.Instance.statRedCombo = 0f;
				GameManager.Instance.statYellowKills = 0f;
				GameManager.Instance.statYellowCombo = 0f;
				GameManager.Instance.statBlueKills = 0f;
				GameManager.Instance.statBlueCombo = 0f;
				GameManager.Instance.statOrangeKills = 0f;
				GameManager.Instance.statOrangeCombo = 0f;
				GameManager.Instance.statPurpleKills = 0f;
				GameManager.Instance.statPurpleCombo = 0f;
				GameManager.Instance.statBombDetonations = 0f;
				GameManager.Instance.statBombKills = 0f;
				GameManager.Instance.statPowerups = 0f;
			}
		}
		if (display != Display.OBJECTIVE)
		{
			return;
		}
		if (PlayerPrefs.GetInt("objectives") >= 11)
		{
			Format(1.85f, 0.65f, 0f);
			textFields[formatIndex - 1].text = "WINRAR/IS/YOUz";
			return;
		}
		Format(2.15f, 0.65f, 0f);
		textFields[formatIndex - 1].text = "OBJECTIVE/" + PlayerPrefs.GetInt("objectives") + ".";
		if (Objectives.Instance.goalOne != 0)
		{
			Format(1.15f, 0.65f, 0f);
			if (PlayerPrefs.GetInt("goal1") != 0)
			{
				textFields[formatIndex - 1].text = CreateStringBounds("^" + Objectives.Instance.textInfo(1));
			}
			else
			{
				textFields[formatIndex - 1].text = CreateStringBounds("]" + Objectives.Instance.textInfo(1));
			}
		}
		if (Objectives.Instance.goalTwo != 0)
		{
			Format(1.15f, 0.65f, 0f);
			if (PlayerPrefs.GetInt("goal2") != 0)
			{
				textFields[formatIndex - 1].text = CreateStringBounds("^" + Objectives.Instance.textInfo(2));
			}
			else
			{
				textFields[formatIndex - 1].text = CreateStringBounds("]" + Objectives.Instance.textInfo(2));
			}
		}
		if (Objectives.Instance.goalThree != 0)
		{
			Format(1.15f, 0.65f, 0f);
			if (PlayerPrefs.GetInt("goal3") != 0)
			{
				textFields[formatIndex - 1].text = CreateStringBounds("^" + Objectives.Instance.textInfo(3));
			}
			else
			{
				textFields[formatIndex - 1].text = CreateStringBounds("]" + Objectives.Instance.textInfo(3));
			}
		}
	}

	public void Format(float charSize, float line, float offset)
	{
		//textFields[formatIndex].gameObject.active = true;
		textFields[formatIndex].gameObject.SetActive(true);
		textFields[formatIndex].gameObject.name = "TextFields" + formatIndex;
		textFields[formatIndex].characterSize = charSize;
		textFields[formatIndex].lineSpacing = line;
		if (formatIndex == 0)
		{
			textFields[formatIndex].transform.localPosition = new Vector3(-6.1f, 0f, 0f);
		}
		else
		{
			float num = Mathf.RoundToInt((float)textFields[formatIndex - 1].text.Length / (26f / textFields[formatIndex - 1].characterSize));
			textFields[formatIndex].transform.localPosition = new Vector3(textFields[formatIndex - 1].transform.localPosition.x, 0f, textFields[formatIndex - 1].transform.localPosition.z - textFields[formatIndex - 1].characterSize * num - offset);
		}
		formatIndex++;
	}

	public string jouleProgressBar()
	{
		string text = string.Empty;
		float num = (float)PlayerPrefs.GetInt("joules") / (float)GameManager.Instance.joulesToNext;
		for (float num2 = 0f; num2 < num; num2 += 0.07143f)
		{
			text += ".";
		}
		for (int i = text.Length; i < 14; i++)
		{
			text += "/";
		}
		return text;
	}

	public string objectivesCleared(int start, int num)
	{
		string text = string.Empty;
		for (int i = start; i < num; i++)
		{
			text = text + i + "^///";
			if (num - i % 4 == 0)
			{
				text += "\n";
			}
		}
		return text;
	}

	public string numToDigit(float num, int digits, bool zero)
	{
		string result = string.Empty;
		int num2 = digits - num.ToString().Length;
		if (zero)
		{
			switch (num2)
			{
			case 0:
				result = num.ToString();
				break;
			case 1:
				result = "0" + num;
				break;
			case 2:
				result = "00" + num;
				break;
			case 3:
				result = "000" + num;
				break;
			case 4:
				result = "0000" + num;
				break;
			case 5:
				result = "00000" + num;
				break;
			case 6:
				result = "000000" + num;
				break;
			case 7:
				result = "0000000" + num;
				break;
			case 8:
				result = "0000000" + num;
				break;
			}
		}
		else
		{
			switch (num2)
			{
			case 0:
				result = num.ToString();
				break;
			case 1:
				result = "/" + num;
				break;
			case 2:
				result = "//" + num;
				break;
			case 3:
				result = "///" + num;
				break;
			case 4:
				result = "////" + num;
				break;
			case 5:
				result = "/////" + num;
				break;
			case 6:
				result = "//////" + num;
				break;
			case 7:
				result = "//////" + num;
				break;
			case 8:
				result = "///////" + num;
				break;
			}
		}
		return result;
	}

	public void findRatio()
	{
		float num = GameManager.Instance.statBlueKills + GameManager.Instance.statGreenKills + GameManager.Instance.statOrangeKills + GameManager.Instance.statPinkKills + GameManager.Instance.statRedKills + GameManager.Instance.statYellowKills;
		if (num > 0f)
		{
			ratio = scoreVal / num;
		}
		else
		{
			ratio = 0f;
		}
	}

	public string ratioDisplay()
	{
		string empty = string.Empty;
		return Mathf.RoundToInt(ratio).ToString();
	}

	public void updateScore(float points)
	{
		scoreVal += points;
		Objectives.Instance.score(scoreVal);
		int num = int.Parse(best.text);
		if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			num = int.Parse(survival_BestScore.text);
		}
		if ((float)num <= scoreVal)
		{
			best.text = numToDigit(scoreVal, 7, true);
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				survival_BestScore.text = numToDigit(scoreVal, 7, true);
			}
		}
		if (points < 0f)
		{
			StartCoroutine("NegativeScoreHandler", points);
		}
		if (!isScrolling)
		{
			score.text = numToDigit(scoreVal, 7, true);
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				survival_Score.text = numToDigit(scoreVal, 7, true);
			}
			setTheBest();
		}
	}

	public IEnumerator NegativeScoreHandler(float points)
	{
		GameObject gameObj = (GameObject)UnityEngine.Object.Instantiate(negScore);
		TextMesh textMesh = gameObj.GetComponent<TextMesh>();
		Animation ani = gameObj.GetComponent<Animation>();
		textMesh.text = numberToStringRed(points);
		yield return new WaitForSeconds(ani.clip.length);
		UnityEngine.Object.Destroy(gameObj);
	}

	private string numberToStringRed(float number)
	{
		string text = number.ToString();
		string text2 = "_";
		for (int i = 0; i < text.Length; i++)
		{
			if (i != 0)
			{
				text2 += (char)(text[i] + 60);
			}
		}
		return text2;
	}

	public void resetScore()
	{
		scoreVal = 0f;
		strikeIndex = 0;
	}

	public void hit(float dmg)
	{
		float num = Math.Abs(dmg - 3f);
		if (num <= 2f)
		{
			if (num == 0f)
			{
				SoundController.Instance.PlayMySound("o_hurt1");
			}
			if (num == 1f)
			{
				SoundController.Instance.PlayMySound("o_hurt2");
			}
			if (num == 2f)
			{
				SoundController.Instance.PlayMySound("o_hurt_last");
				SoundController.Instance.PlayMySound("Abbi Sounds/abbi_gameover");
			}
			//strikesLit[(int)num].active = true;
			strikesLit[(int)num].SetActive(true);
			strikeIndex++;
			StartCoroutine("strikeEffect", (int)num);
		}
	}

	public IEnumerator strikeEffect(int x)
	{
		strikesLit[x].transform.localScale += Vector3.one * 2f;
		bool loop = true;
		while (loop)
		{
			strikesLit[x].transform.localScale -= Vector3.one * 0.06f * 3f;
			yield return null;
			if (strikesLit[x].transform.localScale.x <= 0.2003238f)
			{
				float diff = 0.2003238f - strikesLit[x].transform.localScale.x;
				Vector3 temp = new Vector3(diff, diff, diff);
				strikesLit[x].transform.localScale += temp;
				loop = false;
			}
		}
	}

	public void setTheBest()
	{
		int num = int.Parse(best.text);
		if ((float)num <= scoreVal)
		{
			if (!didBeatHighScore)
			{
				didBeatHighScore = true;
			}
			if (GameManager.Instance.currentGameMode == GameMode.SCORE)
			{
				PlayerPrefs.SetString("best", numToDigit(scoreVal, 7, true));
			}
			else if (GameManager.Instance.currentGameMode == GameMode.TIME)
			{
				PlayerPrefs.SetString("bestTime", numToDigit(scoreVal, 7, true));
			}
			else if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				PlayerPrefs.SetString("Survival_BestTime", numToDigit(scoreVal, 7, true));
			}
		}
	}

	public void pause()
	{
		GameObject gameObject = GameObject.Find("NegScore(Clone)");
		if ((bool)gameObject)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		if ((bool)timeUpActive)
		{
			UnityEngine.Object.Destroy(timeUpActive);
		}
		if ((bool)clearScript)
		{
			if (clearScript.cleared.active)
			{
				clearScript.cleared.GetComponent<MeshRenderer>().enabled = false;
			}
			if (clearScript.nextSet.active)
			{
				clearScript.nextSet.GetComponent<MeshRenderer>().enabled = false;
			}
			for (short num = 0; num < clearScript.count.Length; num = (short)(num + 1))
			{
				if (clearScript.count[num].active)
				{
					clearScript.count[num].GetComponent<MeshRenderer>().enabled = false;
				}
			}
		}
		retryMenu = false;
		//HUD.SetActiveRecursively(false);
		HUD.SetActive(false);
		//pauseAar.SetActiveRecursively(true);
		pauseAar.SetActive(true);
		for (int i = 0; i < textFields.Length; i++)
		{
			//textFields[i].gameObject.active = false;
			textFields[i].gameObject.SetActive(false);
		}
		//gameCenterBtn.active = false;
		gameCenterBtn.SetActive(false);
		//faceBookBtn.active = false;
		faceBookBtn.SetActive(false);
		dynamicText(Display.AAR, string.Empty);
		textFields[1].GetComponent<Animation>().Play();
		Time.timeScale = 0f;
		GameManager.Instance.isPaused = true;
		//pauseBtn.active = true;
		pauseBtn.SetActive(true);
	}

	public void resume()
	{
		Time.timeScale = 1f;
		StartCoroutine("resumeEnumerator");
	}

	public IEnumerator resumeEnumerator()
	{
		yield return new WaitForSeconds(0.1f);
		if ((bool)clearScript)
		{
			if (clearScript.cleared.active)
			{
				clearScript.cleared.GetComponent<MeshRenderer>().enabled = true;
			}
			if (clearScript.nextSet.active)
			{
				clearScript.nextSet.GetComponent<MeshRenderer>().enabled = true;
			}
			for (int k = 0; k < clearScript.count.Length; k++)
			{
				if (clearScript.count[k].active)
				{
					clearScript.count[k].GetComponent<MeshRenderer>().enabled = true;
				}
			}
		}
		if (GameManager.Instance.currentGameMode == GameMode.SCORE)
		{
			//scoreAttack.SetActiveRecursively(true);
			scoreAttack.SetActive(true);
			MWPowerBarManager.Instance.setRedBlock();
			for (byte l = 0; l < strikesLit.Length; l = (byte)(l + 1))
			{
				strikesLit[l].active = false;
			}
			for (byte m = 0; m < strikeIndex; m = (byte)(m + 1))
			{
				strikesLit[m].active = true;
			}
		}
		if (GameManager.Instance.currentGameMode == GameMode.TIME)
		{
			//timeAttack.SetActiveRecursively(true);
			timeAttack.SetActive(true);
			for (int n = 0; n < pairOne.Length; n++)
			{
				if (pairOne.Length != 2 || pairTwo.Length != 2 || pairThree.Length != 2 || pairFour.Length != 2 || pairFive.Length != 2)
				{
					yield break;
				}
				pairOne[n].gameObject.active = pairIndex == 1;
				pairTwo[n].gameObject.active = pairIndex == 2;
				pairThree[n].gameObject.active = pairIndex == 3;
				pairFour[n].gameObject.active = pairIndex == 4;
				pairFive[n].gameObject.active = pairIndex == 5;
			}
			challengeSet.text = numToDigit(currentSet, 2, true);
			time.text = GameManager.Instance.arcadeTime.ToString();
		}
		if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			//survival.SetActiveRecursively(true);
			survival.SetActive(true);
			for (int j = strikeIndex; j < strikesLit.Length; j++)
			{
				//strikesLit[j].active = false;
				strikesLit[j].SetActive(false);
			}
		}
		for (int i = 0; i < textFields.Length; i++)
		{
			//textFields[i].gameObject.active = false;
			textFields[i].gameObject.SetActive(false);
		}
		//pauseAar.SetActiveRecursively(false);
		pauseAar.SetActive(false);
		GameManager.Instance.isPaused = false;
		//score.gameObject.active = true;
		score.gameObject.SetActive(true);
		//best.gameObject.active = true;
		best.gameObject.SetActive(true);
	}

	public void AAR()
	{
		GameManager.Instance.isOver = true;
		//HUD.SetActiveRecursively(false);
		HUD.SetActive(false);
		//pauseAar.SetActiveRecursively(true);
		pauseAar.SetActive(true);
		pauseAar.transform.localPosition = new Vector3(pauseAar.transform.localPosition.x, pauseAar.transform.localPosition.y, -33.65179f);
		for (int i = 0; i < textFields.Length; i++)
		{
			//textFields[i].gameObject.active = false;
			textFields[i].gameObject.SetActive(false);
		}
		Time.timeScale = 1E-07f;
		//retryBtn.active = false;
		retryBtn.SetActive(false);
		//pauseResumeBtn.active = false;
		pauseResumeBtn.SetActive(false);
		//gameCenterBtn.active = true;
		gameCenterBtn.SetActive(true);
		//faceBookBtn.active = true;
		faceBookBtn.SetActive(true);
		didGetPair = false;
		findRatio();
		dynamicText(Display.AAR, string.Empty);
		textFields[1].GetComponent<Animation>().Play();
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				GameManager.Instance.OFSubmitScore("811326", (int)scoreVal);
			}
			else if (GameManager.Instance.currentGameMode == GameMode.TIME)
			{
				GameManager.Instance.OFSubmitScore("811316", (int)scoreVal);
			}
		}
		StartCoroutine("delayedDestroy");
		GameObject[] array = GameObject.FindGameObjectsWithTag("Weather");
		GameObject[] array2 = array;
		foreach (GameObject obj in array2)
		{
			UnityEngine.Object.Destroy(obj);
		}
	}

	public IEnumerator delayedDestroy()
	{
		yield return new WaitForSeconds(1E-09f);
		Time.timeScale = 1f;
		hudAnimation.Play("AAR slide");
		yield return new WaitForSeconds(hudAnimation["AAR slide"].length);
		UnityEngine.Object.Destroy(GameObject.Find("Game Over(Clone)"));
		GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemy.Length; i++)
		{
			UnityEngine.Object.Destroy(enemy[i]);
		}
		(Resources.Load("Enemies") as Material).SetColor("_Color", Color.white);
		GameManager.Instance.resetPowerUps();
	}

	public IEnumerator TutorialStart()
	{
		tutorialMat.SetColor("_Color", new Color(1f, 1f, 1f, 0f));
		while (tutorialMat.color.a < 0.99f)
		{
			float alph = Mathf.Lerp(tutorialMat.color.a, 1f, 0.05f);
			tutorialMat.SetColor("_Color", new Color(255f, 255f, 255f, alph));
			yield return new WaitForSeconds(1E-10f);
		}
		yield return new WaitForSeconds(0.1f);
		while (tutorialMat.color.a > 0.01f)
		{
			float alph2 = Mathf.Lerp(tutorialMat.color.a, 0f, 0.05f);
			tutorialMat.SetColor("_Color", new Color(1f, 1f, 1f, alph2));
			yield return new WaitForSeconds(1E-10f);
		}
	}

	public IEnumerator tutorialStop()
	{
		while (tutorialMat.color.a > 0.01f)
		{
			float alph = Mathf.Lerp(tutorialMat.color.a, 0f, 0.05f);
			tutorialMat.SetColor("_Color", new Color(1f, 1f, 1f, alph));
			yield return new WaitForSeconds(1E-10f);
		}
	}

	public void ButtonClick(string name)
	{
		switch (name)
		{
		case "SensitivityBar":
			sensitivityBar.transform.position = new Vector3(raycastPosition.x, sensitivityBar.transform.position.y, sensitivityBar.transform.position.z);
			GameManager.Instance.controls.sensitivity = (raycastPosition.x - 103.4332f) * 0.2f + 1f;
			PlayerPrefs.SetFloat("sensitivity", GameManager.Instance.controls.sensitivity);
			break;
		case "pause":
			SoundController.Instance.playClip(buttonPressArr[UnityEngine.Random.Range(0, 2)]);
			GameManager.Instance.controls.disabled = true;
			pause();
			break;
		case "Pause_Resume":
			SoundController.Instance.playClip(buttonPressArr[UnityEngine.Random.Range(0, 2)]);
			GameManager.Instance.controls.disabled = false;
			resume();
			StartCoroutine("buttonPressed", name);
			break;
		case "sound":
			SoundController.Instance.playClip(buttonPressArr[UnityEngine.Random.Range(0, 2)]);
			SoundController.Instance.muteAll();
			if (SoundController.Instance.isAllOn())
			{
				soundLight.text = "\"";
			}
			else
			{
				soundLight.text = " ";
			}
			break;
		case "music":
			SoundController.Instance.playClip(buttonPressArr[UnityEngine.Random.Range(0, 2)]);
			SoundController.Instance.muteMusic();
			if (SoundController.Instance.isMusicOn())
			{
				musicLight.text = "\"";
			}
			else
			{
				musicLight.text = " ";
			}
			break;
		case "AAR_Pause MainMenu":
			Objectives.Instance.setCompletions = 1;
			GameManager.Instance.multiplier = 0f;
			GameManager.Instance.colorComboAmt = 1;
			StartCoroutine("buttonPressed", name);
			SoundController.Instance.playClip(buttonPressArr[UnityEngine.Random.Range(0, 2)]);
			if (GameManager.Instance.currentGameMode != GameMode.MENU)
			{
				StartCoroutine("SetupMainMenu");
			}
			else
			{
				mainMenuObjectiveOff();
			}
			GameManager.Instance.isPaused = false;
			break;
		case "Pause_Retry":
			SoundController.Instance.playClip(buttonPressArr[UnityEngine.Random.Range(0, 2)]);
			if (!retryMenu)
			{
				retryMenu = true;
				GameManager.Instance.enemySpeed = 1f;
				isRetry = true;
				ConfirmRetry();
			}
			break;
		case "AAR_Reheat":
			Objectives.Instance.setCompletions = 1;
			GameManager.Instance.multiplier = 0f;
			GameManager.Instance.colorComboAmt = 1;
			SoundController.Instance.playClip(buttonPressArr[UnityEngine.Random.Range(0, 2)]);
			PowerupManager.Instance.reset();
			if (GameManager.Instance.currentGameMode != GameMode.MENU)
			{
				GameManager.Instance.enemySpeed = 1f;
			}
			Reheat();
			resume();
			break;
		case "gameCenter":
			SoundController.Instance.playClip(buttonPressArr[UnityEngine.Random.Range(0, 2)]);
			if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform == RuntimePlatform.Android)
			{
				GameManager.Instance.OFOpenDashboard();
			}
			break;
		case "faceBookShare":
			SoundController.Instance.playClip(buttonPressArr[UnityEngine.Random.Range(0, 2)]);
			if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform == RuntimePlatform.Android)
			{
				FacebookManager.Instance.PostScoreOnWall("Bet you can't beat my score of " + scoreVal + " in Battle Bears BLAST!", "http://www.battlebears.com/storage/facebook/BB_BLAST_icon.jpg", "https://market.android.com/details?id=com.skyvu.blast", "Get Battle Bears BLAST on the Android Market", "for Android", "★★★★★ \"BLAST is a should buy for only $1. It's the perfect mobile arcade shooter, and truly redefines the genre.\" -AppleNApps.com ★★★★★ \"Battle Bears BLAST is a must play mobile game.\" -AppBattleground.com");
			}
			break;
		case "yes":
			SoundController.Instance.playClip(buttonPressArr[UnityEngine.Random.Range(0, 2)]);
			if (isRetry)
			{
				retryMenu = false;
				resume();
				Reheat();
				Objectives.Instance.setCompletions = 1;
				GameManager.Instance.multiplier = 0f;
				GameManager.Instance.colorComboAmt = 1;
			}
			else
			{
				StartCoroutine("SetupMainMenu");
			}
			PowerupManager.Instance.reset();
			UnityEngine.Object.Destroy(textFields[1].gameObject.GetComponent<Collider>());
			UnityEngine.Object.Destroy(textFields[3].gameObject.GetComponent<Collider>());
			GameManager.Instance.controls.disabled = false;
			break;
		case "no":
			retryMenu = false;
			SoundController.Instance.playClip(buttonPressArr[UnityEngine.Random.Range(0, 2)]);
			UnityEngine.Object.Destroy(textFields[1].gameObject.GetComponent<Collider>());
			UnityEngine.Object.Destroy(textFields[3].gameObject.GetComponent<Collider>());
			//textFields[3].gameObject.active = false;
			textFields[3].gameObject.SetActive(false);
			dynamicText(Display.OBJECTIVE, string.Empty);
			break;
		case "SensitivityDown":
			if (GameManager.Instance.controls.sensitivity > 6f)
			{
				GameManager.Instance.controls.sensitivity -= 1f;
			}
			break;
		case "SensitivityUp":
			if (GameManager.Instance.controls.sensitivity < 12f)
			{
				GameManager.Instance.controls.sensitivity += 1f;
			}
			break;
		}
	}

	public void mainMenu()
	{
		if ((bool)clearedAnimationActive)
		{
			UnityEngine.Object.Destroy(clearedAnimationActive);
		}
		setTheBest();
		Time.timeScale = 1f;
		StopAllCoroutines();
		StopCoroutine("scrollingText");
		GameManager.Instance.playerController.life = 3f;
		if (GameManager.Instance.helmLoadOut == Helm.SAMURAI)
		{
			GameManager.Instance.playerController.isSamuraiHat = true;
		}
		GameManager.Instance.currentGameMode = GameMode.MENU;
		//Application.LoadLevelAsync("MainMenu");
		SceneManager.LoadSceneAsync("MainMenu");
	}

	public void Reheat()
	{
		Objectives.Instance.setIndex = 1;
		Objectives.Instance.reSet(false);
		didGetPair = false;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i]);
		}
		EnemySpawner.Instance.reset();
		//pauseAar.SetActiveRecursively(false);
		pauseAar.SetActive(false);
		tacomOnline();
		GameManager.Instance.isOver = false;
		GameManager.Instance.isWilOut = false;
		GameManager.Instance.playerController.life = 3f;
		if (GameManager.Instance.helmLoadOut == Helm.SAMURAI)
		{
			GameManager.Instance.playerController.isSamuraiHat = true;
		}
		GameManager.Instance.arcadeTime = 60f;
		spawnDelay = 5.75f;
		Time.timeScale = 1f;
		GameManager.Instance.controls.disabled = false;
		GameManager.Instance.colorComboAmt = 1;
		GameManager.Instance.multiplier = 0f;
		GameManager.Instance.StartCoroutine("resetStats");
		GC.Collect();
		Resources.UnloadUnusedAssets();
		StopCoroutine("TimeAttackGameOver");
	}

	public string CreateStringBounds(string str)
	{
		string empty = string.Empty;
		string text = string.Empty;
		string[] array = str.Split('\n');
		empty = array[0] + "\n";
		if (array.Length <= 1)
		{
			return string.Empty;
		}
		array = array[1].Split('/');
		for (int i = 0; i < array.Length; i++)
		{
			if (text.Length + array[i].Length > 22)
			{
				empty = empty + text + "\n/";
				text = array[i] + "/";
			}
			else
			{
				text = text + array[i] + "/";
			}
		}
		return empty + text;
	}

	public void powerCounter(Nuggs type)
	{
		switch (type)
		{
		case Nuggs.FREEZE:
			break;
		case Nuggs.MULTIPLIER:
			break;
		case Nuggs.POINTS:
		case Nuggs.SCREEN:
			break;
		}
	}

	public IEnumerator powerUpFormat(bool refresh, Nuggs type)
	{
		string power;
		switch (type)
		{
		case Nuggs.FREEZE:
			power = "%";
			break;
		case Nuggs.MULTIPLIER:
			power = "-";
			break;
		default:
			yield break;
		}
		if (!refresh)
		{
			powerUpIndex++;
			//powerUps[powerUpIndex].gameObject.active = true;
			powerUps[powerUpIndex].gameObject.SetActive(true);
		}
		float scale = 1f;
		int loops = 22;
		while (loops > 0 && !GameManager.Instance.isOver)
		{
			powerUps[powerUpIndex].text = power;
			if (loops > 17)
			{
				scale = Mathf.Lerp(powerUps[powerUpIndex].transform.localScale.x, 0.31f, 0.2f);
			}
			else if (loops > 13)
			{
				scale = Mathf.Lerp(powerUps[powerUpIndex].transform.localScale.x, 0.25f, 0.3f);
			}
			else if (loops > 9)
			{
				scale = Mathf.Lerp(powerUps[powerUpIndex].transform.localScale.x, 0.31f, 0.3f);
			}
			else if (loops > 4)
			{
				scale = Mathf.Lerp(powerUps[powerUpIndex].transform.localScale.x, 0.25f, 0.3f);
			}
			powerUps[powerUpIndex].transform.localScale = Vector3.one * scale;
			yield return new WaitForSeconds(0.1f);
			loops--;
		}
	}

	private void Update()
	{
		if (!Application.isEditor)
		{
			if (Input.touchCount > 0)
			{
				touchToRay = hudCam.ScreenPointToRay(Input.GetTouch(0).position);
				hasHitObj = Physics.Raycast(touchToRay, out outHit, float.PositiveInfinity);
				Touch[] touches = Input.touches;
				for (int i = 0; i < touches.Length; i++)
				{
					Touch touch = touches[i];
					if (hasHitObj)
					{
						raycastPosition = outHit.point;
					}
					if (touch.phase == TouchPhase.Began)
					{
						if (hasHitObj)
						{
							firstObjectTouched = outHit.collider.gameObject;
							if (outHit.collider.name == "TextFieldsCollider")
							{
								aarScroll = true;
							}
						}
					}
					else
					{
						if (touch.phase == TouchPhase.Stationary)
						{
							continue;
						}
						if (touch.phase == TouchPhase.Moved)
						{
							if (outHit.collider != null && outHit.collider.gameObject.name == "SensitivityBar")
							{
								ButtonClick(outHit.collider.gameObject.name);
							}
						}
						else if (touch.phase != TouchPhase.Canceled && touch.phase == TouchPhase.Ended)
						{
							if (hasHitObj && outHit.collider.name != "TextFieldsCollider" && Time.realtimeSinceStartup > realTime + 0.25f && !aarScroll && firstObjectTouched != null && firstObjectTouched.name == outHit.collider.gameObject.name)
							{
								ButtonClick(outHit.collider.name);
								outHit.collider.SendMessage("skip", SendMessageOptions.DontRequireReceiver);
							}
							firstObjectTouched = null;
						}
					}
				}
				if (aarScroll && currDisplay == Display.AAR)
				{
					aarText.transform.localPosition += Vector3.forward * Input.GetTouch(0).deltaPosition.y * 0.05f;
					if (aarText.transform.localPosition.z > (textFields[formatIndex].transform.localPosition.z - textFields[0].transform.localPosition.z) * -1f + 4.5f)
					{
						aarText.transform.localPosition = new Vector3(aarText.transform.localPosition.x, aarText.transform.transform.localPosition.y, (textFields[formatIndex].transform.localPosition.z - textFields[0].transform.localPosition.z) * -1f + 4.5f);
					}
					if (aarText.transform.localPosition.z < 4.5f)
					{
						aarText.transform.localPosition = new Vector3(aarText.transform.localPosition.x, aarText.transform.transform.localPosition.y, 4.49f);
					}
				}
			}
			else
			{
				aarScroll = false;
			}
		}
		else if (Input.GetMouseButtonDown(0))
		{
			touchToRay = hudCam.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
			if (hasHitObj = Physics.Raycast(touchToRay, out outHit, float.PositiveInfinity))
			{
				raycastPosition = outHit.point;
				ButtonClick(outHit.collider.gameObject.name);
				outHit.collider.SendMessage("skip", SendMessageOptions.DontRequireReceiver);
			}
		}
		if (!canStart || !time.gameObject.active || !(Time.realtimeSinceStartup > realTime + 1f / GameManager.Instance.enemySpeed))
		{
			return;
		}
		if (!didGetPair)
		{
			Objectives.Instance.getPair();
			didGetPair = true;
		}
		GameManager.Instance.arcadeTime -= 1f;
		realTime = Time.realtimeSinceStartup;
		switch (GameManager.Instance.arcadeTime.ToString())
		{
		case "9":
			time.text = "lu";
			return;
		case "8":
			time.text = "lt";
			return;
		case "7":
			time.text = "ls";
			return;
		case "6":
			time.text = "lr";
			return;
		case "5":
			SoundController.Instance.playClip(Resources.Load("timer1") as AudioClip);
			time.text = "lq";
			return;
		case "4":
			SoundController.Instance.playClip(Resources.Load("timer2") as AudioClip);
			time.text = "lp";
			return;
		case "3":
			SoundController.Instance.playClip(Resources.Load("timer3") as AudioClip);
			time.text = "lo";
			return;
		case "2":
			SoundController.Instance.playClip(Resources.Load("timer4") as AudioClip);
			time.text = "ln";
			return;
		case "1":
			SoundController.Instance.playClip(Resources.Load("timer5") as AudioClip);
			time.text = "lm";
			return;
		case "0":
			time.text = "ll";
			StartCoroutine("TimeAttackGameOver");
			return;
		case "-1":
			time.text = "ll";
			return;
		}
		if (GameManager.Instance.arcadeTime > 1f)
		{
			time.text = GameManager.Instance.arcadeTime.ToString();
		}
		else
		{
			time.text = "ll";
		}
	}

	public void setUpdater(int pairNum, string first, string second, int setNum)
	{
		ResetPairs();
		setCompleteScroll = false;
		TextMesh[] array = new TextMesh[2];
		GameObject[] array2 = new GameObject[4];
		switch (pairNum)
		{
		case 1:
			array = pairOne;
			array2[0] = array[0].gameObject;
			array2[1] = array[1].gameObject;
			array2[2] = pairFour[0].gameObject;
			array2[3] = pairFour[1].gameObject;
			break;
		case 2:
			array = pairTwo;
			array2[0] = array[0].gameObject;
			array2[1] = array[1].gameObject;
			array2[2] = pairOne[0].gameObject;
			array2[3] = pairOne[1].gameObject;
			break;
		case 3:
			array = pairThree;
			array2[0] = array[0].gameObject;
			array2[1] = array[1].gameObject;
			array2[2] = pairTwo[0].gameObject;
			array2[3] = pairTwo[1].gameObject;
			break;
		case 4:
			array = pairFour;
			array2[0] = array[0].gameObject;
			array2[1] = array[1].gameObject;
			array2[2] = pairThree[0].gameObject;
			array2[3] = pairThree[1].gameObject;
			break;
		case 5:
			array = pairFive;
			pairFive[0].characterSize = 0.68f;
			pairFive[1].characterSize = 0.68f;
			array2[0] = array[0].gameObject;
			array2[1] = array[1].gameObject;
			array2[2] = pairFour[0].gameObject;
			array2[3] = pairFour[1].gameObject;
			break;
		default:
			array = pairOne;
			break;
		}
		for (int i = 0; i < array.Length; i++)
		{
			//array[i].gameObject.active = true;
			array[i].gameObject.SetActive(true);
		}
		array[0].text = first;
		array[1].text = second;
		pairIndex = pairNum;
		currentSet = pairNum;
		challengeSet.text = numToDigit(pairNum, 2, true);
		if ((bool)array2[0])
		{
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].transform.localScale = new Vector3(0f, 0f, 0f);
			}
			StopCoroutine("PairSwitch");
			StartCoroutine("PairSwitch", array2);
			return;
		}
		pairOne[0].text = string.Empty;
		pairOne[1].text = string.Empty;
		pairTwo[0].text = string.Empty;
		pairTwo[1].text = string.Empty;
		pairThree[0].text = string.Empty;
		pairThree[1].text = string.Empty;
		pairFour[0].text = string.Empty;
		pairFour[1].text = string.Empty;
		pairFive[0].text = string.Empty;
		pairFive[1].text = string.Empty;
	}

	public void ResetPairs()
	{
		pairOne[0].gameObject.active = false;
		pairOne[1].gameObject.active = false;
		pairTwo[0].gameObject.active = false;
		pairTwo[1].gameObject.active = false;
		pairThree[0].gameObject.active = false;
		pairThree[1].gameObject.active = false;
		pairFour[0].gameObject.active = false;
		pairFour[1].gameObject.active = false;
	}

	public IEnumerator setScroll()
	{
		isScrolling = true;
		string oldScore = score.text;
		string[] textToScroll = new string[17]
		{
			"xxxxxxx",
			"yyyyyyS",
			"vvvvvSE",
			"wwwwSET",
			"xxxSET/",
			"yySET/" + numToDigit(currentSet, 2, true).Substring(0, 1),
			"vSET/" + numToDigit(currentSet, 2, true),
			"SET/" + numToDigit(currentSet, 2, true) + "w",
			"ET/" + numToDigit(currentSet, 2, true) + "xx",
			"T/" + numToDigit(currentSet, 2, true) + "yyy",
			"/" + numToDigit(currentSet, 2, true) + "vvvv",
			numToDigit(currentSet, 2, true) + "wwwww",
			numToDigit(currentSet, 2, true).Substring(1, 1) + "xxxxxx",
			"yyyyyyy",
			"vvvvvvv",
			oldScore,
			"///////"
		};
		for (int i = 0; i < textToScroll.Length; i++)
		{
			score.text = textToScroll[i];
			yield return new WaitForSeconds(0.25f);
		}
		setCompleteScroll = true;
		isScrolling = false;
		score.text = numToDigit(scoreVal, 7, true);
		StartCoroutine("setCompleteEffect");
	}

	public IEnumerator setCompleteEffect()
	{
		bool blink = true;
		int index2 = 4;
		TextMesh[] myPair = new TextMesh[2];
		bool toggle = false;
		bool neg = false;
		while (setCompleteScroll)
		{
			if (blink)
			{
				switch (index2)
				{
				case 1:
					myPair = pairOne;
					index2 = 6;
					break;
				case 2:
					myPair = pairTwo;
					break;
				case 3:
					myPair = pairThree;
					break;
				case 4:
					myPair = pairFour;
					break;
				case 5:
					myPair = pairFive;
					break;
				default:
					if (UnityEngine.Random.Range(1, 100) % 2 == 0)
					{
						myPair = pairFour;
						index2 = 4;
					}
					else
					{
						blink = false;
					}
					index2 = 3;
					break;
				}
				for (int j = 0; j < myPair.Length; j++)
				{
					pairOne[j].gameObject.active = true;
					pairTwo[j].gameObject.active = true;
					pairThree[j].gameObject.active = true;
					pairFour[j].gameObject.active = true;
					pairFive[j].gameObject.active = true;
					myPair[j].gameObject.active = false;
				}
				index2--;
			}
			else
			{
				switch (index2)
				{
				case 1:
					myPair = pairOne;
					index2 = 6;
					break;
				case 2:
					myPair = pairTwo;
					break;
				case 3:
					myPair = pairThree;
					break;
				case 4:
					myPair = pairFour;
					neg = true;
					index2 = 4;
					break;
				default:
					if (UnityEngine.Random.Range(1, 100) % 2 == 0)
					{
						myPair = pairFour;
						index2 = 4;
					}
					else
					{
						blink = true;
					}
					break;
				}
				for (int i = 0; i < myPair.Length; i++)
				{
					myPair[i].gameObject.active = toggle;
				}
				if (!neg)
				{
					index2++;
					toggle = false;
				}
				else
				{
					index2--;
					toggle = true;
				}
			}
			yield return new WaitForSeconds(0.25f);
		}
	}

	public void clearSets()
	{
	}

	public void ConfirmRetry()
	{
		formatIndex = 0;
		currDisplay = Display.NONE;
		textFields[0].characterSize = 1.952f;
		textFields[0].lineSpacing = 0.65f;
		textFields[0].transform.localPosition = new Vector3(-6.199f, 0.65f, 0.2676321f);
		if (isRetry)
		{
			textFields[0].text = "ARE/YOU/SUREk";
		}
		else
		{
			textFields[0].text = "/QUIT/GAMEk";
		}
		textFields[1].characterSize = 3f;
		textFields[1].lineSpacing = 0.65f;
		textFields[1].transform.localPosition = new Vector3(-7.7f, 0.65f, -0.57f);
		textFields[1].text = "/YES";
		BoxCollider boxCollider = textFields[1].gameObject.AddComponent<BoxCollider>();
		boxCollider.center = new Vector3(36f, -11f, 0f);
		textFields[1].gameObject.name = "yes";
		textFields[2].characterSize = 3f;
		textFields[2].lineSpacing = 0.65f;
		textFields[2].transform.localPosition = new Vector3(-1.79f, 0.65f, -0.57f);
		textFields[2].text = "/y/";
		//textFields[3].gameObject.active = true;
		textFields[3].gameObject.SetActive(true);
		textFields[3].text = "NO/";
		textFields[3].characterSize = 3f;
		textFields[3].lineSpacing = 0.65f;
		textFields[3].transform.localPosition = new Vector3(2.47f, 0.65f, -0.57f);
		BoxCollider boxCollider2 = textFields[3].gameObject.AddComponent<BoxCollider>();
		boxCollider2.center = new Vector3(15f, -11f, 0f);
		textFields[3].gameObject.name = "no";
	}

	public void mainMenuObjective()
	{
		retryMenu = false;
		//HUD.SetActiveRecursively(false);
		HUD.SetActive(false);
		//pauseAar.SetActiveRecursively(true);
		pauseAar.SetActive(true);
		for (int i = 0; i < textFields.Length; i++)
		{
			//textFields[i].gameObject.active = false;
			textFields[i].gameObject.SetActive(false);
		}
		//gameCenterBtn.active = true;
		gameCenterBtn.SetActive(true);
		//faceBookBtn.active = true;
		faceBookBtn.SetActive(true);
		//pauseResumeBtn.active = false;
		pauseResumeBtn.SetActive(false);
		//reheatBtn.active = false;
		reheatBtn.SetActive(false);
		//retryBtn.active = false;
		retryBtn.SetActive(false);
		dynamicText(Display.OBJECTIVE, string.Empty);
		GameManager.Instance.controls.disabled = true;
	}

	public void mainMenuObjectiveOff()
	{
		StartCoroutine("mainMenuObjectiveOffBam");
	}

	public IEnumerator mainMenuObjectiveOffBam()
	{
		yield return new WaitForSeconds(0.1f);
		//pauseAar.SetActiveRecursively(false);
		pauseAar.SetActive(false);
		for (int i = 0; i < textFields.Length; i++)
		{
			//textFields[i].gameObject.active = false;
			textFields[i].gameObject.SetActive(false);
		}
		StartCoroutine("controlsWait");
	}

	public IEnumerator controlsWait()
	{
		yield return new WaitForSeconds(0.1f);
		GameManager.Instance.controls.disabled = false;
	}

	public IEnumerator EndBeep()
	{
		if (SoundController.Instance.effectsAudio.volume != 0f)
		{
			SoundController.Instance.effectsAudio.volume = 1f;
			SoundController.Instance.PlayMySound("o_hurt_last");
			yield return new WaitForSeconds(1.38f);
			SoundController.Instance.PlayMySound("Abbi Sounds/abbi_timesup");
			yield return new WaitForSeconds(0.85f);
			SoundController.Instance.effectsAudio.volume = 0.75f;
		}
	}

	public IEnumerator TimeAttackGameOver()
	{
		if (!GameManager.Instance.isOver)
		{
			GameManager.Instance.isOver = true;
			StartCoroutine("EndBeep");
			GameManager.Instance.enemySpeed = 1E-05f;
			GameManager.Instance.controls.disabled = true;
			timeUpActive = (GameObject)UnityEngine.Object.Instantiate(timeUp);
			yield return new WaitForSeconds(timeUpActive.GetComponent<Animation>().clip.length);
			if ((bool)timeUpActive)
			{
				UnityEngine.Object.Destroy(timeUpActive);
			}
			Objectives.Instance.setCompletions = 0;
			AAR();
		}
	}

	public IEnumerator ClearSet()
	{
		SoundController.Instance.PlayMySound("achievement_earned");
		GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject g in array)
		{
			g.SendMessage("death", SendMessageOptions.DontRequireReceiver);
		}
		int temp = GameManager.Instance.maxEnemies;
		GameManager.Instance.maxEnemies = 0;
		pauseTimer = true;
		clearedAnimationActive = (GameObject)UnityEngine.Object.Instantiate(clearedAnimation);
		clearScript = clearedAnimationActive.GetComponent<ClearedAnimationHandler>();
		//clearScript.nextSet.active = false;
		clearScript.nextSet.SetActive(false);
		for (int j = 0; j < 3; j++)
		{
			//clearScript.count[j].active = false;
			clearScript.count[j].SetActive(false);
		}
		yield return new WaitForSeconds(2f);
		if ((bool)clearedAnimationActive)
		{
			//clearScript.nextSet.active = true;
			clearScript.nextSet.SetActive(true);
		}
		for (int i = 2; i >= 0; i--)
		{
			if ((bool)clearedAnimationActive)
			{
				SoundController.Instance.playClip(buttonPressArr[2]);
				if (i + 1 < clearScript.count.Length)
				{
					//clearScript.count[i + 1].active = false;
					clearScript.count[i + 1].SetActive(false);
				}
				//clearScript.count[i].active = true;
				clearScript.count[i].SetActive(true);
				yield return new WaitForSeconds(1.1f);
			}
		}
		SoundController.Instance.playClip(Resources.Load("target_select_end") as AudioClip);
		if ((bool)clearedAnimationActive)
		{
			UnityEngine.Object.Destroy(clearedAnimationActive);
		}
		GameManager.Instance.maxEnemies = temp;
		pauseTimer = false;
	}

	public IEnumerator SetupMainMenu()
	{
		Time.timeScale = 1f;
		SoundController.Instance.FadeMusic(true);
		yield return new WaitForSeconds(1.1f);
		mainMenu();
		GameManager.Instance.enemySpeed = 1f;
	}

	public IEnumerator GameOver()
	{
		GameManager.Instance.isOver = true;
		SoundController.Instance.PlayMySound("o_hurt_last");
		yield return new WaitForSeconds(1.5f);
		SoundController.Instance.PlayMySound("Abbi Sounds/abbi_shotwil");
		GameManager.Instance.enemySpeed = 0f;
		GameManager.Instance.controls.disabled = true;
		GameObject go = UnityEngine.Object.Instantiate(gameOverText) as GameObject;
		go.GetComponent<Animation>().Play();
		yield return new WaitForSeconds(go.GetComponent<Animation>().clip.length * 2f);
		AAR();
	}

	public void demoEntro()
	{
		if (GameManager.Instance.currentGameMode == GameMode.TIME)
		{
			base.GetComponent<Animation>().Play("TimeAttack");
		}
		else if (GameManager.Instance.currentGameMode == GameMode.SCORE)
		{
			base.GetComponent<Animation>().Play("ScoreAttack");
		}
		else if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			base.GetComponent<Animation>().Play("Survival_Slide");
		}
	}

	public void turnCrosshairsFire()
	{
		//crosshairsFire.active = true;
		crosshairsFire.SetActive(true);
		//crosshairsSafe.active = false;
		crosshairsSafe.SetActive(false);
	}

	public void turnCrosshairsSafe()
	{
		//crosshairsFire.active = false;
		crosshairsFire.SetActive(false);
		//crosshairsSafe.active = true;
		crosshairsSafe.SetActive(true);
	}

	public void turnCrosshairsOff()
	{
		//crosshairsSafe.SetActiveRecursively(false);
		crosshairsSafe.SetActive(false);
	}

	public void moveCrosshairs(Vector2 moveAmount)
	{
		crosshairsSafe.transform.localPosition = new Vector3(moveAmount.x * -1f, moveAmount.y, 18.14152f);
		if (crosshairsSafe.transform.localPosition.x > 3.5766f)
		{
			crosshairsSafe.transform.localPosition = new Vector3(3.5766f, crosshairsSafe.transform.localPosition.y, 0f);
		}
		if (crosshairsSafe.transform.localPosition.x < -3.5766f)
		{
			crosshairsSafe.transform.localPosition = new Vector3(-3.5766f, crosshairsSafe.transform.localPosition.y, 0f);
		}
		if (crosshairsSafe.transform.localPosition.y > 5.95f)
		{
			crosshairsSafe.transform.localPosition = new Vector3(crosshairsSafe.transform.localPosition.x, 5.95f, 0f);
		}
		if ((double)crosshairsSafe.transform.localPosition.y < -2.55)
		{
			crosshairsSafe.transform.localPosition = new Vector3(crosshairsSafe.transform.localPosition.x, -2.55f, 0f);
		}
	}

	public IEnumerator scaleCrosshairs()
	{
		SoundController.Instance.playClip(Resources.Load("target_appear") as AudioClip);
		crosshairsSafe.transform.localPosition = Vector3.zero;
		turnCrosshairsSafe();
		crosshairsSafe.transform.localScale = Vector3.one * 20f;
		for (int loops = 28; loops > 0; loops--)
		{
			float myScale = Mathf.Lerp(crosshairsSafe.transform.localScale.x, 1f, 0.2f);
			crosshairsSafe.transform.localScale = Vector3.one * myScale;
			yield return new WaitForSeconds(0.005f);
		}
		crosshairsSafe.transform.localScale = Vector3.one;
	}

	public IEnumerator PairSwitch(GameObject[] arr)
	{
		bool isFinished = true;
		for (int j = 0; j < arr.Length; j++)
		{
			arr[j].transform.Rotate(Vector3.up * 40f, Space.World);
			if (arr[j].transform.localScale.x < iconScale.x - 0.01f || arr[j].transform.localScale.y < iconScale.y - 0.01f || arr[j].transform.localScale.z < iconScale.z - 0.01f)
			{
				isFinished = false;
				arr[j].transform.localScale = new Vector3(Mathf.Lerp(arr[j].transform.localScale.x, iconScale.x, 0.3f), Mathf.Lerp(arr[j].transform.localScale.y, iconScale.y, 0.3f), Mathf.Lerp(arr[j].transform.localScale.z, iconScale.z, 0.3f));
			}
		}
		if (!isFinished)
		{
			yield return new WaitForSeconds(0.05f);
			StartCoroutine("PairSwitch", arr);
			yield break;
		}
		for (int i = 0; i < arr.Length; i++)
		{
			arr[i].transform.eulerAngles = new Vector3(90f, 0f, 0f);
		}
	}

	public IEnumerator buttonPressed(string s)
	{
		GameObject obj = GameObject.Find(s + "/ButtonPressGradient");
		MeshRenderer meshRender = obj.GetComponent<MeshRenderer>();
		Animation ani = obj.GetComponent<Animation>();
		meshRender.enabled = true;
		ani.Play();
		yield return new WaitForSeconds(ani.clip.length + 0.1f);
		meshRender.enabled = false;
	}

	public void Facebook()
	{
	}

	public void androidMenu()
	{
		StopCoroutine("androidMenuEnumerator");
		StartCoroutine("androidMenuEnumerator");
	}

	public IEnumerator androidMenuEnumerator()
	{
		yield return new WaitForSeconds(0.2f);
		SoundController.Instance.playClip(buttonPressArr[UnityEngine.Random.Range(0, 2)]);
		if (GameManager.Instance.currentGameMode == GameMode.MENU)
		{
			if (!GameManager.Instance.isPaused)
			{
				mainMenuObjective();
				GameManager.Instance.isPaused = true;
				GameManager.Instance.controls.disabled = true;
			}
			else
			{
				mainMenuObjectiveOff();
				GameManager.Instance.isPaused = false;
				GameManager.Instance.controls.disabled = false;
			}
		}
		else if (GameManager.Instance.currentGameMode != GameMode.MENU || GameManager.Instance.currentGameMode != GameMode.BACKPACK)
		{
			if (!GameManager.Instance.isPaused)
			{
				pause();
			}
			else
			{
				resume();
			}
		}
	}
}
