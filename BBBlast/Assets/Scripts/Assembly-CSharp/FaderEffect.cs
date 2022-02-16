using System.Collections;
using UnityEngine;

public class FaderEffect : MonoBehaviour
{
	public Material mat;

	public bool isFadeOut = true;

	public float alpha = 1f;

	public void StartFader(bool fadeOut)
	{
		if (fadeOut)
		{
			alpha = 1f;
			isFadeOut = true;
		}
		else
		{
			alpha = 0f;
			isFadeOut = false;
		}
		StartCoroutine("fader");
	}

	public IEnumerator fader()
	{
		if (isFadeOut)
		{
			alpha -= Time.deltaTime * 1.3f;
			if (alpha <= 0f)
			{
				Object.Destroy(base.gameObject);
			}
			mat.color = new Color(1f, 1f, 1f, alpha);
		}
		else
		{
			alpha += Time.deltaTime;
			mat.color = new Color(1f, 1f, 1f, alpha);
		}
		yield return null;
		StartCoroutine("fader");
	}
}
