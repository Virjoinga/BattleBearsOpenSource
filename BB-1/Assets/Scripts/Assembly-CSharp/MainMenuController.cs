using UnityEngine;

public class MainMenuController : MonoBehaviour
{
	public GUIButton resumeButton;

	public GameObject openfeintPress;

	private void Awake()
	{
		float num = (float)Screen.width / (float)Screen.height;
		float num2 = 1.7777778f;
		float x = 1.185f * (num / num2);
		base.transform.localScale = new Vector3(x, 0.9f, 0.9f);
	}

	private void Start()
	{
		if (PlayerPrefs.HasKey("character") && GameManager.Instance.version == PlayerPrefs.GetString("version", ""))
		{
			resumeButton.inActive = false;
			resumeButton.disabled = false;
		}
		else
		{
			resumeButton.inActive = true;
			resumeButton.disabled = true;
		}
	}

	public void OnGUIButtonClicked(GUIButton b)
	{
		if (b.name == "OpenFeintButton")
		{
			openfeintPress.active = false;
		}
	}
}
