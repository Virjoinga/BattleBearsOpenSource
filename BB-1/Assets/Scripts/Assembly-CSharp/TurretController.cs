using System.Collections;
using UnityEngine;

public class TurretController : MonoBehaviour
{
	public int scoreValue = 600;

	public bool shootMissiles;

	public bool fireBothMissiles;

	private Transform myTransform;

	public Transform turretTransform;

	private Transform currentTarget;

	public ProjectileControllerPrefab[] missileControllers;

	private int currentMissileControllerIndex;

	public ParticleSystem gunEmitterPS;

	public GameObject destroyedTurret;

	public GameObject turretExplosion;

	private EnemySpawner enemySpawner;

	private bool isDestroyed;

	private Transform gunEmitterTransform;

	private Transform[] missileControllerTransforms;

	public AudioClip fireSound;

	public AudioClip explosionSound;

	private void Awake()
	{
		myTransform = base.transform;
		gunEmitterTransform = gunEmitterPS.transform;
		missileControllerTransforms = new Transform[missileControllers.Length];
		for (int i = 0; i < missileControllerTransforms.Length; i++)
		{
			missileControllerTransforms[i] = missileControllers[i].transform;
		}
		DamageReceiver damageReceiver = GetComponent(typeof(DamageReceiver)) as DamageReceiver;
		if (damageReceiver != null)
		{
			damageReceiver.hitpoints *= GameManager.Instance.bossHPMultipliers[GameManager.Instance.getDifficulty()];
		}
	}

	public void OnInitialize()
	{
		enemySpawner = myTransform.root.GetComponent(typeof(EnemySpawner)) as EnemySpawner;
		if (enemySpawner != null)
		{
			enemySpawner.addTurret(base.gameObject);
		}
		currentTarget = GameObject.FindWithTag("Player").transform;
		StartCoroutine(looker());
		if (shootMissiles)
		{
			StartCoroutine(fireMissiles());
		}
		else
		{
			StartCoroutine(fireGun());
		}
		if (GameManager.Instance.noBadguys)
		{
			BroadcastMessage("OnHit", 10000);
		}
	}

	private IEnumerator fireMissiles()
	{
		while (currentTarget != null)
		{
			yield return new WaitForSeconds(Random.value * 10f);
			if (fireSound != null)
			{
				SoundManager.Instance.playSound(fireSound);
			}
			if (fireBothMissiles)
			{
				for (int i = 0; i < missileControllers.Length; i++)
				{
					missileControllers[i].FireProjectile(myTransform, 1);
				}
				continue;
			}
			missileControllers[currentMissileControllerIndex++].FireProjectile(myTransform, 1);
			if (currentMissileControllerIndex >= missileControllers.Length)
			{
				currentMissileControllerIndex = 0;
			}
		}
	}

	private IEnumerator fireGun()
	{
		while (currentTarget != null)
		{
			yield return new WaitForSeconds(Random.value * 5f + 2.5f);
			int i = 0;
			while (i < Random.Range(3, 6))
			{
				gunEmitterPS.Emit(1);
				if (fireSound != null)
				{
					SoundManager.Instance.playSound(fireSound);
				}
				yield return new WaitForSeconds(0.35f);
				int num = i + 1;
				i = num;
			}
		}
	}

	private IEnumerator looker()
	{
		while (currentTarget != null)
		{
			Vector3 worldPosition = new Vector3(currentTarget.position.x, turretTransform.position.y, currentTarget.position.z);
			turretTransform.LookAt(worldPosition);
			Vector3 localEulerAngles = turretTransform.localEulerAngles;
			localEulerAngles.z = 0f;
			turretTransform.localEulerAngles = localEulerAngles;
			if (!shootMissiles)
			{
				gunEmitterTransform.LookAt(currentTarget);
			}
			else
			{
				for (int i = 0; i < missileControllerTransforms.Length; i++)
				{
					missileControllerTransforms[i].LookAt(currentTarget);
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	private void OnObjectDestroyed(GameObject obj)
	{
		if (!isDestroyed)
		{
			StatsManager.Instance.currentScore += scoreValue;
			HUDController.Instance.updateScore();
			isDestroyed = true;
			if (!shootMissiles)
			{
				gunEmitterPS.enableEmission = false;
				gunEmitterPS.transform.parent = null;
			}
			StatsManager.Instance.currentTurretsKilled++;
			StatsManager.Instance.totalTurretsKilled++;
			int totalTurretsKilled = StatsManager.Instance.totalTurretsKilled;
			int num = 1000;
			GameObject obj2 = Object.Instantiate(turretExplosion);
			obj2.transform.position = myTransform.position;
			obj2.transform.rotation = myTransform.rotation;
			obj2.transform.localScale = myTransform.localScale;
			createDestroyedTurret();
			if (enemySpawner != null)
			{
				enemySpawner.removeTurret(base.gameObject);
			}
			SoundManager.Instance.playSound(explosionSound);
		}
	}

	public void createDestroyedTurret()
	{
		GameObject obj = Object.Instantiate(destroyedTurret);
		obj.transform.parent = myTransform.parent;
		obj.transform.localPosition = myTransform.localPosition;
		obj.transform.localRotation = myTransform.localRotation;
		obj.transform.localScale = myTransform.localScale;
	}
}
