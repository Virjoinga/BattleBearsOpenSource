using UnityEngine;

public class Alerts : MonoBehaviour
{
	private Collider[] colArray;

	private Color defaultBGColor;

	private Color defaultColor;

	private Rect window = new Rect((Screen.width - 300) / 2, (Screen.height - 120) / 2, 300f, 120f);

	public string buyString = string.Empty;

	public MonoBehaviour caller;

	public string callbackMessage;

	public bool hasEnough;

	private Color alertColor = Color.red;

	private void OnGUI()
	{
		defaultBGColor = GUI.backgroundColor;
		if (GUI.Button(new Rect(0f, 0f, Screen.width, Screen.height), string.Empty, "box"))
		{
			CancelBuy();
		}
		GUI.backgroundColor = alertColor;
		if (hasEnough)
		{
			window = GUI.Window(0, window, OnPositive, "Buy Level");
		}
		else
		{
			window = GUI.Window(1, window, NotEnough, "Sorry, Not Enough Joules");
		}
		GUI.backgroundColor = defaultBGColor;
	}

	private void OnPositive(int windowID)
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Do you want to purchase this level for 1,000 Joules?");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		if (GUI.Button(new Rect(window.width / 2f - 110f, window.height - 55f, 100f, 50f), "Yes"))
		{
			Buy();
		}
		if (GUI.Button(new Rect(window.width / 2f + 10f, window.height - 55f, 100f, 50f), "No"))
		{
			CancelBuy();
		}
	}

	private void NotEnough(int windowID)
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Get Free Joules Above or \nBuy Joules From Main Menu");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		if (GUI.Button(new Rect((window.width - 100f) / 2f, window.height - 55f, 100f, 50f), "Ok"))
		{
			CancelBuy();
		}
	}

	private void Awake()
	{
		colArray = Object.FindObjectsOfType(typeof(Collider)) as Collider[];
		Collider[] array = colArray;
		foreach (Collider collider in array)
		{
			collider.enabled = false;
		}
	}

	private void Buy()
	{
		TapJoyManager.Instance.SpendJoules(1000);
		PlayerPrefs.SetString("unlockedItems", PlayerPrefs.GetString("unlockedItems") + buyString);
		CancelBuy();
	}

	private void CancelBuy()
	{
		Collider[] array = colArray;
		foreach (Collider collider in array)
		{
			collider.enabled = true;
		}
		if (caller != null && callbackMessage != null)
		{
			caller.SendMessage(callbackMessage);
		}
		Object.Destroy(this);
	}
}
