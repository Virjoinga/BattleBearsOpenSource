using System.Collections;
using UnityEngine;

public class SlideSplashController : MonoBehaviour
{
	public GameObject bottom;

	public GameObject middle;

	public GameObject top;

	public ParticleEmitter btmDestroy;

	public ParticleEmitter midDestroy;

	public ParticleEmitter topDestroy;

	public ParticleEmitter dust;

	private float index;

	public bool done;

	public AudioClip open;

	public AudioClip close;

	public AudioClip targetHit;

	private void Awake()
	{
		done = false;
		index = 0f;
		//bottom.active = false;
		bottom.SetActive(false);
		btmDestroy.emit = false;
		//middle.active = false;
		middle.SetActive(false);
		midDestroy.emit = false;
		//top.active = false;
		top.SetActive(false);
		topDestroy.emit = false;
		dust.emit = false;
		StartCoroutine("intro");
	}

	private IEnumerator intro()
	{
		base.GetComponent<Animation>()["intro"].speed = -1f;
		base.GetComponent<Animation>()["intro"].time = base.GetComponent<Animation>().GetClip("intro").length;
		base.GetComponent<Animation>()["intro"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>().Play("intro");
		yield return new WaitForSeconds(base.GetComponent<Animation>().GetClip("intro").length * 0.95f);
		Object.Instantiate(Resources.Load("SlideToBlast"));
		//top.active = false;
		top.SetActive(false);
		//middle.active = true;
		middle.SetActive(true);
		//bottom.active = false;
		bottom.SetActive(false);
	}

	private void Update()
	{
		if (index >= 1f)
		{
			base.GetComponent<Animation>()["intro"].speed = 1f;
			base.GetComponent<Animation>()["intro"].time = 0f;
			base.GetComponent<Animation>()["intro"].wrapMode = WrapMode.ClampForever;
			base.GetComponent<Animation>().Play("intro");
			StartCoroutine("delete");
			index = 0f;
			done = true;
		}
	}

	public void ButtonPress(string name)
	{
		SoundController.Instance.playClip(targetHit);
		switch (name)
		{
		case "BottomTarget":
			btmDestroy.emit = true;
			index += 1f;
			StartCoroutine("waitDeath", bottom);
			StartCoroutine("stop");
			break;
		case "MiddleTarget":
			midDestroy.emit = true;
			StartCoroutine("waitDeath", middle);
			index += 1f;
			StartCoroutine("stop");
			break;
		case "TopTarget":
			topDestroy.emit = true;
			StartCoroutine("waitDeath", top);
			index += 1f;
			StartCoroutine("stop");
			break;
		}
	}

	private IEnumerator stop()
	{
		yield return new WaitForSeconds(0.5f);
		btmDestroy.emit = false;
		midDestroy.emit = false;
		topDestroy.emit = false;
	}

	private IEnumerator waitDeath(GameObject obj)
	{
		Object.Destroy(obj.GetComponent<Collider>());
		yield return new WaitForSeconds(0.1f);
		//obj.SetActiveRecursively(false);
		obj.SetActive(false);
	}

	private IEnumerator delete()
	{
		yield return new WaitForSeconds(0.1f);
		dust.emit = true;
		yield return new WaitForSeconds(0.1f);
		dust.emit = false;
		GameManager.Instance.tronActive = true;
		yield return new WaitForSeconds(base.GetComponent<Animation>().GetClip("intro").length);
		Object.Destroy(base.gameObject);
	}
}
