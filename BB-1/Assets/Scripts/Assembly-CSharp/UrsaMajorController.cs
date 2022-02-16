using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UrsaMajorController : MonoBehaviour
{
	private Transform myTransform;

	private float currentHP = 50f;

	private float maxHP = 50f;

	public Transform shipBar;

	private bool isDead;

	public float damageBeforeBattlecry = 8f;

	private float currentBattlecryDamage;

	public string battlecriesDirectory;

	private AudioClip[] battlecries;

	private void Awake()
	{
		currentHP = maxHP;
		myTransform = base.transform;
		updateShipHP();
	}

	private void Start()
	{
		if (GameManager.Instance.useHighres && battlecriesDirectory != "")
		{
			loadBattlecries();
		}
	}

	private void loadBattlecries()
	{
		Object[] array = Resources.LoadAll("Music/High/" + battlecriesDirectory, typeof(AudioClip));
		if (array.Length != 0)
		{
			battlecries = new AudioClip[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				battlecries[i] = array[i] as AudioClip;
			}
		}
	}

	private void OnDamageShip(float damage)
	{
		if (isDead)
		{
			return;
		}
		currentHP -= damage;
		if (battlecries != null && battlecries.Length != 0)
		{
			currentBattlecryDamage += damage;
			if (currentBattlecryDamage > damageBeforeBattlecry)
			{
				currentBattlecryDamage = 0f;
				SoundManager.Instance.playSound(battlecries[Random.Range(0, battlecries.Length)]);
			}
		}
		if (currentHP <= 0f)
		{
			isDead = true;
			StartCoroutine(death());
		}
		else
		{
			updateShipHP();
		}
	}

	private IEnumerator death()
	{
		((Object.Instantiate(Resources.Load("FaderSystem")) as GameObject).GetComponent(typeof(SimpleFader)) as SimpleFader).fadeTime = 1f;
		yield return new WaitForSeconds(0.1f);
		if (GameManager.Instance.ocoLives > 1)
		{
			GameManager.Instance.ocoLives--;
			//Application.LoadLevel("SpacebossFight");
			SceneManager.LoadScene("SpacebossFight");
			yield break;
		}
		PlayerPrefs.DeleteKey("character");
		if (GameManager.Instance.currentGameMode == GameMode.CAMPAIGN)
		{
			yield return new WaitForSeconds(0.2f);
			GameManager.Instance.PlayMovie("oco_spaceboss_loss");
		}
		yield return new WaitForSeconds(0.1f);
		StopAllCoroutines();
		StatsManager.Instance.currentPlayTime += Time.timeSinceLevelLoad;
		Object[] array = Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (!(gameObject.name == "GameManager") && !(gameObject.transform.root.name == "SoundManager") && !(gameObject.name == "Controlls") && !(gameObject.name == "Rewired Input Manager") && !(gameObject.name == "UICanvas") && !(gameObject.name == "EventSystem"))
			{
				Object.Destroy(gameObject);
			}
		}
		Object.Instantiate((Object.FindObjectOfType(typeof(EndMenuSpawner)) as EndMenuSpawner).endMenu);
	}

	private void updateShipHP()
	{
		Vector3 localScale = shipBar.transform.localScale;
		localScale.z = 1f - currentHP / maxHP;
		shipBar.transform.localScale = localScale;
	}
}
