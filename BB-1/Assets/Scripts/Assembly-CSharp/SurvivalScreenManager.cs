using System.Collections;
using BSCore;
using UnityEngine;

public class SurvivalScreenManager : MonoBehaviour
{
	public GameObject oliverRoot;

	public GameObject riggsRoot;

	public GameObject wilRoot;

	public GameObject toaster;

	public GameObject turkey;

	public GameObject vespa;

	public GameObject boat;

	public GameObject grill;

	private GameObject currentRoot;

	public TextMesh prizeText;

	public TextMesh multiplierText;

	public TextMesh bonusScoreText;

	public TextMesh totalScoreText;

	public TextMesh roomClearText;

	public int toasterValue = 3000;

	public int turkeyValue = 5000;

	public int boatValue = 7500;

	public int grillValue = 10000;

	public int vespaValue = 15000;

	private GameObject low;

	private GameObject mid;

	private GameObject high;

	private float currentBonus;

	private bool hasShownAll;

	public AudioClip loopSound;

	public AudioClip oliverCheerSound;

	public AudioClip riggsCheerSound;

	private AudioSource myAudio;

	public GameObject inGameAAR;

	public GameObject fakeLight;

	private void Awake()
	{
		myAudio = GetComponent<AudioSource>();
		myAudio.volume = SoundManager.Instance.getEffectsVolume();
		roomClearText.text = "Rooms Cleared: " + (StatsManager.Instance.currentRoomNumber - 1);
		if (StatsManager.Instance.currentScore > 0)
		{
			totalScoreText.text = string.Format("{0:#,#}", StatsManager.Instance.currentScore);
		}
		else
		{
			totalScoreText.text = "0";
		}
		bonusScoreText.text = "0";
		if (GameManager.Instance.currentCharacter == Character.OLIVER)
		{
			Object.Destroy(riggsRoot);
			Object.Destroy(wilRoot);
			Object.Destroy(fakeLight);
			currentRoot = oliverRoot;
		}
		else if (GameManager.Instance.currentCharacter == Character.RIGGS)
		{
			Object.Destroy(oliverRoot);
			Object.Destroy(wilRoot);
			Object.Destroy(fakeLight);
			currentRoot = riggsRoot;
		}
		else if (GameManager.Instance.currentCharacter == Character.WIL)
		{
			Object.Destroy(oliverRoot);
			Object.Destroy(riggsRoot);
			currentRoot = wilRoot;
		}
		low = currentRoot.transform.Find("low").gameObject;
		mid = currentRoot.transform.Find("mid").gameObject;
		high = currentRoot.transform.Find("high").gameObject;
		low.active = true;
		mid.active = false;
		high.active = false;
		fakeLight.SetActiveRecursively(true);
		StartCoroutine(bonusDisplayer());
	}

	private IEnumerator bonusDisplayer()
	{
		yield return new WaitForSeconds(0.1f);
		yield return StartCoroutine(handlePrize("Toaster", toaster, StatsManager.Instance.toasters, toasterValue));
		yield return StartCoroutine(handlePrize("Turkey", turkey, StatsManager.Instance.turkeys, turkeyValue));
		yield return StartCoroutine(handlePrize("Jetski", boat, StatsManager.Instance.boats, boatValue));
		yield return StartCoroutine(handlePrize("Grill", grill, StatsManager.Instance.grills, grillValue));
		yield return StartCoroutine(handlePrize("Vespa", vespa, StatsManager.Instance.vespas, vespaValue));
		hasShownAll = true;
	}

	private IEnumerator handlePrize(string prizeName, GameObject objToDisplay, int numberOfItems, int itemValue)
	{
		myAudio.clip = loopSound;
		myAudio.loop = true;
		myAudio.Play();
		foreach (Transform item in objToDisplay.transform.parent)
		{
			item.gameObject.active = false;
		}
		objToDisplay.active = true;
		prizeText.text = prizeName;
		yield return new WaitForSeconds(0.25f);
		multiplierText.text = "x " + numberOfItems;
		myAudio.pitch = 1f;
		int i = 0;
		while (i < numberOfItems)
		{
			yield return new WaitForSeconds(0.1f);
			currentBonus += itemValue;
			StatsManager.Instance.currentScore += itemValue;
			bonusScoreText.text = string.Format("{0:#,#}", currentBonus);
			totalScoreText.text = string.Format("{0:#,#}", StatsManager.Instance.currentScore);
			myAudio.pitch += 0.2f;
			if ((double)myAudio.pitch > 7.0)
			{
				myAudio.pitch = 7f;
			}
			int num = i + 1;
			i = num;
		}
		yield return StartCoroutine(cheer());
	}

	private IEnumerator cheer()
	{
		float animationDelay = 0.12f;
		int i = 0;
		while (i < 1)
		{
			yield return new WaitForSeconds(animationDelay);
			low.active = false;
			mid.active = true;
			high.active = false;
			yield return new WaitForSeconds(animationDelay);
			low.active = false;
			mid.active = false;
			high.active = true;
			myAudio.Stop();
			if (GameManager.Instance.currentCharacter == Character.OLIVER)
			{
				myAudio.clip = oliverCheerSound;
			}
			else
			{
				myAudio.clip = riggsCheerSound;
			}
			if (GameManager.Instance.currentCharacter != Character.WIL)
			{
				myAudio.loop = false;
				myAudio.pitch = 1f;
				myAudio.Play();
			}
			yield return new WaitForSeconds(1f);
			low.active = false;
			mid.active = mid;
			high.active = false;
			yield return new WaitForSeconds(animationDelay);
			low.active = true;
			mid.active = false;
			high.active = false;
			yield return new WaitForSeconds(1f);
			int num = i + 1;
			i = num;
		}
	}

	private void Update()
	{
		if (BSCoreInput.GetButton(Option.Clear) || BSCoreInput.GetButton(Option.Fire))
		{
			if (!hasShownAll)
			{
				Time.timeScale = 2f;
			}
			if (!hasShownAll)
			{
				return;
			}
			Time.timeScale = 1f;
			Object[] array = Object.FindObjectsOfType(typeof(GameObject));
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = (GameObject)array[i];
				if (!(gameObject.name == "GameManager") && !(gameObject.transform.root.name == "SoundManager") && !(gameObject.name == "Controlls") && !(gameObject.name == "Rewired Input Manager") && !(gameObject.name == "UICanvas") && !(gameObject.name == "EventSystem"))
				{
					Object.Destroy(gameObject);
				}
			}
			Object.Instantiate(inGameAAR);
		}
		else
		{
			Time.timeScale = 1f;
		}
	}
}
