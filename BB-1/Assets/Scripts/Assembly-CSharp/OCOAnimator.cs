using System.Collections;
using UnityEngine;

public class OCOAnimator : MonoBehaviour
{
	public string[] legAnimations;

	public string[] armAnimations;

	private Animation myAnimator;

	private Transform playerTransform;

	private GameObject currentWeapon;

	private string currentShootingAnimation = "";

	private bool isOverriding;

	private bool firingSingleShot;

	private void Awake()
	{
		myAnimator = GetComponent<Animation>();
		playerTransform = base.transform.parent;
		for (int i = 0; i < legAnimations.Length; i++)
		{
			myAnimator[legAnimations[i]].layer = 0;
			myAnimator[legAnimations[i]].wrapMode = WrapMode.Loop;
		}
		for (int j = 0; j < armAnimations.Length; j++)
		{
			myAnimator[armAnimations[j]].layer = 1;
			myAnimator[armAnimations[j]].wrapMode = WrapMode.Loop;
		}
	}

	private void Start()
	{
		resetToIdle();
	}

	public void OnStun()
	{
		currentShootingAnimation = "";
		myAnimator.Stop();
		myAnimator["stunImpact"].wrapMode = WrapMode.Once;
		myAnimator["stunImpact"].speed = ((Time.timeScale == 0f) ? 1f : (1f / Time.timeScale));
		myAnimator.Play("stunImpact");
		myAnimator["stunLoop"].wrapMode = WrapMode.Loop;
		myAnimator["stunLoop"].speed = ((Time.timeScale == 0f) ? 1f : (1f / Time.timeScale));
		myAnimator.CrossFadeQueued("stunLoop", 0.2f, QueueMode.CompleteOthers);
	}

	public void resetToIdle()
	{
		myAnimator["floatIdle"].speed = ((Time.timeScale == 0f) ? 1f : (1f / Time.timeScale));
		myAnimator.CrossFade("floatIdle");
		myAnimator["idle"].speed = ((Time.timeScale == 0f) ? 1f : (1f / Time.timeScale));
		myAnimator.CrossFade("idle");
	}

	private IEnumerator delayedStartIdle()
	{
		yield return new WaitForSeconds(0.1f * Time.timeScale);
		myAnimator["floatIdle"].speed = ((Time.timeScale == 0f) ? 1f : (1f / Time.timeScale));
		myAnimator.CrossFade("floatIdle");
	}

	public void useLaser()
	{
		if (!isOverriding)
		{
			isOverriding = true;
			myAnimator.Stop();
			myAnimator["attackLaser"].speed = ((Time.timeScale == 0f) ? 1f : (1f / Time.timeScale));
			myAnimator.CrossFade("attackLaser");
			StartCoroutine(delayedOverride(myAnimator["attackLaser"].length));
		}
	}

	public void useScreenClear()
	{
		if (!isOverriding)
		{
			isOverriding = true;
			myAnimator.Stop();
			myAnimator["FullBodyExplosion"].speed = ((Time.timeScale == 0f) ? 1f : (1f / Time.timeScale));
			myAnimator.CrossFade("FullBodyExplosion");
			StartCoroutine(delayedOverride(myAnimator["FullBodyExplosion"].length));
		}
	}

	private IEnumerator delayedOverride(float duration)
	{
		yield return new WaitForSeconds(duration * Time.timeScale);
		resetToIdle();
		isOverriding = false;
	}

	public void setWeapon(GameObject weapon)
	{
		currentWeapon = weapon;
		StopCoroutine("fireSingleShot");
		firingSingleShot = false;
	}

	public void animateBear(Vector3 movement, bool shooting)
	{
		if (!isOverriding)
		{
			movement.y = 0f;
			float magnitude = movement.magnitude;
			if (Mathf.Abs(movement.x) > 0.01f || Mathf.Abs(movement.z) > 0.01f)
			{
				float num = Vector3.Angle(movement, playerTransform.forward);
				string text = "";
				text = ((num < 45f) ? "floatForward" : ((num > 135f) ? "floatBackward" : ((!(Vector3.Angle(movement, playerTransform.right) < 90f)) ? "floatLeftward" : "floatRightward")));
				myAnimator[text].speed = magnitude / 8f;
				myAnimator[text].speed = ((Time.timeScale == 0f) ? 1f : (1f / Time.timeScale));
				myAnimator.CrossFade(text);
				handleArmAnimations(shooting, magnitude, "floatMove");
			}
			else
			{
				myAnimator["floatIdle"].speed = ((Time.timeScale == 0f) ? 1f : (1f / Time.timeScale));
				myAnimator.CrossFade("floatIdle");
				handleArmAnimations(shooting, 0f, "idle");
			}
		}
	}

	private void handleArmAnimations(bool shooting, float movementAmount, string movementString)
	{
		if (!(currentWeapon != null) || firingSingleShot)
		{
			return;
		}
		string text = "";
		if (shooting)
		{
			text = "attack" + currentWeapon.name;
			if (currentWeapon.name == "Normal")
			{
				myAnimator[text].wrapMode = WrapMode.Loop;
				myAnimator[text].speed = ((Time.timeScale == 0f) ? 1f : (1f / Time.timeScale));
			}
		}
		else if (!firingSingleShot)
		{
			text = movementString;
		}
		if (currentShootingAnimation != text)
		{
			myAnimator[text].speed = ((Time.timeScale == 0f) ? 1f : (1f / Time.timeScale));
			myAnimator.CrossFade(text);
			currentShootingAnimation = text;
		}
	}

	private IEnumerator fireSingleShot(float delay)
	{
		firingSingleShot = true;
		yield return new WaitForSeconds(delay * Time.timeScale);
		firingSingleShot = false;
	}
}
