using System.Collections;
using UnityEngine;

public class PointText : MonoBehaviour
{
	private Transform myTransform;

	public TextMesh myText;

	private void Start()
	{
		myTransform = base.transform;
		myTransform.localScale = Vector3.zero;
		myTransform.LookAt(Camera.main.transform);
		myTransform.eulerAngles += Vector3.up * 180f;
	}

	public void getPoints(float score)
	{
		if (GameManager.Instance.multiplier == 4f)
		{
			myText.text = numberToString(score) + "*";
		}
		else if (GameManager.Instance.multiplier == 2f)
		{
			myText.text = numberToString(score) + "!";
		}
		else
		{
			myText.text = numberToString(score);
		}
		StartCoroutine("hurps");
	}

	private string numberToString(float number)
	{
		string text = number.ToString();
		string text2 = string.Empty;
		for (int i = 0; i < text.Length; i++)
		{
			text2 += (char)(text[i] + 49);
		}
		return text2;
	}

	private IEnumerator hurps()
	{
		yield return new WaitForSeconds(0.1f);
		while (myTransform.localScale.x < 0.9f)
		{
			myTransform.localScale += Vector3.one * 0.05f * 10f;
			yield return new WaitForSeconds(0.01f);
		}
		myTransform.localScale = Vector3.one;
		for (int i = 0; i < 4; i++)
		{
			if (i % 2 == 0)
			{
				myTransform.localScale += Vector3.one * 10f * 0.01f;
			}
			else
			{
				myTransform.localScale -= Vector3.one * 10f * 0.01f;
			}
			yield return new WaitForSeconds(0.05f);
		}
		yield return new WaitForSeconds(0.05f);
		while (myTransform.localScale.x > 0.1f)
		{
			myTransform.localScale -= Vector3.one * 0.05f * 10f;
			yield return new WaitForSeconds(0.01f);
		}
		myTransform.localScale = Vector3.zero;
		Object.Destroy(base.gameObject);
	}
}
