using UnityEngine;

public class HuggableMenuController : MonoBehaviour
{
	public TextMesh pink;

	public TextMesh orange;

	public TextMesh green;

	public TextMesh projectile;

	public TextMesh yellow;

	public TextMesh red;

	public TextMesh blue;

	public TextMesh turret;

	public TextMesh secret;

	public TextMesh special;

	private void Start()
	{
		int num = StatsManager.Instance.currentGhostsKilled + StatsManager.Instance.currentHuggabullsKilled + StatsManager.Instance.currentCrushersKilled;
		pink.text = StatsManager.Instance.currentPinksKilled.ToString();
		orange.text = StatsManager.Instance.currentOrangesKilled.ToString();
		green.text = StatsManager.Instance.currentGreensKilled.ToString();
		projectile.text = StatsManager.Instance.currentProjectilesKilled.ToString();
		yellow.text = StatsManager.Instance.currentYellowsKilled.ToString();
		red.text = StatsManager.Instance.currentRedsKilled.ToString();
		blue.text = StatsManager.Instance.currentBluesKilled.ToString();
		turret.text = StatsManager.Instance.currentTurretsKilled.ToString();
		secret.text = StatsManager.Instance.currentSecretsKilled.ToString();
		special.text = num.ToString();
	}
}
