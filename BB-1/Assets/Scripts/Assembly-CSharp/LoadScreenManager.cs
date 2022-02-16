using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreenManager : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(delayedLoad());
	}

	private IEnumerator delayedLoad()
	{
		yield return new WaitForSeconds(0.1f);
		GameManager.Instance.isIntro = true;
		Debug.Log("wat?");
		if (!GameManager.Instance.isLoading)
		{
			if (GameManager.Instance.levelToLoad == "OliverCampaignLevel1")
			{
				//GameManager.Instance.PlayMovie("o_intro");
			}
			else if (GameManager.Instance.levelToLoad == "RiggsCampaignLevel1")
			{
				//GameManager.Instance.PlayMovie("r_intro");
			}
			else if (GameManager.Instance.levelToLoad == "WilCampaignLevel1")
			{
				//GameManager.Instance.PlayMovie("w_intro");
			}
		}
		yield return new WaitForSeconds(1f);
		GameManager.Instance.isIntro = false;
		//Application.LoadLevel(GameManager.Instance.levelToLoad);
		SceneManager.LoadScene(GameManager.Instance.levelToLoad);
	}
}
