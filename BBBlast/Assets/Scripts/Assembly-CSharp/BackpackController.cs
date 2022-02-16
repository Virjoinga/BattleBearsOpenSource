using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackpackController : MonoBehaviour
{
	public enum DescriptionMenuState
	{
		DOWN,
		HALF,
		UP
	}

	public int skinIndex;

	public int helmIndex;

	public int armorIndex;

	private int backpackIndex;

	private int levelIndex;

	private Touch myTouch;

	private float myTime;

	private bool coolDown;

	public Transform bear;

	public GameObject jungle;

	public GameObject sewer;

	public GameObject cliff;

	public GameObject pirate;

	private bool alphaTime;

	public Material backWall;

	public Material glassFloor;

	public GameObject oliver;

	public GameObject[] arms;

	public int[] armPrices;

	private GameObject currArm;

	public Animation backpackCamera;

	public AudioClip[] buttonPress;

	public AudioClip share;

	private bool up;

	private bool up2;

	private bool down;

	private bool down2;

	private bool left;

	private bool right;

	private bool left2;

	private float[] touchArr = new float[5];

	private int touchArrIndex;

	private float rotationSpeed;

	private bool rotateLeft;

	private bool rotateRight;

	public GameObject descriptionDisplay;

	public TextMesh infoDescription;

	public TextMesh titleDescription;

	public TextMesh objectiveDescription;

	public TextMesh bonusDescription;

	public GameObject infoButton;

	public string[] titleSkinArr;

	public int[] objectiveSkinArr;

	private bool[] purchasedSkin;

	public string[] bonusSkinArr;

	public string[] titleHelmArr;

	public int[] objectiveHelmArr;

	private bool[] purchasedHelm;

	public string[] bonusHelmArr;

	public string[] titleChestArr;

	public int[] objectiveChestArr;

	private bool[] purchasedChest;

	public string[] bonusChestArr;

	public string[] titleLevelArr;

	public int[] objectiveLevelArr;

	private bool[] purchasedLevel;

	public string[] bonusLevelArr;

	public string[] titleArmArr;

	public int[] objectiveArmArr;

	private bool[] purchasedArm;

	public string[] bonusArmArr;

	public GameObject[] mouths;

	public MeshRenderer[] helmArr;

	public SkinnedMeshRenderer[] armorArr;

	public string objectiveString;

	public string bonusString;

	public Animation oliverAnimation;

	public string lastHit;

	public GameObject[] oliverEyes;

	public GameObject fader;

	private bool isMiniMicrowave;

	public GameObject miniMicrowave;

	public GameObject huggableHead;

	private bool isUp;

	public TextMesh timeAtkHighScore;

	public TextMesh survivalHighScore;

	public GameObject info;

	private bool descriptionUp;

	public GameObject theLock;

	private UnityEngine.Object[] beemColors;

	private float ratio;

	private string itemCategory = "head";

	private bool isPurchaseUp;

	public GameObject jpgEnc;

	private DescriptionMenuState descMenuPos;

	private Enemy currentBeamSelected;

	private void Start()
	{
		descMenuPos = DescriptionMenuState.DOWN;
		changeDescMenuState(DescriptionMenuState.DOWN);
		purchasedSkin = new bool[objectiveSkinArr.Length];
		purchasedHelm = new bool[objectiveHelmArr.Length];
		purchasedChest = new bool[objectiveChestArr.Length];
		purchasedLevel = new bool[objectiveLevelArr.Length];
		purchasedArm = new bool[objectiveArmArr.Length];
		purchasedSkin[0] = true;
		purchasedArm[8] = true;
		purchasedChest[2] = true;
		purchasedHelm[2] = true;
		purchasedLevel[0] = true;
		purchasedLevel[1] = true;
		purchasedLevel[2] = false;
		purchasedLevel[3] = false;
		ratio = (float)Screen.width / (float)Screen.height;
		base.gameObject.transform.localScale = new Vector3(ratio * 1.5f * base.gameObject.transform.localScale.x, base.gameObject.transform.localScale.y, base.gameObject.transform.localScale.z);
		GameObject gameObject = GameObject.Find("Fader");
		skinIndex = PlayerPrefs.GetInt("skin");
		helmIndex = PlayerPrefs.GetInt("helm");
		armorIndex = PlayerPrefs.GetInt("armor");
		if (PlayerPrefs.GetInt("beamColor") != 0)
		{
			backpackIndex = PlayerPrefs.GetInt("beamColor");
		}
		else
		{
			backpackIndex = 0;
		}
		Debug.Log("The backpackIndex on Start(): " + backpackIndex);
		levelIndex = 0;
		setupDescription(helmIndex, titleHelmArr, objectiveHelmArr, bonusHelmArr);
		backWall.SetColor("_Color", new Color(backWall.color.r, backWall.color.g, backWall.color.b, 1f));
		gameObject.SendMessage("StartFader", true);
		for (int i = 0; i < helmArr.Length; i++)
		{
			if (!(helmArr[i] == null))
			{
				if (helmIndex != i)
				{
					helmArr[i].enabled = false;
				}
				else
				{
					helmArr[i].enabled = true;
				}
			}
		}
		for (int j = 0; j < armorArr.Length; j++)
		{
			if (!(armorArr[j] == null))
			{
				if (armorIndex != j)
				{
					armorArr[j].enabled = false;
				}
				else
				{
					armorArr[j].enabled = true;
				}
			}
		}
		GameObject[] array = mouths;
		foreach (GameObject gameObject2 in array)
		{
			//gameObject2.active = false;
			gameObject2.SetActive(false);
		}
		ShowMicroWave(false);
		//info.active = false;
		info.SetActive(false);
		beemColors = Resources.LoadAll("beem", typeof(Texture));
		string @string = PlayerPrefs.GetString("unlockedItems");
		if (@string.Contains("chest"))
		{
			for (int l = 0; l < purchasedChest.Length; l++)
			{
				if (@string.Contains("chest" + l))
				{
					purchasedChest[l] = true;
				}
			}
		}
		if (@string.Contains("helm"))
		{
			for (int m = 0; m < purchasedHelm.Length; m++)
			{
				if (@string.Contains("helm" + m))
				{
					purchasedHelm[m] = true;
				}
			}
		}
		if (@string.Contains("skin"))
		{
			for (int n = 0; n < purchasedSkin.Length; n++)
			{
				if (@string.Contains("skin" + n))
				{
					purchasedSkin[n] = true;
				}
			}
		}
		if (@string.Contains("arm"))
		{
			for (int num = 0; num < purchasedArm.Length; num++)
			{
				if (@string.Contains("arm" + num))
				{
					purchasedArm[num] = true;
				}
			}
		}
		if (!@string.Contains("level"))
		{
			return;
		}
		for (int num2 = 0; num2 < purchasedLevel.Length; num2++)
		{
			if (@string.Contains("level" + num2))
			{
				purchasedLevel[num2] = true;
			}
		}
	}

	public void ButtonClick(string name)
	{
		switch (name)
		{
		case "skinLeft":
			if (GameManager.Instance.canVibrate)
			{
				Handheld.Vibrate();
			}
			skinCycle(false);
			StartCoroutine("buttonPressed", name);
			SoundController.Instance.playClip(buttonPress[0]);
			break;
		case "skinRight":
			if (GameManager.Instance.canVibrate)
			{
				Handheld.Vibrate();
			}
			skinCycle(true);
			StartCoroutine("buttonPressed", name);
			SoundController.Instance.playClip(buttonPress[0]);
			break;
		case "HelmLeft":
			if (GameManager.Instance.canVibrate)
			{
				Handheld.Vibrate();
			}
			helmCycle(false);
			StartCoroutine("buttonPressed", name);
			SoundController.Instance.playClip(buttonPress[1]);
			break;
		case "HelmRight":
			if (GameManager.Instance.canVibrate)
			{
				Handheld.Vibrate();
			}
			helmCycle(true);
			StartCoroutine("buttonPressed", name);
			SoundController.Instance.playClip(buttonPress[0]);
			break;
		case "ArmorLeft":
			if (GameManager.Instance.canVibrate)
			{
				Handheld.Vibrate();
			}
			armorCycle(false);
			StartCoroutine("buttonPressed", name);
			SoundController.Instance.playClip(buttonPress[0]);
			break;
		case "ArmorRight":
			if (GameManager.Instance.canVibrate)
			{
				Handheld.Vibrate();
			}
			armorCycle(true);
			StartCoroutine("buttonPressed", name);
			SoundController.Instance.playClip(buttonPress[1]);
			break;
		case "MicrowaveLeft":
			if (GameManager.Instance.canVibrate)
			{
				Handheld.Vibrate();
			}
			backpackCycle(false);
			StartCoroutine("buttonPressed", name);
			SoundController.Instance.playClip(buttonPress[1]);
			break;
		case "MicrowaveRight":
			if (GameManager.Instance.canVibrate)
			{
				Handheld.Vibrate();
			}
			backpackCycle(true);
			StartCoroutine("buttonPressed", name);
			SoundController.Instance.playClip(buttonPress[1]);
			break;
		case "Back":
			SoundController.Instance.playClip(buttonPress[1]);
			if (GameManager.Instance.canVibrate)
			{
				Handheld.Vibrate();
			}
			StartCoroutine("buttonPressed", name);
			updatePersona();
			StartCoroutine("delayedLoadLevel");
			PlayerPrefs.Save();
			break;
		case "SavePhoto":
			if (GameManager.Instance.canVibrate)
			{
				Handheld.Vibrate();
			}
			StartCoroutine("buttonPressed", name);
			PhotoHandler("facebook");
			break;
		case "oliverArea":
			rotateBear();
			break;
		case "LevelLeft":
			if (GameManager.Instance.canVibrate)
			{
				Handheld.Vibrate();
			}
			levelCycle(false);
			StartCoroutine("buttonPressed", name);
			SoundController.Instance.playClip(buttonPress[0]);
			break;
		case "LevelRight":
			if (GameManager.Instance.canVibrate)
			{
				Handheld.Vibrate();
			}
			levelCycle(true);
			StartCoroutine("buttonPressed", name);
			SoundController.Instance.playClip(buttonPress[1]);
			break;
		case "PopUp":
			if (GameManager.Instance.canVibrate)
			{
				Handheld.Vibrate();
			}
			StopCoroutine("descriptionInfo");
			theLock.active = false;
			StartCoroutine("descriptionInfo");
			SoundController.Instance.playClip(buttonPress[1]);
			break;
		case "Description":
			SoundController.Instance.playClip(buttonPress[1]);
			if (isPurchaseUp)
			{
				spendJoules();
				infoDescription.text = string.Empty;
			}
			break;
		case "joules":
			TapJoyManager.Instance.ShowOffers();
			break;
		}
	}

	public void spendJoules()
	{
		if (PlayerPrefs.GetInt("joules") < 1)
		{
			return;
		}
		int itemNum = 0;
		switch (itemCategory)
		{
		default:
			return;
		case "skin":
			if (!purchasedSkin[skinIndex])
			{
				if (PlayerPrefs.GetInt("joules") < 900)
				{
					Alerts alerts3 = base.gameObject.AddComponent<Alerts>();
					alerts3.hasEnough = false;
					return;
				}
				TapJoyManager.Instance.SpendJoules(900);
				purchasedSkin[skinIndex] = true;
				itemNum = skinIndex;
			}
			break;
		case "helm":
			if (!purchasedHelm[helmIndex])
			{
				if (PlayerPrefs.GetInt("joules") < 600)
				{
					Alerts alerts4 = base.gameObject.AddComponent<Alerts>();
					alerts4.hasEnough = false;
					return;
				}
				TapJoyManager.Instance.SpendJoules(600);
				purchasedHelm[helmIndex] = true;
				itemNum = helmIndex;
			}
			break;
		case "chest":
			if (!purchasedChest[armorIndex])
			{
				if (PlayerPrefs.GetInt("joules") < 300)
				{
					Alerts alerts2 = base.gameObject.AddComponent<Alerts>();
					alerts2.hasEnough = false;
					return;
				}
				purchasedChest[armorIndex] = true;
				itemNum = armorIndex;
			}
			break;
		case "arm":
			if (!purchasedArm[backpackIndex])
			{
				if (PlayerPrefs.GetInt("joules") < armPrices[backpackIndex])
				{
					Alerts alerts5 = base.gameObject.AddComponent<Alerts>();
					alerts5.hasEnough = false;
					displayPurchaseInfo(itemCategory);
					return;
				}
				TapJoyManager.Instance.SpendJoules(armPrices[backpackIndex]);
				purchasedArm[backpackIndex] = true;
				itemNum = backpackIndex;
				GameManager.Instance.beemColor = currentBeamSelected;
			}
			break;
		case "level":
			if (!purchasedLevel[levelIndex])
			{
				if (PlayerPrefs.GetInt("joules") < 1000)
				{
					Alerts alerts = base.gameObject.AddComponent<Alerts>();
					alerts.hasEnough = false;
					displayPurchaseInfo(itemCategory);
					return;
				}
				TapJoyManager.Instance.SpendJoules(1000);
				purchasedLevel[levelIndex] = true;
				itemNum = levelIndex;
			}
			break;
		}
		changeDescMenuState(DescriptionMenuState.DOWN);
		StartCoroutine(checkWWW(itemNum));
		isPurchaseUp = false;
		StopCoroutine("purchaseInfo");
		updatePersona();
	}

	public IEnumerator checkWWW(int itemNum)
	{
		WWWForm spend = new WWWForm();
		spend.AddField("gameID", "blast");
		spend.AddField("method", "sendItem");
		PlayerPrefs.SetString("unlockedItems", PlayerPrefs.GetString("unlockedItems") + " " + itemCategory + itemNum);
		try
		{
			spend.AddField("ofID", GameManager.Instance.OFGetOFID().ToString());
			if (PlayerPrefs.GetInt("needToSend") != 0)
			{
				StartCoroutine(GameManager.Instance.sendPurchaseInfo());
			}
		}
		catch
		{
			PlayerPrefs.SetInt("needToSend", 1);
			PlayerPrefs.SetString("needSendingItems", PlayerPrefs.GetString("needSendingItems") + " " + itemCategory + itemNum);
			yield break;
		}
		spend.AddField("item", itemCategory + itemNum);
		GameManager.Instance.itemServer = new WWW("http://baoloc9.com/android_blast/zzBlastItems/index.php", spend);
		yield return GameManager.Instance.itemServer;
		PlayerPrefs.SetInt("needToSend", 0);
	}

	public void changeDescMenuState(DescriptionMenuState newState)
	{
		descriptionDisplay.GetComponent<Animation>().Stop();
		switch (descMenuPos)
		{
		case DescriptionMenuState.DOWN:
			switch (newState)
			{
			case DescriptionMenuState.HALF:
				descMenuPos = newState;
				descriptionDisplay.GetComponent<Animation>().Play("UP");
				break;
			case DescriptionMenuState.UP:
				descMenuPos = newState;
				descriptionDisplay.GetComponent<Animation>().Play("Full");
				break;
			}
			break;
		case DescriptionMenuState.HALF:
			switch (newState)
			{
			case DescriptionMenuState.DOWN:
				descMenuPos = newState;
				descriptionDisplay.GetComponent<Animation>().Play("Down");
				break;
			case DescriptionMenuState.UP:
				descMenuPos = newState;
				descriptionDisplay.GetComponent<Animation>().Play("Half");
				break;
			}
			break;
		case DescriptionMenuState.UP:
			switch (newState)
			{
			case DescriptionMenuState.DOWN:
				descMenuPos = newState;
				descriptionDisplay.GetComponent<Animation>().Play("FullDown");
				break;
			case DescriptionMenuState.HALF:
				descMenuPos = newState;
				descriptionDisplay.GetComponent<Animation>().Play("BackToHalf");
				break;
			}
			break;
		}
	}

	private bool isPurchased(string itemCat)
	{
		switch (itemCat)
		{
		case "helm":
			return purchasedHelm[helmIndex];
		case "chest":
			return purchasedChest[armorIndex];
		case "skin":
			return purchasedSkin[skinIndex];
		case "arm":
			return purchasedArm[backpackIndex];
		case "level":
			return purchasedLevel[levelIndex];
		default:
			return false;
		}
	}

	private void displayPurchaseInfo(string itemCat)
	{
		switch (itemCat)
		{
		case "helm":
			StartCoroutine("purchaseInfo", "BUY/NOW/FOR/\n600/JOULES");
			break;
		case "chest":
			StartCoroutine("purchaseInfo", "BUY/NOW/FOR/\n300/JOULES");
			break;
		case "skin":
			StartCoroutine("purchaseInfo", "BUY/NOW/FOR/\n900/JOULES");
			break;
		case "arm":
			StartCoroutine("purchaseInfo", "BUY/NOW/FOR/\n" + armPrices[backpackIndex] + "/JOULES");
			break;
		case "level":
			StartCoroutine("purchaseInfo", "BUY/NOW/FOR/\n1000/JOULES");
			break;
		}
	}

	public IEnumerator descriptionInfo()
	{
		string myString2 = string.Empty;
		isPurchaseUp = false;
		switch (descMenuPos)
		{
		case DescriptionMenuState.DOWN:
		{
			infoDescription.text = string.Empty;
			changeDescMenuState(DescriptionMenuState.UP);
			infoDescription.transform.localPosition = new Vector3(3.89438f, 2.550936f, -7.168885f);
			yield return new WaitForSeconds(0.3f);
			myString2 = bonusString;
			for (int i = 0; i < myString2.Length; i++)
			{
				infoDescription.text += myString2.Substring(i, 1);
				yield return new WaitForSeconds(0.001f);
			}
			break;
		}
		case DescriptionMenuState.HALF:
		{
			infoDescription.text = string.Empty;
			changeDescMenuState(DescriptionMenuState.UP);
			myString2 = bonusString;
			for (int j = 0; j < myString2.Length; j++)
			{
				infoDescription.text += myString2.Substring(j, 1);
				yield return new WaitForSeconds(0.001f);
			}
			break;
		}
		case DescriptionMenuState.UP:
			infoDescription.text = string.Empty;
			if (isPurchased(itemCategory))
			{
				isPurchaseUp = false;
				changeDescMenuState(DescriptionMenuState.DOWN);
			}
			else
			{
				isPurchaseUp = true;
				changeDescMenuState(DescriptionMenuState.HALF);
				displayPurchaseInfo(itemCategory);
			}
			break;
		default:
			changeDescMenuState(DescriptionMenuState.DOWN);
			break;
		}
		yield return null;
	}

	public IEnumerator purchaseInfo(string text)
	{
		isPurchaseUp = true;
		for (int j = infoDescription.text.Length; j > -1; j--)
		{
			if (infoDescription.text.Length != 0 && infoDescription.text.Length > j)
			{
				infoDescription.text = infoDescription.text.Substring(0, j);
			}
			yield return new WaitForSeconds(0.001f);
		}
		for (int i = 0; i < text.Length; i++)
		{
			infoDescription.text += text.Substring(i, 1);
			yield return new WaitForSeconds(0.001f);
		}
	}

	public void skinCycle(bool forward)
	{
		itemCategory = "skin";
		StopCoroutine("descriptionInfo");
		//info.active = true;
		info.SetActive(true);
		oliverAnimation.Play("armorReaction");
		if (isMiniMicrowave)
		{
			ShowMicroWave(false);
			UnityEngine.Object.Destroy(currArm);
		}
		if (!oliver.active)
		{
			setupDescription(skinIndex, titleSkinArr, objectiveSkinArr, bonusSkinArr);
			//oliver.SetActiveRecursively(true);
			oliver.SetActive(true);
			for (int i = 0; i < oliverEyes.Length; i++)
			{
				oliverEyes[i].active = false;
			}
			GameObject[] array = mouths;
			foreach (GameObject gameObject in array)
			{
				//gameObject.active = false;
				gameObject.SetActive(false);
			}
			return;
		}
		if (forward)
		{
			skinIndex++;
		}
		else
		{
			skinIndex--;
		}
		if (skinIndex > 2)
		{
			skinIndex = 0;
		}
		if (skinIndex < 0)
		{
			skinIndex = 2;
		}
		setupDescription(skinIndex, titleSkinArr, objectiveSkinArr, bonusSkinArr);
		StopCoroutine("purchaseInfo");
		if (PlayerPrefs.GetInt("objectives") > objectiveSkinArr[skinIndex])
		{
			if (purchasedSkin[skinIndex])
			{
				changeDescMenuState(DescriptionMenuState.DOWN);
				infoDescription.text = string.Empty;
			}
			else
			{
				changeDescMenuState(DescriptionMenuState.HALF);
				StartCoroutine("purchaseInfo", "BUY/NOW/FOR/\n900/JOULES");
			}
		}
		else if (descriptionUp)
		{
			StartCoroutine("descriptionInfo");
		}
		updatePersona();
	}

	public void helmCycle(bool forward)
	{
		itemCategory = "helm";
		StopCoroutine("descriptionInfo");
		//info.active = false;
		info.SetActive(false);
		if (skinIndex == 0)
		{
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				oliverAnimation.Play("helmReaction");
			}
			else
			{
				oliverAnimation.Play("helmReaction1");
			}
		}
		else
		{
			oliverAnimation.Play("helmReaction");
		}
		if (isMiniMicrowave)
		{
			ShowMicroWave(false);
			UnityEngine.Object.Destroy(currArm);
		}
		if (!oliver.active)
		{
			setupDescription(helmIndex, titleHelmArr, objectiveHelmArr, bonusHelmArr);
			//oliver.SetActiveRecursively(true);
			oliver.SetActive(true);
			for (int i = 0; i < oliverEyes.Length; i++)
			{
				oliverEyes[i].active = false;
			}
			GameObject[] array = mouths;
			foreach (GameObject gameObject in array)
			{
				//gameObject.active = false;
				gameObject.SetActive(false);
			}
			return;
		}
		if (forward)
		{
			helmIndex++;
		}
		else
		{
			helmIndex--;
		}
		if (helmIndex > 3)
		{
			helmIndex = 0;
		}
		if (helmIndex < 0)
		{
			helmIndex = 3;
		}
		for (int k = 0; k < helmArr.Length; k++)
		{
			if (!(helmArr[k] == null))
			{
				if (helmIndex != k)
				{
					helmArr[k].enabled = false;
				}
				else
				{
					helmArr[k].enabled = true;
				}
			}
		}
		setupDescription(helmIndex, titleHelmArr, objectiveHelmArr, bonusHelmArr);
		StopCoroutine("purchaseInfo");
		if (PlayerPrefs.GetInt("objectives") > objectiveHelmArr[helmIndex])
		{
			if (purchasedHelm[helmIndex])
			{
				changeDescMenuState(DescriptionMenuState.DOWN);
				infoDescription.text = string.Empty;
			}
			else
			{
				changeDescMenuState(DescriptionMenuState.HALF);
				StartCoroutine("purchaseInfo", "BUY/NOW/FOR/\n600/JOULES");
			}
		}
		else if (descriptionUp)
		{
			StartCoroutine("descriptionInfo");
		}
		updatePersona();
	}

	public void armorCycle(bool forward)
	{
		itemCategory = "chest";
		StopCoroutine("descriptionInfo");
		//info.active = false;
		info.SetActive(false);
		if (skinIndex == 0)
		{
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				oliverAnimation.Play("armorReaction");
			}
			else
			{
				oliverAnimation.Play("armorReaction1");
			}
		}
		else
		{
			oliverAnimation.Play("armorReaction");
		}
		if (isMiniMicrowave)
		{
			ShowMicroWave(false);
			UnityEngine.Object.Destroy(currArm);
		}
		if (!oliver.active)
		{
			setupDescription(armorIndex, titleChestArr, objectiveChestArr, bonusChestArr);
			//oliver.SetActiveRecursively(true);
			oliver.SetActive(true);
			for (int i = 0; i < oliverEyes.Length; i++)
			{
				oliverEyes[i].active = false;
			}
			GameObject[] array = mouths;
			foreach (GameObject gameObject in array)
			{
				//gameObject.active = false;
				gameObject.SetActive(false);
			}
			return;
		}
		if (forward)
		{
			armorIndex++;
		}
		else
		{
			armorIndex--;
		}
		if (armorIndex > 3)
		{
			armorIndex = 0;
		}
		if (armorIndex < 0)
		{
			armorIndex = 3;
		}
		setupDescription(armorIndex, titleChestArr, objectiveChestArr, bonusChestArr);
		for (int k = 0; k < armorArr.Length; k++)
		{
			if (!(armorArr[k] == null))
			{
				if (armorIndex != k)
				{
					armorArr[k].enabled = false;
				}
				else
				{
					armorArr[k].enabled = true;
				}
			}
		}
		StopCoroutine("purchaseInfo");
		if (PlayerPrefs.GetInt("objectives") > objectiveChestArr[armorIndex])
		{
			if (purchasedChest[armorIndex])
			{
				changeDescMenuState(DescriptionMenuState.DOWN);
				infoDescription.text = string.Empty;
			}
			else
			{
				changeDescMenuState(DescriptionMenuState.HALF);
				StartCoroutine("purchaseInfo", "BUY/NOW/FOR/\n300/JOULES");
			}
		}
		updatePersona();
	}

	public void backpackCycle(bool forward)
	{
		itemCategory = "arm";
		StopCoroutine("descriptionInfo");
		//info.active = true;
		info.SetActive(true);
		if (forward)
		{
			backpackIndex++;
		}
		else
		{
			backpackIndex--;
		}
		if (backpackIndex > 8)
		{
			backpackIndex = 0;
		}
		else if (backpackIndex < 0)
		{
			backpackIndex = 8;
		}
		setupDescription(backpackIndex, titleArmArr, objectiveArmArr, bonusArmArr);
		if (isMiniMicrowave)
		{
			ShowMicroWave(false);
			UnityEngine.Object.Destroy(currArm);
		}
		bool flag = false;
		StopCoroutine("purchaseInfo");
		if (PlayerPrefs.GetInt("objectives") > objectiveArmArr[backpackIndex])
		{
			if (purchasedArm[backpackIndex])
			{
				infoDescription.text = string.Empty;
				changeDescMenuState(DescriptionMenuState.DOWN);
				flag = true;
			}
			else
			{
				changeDescMenuState(DescriptionMenuState.HALF);
				StartCoroutine("purchaseInfo", "BUY/NOW/FOR/\n" + armPrices[backpackIndex] + "/JOULES");
			}
		}
		else
		{
			updatePersona();
		}
		Material material = Resources.Load("mwBlast") as Material;
		oliverAnimation.Play("IdleFireUpper");
		oliverAnimation.CrossFade("idleLower");
		ShowMicroWave(true);
		switch (backpackIndex)
		{
		case 8:
		{
			GameManager.Instance.beemColor = Enemy.NONE;
			currentBeamSelected = Enemy.NONE;
			material.SetTexture("_MainTex", beemColors[2] as Texture);
			for (int num = infoDescription.text.Length; num > -1; num--)
			{
				if (infoDescription.text.Length != 0 && infoDescription.text.Length > num)
				{
					infoDescription.text = infoDescription.text.Substring(0, num);
				}
			}
			StopCoroutine("descriptionInfo");
			break;
		}
		case 0:
			currentBeamSelected = Enemy.BLU;
			if (flag)
			{
				GameManager.Instance.beemColor = Enemy.BLU;
				material.SetTexture("_MainTex", beemColors[0] as Texture);
				StartCoroutine("descriptionInfo");
			}
			currArm = UnityEngine.Object.Instantiate(arms[0], huggableHead.transform.position, huggableHead.transform.rotation) as GameObject;
			currArm.transform.parent = huggableHead.transform;
			break;
		case 1:
			currentBeamSelected = Enemy.GRN;
			if (flag)
			{
				GameManager.Instance.beemColor = Enemy.GRN;
				material.SetTexture("_MainTex", beemColors[1] as Texture);
				StartCoroutine("descriptionInfo");
			}
			currArm = UnityEngine.Object.Instantiate(arms[1], huggableHead.transform.position, huggableHead.transform.rotation) as GameObject;
			currArm.transform.parent = huggableHead.transform;
			break;
		case 2:
			currentBeamSelected = Enemy.ORG;
			if (flag)
			{
				GameManager.Instance.beemColor = Enemy.ORG;
				material.SetTexture("_MainTex", beemColors[3] as Texture);
				StartCoroutine("descriptionInfo");
			}
			currArm = UnityEngine.Object.Instantiate(arms[2], huggableHead.transform.position, huggableHead.transform.rotation) as GameObject;
			currArm.transform.parent = huggableHead.transform;
			break;
		case 3:
			currentBeamSelected = Enemy.PIN;
			if (flag)
			{
				GameManager.Instance.beemColor = Enemy.PIN;
				material.SetTexture("_MainTex", beemColors[4] as Texture);
				StartCoroutine("descriptionInfo");
			}
			currArm = UnityEngine.Object.Instantiate(arms[3], huggableHead.transform.position, huggableHead.transform.rotation) as GameObject;
			currArm.transform.parent = huggableHead.transform;
			break;
		case 4:
			currentBeamSelected = Enemy.RED;
			if (flag)
			{
				GameManager.Instance.beemColor = Enemy.RED;
				material.SetTexture("_MainTex", beemColors[5] as Texture);
				StartCoroutine("descriptionInfo");
			}
			currArm = UnityEngine.Object.Instantiate(arms[4], huggableHead.transform.position, huggableHead.transform.rotation) as GameObject;
			currArm.transform.parent = huggableHead.transform;
			break;
		case 5:
			currentBeamSelected = Enemy.YEL;
			if (flag)
			{
				GameManager.Instance.beemColor = Enemy.YEL;
				material.SetTexture("_MainTex", beemColors[6] as Texture);
				StartCoroutine("descriptionInfo");
			}
			currArm = UnityEngine.Object.Instantiate(arms[5], huggableHead.transform.position, huggableHead.transform.rotation) as GameObject;
			currArm.transform.parent = huggableHead.transform;
			break;
		case 6:
			currentBeamSelected = Enemy.PUR;
			if (flag)
			{
				GameManager.Instance.beemColor = Enemy.PUR;
				StartCoroutine("descriptionInfo");
			}
			currArm = UnityEngine.Object.Instantiate(arms[6], huggableHead.transform.position, huggableHead.transform.rotation) as GameObject;
			currArm.transform.parent = huggableHead.transform;
			break;
		case 7:
			currentBeamSelected = Enemy.WHI;
			if (flag)
			{
				GameManager.Instance.beemColor = Enemy.WHI;
				StartCoroutine("descriptionInfo");
			}
			currArm = UnityEngine.Object.Instantiate(arms[7], huggableHead.transform.position, huggableHead.transform.rotation) as GameObject;
			currArm.transform.parent = huggableHead.transform;
			break;
		default:
			GameManager.Instance.beemColor = Enemy.NONE;
			currentBeamSelected = Enemy.NONE;
			material.SetTexture("_MainTex", beemColors[2] as Texture);
			break;
		}
		updatePersona();
	}

	public void levelCycle(bool forward)
	{
		itemCategory = "level";
		StopCoroutine("descriptionInfo");
		info.active = false;
		descriptionDisplay.SetActiveRecursively(true);
		if (isMiniMicrowave)
		{
			ShowMicroWave(false);
			UnityEngine.Object.Destroy(currArm);
			oliverAnimation.Play("idle");
		}
		if (!oliver.active)
		{
			setupDescription(levelIndex, titleLevelArr, objectiveLevelArr, bonusLevelArr);
			oliver.SetActiveRecursively(true);
			for (int i = 0; i < oliverEyes.Length; i++)
			{
				oliverEyes[i].active = false;
			}
			GameObject[] array = mouths;
			foreach (GameObject gameObject in array)
			{
				gameObject.active = false;
			}
			return;
		}
		if (forward)
		{
			levelIndex++;
		}
		else
		{
			levelIndex--;
		}
		if (levelIndex > 4)
		{
			levelIndex = 0;
		}
		if (levelIndex < 0)
		{
			levelIndex = 4;
		}
		setupDescription(levelIndex, titleLevelArr, objectiveLevelArr, bonusLevelArr);
		StopCoroutine("purchaseInfo");
		if (PlayerPrefs.GetInt("objectives") > objectiveLevelArr[levelIndex])
		{
			if (purchasedLevel[levelIndex])
			{
				changeDescMenuState(DescriptionMenuState.DOWN);
				infoDescription.text = string.Empty;
			}
			else
			{
				changeDescMenuState(DescriptionMenuState.HALF);
				StartCoroutine("purchaseInfo", "BUY/NOW/FOR/\n1000/JOULES");
			}
		}
		else if (descriptionUp)
		{
			StartCoroutine("descriptionInfo");
		}
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("Level");
		foreach (GameObject obj in array2)
		{
			UnityEngine.Object.Destroy(obj);
		}
		if (levelIndex == 0)
		{
			alphaTime = false;
			GameObject[] array3 = GameObject.FindGameObjectsWithTag("Level");
			foreach (GameObject obj2 in array3)
			{
				UnityEngine.Object.Destroy(obj2);
			}
		}
		else if (levelIndex == 1)
		{
			alphaTime = true;
			GameObject gameObject2 = UnityEngine.Object.Instantiate(jungle, new Vector3(2.2f, -4.5f, -21.66f), Quaternion.Euler(Vector3.zero)) as GameObject;
			gameObject2.transform.localScale = Vector3.one * 0.75f;
		}
		else if (levelIndex == 2)
		{
			alphaTime = true;
			GameObject gameObject3 = UnityEngine.Object.Instantiate(sewer, new Vector3(0f, -4.97f, -20.4f), Quaternion.Euler(0f, 180f, 0f)) as GameObject;
			gameObject3.transform.localScale = Vector3.one * 0.75f;
		}
		else if (levelIndex == 3)
		{
			alphaTime = true;
			GameObject gameObject4 = UnityEngine.Object.Instantiate(cliff, new Vector3(-13.73f, -5.63f, -28.8f), Quaternion.Euler(0f, 181.42f, 0f)) as GameObject;
			gameObject4.transform.localScale = Vector3.one * 0.75f;
		}
		else if (levelIndex == 4)
		{
			alphaTime = true;
			GameObject gameObject5 = UnityEngine.Object.Instantiate(pirate, new Vector3(7.002976f, 0f, -44.45297f), Quaternion.Euler(0f, 44.2048f, 0f)) as GameObject;
			gameObject5.transform.localScale = Vector3.one * 0.75f;
		}
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	public void updatePersona()
	{
		if (PlayerPrefs.GetInt("objectives") > objectiveSkinArr[skinIndex])
		{
			if (purchasedSkin[skinIndex])
			{
				PlayerPrefs.SetInt("skin", skinIndex);
			}
		}
		else
		{
			PlayerPrefs.SetInt("skin", 0);
		}
		if (PlayerPrefs.GetInt("objectives") > objectiveHelmArr[helmIndex])
		{
			if (purchasedHelm[helmIndex])
			{
				PlayerPrefs.SetInt("helm", helmIndex);
			}
		}
		else
		{
			PlayerPrefs.SetInt("helm", 2);
		}
		if (PlayerPrefs.GetInt("objectives") > objectiveChestArr[armorIndex])
		{
			if (purchasedChest[armorIndex])
			{
				PlayerPrefs.SetInt("armor", armorIndex);
			}
		}
		else
		{
			PlayerPrefs.SetInt("armor", 2);
		}
		if (PlayerPrefs.GetInt("objectives") > objectiveArmArr[backpackIndex])
		{
			if (purchasedArm[backpackIndex])
			{
				PlayerPrefs.SetInt("beamColor", backpackIndex);
			}
		}
		else
		{
			PlayerPrefs.SetInt("beamColor", 8);
		}
		Resources.UnloadUnusedAssets();
	}

	private void Update()
	{
		if (!oliverAnimation.isPlaying)
		{
			if (isMiniMicrowave)
			{
				oliverAnimation.Play("IdleFireUpper");
				oliverAnimation.CrossFade("IdleLower");
			}
			else if (skinIndex == 0)
			{
				if (UnityEngine.Random.Range(0, 2) == 0)
				{
					oliverAnimation.Play("idle");
				}
				else
				{
					oliverAnimation.Play("idle1");
				}
			}
			else
			{
				oliverAnimation.Play("idle");
			}
		}
		if (isMiniMicrowave)
		{
			bear.localEulerAngles = Vector3.Lerp(bear.localEulerAngles, Vector3.zero, 0.1f);
		}
		touchArrIndex++;
		if (touchArrIndex >= 5)
		{
			touchArrIndex = 0;
		}
		if (!Application.isEditor)
		{
			if (Input.touchCount > 0)
			{
				myTouch = Input.GetTouch(0);
				touchArr[touchArrIndex] = myTouch.deltaPosition.x;
				Ray ray = Camera.main.ScreenPointToRay(myTouch.position);
				RaycastHit hitInfo;
				bool flag = Physics.Raycast(ray, out hitInfo, float.PositiveInfinity);
				if ((bool)hitInfo.collider)
				{
					if (myTouch.phase == TouchPhase.Began)
					{
						lastHit = hitInfo.collider.gameObject.name;
					}
					if (flag && !coolDown && hitInfo.collider.name != "oliverArea" && myTouch.phase == TouchPhase.Ended && lastHit == hitInfo.collider.gameObject.name)
					{
						ButtonClick(hitInfo.collider.gameObject.name);
					}
					if (!isMiniMicrowave && flag && hitInfo.collider.name == "oliverArea")
					{
						myTime = Time.time;
						coolDown = true;
						rotateBear();
					}
				}
			}
			else
			{
				touchArr[touchArrIndex] = 0f;
			}
		}
		else if (Input.GetMouseButtonDown(0))
		{
			Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
			Ray ray2 = Camera.main.ScreenPointToRay(position);
			RaycastHit hitInfo2;
			Physics.Raycast(ray2.origin, ray2.direction, out hitInfo2, 150f);
			if ((bool)hitInfo2.collider)
			{
				ButtonClick(hitInfo2.collider.gameObject.name);
			}
		}
		if (Time.time > myTime + 0.15f)
		{
			coolDown = false;
		}
		if (alphaTime)
		{
			backWall.SetColor("_Color", new Color(backWall.color.r, backWall.color.g, backWall.color.b, Mathf.Lerp(backWall.color.a, 0f, 0.05f)));
			glassFloor.SetColor("_Color", new Color(glassFloor.color.r, glassFloor.color.g, glassFloor.color.b, Mathf.Lerp(glassFloor.color.a, 0f, 0.05f)));
		}
		else
		{
			backWall.SetColor("_Color", new Color(backWall.color.r, backWall.color.g, backWall.color.b, Mathf.Lerp(backWall.color.a, 1f, 0.05f)));
			glassFloor.SetColor("_Color", new Color(glassFloor.color.r, glassFloor.color.g, glassFloor.color.b, Mathf.Lerp(glassFloor.color.a, 1f, 0.05f)));
		}
		if (rotateRight)
		{
			rotationSpeed -= 0.5f;
		}
		if (rotateLeft)
		{
			rotationSpeed += 0.5f;
		}
		if (rotationSpeed > 0f && rotateRight)
		{
			bear.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime * 45f;
		}
		else
		{
			rotateRight = false;
		}
		if (rotationSpeed < 0f && rotateLeft)
		{
			bear.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime * 45f;
		}
		else
		{
			rotateLeft = false;
		}
	}

	public void rotateBear()
	{
		float num = 0f;
		for (int i = 0; i < touchArr.Length; i++)
		{
			num += touchArr[i];
		}
		rotationSpeed = num / (float)touchArr.Length;
		if (rotationSpeed > 0f)
		{
			rotateRight = true;
			rotateLeft = false;
		}
		else
		{
			rotateLeft = true;
			rotateRight = false;
		}
	}

	public IEnumerator PostImageToFacebook()
	{
		//infoButton.gameObject.active = false;
		infoButton.gameObject.SetActive(false);
		//titleDescription.gameObject.active = false;
		titleDescription.gameObject.SetActive(false);
		//timeAtkHighScore.gameObject.active = true;
		timeAtkHighScore.gameObject.SetActive(true);
		//survivalHighScore.gameObject.active = true;
		survivalHighScore.gameObject.SetActive(true);
		timeAtkHighScore.text = "60/SECS." + PlayerPrefs.GetString("bestTime");
		survivalHighScore.text = "SURVIVAL." + PlayerPrefs.GetString("Survival_BestTime");
		backpackCamera.Play("CameraZoomInBackpack");
		yield return new WaitForSeconds(backpackCamera["CameraZoomInBackpack"].length);
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			SendMessage("ScreenshotEncode");
		}
		backpackCamera.Play("CameraZoomOutBackpack");
		yield return new WaitForSeconds(backpackCamera.GetClip("CameraZoomOutBackpack").length);
		//titleDescription.gameObject.active = true;
		titleDescription.gameObject.SetActive(true);
		//infoButton.gameObject.active = true;
		infoButton.gameObject.SetActive(true);
		//timeAtkHighScore.gameObject.active = false;
		timeAtkHighScore.gameObject.SetActive(false);
		//survivalHighScore.gameObject.active = false;
		survivalHighScore.gameObject.SetActive(false);
	}

	public IEnumerator SaveScreenShotToPhotoAlbum()
	{
		//infoButton.gameObject.active = false;
		infoButton.gameObject.SetActive(false);
		//titleDescription.gameObject.active = false;
		titleDescription.gameObject.SetActive(false);
		//timeAtkHighScore.gameObject.active = true;
		timeAtkHighScore.gameObject.SetActive(true);
		//survivalHighScore.gameObject.active = true;
		survivalHighScore.gameObject.SetActive(true);
		timeAtkHighScore.text = "60/SECS." + PlayerPrefs.GetString("bestTime");
		survivalHighScore.text = "SURVIVAL." + PlayerPrefs.GetString("Survival_BestTime");
		backpackCamera.Play("CameraZoomInBackpack");
		yield return new WaitForSeconds(1.5f);
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
		}
		backpackCamera.Play("CameraZoomOutBackpack");
		//titleDescription.gameObject.active = true;
		titleDescription.gameObject.SetActive(true);
		//infoButton.gameObject.active = true;
		infoButton.gameObject.SetActive(true);
		//timeAtkHighScore.gameObject.active = false;
		timeAtkHighScore.gameObject.SetActive(false);
		//survivalHighScore.gameObject.active = false;
		survivalHighScore.gameObject.SetActive(false);
	}

	public IEnumerator buttonPressed(string s)
	{
		SoundController.Instance.playClip(buttonPress[1]);
		GameObject obj = GameObject.Find(s + "/ButtonPressGradient");
		MeshRenderer meshRender = obj.GetComponent<MeshRenderer>();
		Animation ani = obj.GetComponent<Animation>();
		meshRender.enabled = true;
		ani.Play();
		yield return new WaitForSeconds(ani.clip.length + 0.1f);
		meshRender.enabled = false;
	}

	public string CreateStringBounds(string str, int maxLineNum)
	{
		string text = string.Empty;
		if (str != string.Empty)
		{
			string text2 = string.Empty;
			string[] array = str.Split('\n');
			array = array[0].Split('/');
			for (int i = 0; i < array.Length; i++)
			{
				if (text2.Length + array[i].Length > maxLineNum)
				{
					text = text + text2 + "\n";
					text2 = array[i] + "/";
				}
				else
				{
					text2 = text2 + array[i] + "/";
				}
			}
			text += text2;
		}
		return text;
	}

	public void setupDescription(int index, string[] title, int[] objective, string[] bonus)
	{
		objectiveString = string.Empty;
		//theLock.active = false;
		theLock.SetActive(false);
		bonusString = CreateStringBounds(bonus[index], 13);
		if (bonusString != string.Empty)
		{
			//info.active = true;
			info.SetActive(true);
		}
		else
		{
			//info.active = false;
			info.SetActive(false);
		}
		StartCoroutine("animateDisplayName", title[index]);
	}

	public IEnumerator animateDisplayName(string title)
	{
		int oriLength = title.Length;
		if (oriLength < 15)
		{
			for (int x2 = 0; x2 < 15 - oriLength; x2++)
			{
				title += "/";
			}
		}
		string original = titleDescription.text;
		for (int x = 0; x < 16; x++)
		{
			titleDescription.text = title.Substring(15 - x, x) + original.Substring(0, 15 - x);
			yield return new WaitForSeconds(0.05f);
		}
	}

	public IEnumerator displayPopUp()
	{
		//descriptionDisplay.SetActiveRecursively(true);
		descriptionDisplay.SetActive(true);
		objectiveDescription.text = string.Empty;
		infoDescription.text = string.Empty;
		for (int x2 = 0; x2 < objectiveString.Length; x2++)
		{
			bonusDescription.text += objectiveString.Substring(x2, 1);
			infoDescription.text += objectiveString.Substring(x2, 1);
			yield return new WaitForSeconds(0.01666667f);
		}
		bonusDescription.text += "\n\n";
		infoDescription.text += "\n\n";
		for (int x = 0; x < bonusString.Length; x++)
		{
			bonusDescription.text += bonusString.Substring(x, 1);
			infoDescription.text += bonusString.Substring(x, 1);
			yield return new WaitForSeconds(0.01666667f);
		}
	}

	public IEnumerator delayedLoadLevel()
	{
		GameObject temp = (GameObject)UnityEngine.Object.Instantiate(fader);
		temp.SendMessage("StartFader", false);
		yield return new WaitForSeconds(2f);
		//Application.LoadLevelAsync("MainMenu");
		SceneManager.LoadSceneAsync("MainMenu");
	}

	public void PhotoHandler(string str)
	{
		switch (str)
		{
		case "facebook":
			StartCoroutine(PostImageToFacebook());
			break;
		case "album":
			StartCoroutine(SaveScreenShotToPhotoAlbum());
			break;
		}
	}

	public void ShowMicroWave(bool showMicrowave)
	{
		isMiniMicrowave = showMicrowave;
		//miniMicrowave.SetActiveRecursively(showMicrowave);
		miniMicrowave.SetActive(showMicrowave);
	}
}
