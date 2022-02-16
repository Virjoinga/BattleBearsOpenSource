using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovieManager : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(delayedLoad());
	}

	private IEnumerator delayedLoad()
	{
		yield return new WaitForSeconds(1f);
		if (GameManager.Instance.movieToPlay != "")
		{
			GameManager.Instance.PlayMovie(GameManager.Instance.movieToPlay ?? "");
			GameManager.Instance.movieToPlay = "";
		}
		yield return new WaitForSeconds(0.25f);
		//Application.LoadLevel(GameManager.Instance.levelToLoad);
		SceneManager.LoadScene(GameManager.Instance.levelToLoad);
	}
}
