using System.Collections;
using UnityEngine;

public class TentacleController : MonoBehaviour
{
	private Animation myAnimation;

	public Material whiteMaterial;

	private Material originalMaterial;

	private bool isWhite;

	public Renderer myRenderer;

	public float maxHP = 10f;

	private float currentHP;

	private bool canAttach = true;

	public bool isFront;

	private Transform myTransform;

	private bool hasFallen;

	public Material darkMaterial;

	private bool isInactive;

	public AudioClip hitSound;

	private bool hasRecentlyMadeSound;

	private void Awake()
	{
		maxHP *= GameManager.Instance.bossHPMultipliers[GameManager.Instance.getDifficulty()];
		currentHP = maxHP;
		myTransform = base.transform;
		if (myRenderer == null && GetComponent<Renderer>() != null)
		{
			myRenderer = GetComponent<Renderer>();
		}
		if (myRenderer != null)
		{
			originalMaterial = myRenderer.sharedMaterial;
		}
		myAnimation = GetComponent<Animation>();
		myAnimation["GrabIdle"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("GrabIdle");
	}

	public void OnStunned()
	{
		canAttach = false;
	}

	public void OnRecovered()
	{
		canAttach = true;
	}

	private IEnumerator temporarySoundDisable()
	{
		hasRecentlyMadeSound = true;
		yield return new WaitForSeconds(1f);
		hasRecentlyMadeSound = false;
	}

	private void OnBodyHasFallen()
	{
		hasFallen = true;
		StopAllCoroutines();
		if (!isFront)
		{
			StartCoroutine(fallAndDie());
			return;
		}
		myAnimation["DetachIdle"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("DetachIdle");
		currentHP = 3f;
	}

	private IEnumerator fallAndDie()
	{
		BroadcastMessage("OnSinkingIntoGround");
		yield return new WaitForSeconds(0.1f);
		myTransform.parent = null;
		while (myTransform.position.y > -50f)
		{
			Vector3 position = myTransform.position;
			position.y -= 0.2f;
			myTransform.position = position;
			yield return new WaitForSeconds(0.04f);
		}
		Object.Destroy(base.gameObject);
	}

	private IEnumerator fallAndDieFromGround()
	{
		SendMessageUpwards("OnGroundTentacleDetached", this);
		BroadcastMessage("OnSinkingIntoGround");
		myAnimation["DetachIdle"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("DetachIdle");
		yield return new WaitForSeconds(0.1f);
		myTransform.parent = null;
		while (myTransform.position.y > -50f)
		{
			Vector3 position = myTransform.position;
			position.y -= 0.2f;
			myTransform.position = position;
			yield return new WaitForSeconds(0.04f);
		}
		Object.Destroy(base.gameObject);
	}

	public void attackFromGround()
	{
		StartCoroutine(attack());
	}

	private IEnumerator attack()
	{
		myAnimation["DetachAtk"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("DetachAtk");
		yield return new WaitForSeconds(myAnimation["DetachAtk"].length);
		myAnimation["DetachIdle"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("DetachIdle");
	}

	public IEnumerator detach()
	{
		SendMessageUpwards("OnTentacleDetached");
		myAnimation["Fall"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("Fall");
		yield return new WaitForSeconds(myAnimation["Fall"].length);
		myAnimation["FallIdle"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("FallIdle");
		StartCoroutine(attach());
	}

	private IEnumerator attach()
	{
		yield return new WaitForSeconds(20f);
		while (!canAttach)
		{
			yield return new WaitForSeconds(3f);
		}
		SendMessageUpwards("OnTentacleAttached");
		myAnimation["Grab"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("Grab");
		yield return new WaitForSeconds(myAnimation["Grab"].length);
		myAnimation["GrabIdle"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("GrabIdle");
		currentHP = maxHP;
	}

	private void OnTentaclePartHit(float damage)
	{
		if (currentHP < 0f || isInactive)
		{
			return;
		}
		if (!hasRecentlyMadeSound)
		{
			SoundManager.Instance.playSound(hitSound);
			StartCoroutine(temporarySoundDisable());
		}
		if (!isWhite)
		{
			StartCoroutine(turnWhite());
		}
		currentHP -= damage;
		if (currentHP < 0f)
		{
			if (!hasFallen)
			{
				StartCoroutine(detach());
				return;
			}
			StopAllCoroutines();
			StartCoroutine(fallAndDieFromGround());
		}
	}

	private IEnumerator turnWhite()
	{
		isWhite = true;
		myRenderer.sharedMaterial = whiteMaterial;
		yield return new WaitForSeconds(0.2f);
		myRenderer.sharedMaterial = originalMaterial;
		isWhite = false;
	}

	private void OnGoInactive()
	{
		isInactive = true;
		StopAllCoroutines();
		StartCoroutine(goInactive());
		myRenderer.sharedMaterial = darkMaterial;
	}

	private IEnumerator goInactive()
	{
		myAnimation["GrabIdle"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("GrabIdle");
		yield return new WaitForSeconds(myAnimation["GrabIdle"].length);
		myAnimation.Stop();
	}
}
