using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float life = 3f;

	public bool godMode;

	public GameObject tronRoomOrg;

	[HideInInspector]
	public bool hit;

	private GameObject[] helm;

	private GameObject[] armor;

	private bool shake;

	private bool isDead;

	public GameObject rings;

	private bool toRed;

	public Material red;

	private bool isPulsing;

	private Object[] battleCries;

	private Transform myCamTrans;

	public bool isSamuraiHat;

	public void Start()
	{
		SoundController.Instance.soundTrack();
		GameObject[] array = GameObject.FindGameObjectsWithTag("Knight");
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("Shogun");
		GameObject[] array3 = GameObject.FindGameObjectsWithTag("Pirate");
		helm = new GameObject[4];
		armor = new GameObject[4];
		helm[0] = array[0];
		helm[1] = array2[0];
		helm[2] = null;
		helm[3] = array3[0];
		armor[0] = array[1];
		armor[1] = array2[1];
		armor[2] = null;
		armor[3] = array3[1];
		GameObject[] array4 = GameObject.FindGameObjectsWithTag("Face");
		for (byte b = 0; b < array4.Length; b = (byte)(b + 1))
		{
			//array4[b].active = false;
			array4[b].SetActive(false);
		}
		myCamTrans = Camera.main.transform;
		for (int i = 0; i < helm.Length; i++)
		{
			if (armor[i] != null)
			{
				//armor[i].active = false;
				armor[i].SetActive(false);
			}
			if (helm[i] != null)
			{
				//helm[i].active = false;
				helm[i].SetActive(false);
			}
		}
		if (PlayerPrefs.GetInt("armor") != 2)
		{
			//armor[PlayerPrefs.GetInt("armor")].active = true;
			armor[PlayerPrefs.GetInt("armor")].SetActive(true);
		}
		if (PlayerPrefs.GetInt("helm") != 2)
		{
			//helm[PlayerPrefs.GetInt("helm")].active = true;
			helm[PlayerPrefs.GetInt("helm")].SetActive(true);
		}
		GameManager.Instance.animC.idle(false);
		isDead = false;
		red.SetColor("_Color", new Color(1f, 1f, 1f, 0f));
		battleCries = Resources.LoadAll("BattleCry");
	}

	public IEnumerator afterAction()
	{
		if (!GameManager.Instance.isOver)
		{
			PowerupManager.Instance.reset();
			GameManager.Instance.controls.disabled = true;
			GameManager.Instance.enemySpeed = 0f;
			GameManager.Instance.multiplier = 0f;
			GameManager.Instance.colorComboAmt = 1;
			GameManager.Instance.prev = Enemy.NONE;
			GameManager.Instance.canHasNuggs = true;
			GameManager.Instance.canHasWil = true;
			GameManager.Instance.isWilOut = false;
			GameManager.Instance.isOver = true;
			yield return new WaitForSeconds(GameManager.Instance.animC.death());
			GameManager.Instance.hudController.setTheBest();
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				GameObject go = Object.Instantiate(GameManager.Instance.hudController.gameOverText) as GameObject;
				go.GetComponent<Animation>().Play();
			}
			yield return new WaitForSeconds(1.5f);
			GameManager.Instance.hudController.AAR();
			yield return new WaitForSeconds(0.1f);
			GameManager.Instance.saveStats();
			yield return new WaitForSeconds(0.5f);
			Resources.UnloadUnusedAssets();
			isDead = false;
			toRed = false;
			Material enemy = Resources.Load("Enemies") as Material;
			enemy.SetColor("_Color", Color.white);
			toRed = false;
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL && (float)int.Parse(GameManager.Instance.hudController.survival_BestScore.text) < GameManager.Instance.hudController.scoreVal)
			{
				PlayerPrefs.SetString("Survival_BestScore", GameManager.Instance.hudController.scoreVal.ToString());
			}
		}
	}

	public void onHit()
	{
		if (!GameManager.Instance.inTutorial)
		{
			if (godMode || hit || isDead)
			{
				return;
			}
			hit = true;
			if (GameManager.Instance.currentGameMode == GameMode.SCORE)
			{
				MWPowerBarManager.Instance.GetHit(200f);
			}
			if (GameManager.Instance.currentGameMode == GameMode.TIME)
			{
				float num = 1f;
				if (GameManager.Instance.helmLoadOut == Helm.KNIGHT)
				{
					num = 0.5f;
				}
				GameManager.Instance.hudController.updateScore(Mathf.Round((0f - GameManager.Instance.hudController.scoreVal) * 0.2f * num));
			}
			if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
			{
				if (life == 1f)
				{
					StartCoroutine("afterAction");
				}
				if (isSamuraiHat)
				{
					isSamuraiHat = false;
					SoundController.Instance.PlayMySound("o_hurt1");
				}
				else
				{
					GameManager.Instance.hudController.hit(life);
					life -= 1f;
				}
			}
			StartCoroutine("shakeage");
			StartCoroutine("hitDelay");
		}
		else
		{
			SoundController.Instance.PlayMySound("o_hurt1");
		}
	}

	public IEnumerator hitDelay()
	{
		SoundController.Instance.playClip(battleCries[Random.Range(1, 5)] as AudioClip);
		yield return new WaitForSeconds(1.5f);
		hit = false;
	}

	public IEnumerator shakeage()
	{
		shake = true;
		yield return new WaitForSeconds(0.25f);
		shake = false;
	}

	private void Update()
	{
		if (GameManager.Instance.allMute)
		{
			base.GetComponent<AudioSource>().volume = 0f;
		}
		else
		{
			base.GetComponent<AudioSource>().volume = 0.5f;
		}
		if (shake)
		{
			myCamTrans.localPosition = new Vector3(Camera.main.transform.localPosition.x, Random.Range(-0.1f, 0.1f) + Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z);
		}
		else
		{
			myCamTrans.localPosition = new Vector3(Camera.main.transform.localPosition.x, 0f, Camera.main.transform.localPosition.z);
		}
		if (GameManager.Instance.isLevel)
		{
			//rings.SetActiveRecursively(true);
			rings.SetActive(true);
		}
		else
		{
			//rings.SetActiveRecursively(false);
			rings.SetActive(false);
		}
		if (toRed)
		{
			float a = Mathf.Lerp(red.color.a, 1f, 0.1f);
			red.SetColor("_Color", new Color(1f, 1f, 1f, a));
			if (!isPulsing)
			{
				rings.transform.localScale += Vector3.one * 0.1f;
				if (rings.transform.localScale.x > 3.5f)
				{
					isPulsing = true;
				}
			}
			else
			{
				rings.transform.localScale -= Vector3.one * 0.1f;
				if (rings.transform.localScale.x < 2.5f)
				{
					isPulsing = false;
				}
			}
		}
		else
		{
			float a2 = Mathf.Lerp(red.color.a, 0f, 0.1f);
			red.SetColor("_Color", new Color(1f, 1f, 1f, a2));
			if (rings.transform.localScale.x > 3f)
			{
				rings.transform.localScale -= Vector3.one * 0.01f;
			}
			else if (rings.transform.localScale.x < 3f)
			{
				rings.transform.localScale += Vector3.one * 0.01f;
			}
		}
	}

	public void gonnaGetHit()
	{
		toRed = true;
	}

	public void notGetHit()
	{
		toRed = false;
	}
}
