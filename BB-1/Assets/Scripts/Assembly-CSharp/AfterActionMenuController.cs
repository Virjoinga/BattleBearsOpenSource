using UnityEngine;

public class AfterActionMenuController : MonoBehaviour
{
	public TextMesh timeLeft;

	public TextMesh kills;

	public TextMesh shotsFired;

	public TextMesh killRatio;

	public TextMesh deaths;

	public Material redTextMaterial;

	public Material orangeTextMaterial;

	public TextMesh deathLabel;

	private void Start()
	{
		Screen.lockCursor = false;
		int num = StatsManager.Instance.currentPinksKilled + StatsManager.Instance.currentGreensKilled + StatsManager.Instance.currentOrangesKilled + StatsManager.Instance.currentProjectilesKilled + StatsManager.Instance.currentYellowsKilled + StatsManager.Instance.currentRedsKilled + StatsManager.Instance.currentBluesKilled + StatsManager.Instance.currentTurretsKilled + StatsManager.Instance.currentSecretsKilled + StatsManager.Instance.currentGhostsKilled + StatsManager.Instance.currentHuggabullsKilled + StatsManager.Instance.currentCrushersKilled;
		kills.text = num.ToString();
		timeLeft.text = StatsManager.Instance.getPlaytimeString();
		shotsFired.text = StatsManager.Instance.currentShotsFired.ToString();
		float num2 = (float)num / (float)StatsManager.Instance.currentShotsFired;
		killRatio.text = string.Format("{0:0.00}", num2);
		if (num2 < 0.1f)
		{
			killRatio.GetComponent<Renderer>().sharedMaterial = redTextMaterial;
		}
		else if (num2 < 0.5f)
		{
			killRatio.GetComponent<Renderer>().sharedMaterial = orangeTextMaterial;
		}
		if (GameManager.Instance.currentGameMode == GameMode.SURVIVAL)
		{
			deathLabel.text = "ROOMS:";
			deaths.text = StatsManager.Instance.currentRoomNumber.ToString();
		}
		else
		{
			deaths.text = StatsManager.Instance.currentDeaths.ToString();
		}
	}
}
