using System.Collections;
using UnityEngine;

public class DynamicLoadText : MonoBehaviour
{
	public string[] sayings;

	private TextMesh textMesh;

	private void Awake()
	{
		textMesh = GetComponent(typeof(TextMesh)) as TextMesh;
		StartCoroutine(randomText());
	}

	private IEnumerator randomText()
	{
		int lastIndex = -1;
		while (sayings.Length > 1)
		{
			int num = Random.Range(0, sayings.Length);
			if (num != lastIndex)
			{
				textMesh.text = sayings[num];
				yield return new WaitForSeconds(0.05f);
			}
		}
	}
}
