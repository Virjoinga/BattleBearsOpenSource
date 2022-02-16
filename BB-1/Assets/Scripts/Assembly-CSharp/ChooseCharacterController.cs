using System.Collections;
using UnityEngine;

public class ChooseCharacterController : MonoBehaviour
{
	public GameObject oliverUp;

	public GameObject oliverPress;

	public GameObject riggsUp;

	public GameObject riggsPress;

	public GameObject wilUp;

	public GameObject wilPress;

	public GUIButton goButton;

	public AudioClip oliverClip;

	public AudioClip riggsClip;

	public AudioClip wilClip;

	private void Start()
	{
		GameManager.Instance.currentCharacter = Character.NONE;
		goButton.disable();
	}

	private IEnumerator delayedUpdateSelection()
	{
		yield return new WaitForSeconds(0.01f);
		updateSelection();
	}

	private void updateSelection()
	{
		switch (GameManager.Instance.currentCharacter)
		{
		case Character.OLIVER:
			riggsUp.SetActiveRecursively(true);
			riggsPress.SetActiveRecursively(false);
			oliverUp.SetActiveRecursively(false);
			oliverPress.SetActiveRecursively(true);
			wilUp.SetActiveRecursively(true);
			wilPress.SetActiveRecursively(false);
			break;
		case Character.RIGGS:
			riggsUp.SetActiveRecursively(false);
			riggsPress.SetActiveRecursively(true);
			oliverUp.SetActiveRecursively(true);
			oliverPress.SetActiveRecursively(false);
			wilUp.SetActiveRecursively(true);
			wilPress.SetActiveRecursively(false);
			break;
		case Character.WIL:
			oliverUp.SetActiveRecursively(true);
			oliverPress.SetActiveRecursively(false);
			riggsUp.SetActiveRecursively(true);
			riggsPress.SetActiveRecursively(false);
			wilUp.SetActiveRecursively(false);
			wilPress.SetActiveRecursively(true);
			break;
		case Character.NONE:
			break;
		}
	}

	public void OnGUIButtonClicked(GUIButton b)
	{
		PlayerPrefs.SetInt("isWil", 0);
		switch (b.gameObject.name)
		{
		case "Oliver_btn":
			SoundManager.Instance.playSound(oliverClip);
			GameManager.Instance.currentCharacter = Character.OLIVER;
			updateSelection();
			SendMessageUpwards("OnGUIButtonClicked", goButton);
			break;
		case "Riggs_btn":
			SoundManager.Instance.playSound(riggsClip);
			GameManager.Instance.currentCharacter = Character.RIGGS;
			updateSelection();
			SendMessageUpwards("OnGUIButtonClicked", goButton);
			break;
		case "Wil_btn":
			SoundManager.Instance.playSound(wilClip);
			GameManager.Instance.currentCharacter = Character.WIL;
			updateSelection();
			SendMessageUpwards("OnGUIButtonClicked", goButton);
			break;
		}
	}
}
