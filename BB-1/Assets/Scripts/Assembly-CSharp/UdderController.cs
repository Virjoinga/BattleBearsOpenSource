using System.Collections;
using UnityEngine;

public class UdderController : MonoBehaviour
{
	public Material whiteMaterial;

	private Material originalMaterial;

	public Renderer myRenderer;

	public float maxHP = 350f;

	private float currentHP;

	private bool isWhite;

	public bool isHanging = true;

	private float damageSoFar;

	public float spewEveryXDamage = 50f;

	private Animation myAnimation;

	public ProjectileControllerPrefab spewer;

	private Transform myTransform;

	public GameObject detachedUdder;

	private Transform rootTransform;

	public ParticleSystem milkShooterPS;

	public AudioClip spitSound;

	public AudioClip hitSound;

	public AudioClip spawnSound;

	private bool hasRecentlyMadeSound;

	public TentacleeseController tc;

	private void Awake()
	{
		myTransform = base.transform;
		myAnimation = GetComponent<Animation>();
		maxHP *= GameManager.Instance.bossHPMultipliers[GameManager.Instance.getDifficulty()];
		currentHP = maxHP;
		if (myRenderer == null && GetComponent<Renderer>() != null)
		{
			myRenderer = GetComponent<Renderer>();
		}
		if (myRenderer != null)
		{
			originalMaterial = myRenderer.sharedMaterial;
		}
		myAnimation["UdderHangingIdle"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("UdderHangingIdle");
		StartCoroutine(periodicShooter());
	}

	private IEnumerator temporarySoundDisable()
	{
		hasRecentlyMadeSound = true;
		yield return new WaitForSeconds(1f);
		hasRecentlyMadeSound = false;
	}

	private IEnumerator periodicShooter()
	{
		while (isHanging)
		{
			yield return new WaitForSeconds(Random.value * 10f + 5f);
			int i = 0;
			while (i < 3)
			{
				myAnimation["UdderAttachAtk"].wrapMode = WrapMode.Once;
				myAnimation.CrossFade("UdderAttachAtk");
				yield return new WaitForSeconds(0.5f);
				ParticleSystem.EmissionModule emit = milkShooterPS.emission;
				emit.enabled = true;
				milkShooterPS.Play();
				SoundManager.Instance.playSound(spitSound);
				yield return new WaitForSeconds(1.25f);
				emit.enabled = false;
				milkShooterPS.Stop();
				yield return new WaitForSeconds(myAnimation["UdderAttachAtk"].length - 1.75f);
				myAnimation["UdderHangingIdle"].wrapMode = WrapMode.Loop;
				myAnimation.CrossFade("UdderHangingIdle");
				yield return new WaitForSeconds(1f);
				int num = i + 1;
				i = num;
			}
			myAnimation["UdderHangingIdle"].wrapMode = WrapMode.Loop;
			myAnimation.CrossFade("UdderHangingIdle");
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

	public void OnUdderHit(float damage)
	{
		if (currentHP <= 0f || !isHanging)
		{
			return;
		}
		if (!isWhite)
		{
			StartCoroutine(turnWhite());
		}
		if (!hasRecentlyMadeSound)
		{
			SoundManager.Instance.playSound(hitSound);
			StartCoroutine(temporarySoundDisable());
		}
		currentHP -= damage;
		damageSoFar += damage;
		if (currentHP <= 0f)
		{
			ParticleSystem.EmissionModule emission = milkShooterPS.emission;
			emission.enabled = false;
			isHanging = false;
			myAnimation.Stop();
			StopAllCoroutines();
			myRenderer.sharedMaterial = originalMaterial;
			SendMessageUpwards("OnUdderDetached");
			rootTransform = myTransform.root;
			myTransform.parent = null;
			if (tc.isHanging)
			{
				StartCoroutine(fall());
			}
		}
		else if (damageSoFar > spewEveryXDamage)
		{
			damageSoFar = 0f;
			ParticleSystem.EmissionModule emission2 = milkShooterPS.emission;
			emission2.enabled = false;
			myAnimation["UdderHuggableSpew"].wrapMode = WrapMode.Loop;
			myAnimation.CrossFade("UdderHuggableSpew");
			StartCoroutine(spew());
		}
	}

	private IEnumerator spew()
	{
		int i = 0;
		while (i < 3)
		{
			if (HuggableController.currentBaddies.Count < 20 && isHanging)
			{
				spewer.FireProjectile(myTransform, 1);
				SoundManager.Instance.playSound(spawnSound);
			}
			yield return new WaitForSeconds(1f);
			int num = i + 1;
			i = num;
		}
		myAnimation["UdderHangingIdle"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("UdderHangingIdle");
	}

	private IEnumerator fall()
	{
		while (myTransform.position.y > 7.75f)
		{
			Vector3 position = myTransform.position;
			position.y -= 0.3f;
			myTransform.position = position;
			yield return new WaitForSeconds(0.04f);
		}
		GameObject obj = Object.Instantiate(detachedUdder);
		obj.transform.position = new Vector3(myTransform.position.x, 0f, myTransform.position.z);
		obj.transform.parent = rootTransform;
		Object.Destroy(base.gameObject);
	}
}
