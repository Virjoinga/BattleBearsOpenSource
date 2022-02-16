using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicText : MonoBehaviour
{
	public string[] options;

	public TextMesh myText;

	private int randomNum;

	private void Start()
	{
		randomNum = UnityEngine.Random.Range(0, options.Length);
		myText.text = options[randomNum];
		StartCoroutine("holdUp");
	}

	private IEnumerator holdUp()
	{
		yield return new WaitForSeconds(0.5f);
		//Application.LoadLevel("MainMenu");
		SceneManager.LoadScene("MainMenu");
	}
}
