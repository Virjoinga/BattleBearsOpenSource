using UnityEngine;

public class ObjectiveDisplay : MonoBehaviour
{
	private string objective;

	private void Start()
	{
		if (PlayerPrefs.GetInt("objectives") > 9)
		{
			objective = PlayerPrefs.GetInt("objectives").ToString();
		}
		else
		{
			objective = "0" + PlayerPrefs.GetInt("objectives");
		}
	}

	private void Update()
	{
	}
}
