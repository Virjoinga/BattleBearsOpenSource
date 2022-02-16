using System.Collections;
using UnityEngine;

public class TutorialEvent : MonoBehaviour
{
	public TextMesh tutorialText;

	public GameObject[] enemies;

	public GameObject[] enemySpawn;

	public GameObject wil;

	public GameObject wilSpawn;

	public GameObject nuggs;

	public GameObject nuggsSpawn;

	private int partIndex;

	private Material textMat;

	private bool specBool;

	private int huggyCount;

	private int nuggsCount;

	private int wilCount;

	private int bombCount;

	private bool firstWil;

	private string endString;

	private void Start()
	{
		GameManager.Instance.inTutorial = true;
		textMat = Resources.Load("Font_Helv_Yellow") as Material;
		StartCoroutine("eventOne");
		endString = "Well, that is most of what you need to know!";
	}

	private void Update()
	{
		if (!specBool)
		{
			return;
		}
		if (partIndex == 2 && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
		{
			StartCoroutine("timeWait");
		}
		if (partIndex == 3 && huggyCount <= 0)
		{
			StartCoroutine("eventThree");
			specBool = false;
		}
		if (partIndex == 4 && huggyCount <= 0)
		{
			StartCoroutine("eventFour");
			specBool = false;
		}
		if (partIndex == 5 && nuggsCount <= 0)
		{
			StartCoroutine("eventFive");
			specBool = false;
		}
		if (partIndex != 6)
		{
			return;
		}
		if (wilCount <= 0)
		{
			if (!firstWil)
			{
				StartCoroutine("respawnWil");
				firstWil = true;
				specBool = false;
			}
			else
			{
				StartCoroutine("wilFail");
				specBool = false;
			}
		}
		if (bombCount <= 0)
		{
			StartCoroutine("bombExplode");
			specBool = false;
		}
	}

	public IEnumerator respawnWil()
	{
		StopCoroutine("waitForBomb");
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_19") as AudioClip);
		yield return StartCoroutine("wordType", "That's what your NOT supposed to do!");
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_18") as AudioClip);
		yield return StartCoroutine("wordType", "Let's try that again.");
		GameObject myWil = Object.Instantiate(wil, wilSpawn.transform.position, wilSpawn.transform.rotation) as GameObject;
		myWil.name = wil.name + "_" + wilSpawn.name;
		StartCoroutine("waitForBomb");
		wilCount = 1;
		bombCount = 1;
		specBool = true;
	}

	public IEnumerator wilFail()
	{
		yield return StartCoroutine("wordType", "I guess you don't need to learn this to win THE GAME");
		StartCoroutine("eventSix");
	}

	public IEnumerator bombExplode()
	{
		yield return StartCoroutine("lightUp");
		StartCoroutine("eventSix");
	}

	public IEnumerator timeWait()
	{
		specBool = false;
		yield return new WaitForSeconds(0f);
		StartCoroutine("eventTwo");
	}

	public IEnumerator eventOne()
	{
		yield return StartCoroutine(GameManager.Instance.hudController.scaleCrosshairs());
		tutorialText.transform.localPosition = new Vector3(104.469f, 8.66f, 18.7411f);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_1") as AudioClip);
		yield return StartCoroutine("wordType", "Pre-compiled Learning Event!");
		partIndex = 1;
		yield return new WaitForSeconds(0.5f);
		tutorialText.transform.localPosition = new Vector3(103.08f, 10f, 17f);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_2") as AudioClip);
		yield return StartCoroutine("wordType", "To aim, place your finger anywhere on screen and drag around.");
		specBool = true;
		partIndex = 2;
	}

	public IEnumerator eventTwo()
	{
		yield return StartCoroutine("fadeText");
		partIndex = 3;
		GameObject enemy = Object.Instantiate(enemies[0], enemySpawn[0].transform.position, enemySpawn[0].transform.rotation) as GameObject;
		enemy.name = enemies[0].name + "_" + enemySpawn[0].name;
		huggyCount++;
		yield return new WaitForSeconds(1f);
		yield return StartCoroutine("slowAndDark");
		GameManager.Instance.isTutorial = true;
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_3") as AudioClip);
		yield return StartCoroutine("wordType", "This is a deadly huggable.");
		yield return StartCoroutine("fadeText");
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_4") as AudioClip);
		yield return StartCoroutine("wordType", "Hold the crosshairs over the target until it explodes!");
		yield return StartCoroutine("fadeText");
		GameManager.Instance.isTutorial = false;
		yield return StartCoroutine("speedUp");
		specBool = true;
	}

	public IEnumerator eventThree()
	{
		StopCoroutine("eventTwo");
		yield return StartCoroutine("lightUp");
		partIndex = 4;
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_5") as AudioClip);
		yield return StartCoroutine("wordType", "Bake the same color to increase your points!");
		yield return StartCoroutine("wordType", "White Huggables can be used to chain combos even among different colors.");
		yield return StartCoroutine("fadeText");
		GameObject enemy = Object.Instantiate(enemies[1], enemySpawn[0].transform.position, enemySpawn[0].transform.rotation) as GameObject;
		enemy.name = enemies[1].name + "_" + enemySpawn[0].name;
		huggyCount++;
		GameObject enemy2 = Object.Instantiate(enemies[1], enemySpawn[1].transform.position, enemySpawn[1].transform.rotation) as GameObject;
		enemy2.name = enemies[1].name + "_" + enemySpawn[1].name;
		huggyCount++;
		specBool = true;
		yield return StartCoroutine("darkDown");
	}

	public IEnumerator eventFour()
	{
		StopCoroutine("eventTwo");
		StopCoroutine("eventThree");
		yield return StartCoroutine("lightUp");
		partIndex = 5;
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_6") as AudioClip);
		yield return StartCoroutine("wordType", "They may seem stupid, but they are");
		yield return new WaitForSeconds(1f);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_7") as AudioClip);
		yield return StartCoroutine("wordType", "Speaking of stupid...");
		GameObject myNuggs = Object.Instantiate(nuggs, nuggsSpawn.transform.position, nuggsSpawn.transform.rotation) as GameObject;
		myNuggs.name = nuggs.name + "_fly1";
		nuggsCount++;
		yield return StartCoroutine("fadeText");
		yield return new WaitForSeconds(1.5f);
		GameManager.Instance.isTutorial = true;
		yield return StartCoroutine("slowAndDark");
		tutorialText.transform.localPosition = new Vector3(103.08f, 56.04f, 20.07f);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_8") as AudioClip);
		yield return StartCoroutine("wordType", "This is a Nuggs");
		tutorialText.transform.localPosition = new Vector3(103.08f, 56.04f, 16.728f);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_9") as AudioClip);
		yield return StartCoroutine("wordType", "Baking this little fella will result in temporary abilities.");
		yield return StartCoroutine("fadeText");
		GameManager.Instance.isTutorial = false;
		yield return StartCoroutine("speedUp");
		specBool = true;
	}

	public IEnumerator eventFive()
	{
		StopCoroutine("eventTwo");
		StopCoroutine("eventThree");
		StopCoroutine("eventFour");
		yield return StartCoroutine("lightUp");
		partIndex = 6;
		tutorialText.transform.localPosition = new Vector3(103.08f, 56.04f, 17f);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_10") as AudioClip);
		yield return StartCoroutine("wordType", "Meanwhile, Wil is running around doing what I can only assume is helping...");
		yield return new WaitForSeconds(2f);
		tutorialText.transform.localPosition = new Vector3(105.15f, 56.04f, 13.12f);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_11") as AudioClip);
		yield return StartCoroutine("wordType", "Don't bake him!");
		yield return StartCoroutine("darkDown");
		yield return StartCoroutine("fadeText");
		GameObject myWil = Object.Instantiate(wil, wilSpawn.transform.position, wilSpawn.transform.rotation) as GameObject;
		myWil.name = wil.name + "_" + wilSpawn.name;
		StartCoroutine("waitForBomb");
		wilCount++;
		bombCount++;
		specBool = true;
	}

	public IEnumerator waitForBomb()
	{
		yield return new WaitForSeconds(3.5f);
		tutorialText.transform.localPosition = new Vector3(103.081f, 56.04f, 14.64f);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_12") as AudioClip);
		yield return StartCoroutine("wordType", "You can bake DA BOMB to clear the screen of enemies!");
	}

	public IEnumerator eventSix()
	{
		StopCoroutine("eventTwo");
		StopCoroutine("eventThree");
		StopCoroutine("eventFour");
		StopCoroutine("eventFive");
		partIndex = 7;
		yield return StartCoroutine("lightUp");
		yield return StartCoroutine("fadeText");
		if (GameManager.Instance.currentGameMode == GameMode.TIME)
		{
			StartCoroutine("time");
		}
		else if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			StartCoroutine("score");
		}
	}

	public IEnumerator time()
	{
		GameManager.Instance.hudController.sexRobots("TimeAttack");
		yield return StartCoroutine("darkDown");
		tutorialText.transform.localPosition = new Vector3(103.08f, 10f, 18.2f);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_13a") as AudioClip);
		yield return StartCoroutine("wordType", "You have 60 seconds to get the highest score possible");
		yield return new WaitForSeconds(1.5f);
		tutorialText.transform.localPosition = new Vector3(104.9109f, 10f, 17.34687f);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_14") as AudioClip);
		yield return StartCoroutine("wordType", "Baking this ^ pair will increase your overall multiplier");
		yield return new WaitForSeconds(1.5f);
		tutorialText.transform.localPosition = new Vector3(103.08f, 10f, 17f);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_15") as AudioClip);
		yield return StartCoroutine("wordType", "You can bake the huggables in any order but you must get these two colors in a row!");
		yield return StartCoroutine("fadeText");
		tutorialText.transform.localPosition = new Vector3(103.08f, 10f, 17f);
		yield return StartCoroutine("fadeText");
		yield return StartCoroutine("lightUp");
		tutorialText.transform.localPosition = new Vector3(103.08f, 10f, 16.24f);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_16") as AudioClip);
		yield return StartCoroutine("wordType", endString);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_17") as AudioClip);
		yield return StartCoroutine("fadeText");
		yield return new WaitForSeconds(2.5f);
		GameManager.Instance.tutorialTime = true;
		endTutorial();
	}

	public IEnumerator score()
	{
		GameManager.Instance.hudController.sexRobots("Survival_Slide");
		tutorialText.transform.localPosition = new Vector3(103.12f, 56.04f, 17.4f);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_16") as AudioClip);
		yield return StartCoroutine("wordType", endString);
		SoundController.Instance.playClip(Resources.Load("Abbi Tutorial/abbi_17") as AudioClip);
		yield return StartCoroutine("fadeText");
		GameManager.Instance.tutorialScore = true;
		endTutorial();
	}

	public IEnumerator slowAndDark()
	{
		Material oliver = Resources.Load("OliverMat") as Material;
		Material environment = Resources.Load("Environment Tile") as Material;
		yield return new WaitForSeconds(0.25f);
		for (int loops = 64; loops > 0; loops--)
		{
			GameManager.Instance.enemySpeed = Mathf.Lerp(GameManager.Instance.enemySpeed, 0f, 0.05f);
			float dark = Mathf.Lerp(environment.color.r, 0.5f, 0.05f);
			oliver.SetColor("_Color", new Color(dark, dark, dark, 1f));
			environment.SetColor("_Color", new Color(dark, dark, dark, 1f));
			yield return new WaitForSeconds(0.01f);
		}
		GameManager.Instance.animC.pause();
		GameManager.Instance.enemySpeed = 0f;
	}

	public IEnumerator speedUp()
	{
		for (int loops = 64; loops > 0; loops--)
		{
			GameManager.Instance.enemySpeed = Mathf.Lerp(GameManager.Instance.enemySpeed, 1f, 0.05f);
			yield return new WaitForSeconds(0.01f);
		}
		GameManager.Instance.animC.idle(false);
		GameManager.Instance.enemySpeed = 1f;
	}

	public IEnumerator lightUp()
	{
		Material oliver = Resources.Load("OliverMat") as Material;
		Material environment = Resources.Load("Environment Tile") as Material;
		yield return new WaitForSeconds(0.25f);
		for (int loops = 65; loops > 0; loops--)
		{
			float dark = Mathf.Lerp(environment.color.r, 1f, 0.05f);
			oliver.SetColor("_Color", new Color(dark, dark, dark, 1f));
			environment.SetColor("_Color", new Color(dark, dark, dark, 1f));
			yield return new WaitForSeconds(0.01f);
		}
		oliver.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
		environment.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
	}

	public IEnumerator darkDown()
	{
		Material oliver = Resources.Load("OliverMat") as Material;
		Material environment = Resources.Load("Environment Tile") as Material;
		yield return new WaitForSeconds(0.25f);
		for (int loops = 65; loops > 0; loops--)
		{
			float dark = Mathf.Lerp(environment.color.r, 0.5f, 0.05f);
			oliver.SetColor("_Color", new Color(dark, dark, dark, 1f));
			environment.SetColor("_Color", new Color(dark, dark, dark, 1f));
			yield return new WaitForSeconds(0.01f);
		}
		oliver.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1f));
		environment.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1f));
	}

	public IEnumerator wordType(string words)
	{
		tutorialText.alignment = TextAlignment.Left;
		tutorialText.anchor = TextAnchor.UpperLeft;
		tutorialText.characterSize = 0.15f;
		tutorialText.lineSpacing = 0.75f;
		tutorialText.text = string.Empty;
		textMat.SetColor("_Color", new Color(textMat.color.r, textMat.color.g, textMat.color.b, 1f));
		string temp = string.Empty;
		string endComplete2 = string.Empty;
		string[] spaceArray = words.Split(' ');
		for (int j = 0; j < spaceArray.Length; j++)
		{
			if (temp.Length + spaceArray[j].Length > 18)
			{
				endComplete2 = endComplete2 + temp + "@";
				temp = spaceArray[j] + " ";
			}
			else
			{
				temp = temp + spaceArray[j] + " ";
			}
		}
		endComplete2 += temp;
		char[] letters = endComplete2.ToCharArray();
		for (int i = 0; i < letters.Length; i++)
		{
			if (letters[i].Equals('@'))
			{
				tutorialText.text += "\n";
			}
			else
			{
				tutorialText.text += letters[i];
			}
			yield return new WaitForSeconds(0.005f);
		}
		yield return new WaitForSeconds(1.5f);
	}

	public IEnumerator specialWait()
	{
		yield return StartCoroutine("wordType", "Notice that the crosshair can aim around the screen without your finger being directly on top of it.");
		specBool = true;
	}

	public IEnumerator fadeText()
	{
		for (byte loops = 12; loops > 0; loops = (byte)(loops - 1))
		{
			float alpha = Mathf.Lerp(textMat.color.a, 0f, 0.35f);
			textMat.SetColor("_Color", new Color(textMat.color.r, textMat.color.g, textMat.color.b, alpha));
			yield return new WaitForSeconds(0.05f);
		}
	}

	public void huggyDead()
	{
		huggyCount--;
	}

	public void nuggsDead()
	{
		nuggsCount--;
	}

	public void wilDown()
	{
		wilCount--;
	}

	public void bombDown()
	{
		bombCount--;
	}

	public void endTutorial()
	{
		GameManager.Instance.inTutorial = false;
		if (GameManager.Instance.currentGameMode == GameMode.TIME)
		{
			GameManager.Instance.tutorialTime = true;
		}
		if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			GameManager.Instance.tutorialScore = true;
		}
		GameManager.Instance.enemySpeed = 1f;
		GameManager.Instance.tutorialComplete = true;
		GameManager.Instance.isTutorial = false;
		GameManager.Instance.canHasWil = true;
		GameManager.Instance.isWilOut = false;
		GameManager.Instance.canHasNuggs = true;
		GameObject gameObject = GameObject.Find("MainMenu");
		gameObject.SendMessage("tronRoom", SendMessageOptions.DontRequireReceiver);
		GameManager.Instance.hudController.tacomOnline();
		Object.Destroy(base.gameObject);
	}

	public IEnumerator hit()
	{
		if (partIndex == 3)
		{
			yield return StartCoroutine("wordType", "er... lets try that again shall we?");
			GameObject enemy = Object.Instantiate(enemies[0], enemySpawn[0].transform.position, enemySpawn[0].transform.rotation) as GameObject;
			enemy.name = enemies[0].name + "_" + enemySpawn[0].name;
			yield return StartCoroutine("fadeText");
		}
		if (partIndex == 4)
		{
			specBool = false;
			huggyCount = 0;
			GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject g in array)
			{
				Object.Destroy(g);
			}
			yield return StartCoroutine("wordType", "Maybe when you are older");
			yield return StartCoroutine("fadeText");
			StartCoroutine("eventFour");
		}
		if (partIndex == 5)
		{
			yield return StartCoroutine("wordType", "Well... i am pretty sure youâ€™ll do better next time");
			specBool = false;
			nuggsCount = 0;
			StartCoroutine("eventFive");
		}
	}

	public void skipEvent()
	{
		if (partIndex != 9)
		{
			partIndex = 9;
			endTutorial();
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject obj in array)
		{
			Object.Destroy(obj);
		}
	}

	public void backEvent()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject obj in array)
		{
			Object.Destroy(obj);
		}
		StopAllCoroutines();
		specBool = false;
		GameManager.Instance.isTutorial = true;
		StartCoroutine("lightUp");
		StartCoroutine("speedUp");
		StartCoroutine("fadeText");
		partIndex = 0;
		switch (partIndex)
		{
		case 0:
		case 1:
		case 2:
		case 3:
			StartCoroutine("eventOne");
			partIndex = 0;
			break;
		case 4:
			StartCoroutine("eventTwo");
			partIndex = 3;
			break;
		case 5:
			StartCoroutine("eventThree");
			partIndex = 4;
			break;
		case 6:
			StartCoroutine("eventFour");
			partIndex = 5;
			break;
		case 7:
			StartCoroutine("eventFive");
			partIndex = 6;
			break;
		}
	}
}
