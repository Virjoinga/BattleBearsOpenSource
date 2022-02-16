using System.Collections;
using UnityEngine;

public class InGameMenuController : MonoBehaviour
{
	public GameObject mainMenu;

	public GameObject huggableMenu;

	public GameObject pickupsMenu;

	public GameObject horizontalAnimator;

	public GameObject verticalAnimator;

	private GameObject currentMenu;

	public GameObject scanlinesCamera;

	public AudioClip menuClick;

	public string menuMusic;

	private void Start()
	{
		if (GameManager.Instance.isIpad)
		{
			scanlinesCamera.SetActiveRecursively(false);
		}
		SoundManager.Instance.stopAll();
		if (menuMusic != "")
		{
			if (GameManager.Instance.useHighres)
			{
				SoundManager.Instance.playMusic(Resources.Load("Music/High/" + menuMusic) as AudioClip, true);
			}
			else
			{
				SoundManager.Instance.playMusic(Resources.Load("Music/Low/" + menuMusic) as AudioClip, true);
			}
		}
		switchToMenu(mainMenu, true, false);
	}

	private void switchToMenu(GameObject newMenu, bool instantPop, bool goForward)
	{
		GUIController.isActive = false;
		StartCoroutine(delayedSwitch(newMenu, instantPop, goForward));
	}

	private IEnumerator delayedSwitch(GameObject newMenu, bool instantPop, bool goForward)
	{
		if (!instantPop)
		{
			GameObject outgoingTopSlider = Object.Instantiate(verticalAnimator);
			GameObject outgoingBottomSlider = Object.Instantiate(verticalAnimator);
			GameObject outgoingHorizontalSlider = Object.Instantiate(horizontalAnimator);
			if (currentMenu != null)
			{
				currentMenu.transform.Find("MiddleRow").parent = outgoingHorizontalSlider.transform;
				currentMenu.transform.Find("TopRow").parent = outgoingTopSlider.transform;
				currentMenu.transform.Find("BottomRow").parent = outgoingBottomSlider.transform;
				outgoingTopSlider.GetComponent<Animation>().Play("middleToTop");
				outgoingBottomSlider.GetComponent<Animation>().Play("middleToBottom");
				if (goForward)
				{
					outgoingHorizontalSlider.GetComponent<Animation>().Play("middleToLeft");
				}
				else
				{
					outgoingHorizontalSlider.GetComponent<Animation>().Play("middleToRight");
				}
			}
			GameObject incomingTopSlider = Object.Instantiate(verticalAnimator);
			GameObject incomingBottomSlider = Object.Instantiate(verticalAnimator);
			GameObject incomingHorizontalSlider = Object.Instantiate(horizontalAnimator);
			if (newMenu != null)
			{
				GameObject incomingMenu = Object.Instantiate(newMenu);
				incomingMenu.transform.Find("MiddleRow").parent = incomingHorizontalSlider.transform;
				incomingMenu.transform.Find("TopRow").parent = incomingTopSlider.transform;
				incomingMenu.transform.Find("BottomRow").parent = incomingBottomSlider.transform;
				incomingTopSlider.GetComponent<Animation>().Play("topToMiddle");
				incomingBottomSlider.GetComponent<Animation>().Play("bottomToMiddle");
				if (goForward)
				{
					incomingHorizontalSlider.GetComponent<Animation>().Play("rightToMiddle");
					yield return new WaitForSeconds(incomingHorizontalSlider.GetComponent<Animation>()["rightToMiddle"].length);
				}
				else
				{
					incomingHorizontalSlider.GetComponent<Animation>().Play("leftToMiddle");
					yield return new WaitForSeconds(incomingHorizontalSlider.GetComponent<Animation>()["leftToMiddle"].length);
				}
				incomingHorizontalSlider.transform.Find("MiddleRow").parent = incomingMenu.transform;
				incomingTopSlider.transform.Find("TopRow").parent = incomingMenu.transform;
				incomingBottomSlider.transform.Find("BottomRow").parent = incomingMenu.transform;
				incomingMenu.transform.parent = base.transform;
				if (currentMenu != null)
				{
					Object.Destroy(currentMenu);
				}
				currentMenu = incomingMenu;
			}
			else
			{
				yield return new WaitForSeconds(outgoingHorizontalSlider.GetComponent<Animation>()["leftToMiddle"].length);
			}
			yield return new WaitForSeconds(0.5f);
			Object.Destroy(incomingHorizontalSlider);
			Object.Destroy(outgoingHorizontalSlider);
			Object.Destroy(outgoingTopSlider);
			Object.Destroy(outgoingBottomSlider);
			Object.Destroy(incomingTopSlider);
			Object.Destroy(incomingBottomSlider);
		}
		else
		{
			yield return new WaitForSeconds(0.25f);
			if (currentMenu != null)
			{
				Object.Destroy(currentMenu);
			}
			currentMenu = Object.Instantiate(newMenu);
			currentMenu.transform.parent = base.transform;
		}
		GUIController.isActive = true;
	}

	public void OnGUIButtonClicked(GUIButton b)
	{
		SoundManager.Instance.playSound(menuClick);
		switch (b.name)
		{
		case "exit_btn":
			StartCoroutine(delayedLoadLevel("MainMenu"));
			break;
		case "MainStatsFromLeft_btn":
			switchToMenu(mainMenu, false, true);
			break;
		case "MainStatsFromRight_btn":
			switchToMenu(mainMenu, false, false);
			break;
		case "pickup_btn":
			switchToMenu(pickupsMenu, false, false);
			break;
		case "huggables_btn":
			switchToMenu(huggableMenu, false, true);
			break;
		}
	}

	private IEnumerator delayedLoadLevel(string level)
	{
		switchToMenu(null, false, true);
		yield return new WaitForSeconds(1f);
		Application.LoadLevel(level);
	}
}
