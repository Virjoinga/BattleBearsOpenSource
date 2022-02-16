using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectives : MonoBehaviour
{
	public GoalType goalOne;

	public GoalType goalTwo;

	public GoalType goalThree;

	public GoalType goalFour;

	private int objectiveIndex;

	private float scoreAmt;

	private float scoreAmtTwo;

	private float killComboNumber;

	private float detonationsAmt;

	private float detonationsIndex;

	private int setAmount;

	public int setIndex;

	private Enemy beemType;

	private float killComboIndex;

	private float blueCombo;

	private float greenCombo;

	private float orangeCombo;

	private float pinkCombo;

	private float purpleCombo;

	private float redCombo;

	private float yellowCombo;

	private Enemy comboTarget;

	private Enemy comboTarget2;

	private Enemy comboTarget3;

	private string noHit;

	private bool hit;

	private string burnTarget;

	private string burnTargetTwo;

	private bool burn;

	private Enemy[] Order;

	private int bakeCount;

	private int bakeGoal;

	private int explosionIndex;

	private int explosionAmt;

	public Stack<Enemy> comboStack;

	private string bakeTarget;

	private bool[] nuggTypes;

	[HideInInspector]
	public Enemy setOne;

	private Enemy oneBackup;

	private Enemy lastKilledHuggable;

	[HideInInspector]
	public Enemy setTwo;

	public bool[] setList;

	public int pairIndex;

	private int[] setOrder;

	public int setCompletions;

	private Enemy limitTarget;

	private int limitAmount;

	private int limitTime;

	public int limitIndex;

	public AudioClip achievement_earned;

	private bool condition;

	private string lastEnemy;

	private int burnAmt;

	private int burnIndex;

	private int orderIndex;

	private int duration;

	private int howMany;

	private int keepIndex;

	private int bakeAmt;

	private int bakeTime;

	private int bakeIndex;

	private bool shouldUpdate;

	private bool whiting;

	private int bombIndex;

	private float ratioAmt;

	private float exactAmt;

	private int motionDuration;

	private static Objectives instance;

	private bool isStarted;

	public static Objectives Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		Object.DontDestroyOnLoad(this);
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		setCompletions = 1;
		if (!PlayerPrefs.HasKey("objectives"))
		{
			PlayerPrefs.SetInt("objectives", 1);
			PlayerPrefs.SetInt("goal1", 0);
			PlayerPrefs.SetInt("goal2", 0);
			PlayerPrefs.SetInt("goal3", 0);
		}
		objectiveIndex = PlayerPrefs.GetInt("objectives");
		bakeTarget = string.Empty;
		nuggTypes = new bool[3];
		setList = new bool[8];
		setOrder = new int[8];
		setIndex = 1;
		newObjectiveGet(true);
	}

	public void goalComplete(string goal)
	{
		PlayerPrefs.SetInt(goal, 1);
		if (goalOne == GoalType.NONE)
		{
			PlayerPrefs.SetInt("goal1", 1);
		}
		if (goalTwo == GoalType.NONE)
		{
			PlayerPrefs.SetInt("goal2", 1);
		}
		if (goalThree == GoalType.NONE)
		{
			PlayerPrefs.SetInt("goal3", 1);
		}
		if (PlayerPrefs.GetInt("goal1") != 0 && PlayerPrefs.GetInt("goal2") != 0 && PlayerPrefs.GetInt("goal3") != 0)
		{
			objectiveComplete();
		}
	}

	public void objectiveComplete()
	{
		goalOne = GoalType.NONE;
		goalTwo = GoalType.NONE;
		goalThree = GoalType.NONE;
		PlayerPrefs.SetInt("goal1", 0);
		PlayerPrefs.SetInt("goal2", 0);
		PlayerPrefs.SetInt("goal3", 0);
		if (PlayerPrefs.GetInt("objectives") == 10)
		{
			objectiveTen();
		}
		newObjectiveGet(false);
		bool flag = false;
		for (int i = 0; i < GameManager.Instance.objectiveItemUnlocks.Length; i++)
		{
			if (PlayerPrefs.GetInt("objectives") - 1 == GameManager.Instance.objectiveItemUnlocks[i])
			{
				flag = true;
			}
		}
	}

	public void newObjectiveGet(bool isFresh)
	{
		hit = false;
		noHit = string.Empty;
		bakeTarget = string.Empty;
		burn = false;
		burnTarget = string.Empty;
		condition = false;
		Order = new Enemy[1];
		shouldUpdate = false;
		for (int i = 0; i < nuggTypes.Length; i++)
		{
			nuggTypes[i] = false;
		}
		if (!isFresh)
		{
			PlayerPrefs.SetInt("objectives", objectiveIndex + 1);
			for (int j = 1; j < PlayerPrefs.GetInt("objectives"); j++)
			{
				GameManager.Instance.getAchievement("objective" + j, 100f);
			}
			SoundController.Instance.playClip(achievement_earned);
			StartCoroutine("DelayedPlayAudio");
		}
		switch (PlayerPrefs.GetInt("objectives"))
		{
		case 1:
			goalOne = GoalType.SCORE;
			scoreAmt = 300f;
			beemType = Enemy.NONE;
			break;
		case 2:
			goalOne = GoalType.COMBO;
			killComboNumber = 1f;
			comboTarget = Enemy.PIN;
			goalTwo = GoalType.BAKE;
			bakeTarget = "Nuggs";
			break;
		case 3:
			goalOne = GoalType.BAKE;
			bakeTarget = "wil";
			goalTwo = GoalType.SET;
			setAmount = 2;
			setIndex = 1;
			break;
		case 4:
			goalOne = GoalType.SCORE;
			scoreAmt = 15000f;
			noHit = "wil";
			beemType = Enemy.NONE;
			goalTwo = GoalType.COMBO;
			comboTarget = Enemy.PIN;
			comboTarget2 = Enemy.BLU;
			killComboNumber = 2f;
			break;
		case 5:
			goalOne = GoalType.SET;
			setAmount = 3;
			setIndex = 1;
			goalTwo = GoalType.ORDER;
			Order = new Enemy[4];
			Order[0] = Enemy.PIN;
			Order[1] = Enemy.GRN;
			Order[2] = Enemy.YEL;
			Order[3] = Enemy.ORG;
			break;
		case 6:
			goalOne = GoalType.BAKE;
			comboTarget = Enemy.RED;
			comboTarget2 = Enemy.GRN;
			killComboNumber = 7f;
			killComboIndex = 0f;
			goalTwo = GoalType.BAKE;
			goalThree = GoalType.DETONATIONS;
			detonationsAmt = 5f;
			detonationsIndex = 0f;
			break;
		case 7:
			goalOne = GoalType.BAKE;
			bakeTarget = string.Empty;
			bakeGoal = 3;
			goalTwo = GoalType.EXPLOSION;
			explosionAmt = 20;
			break;
		case 8:
			goalOne = GoalType.SCORE;
			scoreAmt = 35000f;
			beemType = Enemy.RED;
			goalTwo = GoalType.SCORECONDITION;
			scoreAmtTwo = 25000f;
			break;
		case 9:
			goalOne = GoalType.ORDER;
			Order = new Enemy[6];
			Order[0] = Enemy.RED;
			Order[1] = Enemy.ORG;
			Order[2] = Enemy.YEL;
			Order[3] = Enemy.GRN;
			Order[4] = Enemy.BLU;
			Order[5] = Enemy.PUR;
			goalTwo = GoalType.SET;
			beemType = Enemy.ORG;
			setAmount = 4;
			goalThree = GoalType.BAKE;
			bakeAmt = 5;
			bakeTime = 3;
			bakeIndex = 0;
			break;
		case 10:
			goalOne = GoalType.SCORE;
			objectiveTen();
			shouldUpdate = true;
			break;
		case 11:
			goalOne = GoalType.SET;
			beemType = Enemy.NONE;
			setAmount = 5;
			setIndex = 1;
			goalTwo = GoalType.BAKE;
			bakeTarget = "NuggsWhiteOut";
			break;
		case 12:
			goalOne = GoalType.ORDER;
			Order = new Enemy[10];
			Order[0] = Enemy.WHI;
			Order[1] = Enemy.WHI;
			Order[2] = Enemy.WHI;
			Order[3] = Enemy.WHI;
			Order[4] = Enemy.WHI;
			Order[5] = Enemy.WHI;
			Order[6] = Enemy.WHI;
			Order[7] = Enemy.WHI;
			Order[8] = Enemy.WHI;
			Order[9] = Enemy.WHI;
			break;
		case 13:
			goalOne = GoalType.NUGGSMULTI;
			break;
		case 14:
			goalOne = GoalType.LEET;
			break;
		case 15:
			goalOne = GoalType.SCORE;
			scoreAmt = 45000f;
			beemType = Enemy.YEL;
			goalTwo = GoalType.ORDER;
			Order = new Enemy[4];
			Order[0] = Enemy.YEL;
			Order[1] = Enemy.YEL;
			Order[2] = Enemy.YEL;
			Order[3] = Enemy.YEL;
			break;
		case 16:
			goalOne = GoalType.LIMIT;
			limitTarget = Enemy.PUR;
			beemType = Enemy.PUR;
			limitIndex = 0;
			limitAmount = 10;
			goalTwo = GoalType.RATIO;
			ratioAmt = 500f;
			break;
		case 17:
			goalOne = GoalType.SCORE;
			scoreAmt = 60000f;
			beemType = Enemy.NONE;
			break;
		}
		objectiveIndex = PlayerPrefs.GetInt("objectives");
	}

	public string textInfo(int goalNum)
	{
		string result = string.Empty;
		switch (PlayerPrefs.GetInt("objectives"))
		{
		case 1:
			if (goalNum == 1)
			{
				result = "GOAL/A.\n/SCORE/300/POINTS";
			}
			break;
		case 2:
			if (goalNum == 1)
			{
				result = "GOAL/A.////////////\n/GET/A/2/:/COMBO";
			}
			if (goalNum == 2)
			{
				result = "GOAL/B.\n/BAKE/A/NUGGS/";
			}
			break;
		case 3:
			switch (goalNum)
			{
			case 1:
				result = "GOAL/A.\n/ACCIDENTALLY/BLOW/UP/WIL//ON/PURPOSE";
				break;
			case 2:
				result = "GOAL/B.\n/GET/A/2X/MULTIPLIER/IN/60/SECONDS/MODE";
				break;
			}
			break;
		case 4:
			switch (goalNum)
			{
			case 1:
				result = "GOAL/A.\n/GET/15K/POINTS/WITHOUT/TOUCHING/WIL";
				break;
			case 2:
				result = "GOAL/B.\n/GET/A/2/:/AND/A/2/>/COMBO/IN/ONE/ROUND";
				break;
			}
			break;
		case 5:
			switch (goalNum)
			{
			case 1:
				result = "GOAL/A.\n/GET/A/3X/MULTIPLIER/IN/60/SECONDS/MODE";
				break;
			case 2:
				result = "GOAL/B.\n/BAKE/HUGGABLES/IN/THE/FOLLOWING/ORDER./:;=?";
				break;
			}
			break;
		case 6:
			switch (goalNum)
			{
			case 1:
				result = "GOAL/A.\n/GET/A/3/</AND/A/4/;/COMBO/IN/ONE/ROUND";
				break;
			case 2:
				result = "GOAL/B.\n/BAKE/A/BLUE/PURPLE/AND/YELLOW/NUGGS/IN/ONE/ROUND";
				break;
			case 3:
				result = "GOAL/C.\n/USE/5/BOMBS/IN/ONE/ROUND";
				break;
			}
			break;
		case 7:
			switch (goalNum)
			{
			case 1:
				result = "GOAL/A.\n/BAKE/3/HUGGABLES/IN/MID/AIR";
				break;
			case 2:
				result = "GOAL/B.\n/USE/BOMBS/TO/BLOW/UP/20/ENEMIES/IN/ONE/ROUND";
				break;
			}
			break;
		case 8:
			switch (goalNum)
			{
			case 1:
				result = "GOAL/A.\n/SCORE/35K/POINTS/USING/RED/RADIATION";
				break;
			case 2:
				result = "GOAL/B.\n/IN/60/SECONDS/MODE/REACH/25K/POINTS";
				break;
			}
			break;
		case 9:
			switch (goalNum)
			{
			case 1:
				result = "GOAL/A.\n/BAKE/HUGGABLES/IN/THE/FOLLOWING/ORDER.<?=;>+";
				break;
			case 2:
				result = "GOAL/B.\n/GET/A/4X/MULTIPLIER/IN/60/SECONDS/MODE/USING/ORANGE/OVERLOAD";
				break;
			case 3:
				result = "GOAL/C.\n/BAKE/5/HUGGABLES/IN/3/SECONDS";
				break;
			}
			break;
		case 10:
			if (goalNum == 1)
			{
				result = "GOAL/A.\n/BEAT/YOUR/HIGH/SCOREz";
			}
			break;
		case 11:
			switch (goalNum)
			{
			case 1:
				result = "GOAL/A.\n/GET/A/5X/MULTIPLIER/IN/60/SECONDS/MODE";
				break;
			case 2:
				result = "GOAL/B.\n/BAKE/A/WHITE/NUGGS";
				break;
			}
			break;
		case 12:
			if (goalNum == 1)
			{
				result = "GOAL/A.\n/BAKE/10/WHITE/HUGGABLES/IN/A/ROW";
			}
			break;
		case 13:
			if (goalNum == 1)
			{
				result = "GOAL/A.\n/BAKE/2/PURPLES/NUGGS/QUICKLY/TO/GAIN/A/4X/BONUS";
			}
			break;
		case 14:
			if (goalNum == 1)
			{
				result = "GOAL/A.\n/REACH/A/COMBO/WORTH/1337/WITHOUT/USING/WHITE/HUGGABLES";
			}
			break;
		case 15:
			switch (goalNum)
			{
			case 1:
				result = "GOAL/A.\n/SCORE/45000/POINTS/IN/60/SECOND/MODE/USING/YELLOW/FEVER";
				break;
			case 2:
				result = "GOAL/B.\n/BAKE/4/YELLOW/HUGGABLES/IN/A/ROW";
				break;
			}
			break;
		case 16:
			switch (goalNum)
			{
			case 1:
				result = "GOAL/A.\n/BAKE/10/PURPLE/HUGGABLES/WITH/THE/PURPLE/PLUMPER";
				break;
			case 2:
				result = "GOAL/B.\n/OBTAIN/A/POINT/RATIO/OF/500/OR/HIGHER";
				break;
			}
			break;
		case 17:
			if (goalNum == 1)
			{
				result = "GOAL/A.\n/SCORE/60000/POINTS/IN/ANY/MODE";
			}
			break;
		case 18:
			result = "WINRAR/IS/YOUz";
			break;
		default:
			result = "SKYVUS/LEET/CODING/TEAM/MIGHT/HAVE/ACCIDENTLY/ALL/OF/YOUR/OBJECTIVES?/OUR/BAD";
			break;
		}
		return result;
	}

	public void score(float val)
	{
		checkTimeScore(val);
		if (val >= scoreAmt && (GameManager.Instance.beemColor == beemType || beemType == Enemy.NONE))
		{
			if (PlayerPrefs.GetInt("objective") == 15 && GameManager.Instance.currentGameMode == GameMode.TIME)
			{
				if (goalOne == GoalType.SCORE)
				{
					goalComplete("goal1");
				}
				else if (goalTwo == GoalType.SCORE)
				{
					goalComplete("goal2");
				}
				else if (goalThree == GoalType.SCORE)
				{
					goalComplete("goal3");
				}
			}
			if (!hit && PlayerPrefs.GetInt("objective") != 15)
			{
				if (goalOne == GoalType.SCORE)
				{
					goalComplete("goal1");
				}
				else if (goalTwo == GoalType.SCORE)
				{
					goalComplete("goal2");
				}
				else if (goalThree == GoalType.SCORE)
				{
					goalComplete("goal3");
				}
			}
			if (burn)
			{
				if (goalOne == GoalType.SCORE)
				{
					goalComplete("goal1");
				}
				else if (goalTwo == GoalType.SCORE)
				{
					goalComplete("goal2");
				}
				else if (goalThree == GoalType.SCORE)
				{
					goalComplete("goal3");
				}
			}
			else
			{
				burnTarget = string.Empty;
			}
		}
		else if (shouldUpdate)
		{
			if (GameManager.Instance.currentGameMode == GameMode.TIME)
			{
				scoreAmt = PlayerPrefs.GetInt("bestTime");
			}
			else if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				scoreAmt = PlayerPrefs.GetInt(string.Empty);
			}
			shouldUpdate = false;
		}
	}

	public void checkTimeScore(float val)
	{
		if ((goalOne != GoalType.SCORECONDITION || (PlayerPrefs.GetInt("goal1") == 0 && (goalTwo != GoalType.SCORECONDITION || (PlayerPrefs.GetInt("goal2") == 0 && (goalThree != GoalType.SCORECONDITION || PlayerPrefs.GetInt("goal3") == 0))))) && val >= scoreAmtTwo && GameManager.Instance.currentGameMode == GameMode.TIME)
		{
			if (goalOne == GoalType.SCORECONDITION)
			{
				goalComplete("goal1");
			}
			if (goalTwo == GoalType.SCORECONDITION)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.SCORECONDITION)
			{
				goalComplete("goal3");
			}
		}
	}

	private bool checkCondition()
	{
		if (GameManager.Instance.playerController.life >= 3f && PlayerPrefs.GetInt("objectives") == 11)
		{
			return false;
		}
		if (GameManager.Instance.playerController.life == 1f && PlayerPrefs.GetInt("objectives") == 13)
		{
			return false;
		}
		return true;
	}

	public void exactScore(float val)
	{
		if (val == exactAmt)
		{
			if (goalOne == GoalType.EXACT)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.EXACT)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.EXACT)
			{
				goalComplete("goal3");
			}
		}
	}

	public void bombCount()
	{
		bombIndex++;
		if (bombIndex >= 3)
		{
			if (goalOne == GoalType.BOMBS)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.BOMBS)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.BOMBS)
			{
				goalComplete("goal3");
			}
		}
	}

	public void killCombo(float val, Enemy color)
	{
		if (comboTarget == Enemy.NONE && comboTarget2 == Enemy.NONE && comboTarget3 != 0)
		{
			return;
		}
		if (PlayerPrefs.GetInt("objectives") == 8)
		{
			condition = true;
		}
		if (goalOne == GoalType.COMBO && (PlayerPrefs.GetInt("goal1") != 0 || (goalTwo == GoalType.COMBO && (PlayerPrefs.GetInt("goal2") != 0 || (goalThree == GoalType.COMBO && PlayerPrefs.GetInt("goal3") != 0)))))
		{
			return;
		}
		if (color == comboTarget)
		{
			killComboIndex += 1f;
		}
		if (color == comboTarget2)
		{
			killComboIndex += 1f;
		}
		if (color == comboTarget3)
		{
			killComboIndex += 1f;
		}
		if (killComboIndex >= killComboNumber)
		{
			if (goalOne == GoalType.COMBO)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.COMBO)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.COMBO)
			{
				goalComplete("goal3");
			}
		}
	}

	public void limitCheck(Enemy type)
	{
		if (goalOne == GoalType.LIMIT && (PlayerPrefs.GetInt("goal1") != 0 || (goalTwo == GoalType.LIMIT && (PlayerPrefs.GetInt("goal2") != 0 || (goalThree == GoalType.LIMIT && PlayerPrefs.GetInt("goal3") != 0)))))
		{
			return;
		}
		if (type == limitTarget && (GameManager.Instance.beemColor == beemType || beemType == Enemy.NONE))
		{
			limitIndex++;
		}
		if (limitIndex >= limitAmount)
		{
			if (goalOne == GoalType.LIMIT)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.LIMIT)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.LIMIT)
			{
				goalComplete("goal3");
			}
		}
	}

	public IEnumerator limitTimeCount()
	{
		yield return new WaitForSeconds(limitTime);
		limitIndex = 1;
	}

	public void bake(string name, Vector3 pos, Enemy color, int time)
	{
		if (goalOne == GoalType.BAKE && (PlayerPrefs.GetInt("goal1") != 0 || (goalTwo == GoalType.BAKE && (PlayerPrefs.GetInt("goal2") != 0 || (goalThree == GoalType.BAKE && PlayerPrefs.GetInt("goal3") != 0)))))
		{
			return;
		}
		killOrder(color);
		keepAlive(time);
		if (name.Contains("Huggy"))
		{
			burnThenBake();
		}
		if (name.Contains(bakeTarget))
		{
			if (goalOne == GoalType.BAKE)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.BAKE)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.BAKE)
			{
				goalComplete("goal3");
			}
		}
		else
		{
			if (!(bakeTarget == string.Empty) || !(pos.y > -3f))
			{
				return;
			}
			bakeCount++;
			if (bakeCount >= bakeGoal)
			{
				if (goalOne == GoalType.BAKE)
				{
					goalComplete("goal1");
				}
				else if (goalTwo == GoalType.BAKE)
				{
					goalComplete("goal2");
				}
				else if (goalThree == GoalType.BAKE)
				{
					goalComplete("goal3");
				}
			}
		}
	}

	private void withIn()
	{
		bakeIndex++;
		if (bakeIndex >= bakeAmt)
		{
			if (goalOne == GoalType.BAKE)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.BAKE)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.BAKE)
			{
				goalComplete("goal3");
			}
		}
		else
		{
			StartCoroutine("bakeCoolDown");
		}
	}

	private IEnumerator bakeCoolDown()
	{
		yield return new WaitForSeconds(bakeTime);
		bakeIndex = 0;
	}

	private void keepAlive(int time)
	{
		if (time < duration)
		{
			return;
		}
		if (howMany == 1)
		{
			if (goalOne == GoalType.KEEP)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.KEEP)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.KEEP)
			{
				goalComplete("goal3");
			}
			return;
		}
		keepIndex++;
		if (keepIndex >= howMany)
		{
			if (goalOne == GoalType.KEEP)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.KEEP)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.KEEP)
			{
				goalComplete("goal3");
			}
		}
		else
		{
			StartCoroutine("keepCoolDown");
		}
	}

	private IEnumerator keepCoolDown()
	{
		yield return new WaitForSeconds(duration);
		keepIndex = 0;
	}

	private void killOrder(Enemy type)
	{
		if (orderIndex < Order.Length && Order[orderIndex] == type)
		{
			orderIndex++;
		}
		if (orderIndex >= Order.Length)
		{
			if (goalOne == GoalType.ORDER)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.ORDER)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.ORDER)
			{
				goalComplete("goal3");
			}
		}
	}

	public void burnThenBake()
	{
		if (burnIndex - 1 >= burnAmt)
		{
			if (goalOne == GoalType.BURN)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.BURN)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.BURN)
			{
				goalComplete("goal3");
			}
		}
		else
		{
			burnIndex = 0;
		}
	}

	public void wilExplode()
	{
		if (goalOne == GoalType.NONE)
		{
			if (PlayerPrefs.GetInt("goal1") == 0)
			{
				PlayerPrefs.SetInt("goal1", 1);
			}
		}
		else if (goalTwo == GoalType.NONE)
		{
			if (PlayerPrefs.GetInt("goal2") == 0)
			{
				PlayerPrefs.SetInt("goal2", 1);
			}
		}
		else if (goalThree == GoalType.NONE && PlayerPrefs.GetInt("goal3") == 0)
		{
			PlayerPrefs.SetInt("goal3", 1);
		}
	}

	public void Detonations()
	{
		if (goalOne == GoalType.DETONATIONS)
		{
			if (PlayerPrefs.GetInt("goal1") != 0)
			{
				return;
			}
			if (goalTwo == GoalType.DETONATIONS)
			{
				if (PlayerPrefs.GetInt("goal2") != 0)
				{
					return;
				}
				if (goalThree == GoalType.DETONATIONS)
				{
					if (PlayerPrefs.GetInt("goal3") == 0)
					{
					}
					return;
				}
			}
		}
		bombIndex--;
		detonationsIndex += 1f;
		if (detonationsIndex >= detonationsAmt)
		{
			if (goalOne == GoalType.DETONATIONS)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.DETONATIONS)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.DETONATIONS)
			{
				goalComplete("goal3");
			}
		}
	}

	public void everyNuggs(Nuggs type)
	{
		if ((goalTwo != GoalType.BAKE || PlayerPrefs.GetInt("goal2") == 0) && PlayerPrefs.GetInt("objectives") == 6)
		{
			switch (type)
			{
			case Nuggs.FREEZE:
				nuggTypes[0] = true;
				break;
			case Nuggs.MULTIPLIER:
				nuggTypes[1] = true;
				break;
			case Nuggs.POINTS:
				nuggTypes[2] = true;
				break;
			case Nuggs.WHITE:
				nuggTypes[3] = true;
				break;
			}
			if (nuggTypes[0] && nuggTypes[1] && nuggTypes[2] && goalTwo == GoalType.BAKE)
			{
				goalComplete("goal2");
			}
		}
	}

	public void explosions()
	{
		if (goalOne == GoalType.EXPLOSION)
		{
			if (PlayerPrefs.GetInt("goal1") != 0)
			{
				return;
			}
			if (goalTwo == GoalType.EXPLOSION)
			{
				if (PlayerPrefs.GetInt("goal2") != 0)
				{
					return;
				}
				if (goalThree == GoalType.EXPLOSION)
				{
					if (PlayerPrefs.GetInt("goal3") == 0)
					{
					}
					return;
				}
			}
		}
		explosionIndex++;
		if (explosionIndex > explosionAmt)
		{
			if (goalOne == GoalType.EXPLOSION)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.EXPLOSION)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.EXPLOSION)
			{
				goalComplete("goal3");
			}
		}
	}

	public void leet()
	{
		if (goalOne == GoalType.LEET)
		{
			if (PlayerPrefs.GetInt("goal1") != 0)
			{
				return;
			}
			if (goalTwo == GoalType.LEET)
			{
				if (PlayerPrefs.GetInt("goal2") != 0)
				{
					return;
				}
				if (goalThree == GoalType.LEET)
				{
					if (PlayerPrefs.GetInt("goal3") == 0)
					{
					}
					return;
				}
			}
		}
		if (!comboStack.Contains(Enemy.WHI))
		{
			if (goalOne == GoalType.LEET)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.LEET)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.LEET)
			{
				goalComplete("goal3");
			}
		}
	}

	public void nuggsMulti()
	{
		if ((goalOne != GoalType.NUGGSMULTI || (PlayerPrefs.GetInt("goal1") == 0 && (goalTwo != GoalType.NUGGSMULTI || (PlayerPrefs.GetInt("goal2") == 0 && (goalThree != GoalType.NUGGSMULTI || PlayerPrefs.GetInt("goal3") == 0))))) && GameManager.Instance.multiplier == 4f)
		{
			if (goalOne == GoalType.NUGGSMULTI)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.NUGGSMULTI)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.NUGGSMULTI)
			{
				goalComplete("goal3");
			}
		}
	}

	public void startCountDown()
	{
		if (!isStarted)
		{
			StartCoroutine("countDown");
		}
	}

	public void stopCountDown()
	{
		StopCoroutine("countDown");
		isStarted = false;
	}

	private IEnumerator countDown()
	{
		yield return new WaitForSeconds(motionDuration);
		if (goalOne == GoalType.MOTION)
		{
			goalComplete("goal1");
		}
		else if (goalTwo == GoalType.MOTION)
		{
			goalComplete("goal2");
		}
		else if (goalThree == GoalType.MOTION)
		{
			goalComplete("goal3");
		}
	}

	public void hitConfirm(string name)
	{
		if (name.Contains(noHit))
		{
			hit = true;
		}
		if (name.Contains(burnTarget))
		{
			burn = true;
		}
		if (!name.Contains(burnTargetTwo) || !(name != lastEnemy))
		{
			return;
		}
		burnIndex++;
		lastEnemy = name;
		if (burnIndex >= burnAmt && PlayerPrefs.GetInt("objectives") == 11)
		{
			if (goalOne == GoalType.BURN)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.BURN)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.BURN)
			{
				goalComplete("goal3");
			}
		}
	}

	public void ratioCheck(float ratio)
	{
		if ((goalOne != GoalType.RATIO || (PlayerPrefs.GetInt("goal1") == 0 && (goalTwo != GoalType.RATIO || (PlayerPrefs.GetInt("goal2") == 0 && (goalThree != GoalType.RATIO || PlayerPrefs.GetInt("goal3") == 0))))) && ratio >= ratioAmt)
		{
			if (goalOne == GoalType.RATIO)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.RATIO)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.RATIO)
			{
				goalComplete("goal3");
			}
		}
	}

	public void setGoal()
	{
		setIndex++;
		if (setIndex >= setAmount && (GameManager.Instance.beemColor == beemType || beemType == Enemy.NONE))
		{
			if (goalOne == GoalType.SET)
			{
				goalComplete("goal1");
			}
			else if (goalTwo == GoalType.SET)
			{
				goalComplete("goal2");
			}
			else if (goalThree == GoalType.SET)
			{
				goalComplete("goal3");
			}
		}
	}

	public void setCheck(Enemy color)
	{
		bool flag = false;
		Enemy enemy = lastKilledHuggable;
		lastKilledHuggable = color;
		if ((setList[7] && setList[6]) || setCompletions >= 5 || GameManager.Instance.currentGameMode != 0 || pairIndex < 1 || pairIndex > 4)
		{
			return;
		}
		if (objectiveIndex == 5 && pairIndex == 2)
		{
			PlayerPrefs.SetInt("goal1", 1);
			goalComplete("goalOne");
			PlayerPrefs.SetInt("goal1", 0);
		}
		if (!setList[(pairIndex - 1) * 2])
		{
			if (color == setOne)
			{
				oneBackup = color;
				setOne = Enemy.NONE;
				switch (pairIndex)
				{
				case 1:
					setList[0] = true;
					setOrder[0] = 0;
					flag = true;
					break;
				case 2:
					setList[2] = true;
					setOrder[2] = 2;
					flag = true;
					break;
				case 3:
					setList[4] = true;
					setOrder[4] = 4;
					flag = true;
					break;
				case 4:
					setList[6] = true;
					setOrder[6] = 6;
					flag = true;
					break;
				case 5:
					setList[8] = true;
					setOrder[8] = 8;
					flag = true;
					break;
				}
			}
			if (setList.Length > (pairIndex - 1) * 2 + 1 && (pairIndex - 1) * 2 >= 0 && ((setList[(pairIndex - 1) * 2] && setList[(pairIndex - 1) * 2 + 1]) || (setList[(pairIndex - 1) * 2] && color == Enemy.WHI) || (setList[(pairIndex - 1) * 2 + 1] && color == Enemy.WHI) || (setList[(pairIndex - 1) * 2] && enemy == Enemy.WHI) || (setList[(pairIndex - 1) * 2 + 1] && enemy == Enemy.WHI) || (color == Enemy.WHI && enemy == Enemy.WHI)))
			{
				if (pairIndex < 5)
				{
					flag = true;
					setGoal();
					lastKilledHuggable = Enemy.NONE;
					getPair();
					setCompletions++;
					GameManager.Instance.hudController.challengeSet.text = GameManager.Instance.hudController.numToDigit(setCompletions, 2, true);
				}
				else
				{
					setList[(pairIndex - 1) * 2] = true;
					setList[(pairIndex - 1) * 2 + 1] = true;
					setGoal();
					StartCoroutine(GameManager.Instance.hudController.setScroll());
				}
			}
			if (flag)
			{
				return;
			}
		}
		if (color == setTwo)
		{
			switch (pairIndex)
			{
			case 1:
				setList[1] = true;
				setOrder[1] = 1;
				break;
			case 2:
				setList[3] = true;
				setOrder[3] = 3;
				break;
			case 3:
				setList[5] = true;
				setOrder[5] = 5;
				break;
			case 4:
				setList[7] = true;
				setOrder[7] = 7;
				break;
			case 5:
				setList[9] = true;
				setOrder[9] = 9;
				break;
			}
		}
		if (GameManager.Instance.currentGameMode == GameMode.TIME && setList.Length > (pairIndex - 1) * 2 + 1 && (pairIndex - 1) * 2 >= 0 && ((setList[(pairIndex - 1) * 2] && setList[(pairIndex - 1) * 2 + 1]) || (setList[(pairIndex - 1) * 2] && color == Enemy.WHI) || (setList[(pairIndex - 1) * 2 + 1] && color == Enemy.WHI) || (setList[(pairIndex - 1) * 2] && enemy == Enemy.WHI) || (setList[(pairIndex - 1) * 2 + 1] && enemy == Enemy.WHI) || (color == Enemy.WHI && enemy == Enemy.WHI)))
		{
			if (pairIndex < 5)
			{
				setGoal();
				lastKilledHuggable = Enemy.NONE;
				getPair();
				setCompletions++;
				GameManager.Instance.hudController.challengeSet.text = GameManager.Instance.hudController.numToDigit(setCompletions, 2, true);
			}
			else
			{
				setList[(pairIndex - 1) * 2] = true;
				setList[(pairIndex - 1) * 2 + 1] = true;
				setGoal();
				getPair();
				StartCoroutine(GameManager.Instance.hudController.setScroll());
			}
		}
		if (color != oneBackup && color != setTwo && color != Enemy.WHI)
		{
			if (pairIndex == 1)
			{
				setList[0] = false;
				setList[1] = false;
			}
			else if (pairIndex == 2)
			{
				setList[2] = false;
				setList[3] = false;
			}
			else if (pairIndex == 3)
			{
				setList[4] = false;
				setList[5] = false;
			}
			else if (pairIndex == 4)
			{
				setList[6] = false;
				setList[7] = false;
			}
			if (oneBackup != 0)
			{
				setOne = oneBackup;
			}
		}
	}

	public void getPair()
	{
		oneBackup = Enemy.NONE;
		string empty = string.Empty;
		string second = string.Empty;
		switch (Random.Range(0, 6))
		{
		case 0:
			setOne = Enemy.BLU;
			empty = ">";
			break;
		case 1:
			setOne = Enemy.GRN;
			empty = ";";
			break;
		case 2:
			setOne = Enemy.ORG;
			empty = "?";
			break;
		case 3:
			setOne = Enemy.PIN;
			empty = ":";
			break;
		case 4:
			setOne = Enemy.RED;
			empty = "<";
			break;
		case 5:
			setOne = Enemy.YEL;
			empty = "=";
			break;
		default:
			setOne = Enemy.NONE;
			empty = string.Empty;
			break;
		}
		switch (Random.Range(0, 6))
		{
		case 0:
			setTwo = Enemy.BLU;
			second = ">";
			break;
		case 1:
			setTwo = Enemy.GRN;
			second = ";";
			break;
		case 2:
			setTwo = Enemy.ORG;
			second = "?";
			break;
		case 3:
			setTwo = Enemy.PIN;
			second = ":";
			break;
		case 4:
			setTwo = Enemy.RED;
			second = "<";
			break;
		case 5:
			setTwo = Enemy.YEL;
			second = "=";
			break;
		}
		pairIndex++;
		if (pairIndex == 5)
		{
			empty = "^";
			second = "&";
		}
		GameManager.Instance.hudController.setUpdater(pairIndex, empty, second, setIndex);
	}

	public void reSet(bool didComplete)
	{
		for (int i = 0; i < setList.Length; i++)
		{
			setList[i] = false;
		}
		pairIndex = 1;
		GameManager.Instance.hudController.clearSets();
		if (didComplete)
		{
			getPair();
		}
	}

	public bool isSetComplete()
	{
		if (setList[7] && setList[6])
		{
			reSet(true);
			setCompletions++;
			GameManager.Instance.arcadeTime = 60f;
			return true;
		}
		reSet(false);
		GameManager.Instance.playerController.afterAction();
		return false;
	}

	public void devCompletePair()
	{
		setCheck(setOne);
		setCheck(setTwo);
	}

	public IEnumerator DelayedPlayAudio()
	{
		yield return new WaitForSeconds(2f);
	}

	public void objectiveTen()
	{
		if (PlayerPrefs.GetInt("objectives") == 10)
		{
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				scoreAmt = (int)PlayerPrefs.GetFloat("statSurvivalScore");
			}
			else if (GameManager.Instance.currentGameMode == GameMode.TIME)
			{
				scoreAmt = (int)PlayerPrefs.GetFloat("statTimeScore");
			}
		}
	}
}
