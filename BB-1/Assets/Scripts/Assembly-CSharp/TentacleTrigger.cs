using System.Collections;
using UnityEngine;

public class TentacleTrigger : MonoBehaviour
{
	public Animation thisAnimator;

	public AudioClip entrySound;

	public AudioClip idleSound;

	private bool playIdle;

	private bool tripped;

	public ParticleSystem pePS;

	public void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Player")
		{
			StartCoroutine(TentacleEvent());
			SoundManager.Instance.playSound(entrySound);
			Object.Destroy(base.gameObject.GetComponent<Collider>());
		}
	}

	private IEnumerator TentacleHandler()
	{
		if (!tripped)
		{
			thisAnimator["tentacleEntrance"].wrapMode = WrapMode.Once;
			thisAnimator.CrossFade("tentacleEntrance");
			yield return new WaitForSeconds(thisAnimator["tentacleEntrance"].length);
			thisAnimator["tentacleIdle"].wrapMode = WrapMode.Loop;
			thisAnimator.CrossFade("tentacleIdle");
			yield return new WaitForSeconds(Random.Range(1, 15));
			thisAnimator["tentacleExit"].wrapMode = WrapMode.Once;
			thisAnimator.CrossFade("tentacleExit");
			yield return new WaitForSeconds(thisAnimator["tentacleExit"].length + 0.5f);
		}
		else
		{
			yield return new WaitForSeconds(Random.Range(5, 20));
			thisAnimator["tentacleEntrance"].wrapMode = WrapMode.Once;
			thisAnimator.CrossFade("tentacleEntrance");
			yield return new WaitForSeconds(thisAnimator["tentacleEntrance"].length);
			thisAnimator["tentacleIdle"].wrapMode = WrapMode.Loop;
			thisAnimator.CrossFade("tentacleIdle");
			yield return new WaitForSeconds(Random.Range(1, 15));
			thisAnimator["tentacleExit"].wrapMode = WrapMode.Once;
			thisAnimator.CrossFade("tentacleExit");
			yield return new WaitForSeconds(thisAnimator["tentacleExit"].length + 3.5f);
		}
	}

	private IEnumerator TentacleEvent()
	{
		thisAnimator["tentacleEntrance"].wrapMode = WrapMode.Once;
		thisAnimator.CrossFade("tentacleEntrance");
		pePS.Emit(4);
		yield return new WaitForSeconds(thisAnimator["tentacleEntrance"].length);
		playIdle = true;
		StartCoroutine(PlayIdle());
		thisAnimator["tentacleIdle"].wrapMode = WrapMode.Loop;
		thisAnimator.CrossFade("tentacleIdle");
		yield return new WaitForSeconds(Random.Range(3, 5));
		playIdle = false;
		thisAnimator["tentacleExit"].wrapMode = WrapMode.Once;
		thisAnimator.CrossFade("tentacleExit");
		yield return new WaitForSeconds(thisAnimator["tentacleExit"].length + 1.5f);
		StartCoroutine(TentacleEvent());
	}

	private IEnumerator PlayIdle()
	{
		SoundManager.Instance.playSound(idleSound);
		yield return new WaitForSeconds(idleSound.length);
		if (playIdle)
		{
			StartCoroutine(PlayIdle());
		}
	}
}
