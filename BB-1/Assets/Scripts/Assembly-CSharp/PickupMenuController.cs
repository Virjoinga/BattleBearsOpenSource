using UnityEngine;

public class PickupMenuController : MonoBehaviour
{
	public TextMesh spreadshot;

	public TextMesh bearzooka;

	public TextMesh satellite;

	public TextMesh food;

	public TextMesh shield;

	public TextMesh coffee;

	public TextMesh lives;

	public TextMesh specials;

	public TextMesh screenclears;

	private void Start()
	{
		spreadshot.text = StatsManager.Instance.currentSpreadshotsPickedUp.ToString();
		bearzooka.text = StatsManager.Instance.currentBearzookasPickedUp.ToString();
		satellite.text = StatsManager.Instance.currentSatellitesPickedUp.ToString();
		food.text = StatsManager.Instance.currentFoodsPickedUp.ToString();
		shield.text = StatsManager.Instance.currentShieldsPickedUp.ToString();
		coffee.text = StatsManager.Instance.currentCoffeesPickedUp.ToString();
		lives.text = StatsManager.Instance.currentLivesPickedUp.ToString();
		specials.text = StatsManager.Instance.currentSpecialsPickedUp.ToString();
		screenclears.text = StatsManager.Instance.currentScreenclearsPickedUp.ToString();
	}
}
