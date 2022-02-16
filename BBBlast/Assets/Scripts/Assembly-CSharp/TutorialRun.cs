using System.Collections;
using UnityEngine;

public class TutorialRun : MonoBehaviour
{
	public TextMesh tutorialText;

	public GameObject[] enemies;

	public GameObject[] enemySpawn;

	public GameObject wil;

	public GameObject wilSpawn;

	public GameObject nuggs;

	public GameObject nuggsSpawn;

	private Material textMat;

	private int huggyCount;

	private int nuggsCount;

	private int wilCount;

	private int bombCount;

	private string curRout;

	private bool next;

	private void Start()
	{
		textMat = Resources.Load("Font_Helv_Yellow") as Material;
		StartCoroutine(runTutorial());
	}

	private IEnumerator runTutorial()
	{
		curRout = "part1";
		yield return StartCoroutine(curRout);
		curRout = "part2";
		yield return StartCoroutine(curRout);
	}

	private IEnumerator part1()
	{
		yield return StartCoroutine(GameManager.Instance.hudController.scaleCrosshairs());
		yield return new WaitForSeconds(3.5f);
		tutorialText.transform.localPosition = new Vector3(103.08f, 10f, 17f);
		yield return StartCoroutine("wordType", "To aim, place your finger anywhere on screen and drag around.");
	}

	private IEnumerator part2()
	{
		yield return StartCoroutine("fadeText");
		GameObject enemy = Object.Instantiate(enemies[0], enemySpawn[0].transform.position, enemySpawn[0].transform.rotation) as GameObject;
		enemy.name = enemies[0].name + "_" + enemySpawn[0].name;
		huggyCount++;
		yield return new WaitForSeconds(1f);
		yield return StartCoroutine("slowAndDark");
		GameManager.Instance.isTutorial = true;
		yield return StartCoroutine("wordType", "This is a deadly huggable.");
		yield return StartCoroutine("fadeText");
		yield return StartCoroutine("wordType", "Hold the crosshairs over the target until it explodes!");
		yield return StartCoroutine("fadeText");
		GameManager.Instance.isTutorial = false;
		yield return StartCoroutine("speedUp");
	}

	private void Update()
	{
	}

	private void skipEvent()
	{
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

	public IEnumerator fadeText()
	{
		for (byte loops = 12; loops > 0; loops = (byte)(loops - 1))
		{
			float alpha = Mathf.Lerp(textMat.color.a, 0f, 0.35f);
			textMat.SetColor("_Color", new Color(textMat.color.r, textMat.color.g, textMat.color.b, alpha));
			yield return new WaitForSeconds(0.05f);
		}
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
}
