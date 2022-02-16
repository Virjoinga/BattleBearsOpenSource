using UnityEngine;

public class inGameInput : MonoBehaviour
{
	private void Start()
	{
		PlayerPrefs.SetString("KeyCodes", "durp");
		Debug.Log(PlayerPrefs.GetString("KeyCodes"));
	}

	private void Update()
	{
	}
}
