using UnityEngine;

public class FogEnabler : MonoBehaviour
{
	private void OnPreRender()
	{
		RenderSettings.fog = false;
	}

	private void OnPostRender()
	{
		if (GameManager.Instance.currentCharacter == Character.WIL && Time.timeScale != 0f)
		{
			RenderSettings.fog = true;
		}
	}
}
