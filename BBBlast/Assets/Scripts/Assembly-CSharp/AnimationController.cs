using System;
using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	private bool canBeIdle;

	private float waitTime;

	private float myTime;

	public string[] anims;

	public Animation cameraAnim;

	private string[] upper;

	private string[] lower;

	private bool isDead;

	private Transform myTransform;

	private Animation myAnimation;

	private void Awake()
	{
		myTransform = base.transform;
		myAnimation = base.GetComponent<Animation>();
	}

	private void Start()
	{
		isDead = false;
		for (int i = 0; i < anims.Length; i++)
		{
			if (anims[i].Contains("Upper"))
			{
				myAnimation[anims[i]].layer = 0;
				myAnimation[anims[i]].weight = 1f;
			}
			if (anims[i].Contains("Lower"))
			{
				myAnimation[anims[i]].layer = 1;
				myAnimation[anims[i]].weight = 1f;
			}
		}
	}

	public void idle(bool shoot)
	{
		if (!isDead)
		{
			myAnimation["IdleLower"].wrapMode = WrapMode.Loop;
			myAnimation["IdleLower"].speed = 1f;
			myAnimation.CrossFade("IdleLower");
			if (shoot)
			{
				myAnimation["IdleFireUpper"].wrapMode = WrapMode.Loop;
				myAnimation["IdleFireUpper"].speed = 1f;
				myAnimation.CrossFade("IdleFireUpper");
			}
			else
			{
				myAnimation["IdleUpper"].wrapMode = WrapMode.Loop;
				myAnimation["IdleUpper"].speed = 1f;
				myAnimation.CrossFade("IdleUpper");
			}
		}
	}

	public void walkLeft(bool shoot)
	{
		if (!isDead)
		{
			myAnimation["WalkLeftLower"].wrapMode = WrapMode.Loop;
			myAnimation["WalkLeftLower"].speed = 1f;
			myAnimation.CrossFade("WalkLeftLower");
			if (shoot)
			{
				myAnimation["WalkAtkLeftUpper"].wrapMode = WrapMode.Loop;
				myAnimation["WalkAtkLeftUpper"].speed = 1f;
				myAnimation.CrossFade("WalkAtkLeftUpper");
			}
			else
			{
				myAnimation["WalkLeftUpper"].wrapMode = WrapMode.Loop;
				myAnimation["WalkLeftUpper"].speed = 1f;
				myAnimation.CrossFade("WalkLeftUpper");
			}
		}
	}

	public void walkRight(bool shoot)
	{
		if (!isDead)
		{
			myAnimation["WalkRightLower"].wrapMode = WrapMode.Loop;
			myAnimation["WalkRightLower"].speed = 1f;
			myAnimation.CrossFade("WalkRightLower");
			if (shoot)
			{
				myAnimation["WalkAtkRightUpper"].wrapMode = WrapMode.Loop;
				myAnimation["WalkAtkRightUpper"].speed = 1f;
				myAnimation.CrossFade("WalkAtkRightUpper");
			}
			else
			{
				myAnimation["WalkRightUpper"].wrapMode = WrapMode.Loop;
				myAnimation["WalkRightUpper"].speed = 1f;
				myAnimation.CrossFade("WalkRightUpper");
			}
		}
	}

	public void news()
	{
		myAnimation.Play("news");
		StartCoroutine(backToIdle("news"));
	}

	public void music()
	{
		myAnimation.Play("music");
		StartCoroutine(backToIdle("music"));
	}

	public void mute()
	{
		myAnimation.Play("mute");
		StartCoroutine(backToIdle("mute"));
	}

	public void store()
	{
		myAnimation.Play("store");
		StartCoroutine(backToIdle("store"));
	}

	public void backpack()
	{
		myAnimation.Stop();
		myAnimation.CrossFade("backpackOpen");
		cameraAnim.Play("BackpackZoom");
	}

	public float death()
	{
		isDead = true;
		myAnimation.Stop();
		myAnimation.CrossFade("Death");
		StartCoroutine("deathOff");
		return myAnimation.GetClip("Death").length;
	}

	private IEnumerator deathOff()
	{
		yield return new WaitForSeconds(myAnimation.GetClip("Death").length + 1f);
		isDead = false;
	}

	public void pause()
	{
		myAnimation.Stop();
	}

	public IEnumerator backToIdle(string anim)
	{
		yield return new WaitForSeconds(myAnimation.GetClip(anim).length * 2f);
		idle(false);
	}

	public IEnumerator move(float target, bool shoot)
	{
		if (isDead)
		{
			yield break;
		}
		yield return new WaitForSeconds(0.15f);
		if (target == 0f)
		{
			if (myTransform.localScale.x < 0f)
			{
				walkRight(shoot);
			}
			else
			{
				walkRight(shoot);
			}
		}
		else if (target < 0f)
		{
			walkRight(shoot);
		}
		else if (target > 0f)
		{
			walkLeft(shoot);
		}
		float newX = Mathf.Lerp(myTransform.localPosition.x, target, 0.05f);
		myTransform.localPosition = new Vector3(newX, myTransform.localPosition.y, myTransform.localPosition.z);
		if (Math.Round(myTransform.localPosition.x) == (double)target)
		{
			idle(shoot);
			Objectives.Instance.stopCountDown();
		}
		else
		{
			Objectives.Instance.startCountDown();
		}
	}

	public IEnumerator scoot(float target)
	{
		yield return new WaitForSeconds(1f);
		for (int loops = 36; loops > 0; loops--)
		{
			float z = Mathf.Lerp(myTransform.localPosition.z, target, 0.06f);
			myTransform.localPosition = new Vector3(myTransform.localPosition.x, myTransform.localPosition.y, z);
			yield return new WaitForSeconds(0.1f);
		}
	}
}
