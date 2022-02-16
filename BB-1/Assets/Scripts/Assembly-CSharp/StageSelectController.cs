using System.Collections;
using UnityEngine;

public class StageSelectController : MonoBehaviour
{
	public GameObject stage1Up;

	public GameObject stage1Press;

	public GameObject stage2Up;

	public GameObject stage2Press;

	public GameObject stage3Up;

	public GameObject stage3Press;

	public GUIButton goButton;

	public GUIButton[] stageButtons;

	public GUIButton[] cutsceneButtons;

	private GUIButton lastButtonPressed;

	public GameObject[] oliverPics;

	public GameObject[] riggsPics;

	public GameObject[] wilPics;

	private void Start()
	{
		goButton.disable();
		if (GameManager.Instance.currentCharacter == Character.OLIVER)
		{
			for (int i = 0; i < riggsPics.Length; i++)
			{
				Object.Destroy(riggsPics[i]);
				Object.Destroy(wilPics[i]);
			}
			for (int j = 0; j < 3; j++)
			{
				if (GameManager.Instance.oliverUnlockedStage >= j)
				{
					stageButtons[j].enable();
				}
				else
				{
					stageButtons[j].disable();
				}
				if (GameManager.Instance.oliverUnlockedStage >= j + 1)
				{
					cutsceneButtons[j].enable();
				}
				else
				{
					cutsceneButtons[j].disable();
				}
			}
		}
		else if (GameManager.Instance.currentCharacter == Character.RIGGS)
		{
			for (int k = 0; k < oliverPics.Length; k++)
			{
				Object.Destroy(oliverPics[k]);
				Object.Destroy(wilPics[k]);
			}
			for (int l = 0; l < 3; l++)
			{
				if (GameManager.Instance.riggsUnlockedStage >= l)
				{
					stageButtons[l].enable();
				}
				else
				{
					stageButtons[l].disable();
				}
				if (GameManager.Instance.riggsUnlockedStage >= l + 1)
				{
					cutsceneButtons[l].enable();
				}
				else
				{
					cutsceneButtons[l].disable();
				}
			}
		}
		else
		{
			if (GameManager.Instance.currentCharacter != Character.WIL)
			{
				return;
			}
			for (int m = 0; m < wilPics.Length; m++)
			{
				Object.Destroy(oliverPics[m]);
				Object.Destroy(riggsPics[m]);
			}
			for (int n = 0; n < 3; n++)
			{
				if (GameManager.Instance.wilUnlockedStage >= n)
				{
					stageButtons[n].enable();
				}
				else
				{
					stageButtons[n].disable();
				}
				if (GameManager.Instance.wilUnlockedStage >= n + 1)
				{
					cutsceneButtons[n].enable();
				}
				else
				{
					cutsceneButtons[n].disable();
				}
			}
		}
	}

	private IEnumerator delayedUpdateSelection()
	{
		yield return new WaitForSeconds(0.01f);
		updateSelection();
		yield return new WaitForSeconds(0.01f);
	}

	private void updateSelection()
	{
		switch (GameManager.Instance.startingStage)
		{
		case 0:
			goButton.disable();
			break;
		case 1:
			if (!stageButtons[2].isDisabled())
			{
				stage3Up.SetActiveRecursively(true);
				stage3Press.SetActiveRecursively(false);
			}
			if (!stageButtons[1].isDisabled())
			{
				stage2Up.SetActiveRecursively(true);
				stage2Press.SetActiveRecursively(false);
			}
			if (!stageButtons[0].isDisabled())
			{
				stage1Up.SetActiveRecursively(false);
				stage1Press.SetActiveRecursively(true);
			}
			goButton.enable();
			break;
		case 2:
			if (!stageButtons[2].isDisabled())
			{
				stage3Up.SetActiveRecursively(true);
				stage3Press.SetActiveRecursively(false);
			}
			if (!stageButtons[1].isDisabled())
			{
				stage2Up.SetActiveRecursively(false);
				stage2Press.SetActiveRecursively(true);
			}
			if (!stageButtons[0].isDisabled())
			{
				stage1Up.SetActiveRecursively(true);
				stage1Press.SetActiveRecursively(false);
			}
			goButton.enable();
			break;
		case 3:
			if (!stageButtons[2].isDisabled())
			{
				stage3Up.SetActiveRecursively(false);
				stage3Press.SetActiveRecursively(true);
			}
			if (!stageButtons[1].isDisabled())
			{
				stage2Up.SetActiveRecursively(true);
				stage2Press.SetActiveRecursively(false);
			}
			if (!stageButtons[0].isDisabled())
			{
				stage1Up.SetActiveRecursively(true);
				stage1Press.SetActiveRecursively(false);
			}
			goButton.enable();
			break;
		}
	}

	public void OnGUIButtonClicked(GUIButton b)
	{
		switch (b.gameObject.name)
		{
		case "Stage1_btn":
			GameManager.Instance.startingStage = 1;
			if (lastButtonPressed == b)
			{
				SendMessageUpwards("OnGUIButtonClicked", goButton);
			}
			else
			{
				updateSelection();
			}
			lastButtonPressed = b;
			break;
		case "Stage2_btn":
			GameManager.Instance.startingStage = 2;
			if (lastButtonPressed == b)
			{
				SendMessageUpwards("OnGUIButtonClicked", goButton);
			}
			else
			{
				updateSelection();
			}
			lastButtonPressed = b;
			break;
		case "Stage3_btn":
			GameManager.Instance.startingStage = 3;
			if (lastButtonPressed == b)
			{
				SendMessageUpwards("OnGUIButtonClicked", goButton);
			}
			else
			{
				updateSelection();
			}
			lastButtonPressed = b;
			break;
		case "Stage1cutscenes_btn":
			if (GameManager.Instance.currentCharacter == Character.OLIVER)
			{
				StartCoroutine(playMovies(new string[3] { "o_intro", "o_katana_weapon", "o_elevator_1" }));
			}
			else if (GameManager.Instance.currentCharacter == Character.RIGGS)
			{
				StartCoroutine(playMovies(new string[3] { "r_intro", "r_laser_weapon", "r_elevator_1" }));
			}
			else if (GameManager.Instance.currentCharacter == Character.WIL)
			{
				Debug.Log("playing");
				StartCoroutine(playMovies(new string[2] { "w_intro", "w_loss" }));
			}
			break;
		case "Stage2cutscenes_btn":
			if (GameManager.Instance.currentCharacter == Character.OLIVER)
			{
				StartCoroutine(playMovies(new string[3] { "o_boss1_intro", "o_boss1_loss", "o_boss1_win" }));
			}
			else if (GameManager.Instance.currentCharacter == Character.RIGGS)
			{
				StartCoroutine(playMovies(new string[5] { "r_boss1_intro", "r_boss1_loss", "r_boss1_win", "r_boss1udder_loss", "r_boss1udder_win" }));
			}
			else if (GameManager.Instance.currentCharacter == Character.WIL)
			{
				Debug.Log("playing");
				StartCoroutine(playMovies(new string[2] { "w_computer", "w_elevator" }));
			}
			break;
		case "Stage3cutscenes_btn":
			if (GameManager.Instance.currentCharacter == Character.OLIVER)
			{
				StartCoroutine(playMovies(new string[3] { "oco_intro", "oco_spaceboss_loss", "oco_spaceboss_win" }));
			}
			else if (GameManager.Instance.currentCharacter == Character.RIGGS)
			{
				StartCoroutine(playMovies(new string[5] { "r_boss1_intro.m4v", "r_boss1_loss.m4v", "r_boss1_win.m4v", "r_boss1udder_loss.m4v", "r_boss1udder_win.m4v" }));
			}
			else if (GameManager.Instance.currentCharacter == Character.WIL)
			{
				Debug.Log("playing");
				StartCoroutine(playMovies(new string[1] { "w_win" }));
			}
			break;
		}
	}

	private IEnumerator playMovies(string[] movies)
	{
		GUIController.isActive = false;
		GameObject fader = Object.Instantiate(Resources.Load("FaderSystem")) as GameObject;
		(fader.GetComponent(typeof(SimpleFader)) as SimpleFader).fadeTime = 1.5f;
		yield return new WaitForSeconds(0.1f);
		GameManager.Instance.isIntro = true;
		SoundManager.Instance.OnTempMusicOff();
		int i = 0;
		while (i < movies.Length)
		{
			GameManager.Instance.PlayMovie(movies[i] ?? "");
			yield return new WaitForSeconds(1f);
			int num = i + 1;
			i = num;
		}
		SoundManager.Instance.OnTempMusicOn();
		GameManager.Instance.isIntro = false;
		Object.Destroy(fader);
		GUIController.isActive = true;
	}
}
