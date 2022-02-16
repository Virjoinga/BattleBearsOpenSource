using UnityEngine;

public class AchievementManager : MonoBehaviour
{
	private static AchievementManager instance;

	public int numberOfRoomsWithSatellite;

	public static AchievementManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	public void OnReset()
	{
		numberOfRoomsWithSatellite = 0;
	}
}
