using System.Collections;
using UnityEngine;

public class DelayedSpecialWeaponScene : MonoBehaviour
{
	public float timeUntilSpecialWeapon = 20f;

	public GameObject specialPickupPrefab;

	private void Start()
	{
		StartCoroutine(delayedSpecialWeapon());
	}

	private IEnumerator delayedSpecialWeapon()
	{
		yield return new WaitForSeconds(timeUntilSpecialWeapon);
		GameObject fader = Object.Instantiate(Resources.Load("FaderSystem")) as GameObject;
		(fader.GetComponent(typeof(SimpleFader)) as SimpleFader).fadeTime = 1.5f;
		yield return new WaitForSeconds(0.01f);
		Object.Destroy(fader);
		if (GameManager.Instance.currentGameMode == GameMode.CAMPAIGN)
		{
			if (GameManager.Instance.currentCharacter == Character.OLIVER)
			{
				GameManager.Instance.PlayMovie("o_katana_weapon");
			}
			else if (GameManager.Instance.currentCharacter == Character.RIGGS)
			{
				GameManager.Instance.PlayMovie("r_laser_weapon");
			}
		}
		Vector3 position = GameObject.FindWithTag("Player").transform.root.position;
		Object.Instantiate(specialPickupPrefab, position, Quaternion.identity);
		GameManager.Instance.hasAcquiredSpecial = true;
		yield return new WaitForSeconds(0.5f);
		if (GameManager.Instance.currentCharacter == Character.OLIVER)
		{
			HUDController.Instance.switchToWeapon(3);
		}
		else if (GameManager.Instance.currentCharacter == Character.RIGGS)
		{
			HUDController.Instance.switchToWeapon(4);
		}
		Object.Destroy(this);
	}
}
