using System.Collections;
using UnityEngine;

public class OCOHUDController : MonoBehaviour
{
	private OCOController ocoController;

	private int currentPowerLevel;

	private float timeToGainLevel = 10f;

	private float powerGainTimeLeft = 10f;

	public GameObject[] onButtons;

	public Transform[] powerLevels;

	public GameObject oldLife;

	public GameObject oldAmmo;

	private void Start()
	{
		ocoController = Object.FindObjectOfType(typeof(OCOController)) as OCOController;
		if (ocoController == null)
		{
			Debug.Log("no OCO in the scene!");
		}
		oldLife.SetActiveRecursively(false);
		oldLife.SetActiveRecursively(false);
		StartCoroutine(powerGainer());
	}

	private IEnumerator powerGainer()
	{
		resetPowerLevels();
		currentPowerLevel = 0;
		powerGainTimeLeft = timeToGainLevel;
		while (true)
		{
			if (currentPowerLevel < powerLevels.Length)
			{
				powerGainTimeLeft -= 0.1f;
			}
			if (powerGainTimeLeft <= 0f)
			{
				powerGainTimeLeft = 0f;
				if (currentPowerLevel < powerLevels.Length)
				{
					currentPowerLevel++;
					powerGainTimeLeft = timeToGainLevel;
				}
			}
			updatePowerLevels();
			yield return new WaitForSeconds(0.1f * Time.timeScale);
		}
	}

	private void resetPowerLevels()
	{
		for (int i = 0; i < powerLevels.Length; i++)
		{
			Vector3 localScale = powerLevels[i].localScale;
			localScale.z = 0f;
			powerLevels[i].localScale = localScale;
			onButtons[i].active = false;
		}
	}

	private void updatePowerLevels()
	{
		for (int i = 0; i < currentPowerLevel; i++)
		{
			Vector3 localScale = powerLevels[i].localScale;
			localScale.z = 1f;
			powerLevels[i].localScale = localScale;
		}
		for (int j = currentPowerLevel; j < onButtons.Length; j++)
		{
			Vector3 localScale2 = powerLevels[j].localScale;
			localScale2.z = 0f;
			powerLevels[j].localScale = localScale2;
		}
		if (currentPowerLevel < powerLevels.Length)
		{
			Vector3 localScale3 = powerLevels[currentPowerLevel].localScale;
			localScale3.z = 1f - powerGainTimeLeft / timeToGainLevel;
			powerLevels[currentPowerLevel].localScale = localScale3;
		}
	}

	public void Lazer()
	{
		if (currentPowerLevel >= 1 && !ocoController.isStunned() && !ocoController.isUsingSpecial())
		{
			currentPowerLevel--;
			ocoController.OnLaser();
		}
	}

	public void Screenclear()
	{
		if (currentPowerLevel >= 3 && !ocoController.isStunned() && !ocoController.isUsingSpecial())
		{
			currentPowerLevel -= 3;
			ocoController.OnScreenClearAttack();
		}
	}

	public void Timeslow()
	{
		if (currentPowerLevel >= 2 && !ocoController.isStunned())
		{
			currentPowerLevel -= 2;
			ocoController.OnSlowTime();
		}
	}

	public void updateSelection(int x)
	{
		for (int i = 0; i < onButtons.Length; i++)
		{
			onButtons[i].active = false;
		}
		if (x != 3)
		{
			onButtons[x].active = true;
		}
	}

	public int FireSelection()
	{
		int result = 3;
		for (int i = 0; i < onButtons.Length; i++)
		{
			if (onButtons[0].active)
			{
				Lazer();
				result = 0;
			}
			else if (onButtons[1].active)
			{
				Timeslow();
				result = 1;
			}
			else if (onButtons[2].active)
			{
				Screenclear();
				result = 2;
			}
		}
		return result;
	}

	public void OnGUIButtonClicked(GUIButton b)
	{
		switch (b.name)
		{
		case "Lazer":
			Lazer();
			break;
		case "Screenclear":
			Screenclear();
			break;
		case "Timeslow":
			Timeslow();
			break;
		}
	}
}
