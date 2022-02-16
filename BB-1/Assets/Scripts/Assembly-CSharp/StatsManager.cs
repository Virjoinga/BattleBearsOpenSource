using UnityEngine;

public class StatsManager : MonoBehaviour
{
	public float currentPlayTime;

	public int currentShotsFired;

	public int currentDeaths;

	public int currentBearzookasPickedUp;

	public int currentSpreadshotsPickedUp;

	public int currentSatellitesPickedUp;

	public int currentFoodsPickedUp;

	public int currentShieldsPickedUp;

	public int currentCoffeesPickedUp;

	public int currentLivesPickedUp;

	public int currentScreenclearsPickedUp;

	public int currentSpecialsPickedUp;

	public int currentPinksKilled;

	public int currentOrangesKilled;

	public int currentGreensKilled;

	public int currentProjectilesKilled;

	public int currentYellowsKilled;

	public int currentRedsKilled;

	public int currentBluesKilled;

	public int currentTurretsKilled;

	public int currentSecretsKilled;

	public int currentGhostsKilled;

	public int currentHuggabullsKilled;

	public int currentCrushersKilled;

	public int currentBluesKilledByKatana;

	public int currentScore;

	public int currentRoomNumber = 1;

	public int toasters;

	public int turkeys;

	public int vespas;

	public int boats;

	public int grills;

	public int totalHuggablesKilled;

	public int totalTurretsKilled;

	public int currentRoomsVisited;

	public int pickupsMissed;

	public int numberOfRoomsWithSatellite;

	public int currentFalls;

	public int pinksInRow;

	public string roygbivString = "";

	public int wasHit;

	private static StatsManager instance;

	public static StatsManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		totalHuggablesKilled = PlayerPrefs.GetInt("totalHuggablesKilled", 0);
		totalTurretsKilled = PlayerPrefs.GetInt("totalTurretsKilled", 0);
	}

	public void OnLoad()
	{
		currentScore = PlayerPrefs.GetInt("currentScore", 0);
		currentRoomNumber = PlayerPrefs.GetInt("currentRoomNumber", 1);
		toasters = PlayerPrefs.GetInt("toasters", 0);
		turkeys = PlayerPrefs.GetInt("turkeys", 0);
		vespas = PlayerPrefs.GetInt("vespas", 0);
		boats = PlayerPrefs.GetInt("boats", 0);
		grills = PlayerPrefs.GetInt("grills", 0);
		currentRoomsVisited = PlayerPrefs.GetInt("currentRoomsVisited", 0);
		pickupsMissed = PlayerPrefs.GetInt("pickupsMissed", 0);
		numberOfRoomsWithSatellite = PlayerPrefs.GetInt("numberOfRoomsWithSatellite", 0);
		currentFalls = PlayerPrefs.GetInt("currentFalls", 0);
		pinksInRow = PlayerPrefs.GetInt("pinksInRow", 0);
		roygbivString = PlayerPrefs.GetString("roygbivString", "");
		wasHit = PlayerPrefs.GetInt("wasHit", 0);
		currentPlayTime = PlayerPrefs.GetFloat("currentPlayTime", 0f);
		currentShotsFired = PlayerPrefs.GetInt("currentShotsFired", 0);
		currentDeaths = PlayerPrefs.GetInt("currentDeaths", 0);
		currentBearzookasPickedUp = PlayerPrefs.GetInt("currentBearzookasPickedUp", 0);
		currentSpreadshotsPickedUp = PlayerPrefs.GetInt("currentSpreadshotsPickedUp", 0);
		currentSatellitesPickedUp = PlayerPrefs.GetInt("currentSatellitesPickedUp", 0);
		currentFoodsPickedUp = PlayerPrefs.GetInt("currentFoodsPickedUp", 0);
		currentShieldsPickedUp = PlayerPrefs.GetInt("currentShieldsPickedUp", 0);
		currentCoffeesPickedUp = PlayerPrefs.GetInt("currentCoffeesPickedUp", 0);
		currentLivesPickedUp = PlayerPrefs.GetInt("currentLivesPickedUp", 0);
		currentScreenclearsPickedUp = PlayerPrefs.GetInt("currentScreenclearsPickedUp", 0);
		currentSpecialsPickedUp = PlayerPrefs.GetInt("currentSpecialsPickedUp", 0);
		currentPinksKilled = PlayerPrefs.GetInt("currentPinksKilled", 0);
		currentOrangesKilled = PlayerPrefs.GetInt("currentOrangesKilled", 0);
		currentGreensKilled = PlayerPrefs.GetInt("currentGreensKilled", 0);
		currentProjectilesKilled = PlayerPrefs.GetInt("currentProjectilesKilled", 0);
		currentYellowsKilled = PlayerPrefs.GetInt("currentYellowsKilled", 0);
		currentRedsKilled = PlayerPrefs.GetInt("currentRedsKilled", 0);
		currentBluesKilled = PlayerPrefs.GetInt("currentBluesKilled", 0);
		currentTurretsKilled = PlayerPrefs.GetInt("currentTurretsKilled", 0);
		currentSecretsKilled = PlayerPrefs.GetInt("currentSecretsKilled", 0);
		currentBluesKilledByKatana = PlayerPrefs.GetInt("currentBluesKilledByKatana", 0);
		currentGhostsKilled = PlayerPrefs.GetInt("currentGhostsKilled", 0);
		currentHuggabullsKilled = PlayerPrefs.GetInt("currentHuggabullsKilled", 0);
		currentCrushersKilled = PlayerPrefs.GetInt("currentCrushersKilled", 0);
	}

	public void OnSave()
	{
		PlayerPrefs.SetString("version", GameManager.Instance.version);
		PlayerPrefs.SetInt("currentScore", currentScore);
		PlayerPrefs.SetInt("currentRoomNumber", currentRoomNumber);
		PlayerPrefs.SetInt("toasters", toasters);
		PlayerPrefs.SetInt("turkeys", turkeys);
		PlayerPrefs.SetInt("vespas", vespas);
		PlayerPrefs.SetInt("boats", boats);
		PlayerPrefs.SetInt("grills", grills);
		PlayerPrefs.SetInt("totalHuggablesKilled", totalHuggablesKilled);
		PlayerPrefs.SetInt("totalTurretsKilled", totalTurretsKilled);
		PlayerPrefs.SetInt("currentRoomsVisited", currentRoomsVisited);
		PlayerPrefs.SetInt("pickupsMissed", pickupsMissed);
		PlayerPrefs.SetInt("numberOfRoomsWithSatellite", numberOfRoomsWithSatellite);
		PlayerPrefs.SetInt("currentFalls", currentFalls);
		PlayerPrefs.SetInt("pinksInRow", pinksInRow);
		PlayerPrefs.SetString("roygbivString", roygbivString);
		PlayerPrefs.SetInt("wasHit", wasHit);
		PlayerPrefs.SetFloat("currentPlayTime", currentPlayTime + Time.timeSinceLevelLoad);
		PlayerPrefs.SetInt("currentShotsFired", currentShotsFired);
		PlayerPrefs.SetInt("currentDeaths", currentDeaths);
		PlayerPrefs.SetInt("currentBearzookasPickedUp", currentBearzookasPickedUp);
		PlayerPrefs.SetInt("currentSpreadshotsPickedUp", currentSpreadshotsPickedUp);
		PlayerPrefs.SetInt("currentSatellitesPickedUp", currentSatellitesPickedUp);
		PlayerPrefs.SetInt("currentFoodsPickedUp", currentFoodsPickedUp);
		PlayerPrefs.SetInt("currentShieldsPickedUp", currentShieldsPickedUp);
		PlayerPrefs.SetInt("currentCoffeesPickedUp", currentCoffeesPickedUp);
		PlayerPrefs.SetInt("currentLivesPickedUp", currentLivesPickedUp);
		PlayerPrefs.SetInt("currentScreenclearsPickedUp", currentScreenclearsPickedUp);
		PlayerPrefs.SetInt("currentSpecialsPickedUp", currentSpecialsPickedUp);
		PlayerPrefs.SetInt("currentPinksKilled", currentPinksKilled);
		PlayerPrefs.SetInt("currentOrangesKilled", currentOrangesKilled);
		PlayerPrefs.SetInt("currentGreensKilled", currentGreensKilled);
		PlayerPrefs.SetInt("currentProjectilesKilled", currentProjectilesKilled);
		PlayerPrefs.SetInt("currentYellowsKilled", currentYellowsKilled);
		PlayerPrefs.SetInt("currentRedsKilled", currentRedsKilled);
		PlayerPrefs.SetInt("currentBluesKilled", currentBluesKilled);
		PlayerPrefs.SetInt("currentTurretsKilled", currentTurretsKilled);
		PlayerPrefs.SetInt("currentSecretsKilled", currentSecretsKilled);
		PlayerPrefs.SetInt("currentBluesKilledByKatana", currentBluesKilledByKatana);
		PlayerPrefs.SetInt("currentGhostsKilled", currentGhostsKilled);
		PlayerPrefs.SetInt("currentHuggabullsKilled", currentHuggabullsKilled);
		PlayerPrefs.SetInt("currentCrushersKilled", currentCrushersKilled);
	}

	public void OnReset()
	{
		currentScore = 0;
		currentRoomNumber = 1;
		toasters = 0;
		turkeys = 0;
		vespas = 0;
		boats = 0;
		grills = 0;
		currentRoomsVisited = 0;
		pickupsMissed = 0;
		numberOfRoomsWithSatellite = 0;
		currentFalls = 0;
		pinksInRow = 0;
		roygbivString = "";
		wasHit = 0;
		currentPlayTime = 0f;
		currentShotsFired = 0;
		currentDeaths = 0;
		currentBearzookasPickedUp = 0;
		currentSpreadshotsPickedUp = 0;
		currentSatellitesPickedUp = 0;
		currentFoodsPickedUp = 0;
		currentShieldsPickedUp = 0;
		currentCoffeesPickedUp = 0;
		currentLivesPickedUp = 0;
		currentScreenclearsPickedUp = 0;
		currentSpecialsPickedUp = 0;
		currentPinksKilled = 0;
		currentOrangesKilled = 0;
		currentGreensKilled = 0;
		currentProjectilesKilled = 0;
		currentYellowsKilled = 0;
		currentRedsKilled = 0;
		currentBluesKilled = 0;
		currentTurretsKilled = 0;
		currentSecretsKilled = 0;
		currentBluesKilledByKatana = 0;
		currentGhostsKilled = 0;
		currentHuggabullsKilled = 0;
		currentCrushersKilled = 0;
	}

	public string getPlaytimeString()
	{
		int num = (int)currentPlayTime;
		int num2 = num % 60;
		num /= 60;
		int num3 = num % 60;
		num /= 60;
		string text = "";
		if (num > 0)
		{
			text = num + ":";
		}
		text = text + string.Format("{0:00}", num3) + ":";
		return text + string.Format("{0:00}", num2);
	}
}
