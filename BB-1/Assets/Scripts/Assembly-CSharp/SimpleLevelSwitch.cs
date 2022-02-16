using System.Collections;
using UnityEngine;

public class SimpleLevelSwitch : MonoBehaviour
{
	private bool isActive;

	public string movieToPlay;

	public string levelToLoad;

	public GameObject onSwitch;

	public GameObject offSwitch;

	private void Awake()
	{
		onSwitch.active = false;
		offSwitch.active = true;
	}

	public void OnTriggerEnter(Collider c)
	{
		if (!isActive && c.CompareTag("Player"))
		{
			isActive = true;
			if (GameManager.Instance.currentCharacter == Character.OLIVER)
			{
				GameManager.Instance.ocoLives = (c.transform.root.GetComponentInChildren(typeof(PlayerController)) as PlayerController).numLives;
			}
			StartCoroutine(switchLevel());
		}
	}

	private IEnumerator switchLevel()
	{
		onSwitch.active = true;
		offSwitch.active = false;
		yield return new WaitForSeconds(0.05f);
		((Object.Instantiate(Resources.Load("FaderSystem")) as GameObject).GetComponent(typeof(SimpleFader)) as SimpleFader).fadeTime = 1.5f;
		yield return new WaitForSeconds(0.2f);
		if (movieToPlay != null && movieToPlay != "")
		{
			GameManager.Instance.PlayMovie(movieToPlay ?? "");
		}
		yield return new WaitForSeconds(0.1f);
		if (GameManager.Instance.currentCharacter == Character.WIL && levelToLoad != "")
		{
			if (GameManager.Instance.currentStage < 3)
			{
				GameManager.Instance.wilUnlockedStage++;
			}
			GameManager.Instance.currentStage++;
			(Object.FindObjectOfType(typeof(PlayerController)) as PlayerController).saveState(Vector3.zero, Vector2.zero, "");
			GameManager.Instance.isGoingUpElevator = true;
			GameManager.Instance.hasAcquiredSpecial = true;
			if (GameManager.Instance.wilUnlockedStage < GameManager.Instance.currentStage)
			{
				GameManager.Instance.wilUnlockedStage++;
			}
		}
		if (levelToLoad != null && levelToLoad != "")
		{
			Application.LoadLevel(levelToLoad);
			yield break;
		}
		Object[] array = Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (!(gameObject.name == "GameManager") && !(gameObject.transform.root.name == "SoundManager") && !(gameObject.name == "Controlls") && !(gameObject.name == "Rewired Input Manager") && !(gameObject.name == "UICanvas") && !(gameObject.name == "EventSystem"))
			{
				Object.Destroy(gameObject);
			}
		}
		Object.Instantiate((Object.FindObjectOfType(typeof(EndMenuSpawner)) as EndMenuSpawner).endMenu);
	}
}
