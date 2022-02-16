using UnityEngine;

public class GetExploreAchievement : MonoBehaviour
{
	public int achievementID;

	private void OnInitialize()
	{
		Object.Destroy(base.gameObject);
	}
}
