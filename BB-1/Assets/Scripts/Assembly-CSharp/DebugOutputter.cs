using UnityEngine;

public class DebugOutputter : MonoBehaviour
{
	public static int maxDebugs = 20;

	public static string[] debugStrings;

	private static string combinedString = "";

	private void Awake()
	{
		debugStrings = new string[maxDebugs];
	}

	public static void addString(string s, int index)
	{
		if (index < maxDebugs)
		{
			debugStrings[index] = s;
			combinedString = "";
			for (int i = 0; i < debugStrings.Length; i++)
			{
				combinedString = combinedString + debugStrings[i] + "\n";
			}
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(0f, 0f, 480f, 320f), combinedString);
	}
}
