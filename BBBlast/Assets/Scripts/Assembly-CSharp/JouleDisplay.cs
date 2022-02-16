using UnityEngine;

public class JouleDisplay : MonoBehaviour
{
	private TextMesh myText;

	private void Start()
	{
		myText = GetComponent<TextMesh>();
	}

	private void Update()
	{
		myText.text = PlayerPrefs.GetInt("joules").ToString();
	}
}
