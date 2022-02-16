using System.Collections;
using UnityEngine;

public class ElevatorChecker : MonoBehaviour
{
	public void OnRoomCleared()
	{
		StartCoroutine(delayedElevator());
	}

	private IEnumerator delayedElevator()
	{
		yield return new WaitForSeconds(0.1f);
		((Object.Instantiate(Resources.Load("FaderSystem")) as GameObject).GetComponent(typeof(SimpleFader)) as SimpleFader).fadeTime = 1.5f;
		yield return new WaitForSeconds(0.2f);
		if (GameManager.Instance.currentGameMode == GameMode.CAMPAIGN)
		{
			if (GameManager.Instance.currentCharacter == Character.OLIVER)
			{
				GameManager.Instance.PlayMovie("o_elevator_1");
			}
			else if (GameManager.Instance.currentCharacter == Character.RIGGS)
			{
				if (GameManager.Instance.currentStage == 1)
				{
					GameManager.Instance.PlayMovie("r_elevator_1");
				}
				else if (GameManager.Instance.currentStage == 2)
				{
					GameManager.Instance.PlayMovie("r_chainsaw");
				}
			}
			else if (GameManager.Instance.currentCharacter == Character.WIL)
			{
				if (GameManager.Instance.currentStage == 2)
				{
					GameManager.Instance.PlayMovie("w_elevator");
				}
				if (GameManager.Instance.currentStage == 4)
				{
					GameManager.Instance.PlayMovie("w_win");
				}
			}
		}
		yield return new WaitForSeconds(0.2f);
		GameManager.Instance.currentStage++;
		GameManager.Instance.isGoingUpElevator = true;
		if (GameManager.Instance.currentGameMode != 0)
		{
			yield break;
		}
		int num = StatsManager.Instance.currentBearzookasPickedUp + StatsManager.Instance.currentSpreadshotsPickedUp + StatsManager.Instance.currentSatellitesPickedUp + StatsManager.Instance.currentFoodsPickedUp + StatsManager.Instance.currentShieldsPickedUp + StatsManager.Instance.currentCoffeesPickedUp + StatsManager.Instance.currentLivesPickedUp + StatsManager.Instance.currentScreenclearsPickedUp + StatsManager.Instance.currentSpecialsPickedUp;
		int pickupsMissed = StatsManager.Instance.pickupsMissed;
		StatsManager.Instance.pickupsMissed = 0;
		int wasHit = StatsManager.Instance.wasHit;
		StatsManager.Instance.wasHit = 0;
		if (GameManager.Instance.currentCharacter == Character.OLIVER)
		{
			if (GameManager.Instance.oliverUnlockedStage == GameManager.Instance.currentStage - 2)
			{
				GameManager.Instance.oliverUnlockedStage++;
				PlayerPrefs.SetInt("oliverUnlockedStage", GameManager.Instance.oliverUnlockedStage);
			}
			Application.LoadLevel("OliverCampaignLevel" + GameManager.Instance.currentStage);
		}
		else if (GameManager.Instance.currentCharacter == Character.RIGGS)
		{
			if (GameManager.Instance.riggsUnlockedStage == GameManager.Instance.currentStage - 2)
			{
				GameManager.Instance.riggsUnlockedStage++;
				PlayerPrefs.SetInt("riggsUnlockedStage", GameManager.Instance.riggsUnlockedStage);
			}
			Application.LoadLevel("RiggsCampaignLevel" + GameManager.Instance.currentStage);
		}
		else
		{
			if (GameManager.Instance.currentCharacter != Character.WIL)
			{
				yield break;
			}
			Debug.Log(GameManager.Instance.currentStage);
			Debug.Log(GameManager.Instance.wilUnlockedStage);
			if (GameManager.Instance.currentStage == 3)
			{
				GameManager.Instance.wilUnlockedStage++;
				PlayerPrefs.SetInt("wilUnlockedStage", GameManager.Instance.wilUnlockedStage);
				Application.LoadLevel("WilCampaignLevel" + GameManager.Instance.currentStage);
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
			if (GameManager.Instance.currentDifficulty != GameDifficulty.MEDIUM)
			{
				GameDifficulty currentDifficulty = GameManager.Instance.currentDifficulty;
				int num2 = 2;
			}
			PlayerPrefs.GetInt("hasShotgunFired");
		}
	}
}
