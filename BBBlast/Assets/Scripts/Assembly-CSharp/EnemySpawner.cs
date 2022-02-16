using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	private ObjectRecycler headRecycler;

	private ObjectRecycler legRecycler;

	private ObjectRecycler armRecycler;

	public GameObject pinkHeadPrefab;

	public GameObject pinkLegPrefab;

	public GameObject pinkArmPrefab;

	public GameObject[] junglePoints;

	public GameObject[] sewerPoints;

	public GameObject[] cliffPoints;

	public GameObject[] uniPoints;

	public GameObject[] piratePoints;

	public Object[] enemies;

	public GameObject[] wilJunglePoints;

	public GameObject[] wilSewerPoints;

	public GameObject[] wilCliffPoints;

	public GameObject[] wilUni;

	public GameObject[] wilPiratePoints;

	public GameObject wil;

	public GameObject wilDouble;

	public GameObject[] nuggsJunglePoints;

	public GameObject[] nuggsSewerPoints;

	public GameObject[] nuggsCliffPoints;

	public GameObject[] nuggsPiratePoints;

	public GameObject pointNuggsPoint;

	public GameObject[] nuggs;

	private float waveAmount;

	public float waitTime;

	public float delay;

	private float delayDelta;

	private GameObject[] spawnPoints;

	private GameObject[] wilPoints;

	private GameObject[] nuggsPoints;

	private float myTime;

	private float cashDelay;

	private int index;

	private int spawnNum;

	public bool oneTimeWhiteNuggs;

	public float enemyCount;

	private float waitTimeCash;

	private bool block;

	private bool[] bomb;

	private float nuggsVal;

	private float wilVal;

	private static EnemySpawner instance;

	private bool ranOnce;

	public static EnemySpawner Instance
	{
		get
		{
			return instance;
		}
	}

	public GameObject freeHead
	{
		get
		{
			return headRecycler.nextFree;
		}
	}

	public GameObject freeArm
	{
		get
		{
			return armRecycler.nextFree;
		}
	}

	public GameObject freeLeg
	{
		get
		{
			return legRecycler.nextFree;
		}
	}

	public void freeMeshEmitObj(GameObject go)
	{
		if (go.name.ToLower().Contains("head"))
		{
			recycleHead(go);
		}
		else if (go.name.ToLower().Contains("arm"))
		{
			recycleArm(go);
		}
		else if (go.name.ToLower().Contains("leg"))
		{
			recycleLeg(go);
		}
	}

	private void recycleHead(GameObject go)
	{
		headRecycler.freeObject(go);
	}

	private void recycleArm(GameObject go)
	{
		armRecycler.freeObject(go);
	}

	private void recycleLeg(GameObject go)
	{
		legRecycler.freeObject(go);
	}

	private void Awake()
	{
		instance = this;
		ranOnce = false;
		cashDelay = delay;
		delayDelta = delay;
		waitTimeCash = waitTime;
		oneTimeWhiteNuggs = false;
		headRecycler = new ObjectRecycler(pinkHeadPrefab, 8);
		legRecycler = new ObjectRecycler(pinkLegPrefab, 8);
		armRecycler = new ObjectRecycler(pinkArmPrefab, 8);
		index = 0;
		enemyCount = 0f;
		nuggsVal = 250f;
		wilVal = 500f;
		GameManager.Instance.canHasWil = true;
	}

	private void Start()
	{
		Object[] array = Resources.LoadAll("Enemies", typeof(GameObject));
		enemies = new Object[array.Length + 1];
		for (int i = 0; i < array.Length; i++)
		{
			enemies[i] = array[i];
		}
		if (GameManager.Instance.beemColor == Enemy.PUR)
		{
			enemies[enemies.Length - 1] = Resources.Load("purpleHuggyReg");
		}
		else
		{
			enemies[enemies.Length - 1] = Resources.Load("purpleHuggy");
		}
		if (GameManager.Instance.currentLevel == Level.JUNGLE)
		{
			spawnPoints = new GameObject[junglePoints.Length + uniPoints.Length];
			nuggsPoints = new GameObject[nuggsJunglePoints.Length];
			junglePoints.CopyTo(spawnPoints, 0);
			uniPoints.CopyTo(spawnPoints, junglePoints.Length);
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				wilPoints = new GameObject[wilJunglePoints.Length + wilUni.Length];
				wilJunglePoints.CopyTo(wilPoints, 0);
				wilUni.CopyTo(wilPoints, wilJunglePoints.Length);
			}
			else
			{
				wilPoints = new GameObject[wilJunglePoints.Length];
				wilJunglePoints.CopyTo(wilPoints, 0);
			}
			for (int j = 0; j < nuggsJunglePoints.Length; j++)
			{
				nuggsPoints[j] = nuggsJunglePoints[j];
			}
		}
		else if (GameManager.Instance.currentLevel == Level.SEWER)
		{
			spawnPoints = new GameObject[sewerPoints.Length + uniPoints.Length];
			nuggsPoints = new GameObject[nuggsSewerPoints.Length];
			sewerPoints.CopyTo(spawnPoints, 0);
			uniPoints.CopyTo(spawnPoints, sewerPoints.Length);
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				wilPoints = new GameObject[wilSewerPoints.Length + wilUni.Length];
				wilSewerPoints.CopyTo(wilPoints, 0);
				wilUni.CopyTo(wilPoints, wilSewerPoints.Length);
			}
			else
			{
				wilPoints = new GameObject[wilSewerPoints.Length];
				wilSewerPoints.CopyTo(wilPoints, 0);
			}
			for (int k = 0; k < nuggsSewerPoints.Length; k++)
			{
				nuggsPoints[k] = nuggsSewerPoints[k];
			}
		}
		else if (GameManager.Instance.currentLevel == Level.CLIFF)
		{
			spawnPoints = new GameObject[cliffPoints.Length + uniPoints.Length];
			nuggsPoints = new GameObject[nuggsCliffPoints.Length];
			cliffPoints.CopyTo(spawnPoints, 0);
			uniPoints.CopyTo(spawnPoints, cliffPoints.Length);
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				wilPoints = new GameObject[wilCliffPoints.Length + wilUni.Length];
				wilCliffPoints.CopyTo(wilPoints, 0);
				wilUni.CopyTo(wilPoints, wilCliffPoints.Length);
			}
			else
			{
				wilPoints = new GameObject[wilCliffPoints.Length];
				wilCliffPoints.CopyTo(wilPoints, 0);
			}
			for (int l = 0; l < nuggsCliffPoints.Length; l++)
			{
				nuggsPoints[l] = nuggsCliffPoints[l];
			}
		}
		else if (GameManager.Instance.currentLevel == Level.PIRATE)
		{
			spawnPoints = new GameObject[piratePoints.Length + uniPoints.Length];
			nuggsPoints = new GameObject[nuggsPiratePoints.Length];
			piratePoints.CopyTo(spawnPoints, 0);
			uniPoints.CopyTo(spawnPoints, piratePoints.Length);
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				wilPoints = new GameObject[wilPiratePoints.Length + wilUni.Length];
				wilPiratePoints.CopyTo(wilPoints, 0);
				wilUni.CopyTo(wilPoints, wilCliffPoints.Length);
			}
			else
			{
				wilPoints = new GameObject[wilPiratePoints.Length];
				wilPiratePoints.CopyTo(wilPoints, 0);
			}
			for (int m = 0; m < nuggsPiratePoints.Length; m++)
			{
				nuggsPoints[m] = nuggsPiratePoints[m];
			}
		}
		bomb = new bool[wilPoints.Length];
		oneTimeWhiteNuggs = false;
		myTime = Time.time;
		waveAmount = GameManager.Instance.maxEnemies;
		if (GameManager.Instance.currentGameMode == GameMode.TIME)
		{
			delay *= 1.1f;
			cashDelay *= 1.1f;
		}
	}

	public void ghettoConcat(int length)
	{
		for (int i = length; i < wilPoints.Length; i++)
		{
		}
	}

	private void Update()
	{
		if (GameManager.Instance.isOver || GameManager.Instance.playerController.life <= 0f || !GameManager.Instance.clearedEntranceExam)
		{
			return;
		}
		if (waveAmount > 0f && Time.time > myTime + waitTime + delay)
		{
			if (!GameManager.Instance.hudController.canStart && GameManager.Instance.currentGameMode == GameMode.TIME)
			{
				GameManager.Instance.hudController.canStart = true;
				GameManager.Instance.hudController.realTime = Time.realtimeSinceStartup;
			}
			if (GameManager.Instance.currentGameMode != GameMode.SCORE || !ranOnce)
			{
			}
			if (enemyCount >= (float)GameManager.Instance.maxEnemies)
			{
				return;
			}
			waitTime = 0f;
			int num = Random.Range(0, spawnPoints.Length - 1);
			int num2 = Random.Range(0, enemies.Length);
			if (enemies[num2].name == "whiteHuggy" && Random.value > 0.5f)
			{
				num2 = Random.Range(0, enemies.Length);
			}
			if (enemies[num2].name == "purpleHuggy" && Random.value > 0.5f)
			{
				num2 = Random.Range(0, enemies.Length);
			}
			if (GameManager.Instance.whiteOut)
			{
				num2 = 5;
			}
			if (num == spawnNum)
			{
				return;
			}
			GameObject gameObject = Object.Instantiate(enemies[num2], spawnPoints[num].transform.position, spawnPoints[num].transform.rotation) as GameObject;
			spawnNum = num;
			gameObject.name = enemies[num2].name + "_" + spawnPoints[num].name + "_" + waveAmount;
			enemyCount += 1f;
			waveAmount -= 1f;
			myTime = Time.time;
			index++;
			if (index >= 2)
			{
				count();
			}
			else
			{
				delay = 0f;
			}
		}
		if (waveAmount <= 0f && !block)
		{
			StartCoroutine("newWave");
		}
		if (GameManager.Instance.canHasNuggs && GameManager.Instance.hudController.scoreVal >= nuggsVal)
		{
			int num3 = Random.Range(0, nuggsPoints.Length);
			int num4 = 0;
			float num5 = Random.value;
			GameObject gameObject2;
			if (GameManager.Instance.currentGameMode == GameMode.TIME && Objectives.Instance.pairIndex >= 5 && !oneTimeWhiteNuggs)
			{
				oneTimeWhiteNuggs = true;
				gameObject2 = Object.Instantiate(nuggs[3], nuggsPoints[num3].transform.position, nuggsPoints[num3].transform.rotation) as GameObject;
				gameObject2.name = nuggs[3].name + "_" + nuggsPoints[num3].name;
				GameManager.Instance.canHasNuggs = false;
				nuggsVal = GameManager.Instance.hudController.scoreVal + 500f;
				return;
			}
			if (GameManager.Instance.skinLoadOut == Skin.OCO)
			{
				num5 = 0.6f;
			}
			if (GameManager.Instance.skinLoadOut == Skin.EVILOCO)
			{
				if (num5 < 0.1f)
				{
					num4 = 0;
				}
				else
				{
					if (!(num5 < 0.55f))
					{
						gameObject2 = Object.Instantiate(nuggs[2], pointNuggsPoint.transform.position, pointNuggsPoint.transform.rotation) as GameObject;
						gameObject2.name = nuggs[2].name + "_fly" + Random.Range(1, 3);
						GameManager.Instance.canHasNuggs = false;
						nuggsVal = GameManager.Instance.hudController.scoreVal + 250f;
						return;
					}
					num4 = 1;
				}
			}
			else if (num5 < 0.25f)
			{
				num4 = 0;
			}
			else
			{
				if (!(num5 < 0.7f))
				{
					gameObject2 = Object.Instantiate(nuggs[2], pointNuggsPoint.transform.position, pointNuggsPoint.transform.rotation) as GameObject;
					gameObject2.name = nuggs[2].name + "_fly" + Random.Range(1, 3);
					GameManager.Instance.canHasNuggs = false;
					nuggsVal = GameManager.Instance.hudController.scoreVal + 250f;
					return;
				}
				num4 = 1;
			}
			gameObject2 = Object.Instantiate(nuggs[num4], nuggsPoints[num3].transform.position, nuggsPoints[num3].transform.rotation) as GameObject;
			gameObject2.name = nuggs[num4].name + "_" + nuggsPoints[num3].name;
			GameManager.Instance.canHasNuggs = false;
			if (GameManager.Instance.chestLoadOut == Chest.KNIGHT)
			{
				nuggsVal = GameManager.Instance.hudController.scoreVal + 300f;
			}
			else
			{
				nuggsVal = GameManager.Instance.hudController.scoreVal + 500f;
			}
		}
		if (!GameManager.Instance.canHasWil || GameManager.Instance.isWilOut || !(GameManager.Instance.hudController.scoreVal >= wilVal))
		{
			return;
		}
		int num6 = Random.Range(0, wilPoints.Length);
		if (bomb[num6])
		{
			return;
		}
		GameManager.Instance.canHasWil = false;
		GameObject gameObject3 = new GameObject();
		if (wilPoints[num6].name.Contains("wilBouncey") || wilPoints[num6].name.Contains("wilCorkscrew") || wilPoints[num6].name.Contains("wilSideways"))
		{
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				gameObject3 = Object.Instantiate(wilDouble, wilPoints[num6].transform.position, wilPoints[num6].transform.rotation) as GameObject;
			}
		}
		else
		{
			gameObject3 = Object.Instantiate(wil, wilPoints[num6].transform.position, wilPoints[num6].transform.rotation) as GameObject;
		}
		bomb[num6] = true;
		gameObject3.name = wil.name + "_" + wilPoints[num6].name + "_" + num6;
		if (GameManager.Instance.chestLoadOut == Chest.KNIGHT)
		{
			wilVal = GameManager.Instance.hudController.scoreVal + 500f;
		}
		else
		{
			wilVal = GameManager.Instance.hudController.scoreVal + 750f;
		}
	}

	private IEnumerator newWave()
	{
		block = true;
		yield return StartCoroutine("Unload");
		waveAmount = GameManager.Instance.maxEnemies;
		block = false;
	}

	private IEnumerator Unload()
	{
		Application.backgroundLoadingPriority = ThreadPriority.High;
		AsyncOperation dumpage = Resources.UnloadUnusedAssets();
		dumpage.priority = 256;
		yield return null;
	}

	public void removeEnemy()
	{
		enemyCount -= 1f;
		if (enemyCount <= 0f)
		{
			enemyCount = 0f;
		}
	}

	private void count()
	{
		index = 0;
		if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			if (delayDelta > 1f)
			{
				delay = delayDelta * 0.99f;
				delayDelta = delay;
			}
			else
			{
				delay = delayDelta;
			}
		}
		else
		{
			delay = cashDelay;
		}
	}

	public void reset()
	{
		enemyCount = 0f;
		delay = cashDelay;
		delayDelta = cashDelay;
		index = 0;
		waitTime = waitTimeCash;
		nuggsVal = 250f;
		wilVal = 500f;
		GameManager.Instance.canHasWil = true;
		GameManager.Instance.canHasNuggs = true;
		for (int i = 0; i < bomb.Length; i++)
		{
			bomb[i] = false;
		}
	}

	public void clearSpawn(string name)
	{
		for (int i = 0; i < wilPoints.Length; i++)
		{
			if (wilPoints[i].name == name)
			{
				bomb[i] = false;
			}
		}
	}
}
