using System.Collections;
using UnityEngine;

public class CrusherController : HuggableController
{
	private bool wasChasingLastTime;

	public GameObject miniWalkImpact;

	public GameObject groundSmash;

	public GameObject deathSmash;

	public AudioClip stompSound;

	public AudioClip smashSound;

	public AudioClip deathFallSound;

	protected override void Start()
	{
		base.Start();
	}

	protected override IEnumerator playDeath(string deathAnimName)
	{
		StartCoroutine(base.playDeath(deathAnimName));
		yield return new WaitForSeconds(4.2f);
		if (deathFallSound != null)
		{
			SoundManager.Instance.playSound(deathFallSound);
		}
		Object.Instantiate(deathSmash).transform.position = myTransform.position;
	}

	protected override IEnumerator mainBehaviour()
	{
		yield return new WaitForSeconds(0.1f);
		while (currentTarget != null && !isDying)
		{
			if (Vector3.Distance(myCollider.bounds.center, currentTarget.position) < attackDistance)
			{
				if (wasChasingLastTime)
				{
					StopCoroutine("spawnStepImpacts");
				}
				StartCoroutine(delayedAttackImpact());
				yield return StartCoroutine(attack());
				wasChasingLastTime = false;
			}
			else
			{
				if (!wasChasingLastTime)
				{
					StartCoroutine("spawnStepImpacts");
				}
				yield return StartCoroutine(chase(GameManager.Instance.chaseTime));
				wasChasingLastTime = true;
			}
		}
	}

	private IEnumerator delayedAttackImpact()
	{
		yield return new WaitForSeconds(1f);
		if (smashSound != null)
		{
			SoundManager.Instance.playSound(smashSound);
		}
		Object.Instantiate(groundSmash).transform.position = myTransform.position;
	}

	private IEnumerator spawnStepImpacts()
	{
		while (true)
		{
			if (stompSound != null)
			{
				SoundManager.Instance.playSound(stompSound);
			}
			Object.Instantiate(miniWalkImpact).transform.position = myTransform.position;
			yield return new WaitForSeconds(3.2f);
		}
	}
}
