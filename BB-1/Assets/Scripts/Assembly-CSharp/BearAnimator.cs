using System.Collections;
using UnityEngine;

public class BearAnimator : MonoBehaviour
{
	public string currentAnimation;

	public string[] walkAnimations;

	public string[] armAnimations;

	public Animation myAnimator;

	private Transform playerTransform;

	private GameObject currentWeapon;

	private string currentShootingAnimation = "";

	private bool firingSingleShot;

	private bool isOverriding;

	private void Awake()
	{
		myAnimator = GetComponent<Animation>();
		playerTransform = base.transform.parent;
		for (int i = 0; i < walkAnimations.Length; i++)
		{
			myAnimator[walkAnimations[i]].layer = 0;
			myAnimator[walkAnimations[i]].wrapMode = WrapMode.Loop;
		}
		for (int j = 0; j < armAnimations.Length; j++)
		{
			myAnimator[armAnimations[j]].layer = 1;
			myAnimator[armAnimations[j]].wrapMode = WrapMode.Loop;
		}
	}

	public void setWeapon(GameObject weapon)
	{
		string text = ((currentWeapon != null) ? currentWeapon.name : "");
		currentWeapon = weapon;
		if (!(this == null) && !(base.gameObject == null))
		{
			StopCoroutine("fireSingleShot");
			firingSingleShot = false;
			if (weapon.name == "MachineGun" && text == "Laser")
			{
				myAnimator.Play("idle");
			}
		}
	}

	public IEnumerator playFallAnimation()
	{
		myAnimator["fall"].wrapMode = WrapMode.ClampForever;
		myAnimator.CrossFade("fall");
		myAnimator["fall" + currentWeapon.name].wrapMode = WrapMode.ClampForever;
		myAnimator.CrossFade("fall" + currentWeapon.name);
		yield return new WaitForSeconds(myAnimator["fall"].length + 0.5f);
		if (GameManager.Instance.FPSMode)
		{
			myAnimator["idle"].wrapMode = WrapMode.ClampForever;
			myAnimator.CrossFade("idle");
			myAnimator["idle" + currentWeapon.name].wrapMode = WrapMode.ClampForever;
			myAnimator.CrossFade("attack" + currentWeapon.name);
		}
	}

	public IEnumerator playDeathAnimation()
	{
		myAnimator["death"].wrapMode = WrapMode.ClampForever;
		myAnimator.CrossFade("death");
		myAnimator["death" + currentWeapon.name].wrapMode = WrapMode.ClampForever;
		myAnimator.CrossFade("death" + currentWeapon.name);
		yield return new WaitForSeconds(myAnimator["death"].length + 2f);
		if (GameManager.Instance.FPSMode)
		{
			myAnimator["idle"].wrapMode = WrapMode.ClampForever;
			myAnimator.CrossFade("idle");
			myAnimator["idle" + currentWeapon.name].wrapMode = WrapMode.ClampForever;
			myAnimator.CrossFade("attack" + currentWeapon.name);
		}
	}

	public void animateBear(Vector3 movement, bool shooting)
	{
		movement.y = 0f;
		float magnitude = movement.magnitude;
		if (Mathf.Abs(movement.x) > 0.01f || Mathf.Abs(movement.z) > 0.01f)
		{
			float num = Vector3.Angle(movement, playerTransform.forward);
			string text = "";
			text = ((num < 45f) ? "walkForward" : ((num > 135f) ? "walkBackward" : ((!(Vector3.Angle(movement, playerTransform.right) < 90f)) ? "walkLeftward" : "walkRightward")));
			myAnimator[text].speed = magnitude * 0.125f;
			myAnimator.CrossFade(text);
			handleArmAnimations(shooting, magnitude, "walk");
		}
		else
		{
			myAnimator.CrossFade("idle");
			handleArmAnimations(shooting, 0f, "idle");
		}
	}

	private void handleArmAnimations(bool shooting, float movementAmount, string movementString)
	{
		if (isOverriding || !(currentWeapon != null) || firingSingleShot)
		{
			return;
		}
		string text = "";
		if (shooting)
		{
			text = "attack" + currentWeapon.name;
			if (currentWeapon.name == "MachineGun")
			{
				myAnimator[text].speed = 0.5f;
			}
			else if (currentWeapon.name == "Spreadshot")
			{
				myAnimator[text].speed = 0.5f;
			}
			else if (currentWeapon.name == "Katana")
			{
				myAnimator[text].speed = 1f;
			}
			else if (currentWeapon.name == "Arrow")
			{
				myAnimator[text].speed = 2f;
			}
			else if (currentWeapon.name == "Chainsaw")
			{
				myAnimator[text].speed = 1f;
			}
			else if (currentWeapon.name == "Bearzooka")
			{
				myAnimator[text].wrapMode = WrapMode.ClampForever;
				myAnimator[text].speed = 1f;
				StartCoroutine("fireSingleShot", myAnimator[text].length * 0.25f);
			}
			else if (currentWeapon.name == "Shotgun")
			{
				myAnimator[text].speed = 1f;
				StartCoroutine(fireSingleShot(0.89f));
			}
		}
		else if (!firingSingleShot)
		{
			text = movementString + currentWeapon.name;
			if (movementString == "walk")
			{
				myAnimator[text].speed = movementAmount * 0.125f;
			}
		}
		if (currentShootingAnimation != text)
		{
			if (text == "attackMachineGun" && currentShootingAnimation == "attackLaser")
			{
				myAnimator.Play(text, PlayMode.StopSameLayer);
			}
			else
			{
				myAnimator.CrossFade(text);
			}
			currentShootingAnimation = text;
		}
	}

	public void blockAnimation()
	{
		if (!isOverriding)
		{
			StartCoroutine(delayedBlockAnimation());
		}
	}

	private IEnumerator delayedBlockAnimation()
	{
		string animation = "block" + currentWeapon.name;
		if (myAnimator[animation] != null)
		{
			isOverriding = true;
			myAnimator[animation].wrapMode = WrapMode.ClampForever;
			myAnimator.CrossFade(animation);
			currentShootingAnimation = animation;
			yield return new WaitForSeconds(myAnimator[animation].length);
			isOverriding = false;
		}
	}

	private IEnumerator fireSingleShot(float delay)
	{
		firingSingleShot = true;
		yield return new WaitForSeconds(delay);
		firingSingleShot = false;
	}
}
