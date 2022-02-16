using System.Collections;
using UnityEngine;

public class GhostController : HuggableController
{
	public float minFadedOutTime = 2f;

	public float maxFadedOutTime = 10f;

	public float minFadedInTime = 2f;

	public float maxFadedInTime = 10f;

	private float originalColliderCenterHeight;

	private bool attacking;

	public AudioClip fadeOutSound;

	public AudioClip fadeInSound;

	public Material fadeInMat;

	protected override void Start()
	{
		base.Start();
		originalColliderCenterHeight = myCollider.bounds.center.y;
		StartCoroutine("fadeSkill");
	}

	protected override IEnumerator playDeath(string deathAnimName)
	{
		StartCoroutine(base.playDeath(deathAnimName));
		StopCoroutine("fadeSkill");
		bool on = false;
		Color c = myRenderer.material.color;
		while (c.a > 0f)
		{
			on = !on;
			c.a -= 0.06f;
			myRenderer.material.color = c;
			yield return new WaitForSeconds(0.05f);
		}
	}

	protected override IEnumerator attack()
	{
		StartCoroutine(base.attack());
		yield return new WaitForSeconds(0.01f);
		StopCoroutine("fadeIn");
		StopCoroutine("fadeOut");
		StartCoroutine("fadeIn");
		attacking = true;
		yield return new WaitForSeconds(1.5f);
		attacking = false;
	}

	private IEnumerator fadeSkill()
	{
		while (true)
		{
			yield return new WaitForSeconds(minFadedInTime + (maxFadedInTime - minFadedInTime) * Random.value);
			while (attacking)
			{
				yield return new WaitForSeconds(0.1f);
			}
			StopCoroutine("fadeIn");
			StartCoroutine("fadeOut");
			yield return new WaitForSeconds(minFadedOutTime + (maxFadedOutTime - minFadedOutTime) * Random.value);
			while (attacking)
			{
				yield return new WaitForSeconds(0.1f);
			}
			StopCoroutine("fadeOut");
			StartCoroutine("fadeIn");
		}
	}

	private IEnumerator fadeOut()
	{
		base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		myRigidbody.detectCollisions = false;
		dmgReceiver.isInvincible = true;
		Color c = myRenderer.material.color;
		SoundManager.Instance.playSound(fadeOutSound);
		while (c.a > 0f)
		{
			c.a -= 0.06f;
			myRenderer.material.color = c;
			yield return new WaitForSeconds(0.05f);
		}
	}

	private IEnumerator fadeIn()
	{
		Color c = myRenderer.material.color;
		SoundManager.Instance.playSound(fadeInSound);
		while (c.a < 1f)
		{
			c.a += 0.06f;
			myRenderer.material.color = c;
			yield return new WaitForSeconds(0.05f);
		}
		myRenderer.sharedMaterial = fadeInMat;
		base.gameObject.layer = LayerMask.NameToLayer("Enemy");
		dmgReceiver.isInvincible = false;
		myRigidbody.detectCollisions = true;
	}
}
