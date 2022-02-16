using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
	private void Start()
	{
		GameObject gameObject;
		if ((bool)(gameObject = GameObject.Find("GhostXHairs")))
		{
			//gameObject.active = false;
			gameObject.SetActive(false);
		}
		if ((bool)(gameObject = GameObject.Find("Tutorial_Illustration")))
		{
			//gameObject.active = false;
			gameObject.SetActive(false);
		}
		if ((bool)(gameObject = GameObject.Find("Tutorial_Text")))
		{
			//gameObject.active = false;
			gameObject.SetActive(false);
		}
		if (GameManager.Instance.clearedEntranceExam)
		{
			GameManager.Instance.isLevel = true;
		}
	}
}
