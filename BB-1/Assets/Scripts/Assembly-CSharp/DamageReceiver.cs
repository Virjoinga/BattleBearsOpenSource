using System.Collections;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
	public float hitpoints = 1f;

	private Vector3 originalLocalPos;

	private Transform myTransform;

	public Renderer myRenderer;

	private bool isShaking;

	public bool shakeWhenHit;

	public Material materialWhenHit;

	private Material originalMaterial;

	private bool isWhite;

	public bool destroyWhenNoHP = true;

	public bool isInvincible;

	public bool pushedByBullets;

	public bool destroyRoot;

	public ArrayList listeners = new ArrayList();

	public bool isPlayer;

	public AudioClip hitSound;

	public float hitSoundChance = 1f;

	public GameObject bloodEffect;

	public float bloodHeight = 2f;

	public float damageMultiplier = 1f;

	private bool hasRecentlyMadeSound;

	public bool useHitMaterialOnDeath;

	private void Awake()
	{
		myTransform = base.transform;
		if (myRenderer == null && GetComponent<Renderer>() != null)
		{
			myRenderer = GetComponent<Renderer>();
		}
		if (myRenderer != null)
		{
			originalMaterial = myRenderer.sharedMaterial;
		}
	}

	private void Start()
	{
		originalLocalPos = myTransform.localPosition;
	}

	public void OnImpact(Vector3 dir)
	{
		if (pushedByBullets)
		{
			dir.y = 0f;
			myTransform.position += dir;
		}
	}

	public void OnHit(float damage)
	{
		if (isInvincible || (isWhite && isPlayer))
		{
			return;
		}
		hitpoints -= damage * damageMultiplier;
		if (damage > 0f)
		{
			for (int i = 0; i < listeners.Count; i++)
			{
				(listeners[i] as GameObject).SendMessage("OnNewHealth", hitpoints, SendMessageOptions.DontRequireReceiver);
				(listeners[i] as GameObject).SendMessage("OnDamageTaken", damage, SendMessageOptions.DontRequireReceiver);
			}
		}
		if (shakeWhenHit && !isShaking)
		{
			StartCoroutine(beginShaking(damage));
		}
		if (!isWhite && myRenderer != null && damage > 0f)
		{
			StartCoroutine(turnWhite());
		}
		if (hitpoints <= 0f)
		{
			if (myRenderer != null && !useHitMaterialOnDeath)
			{
				myRenderer.sharedMaterial = originalMaterial;
			}
			SendMessageUpwards("OnObjectDestroyed", base.gameObject, SendMessageOptions.DontRequireReceiver);
			isWhite = false;
			if (destroyWhenNoHP)
			{
				if (destroyRoot)
				{
					Object.Destroy(myTransform.root.gameObject);
				}
				else
				{
					Object.Destroy(base.gameObject);
				}
			}
		}
		else
		{
			if (bloodEffect != null && GameManager.Instance.currentGraphicsOption == GraphicsOption.ON)
			{
				GameObject obj = Object.Instantiate(bloodEffect);
				Vector3 position = myTransform.position;
				position.y = bloodHeight;
				obj.transform.position = position;
			}
			if (hitSound != null && !hasRecentlyMadeSound && Random.value < hitSoundChance)
			{
				SoundManager.Instance.playSound(hitSound);
				StartCoroutine(temporarySoundDisable());
			}
		}
	}

	private IEnumerator temporarySoundDisable()
	{
		hasRecentlyMadeSound = true;
		yield return new WaitForSeconds(1f);
		hasRecentlyMadeSound = false;
	}

	private IEnumerator beginShaking(float damage)
	{
		isShaking = true;
		int i = 0;
		while (i < 5)
		{
			Vector3 vector = Random.insideUnitSphere * 0.5f;
			Vector3 localPosition = originalLocalPos;
			localPosition += vector;
			myTransform.localPosition = localPosition;
			yield return new WaitForSeconds(0.05f);
			int num = i + 1;
			i = num;
		}
		myTransform.localPosition = originalLocalPos;
		isShaking = false;
	}

	private IEnumerator turnWhite()
	{
		isWhite = true;
		myRenderer.sharedMaterial = materialWhenHit;
		yield return new WaitForSeconds(0.2f);
		myRenderer.sharedMaterial = originalMaterial;
		isWhite = false;
	}
}
