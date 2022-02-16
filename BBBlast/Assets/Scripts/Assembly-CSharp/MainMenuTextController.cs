using System.Collections;
using UnityEngine;

public class MainMenuTextController : MonoBehaviour
{
	public TextMesh myText;

	public string[] shortList;

	private void Start()
	{
	}

	private IEnumerator blastScroll()
	{
		yield return new WaitForSeconds(1f);
		string newText = "///////BLAST////////////BATTLE/BEARS";
		int index = newText.Length - 1;
		char[] scrollArray = newText.ToCharArray();
		while (newText.Length > 0)
		{
			myText.text = scrollArray[index - 12].ToString() + scrollArray[index - 11] + scrollArray[index - 10] + scrollArray[index - 9] + scrollArray[index - 8] + scrollArray[index - 7] + scrollArray[index - 6] + scrollArray[index - 5] + scrollArray[index - 4] + scrollArray[index - 3] + scrollArray[index - 2] + scrollArray[index - 1];
			if (index == 16 || index == 15 || index == 14 || index == 13)
			{
				yield return new WaitForSeconds(0.4f);
			}
			else
			{
				yield return new WaitForSeconds(0.2f);
			}
			index--;
			if (index == 11)
			{
				newText = string.Empty;
			}
		}
		StartCoroutine("hurpyDurpy");
	}

	private IEnumerator hurpyDurpy()
	{
		myText.text = "///////BLAST";
		yield return new WaitForSeconds(0.2f);
		myText.text = "o//////BLAST";
		yield return new WaitForSeconds(0.5f);
		myText.text = "n//////BLAST";
		yield return new WaitForSeconds(0.5f);
		myText.text = "m//////BLAST";
		yield return new WaitForSeconds(0.5f);
		myText.text = "l//////BLAST";
		yield return new WaitForSeconds(0.5f);
		myText.text = "///////BLAST";
		yield return new WaitForSeconds(0.25f);
		myText.text = "l//////BLAST";
		yield return new WaitForSeconds(0.25f);
		myText.text = "///////BLASv";
		yield return new WaitForSeconds(0.2f);
		myText.text = "///////BLAvw";
		yield return new WaitForSeconds(0.2f);
		myText.text = "///////BLvwx";
		yield return new WaitForSeconds(0.2f);
		myText.text = "///////Bvwxy";
		yield return new WaitForSeconds(0.2f);
		string newText = "///////vwxyvwxyvwxy";
		int index = 0;
		char[] scrollArray = newText.ToCharArray();
		while (newText.Length > 0)
		{
			myText.text = scrollArray[index].ToString() + scrollArray[index + 1] + scrollArray[index + 2] + scrollArray[index + 3] + scrollArray[index + 4] + scrollArray[index + 5] + scrollArray[index + 6] + scrollArray[index + 7] + scrollArray[index + 8] + scrollArray[index + 9] + scrollArray[index + 10] + scrollArray[index + 11];
			yield return new WaitForSeconds(0.2f);
			index++;
			if (index == newText.Length - 11)
			{
				newText = string.Empty;
			}
		}
		yield return new WaitForSeconds(0.5f);
		myText.text = "............";
		StartCoroutine("objectiveScroll");
	}

	private IEnumerator quickScroll()
	{
		yield return new WaitForSeconds(1.5f);
		int go = 1;
		int index = 0;
		while (go > 0)
		{
			myText.text = shortList[index];
			yield return new WaitForSeconds(0.5f);
			index++;
			if (index == shortList.Length)
			{
				go = 0;
			}
		}
		StartCoroutine("objectiveScroll");
	}

	public IEnumerator scrollingText(string text)
	{
		text.ToUpper();
		string initialTest = myText.text;
		string[] prePostPend = new string[4] { "xxxxxxx", "yyyyyyy", "vvvvvvv", "wwwwwww" };
		string textBuf = prePostPend[0] + text + prePostPend[0];
		myText.text = string.Empty;
		yield return new WaitForSeconds(0.5f);
		int rotatorStrPos2 = 0;
		int strIndex = 0;
		for (int i = 0; i < textBuf.Length - 12; i++)
		{
			myText.text = textBuf.Substring(strIndex, 12);
			yield return new WaitForSeconds(0.25f);
			strIndex++;
			rotatorStrPos2++;
			rotatorStrPos2 %= 4;
			textBuf = prePostPend[rotatorStrPos2] + text + prePostPend[rotatorStrPos2];
		}
		yield return new WaitForSeconds(0.25f);
		myText.text = initialTest;
		yield return new WaitForSeconds(0.25f);
		StartCoroutine(scrollingText(text));
	}

	public void FlashLocked()
	{
		StopCoroutine("FlashText");
		StartCoroutine("FlashText", "OBJECTIVE 5zz");
	}

	public IEnumerator FlashText(string str)
	{
		string temp = "OBJECTIVES/(";
		myText.text = str;
		yield return new WaitForSeconds(0.3f);
		myText.text = string.Empty;
		yield return new WaitForSeconds(0.3f);
		myText.text = str;
		yield return new WaitForSeconds(0.3f);
		myText.text = string.Empty;
		yield return new WaitForSeconds(0.3f);
		myText.text = str;
		yield return new WaitForSeconds(0.3f);
		myText.text = string.Empty;
		yield return new WaitForSeconds(0.3f);
		myText.text = temp;
	}
}
