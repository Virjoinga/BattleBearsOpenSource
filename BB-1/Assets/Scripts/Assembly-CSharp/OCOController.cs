using System.Collections;
using UnityEngine;

public class OCOController : GenericCharacterController
{
	public OCOAnimator animator;

	public Transform normalAttackTransform;

	private SimpleRangedWeaponController normalAttack;

	private bool usingSpecial;

	public GameObject screenclearSphere;

	public GameObject laser;

	public ParticleSystem laserEffectPS;

	private float slowDownTime = 0.2f;

	private float slowDuration = 15f;

	private Vector3 originalLaserWorldVelocity;

	private Vector3 originalLaserLocalVelocity;

	private float originalLaserEmission;

	private float stunDuration;

	public AudioSource moveSoundSource;

	public AudioSource normalAttackLoop;

	public AudioSource secondaryAttackLoop;

	public AudioSource slowTimeLoop;

	public AudioSource stunLoop;

	public AudioClip laserBlast;

	public AudioClip screenclear;

	public AudioClip slowTimeStart;

	public AudioClip slowTimeEnd;

	public AudioClip stunSound;

	private bool resumeShootingAfterPause;

	protected override void Awake()
	{
		base.Awake();
		weaponTransform = normalAttackTransform;
		normalAttack = normalAttackTransform.GetComponent(typeof(SimpleRangedWeaponController)) as SimpleRangedWeaponController;
		laser.active = false;
		laserEffectPS.enableEmission = false;
		moveSoundSource.volume = SoundManager.Instance.getEffectsVolume();
		normalAttackLoop.volume = SoundManager.Instance.getEffectsVolume();
		secondaryAttackLoop.volume = SoundManager.Instance.getEffectsVolume();
		slowTimeLoop.volume = SoundManager.Instance.getEffectsVolume();
		stunLoop.volume = SoundManager.Instance.getEffectsVolume();
	}

	public bool isStunned()
	{
		return stunDuration > 0f;
	}

	public bool isUsingSpecial()
	{
		return usingSpecial;
	}

	public void OnStun(float duration)
	{
		stopShooting();
		resumeShootingAfterPause = true;
		if (stunDuration == 0f)
		{
			SoundManager.Instance.playSound(stunSound);
			moveSoundSource.Stop();
			StartCoroutine(delayedStunLoop(stunSound.length / 2f));
			StartCoroutine(stunnedForTime(duration));
		}
		else
		{
			stunDuration += duration;
		}
	}

	private IEnumerator delayedStunLoop(float duration)
	{
		yield return new WaitForSeconds(duration * Time.timeScale);
		stunLoop.Play();
	}

	private IEnumerator stunnedForTime(float duration)
	{
		stunDuration += duration;
		currentMoveSpeed = 0f;
		animator.OnStun();
		while (stunDuration > 0f)
		{
			stunDuration -= 0.1f;
			yield return new WaitForSeconds(0.1f * Time.timeScale);
		}
		stunLoop.Stop();
		stunDuration = 0f;
		animator.resetToIdle();
		currentMoveSpeed = moveSpeed;
		moveSoundSource.Play();
		if (resumeShootingAfterPause)
		{
			yield return new WaitForSeconds(0.2f * Time.timeScale);
			startShooting();
		}
	}

	public void OnSlowTime()
	{
		if (Time.timeScale == 1f && !(stunDuration > 0f))
		{
			SoundManager.Instance.playSound(slowTimeStart);
			Time.timeScale = slowDownTime;
			StartCoroutine(handleSlowTimeSounds(slowTimeStart.length));
			StartCoroutine(delayedSlowTimeEnd(slowDuration));
		}
	}

	private IEnumerator handleSlowTimeSounds(float startSoundLength)
	{
		yield return new WaitForSeconds(startSoundLength * Time.timeScale);
		slowTimeLoop.Play();
		moveSoundSource.Stop();
	}

	private IEnumerator delayedSlowTimeEnd(float delay)
	{
		yield return new WaitForSeconds(delay * Time.timeScale);
		slowTimeLoop.Stop();
		moveSoundSource.Play();
		SoundManager.Instance.playSound(slowTimeEnd);
		Time.timeScale = 1f;
		if (!resumeShootingAfterPause && !shoot)
		{
			animator.resetToIdle();
		}
	}

	public void OnLaser()
	{
		if (!usingSpecial && !(stunDuration > 0f))
		{
			stopShooting();
			resumeShootingAfterPause = true;
			usingSpecial = true;
			if (laserBlast != null)
			{
				SoundManager.Instance.playSound(laserBlast);
			}
			StartCoroutine(delayedLaserTurnOn(0.5f));
			animator.useLaser();
			StartCoroutine(delayedLaserTurnOff(animator.GetComponent<Animation>()["attackLaser"].length - 0.2f));
		}
	}

	private IEnumerator delayedLaserTurnOn(float delay)
	{
		yield return new WaitForSeconds(delay * Time.timeScale);
		laser.active = true;
		laserEffectPS.enableEmission = true;
	}

	private IEnumerator delayedLaserTurnOff(float duration)
	{
		yield return new WaitForSeconds(duration * Time.timeScale);
		usingSpecial = false;
		laser.active = false;
		laserEffectPS.enableEmission = false;
		if (resumeShootingAfterPause)
		{
			yield return new WaitForSeconds(0.4f * Time.timeScale);
			startShooting();
		}
	}

	public void OnScreenClearAttack()
	{
		if (!usingSpecial && !(stunDuration > 0f))
		{
			stopShooting();
			resumeShootingAfterPause = true;
			usingSpecial = true;
			if (screenclear != null)
			{
				SoundManager.Instance.playSound(screenclear);
			}
			Object.Instantiate(screenclearSphere).transform.position = myTransform.position;
			animator.useScreenClear();
			StartCoroutine(temporarySpecialDisable(animator.GetComponent<Animation>()["FullBodyExplosion"].length));
		}
	}

	private IEnumerator temporarySpecialDisable(float duration)
	{
		yield return new WaitForSeconds(duration * Time.timeScale);
		usingSpecial = false;
		if (resumeShootingAfterPause)
		{
			yield return new WaitForSeconds(0.2f * Time.timeScale);
			startShooting();
		}
	}

	protected override void Start()
	{
		base.Start();
		HUDController.Instance.forceWeaponToggleHide();
		animator.setWeapon(normalAttack.gameObject);
		laser.active = false;
		HUDController.Instance.updateLives(GameManager.Instance.ocoLives);
		HUDController.Instance.updateBottomAmmo(0f);
		HUDController.Instance.displayMaxHealth();
	}

	protected void Update()
	{
		if (!usingSpecial && !(stunDuration > 0f) && !inputDisabled && Time.timeScale > 0f)
		{
			myTransform.Rotate(turn * Time.deltaTime / Time.timeScale);
			bool shooting = shoot;
			if (shoot)
			{
				normalAttack.Shoot();
			}
			if (!usingDirectAiming)
			{
				weaponTransform.localEulerAngles = weaponDir;
			}
			float num = (lastMovementSpeed = movement.magnitude);
			if (!moveSoundSource.isPlaying && num >= 0.1f)
			{
				moveSoundSource.Play();
			}
			if (num == 0f)
			{
				moveSoundSource.Stop();
			}
			characterController.SimpleMove(movement / Time.timeScale);
			animator.animateBear(movement, shooting);
		}
	}

	public override void startShooting()
	{
		if (!usingSpecial && !(stunDuration > 0f))
		{
			normalAttackLoop.Play();
			StartCoroutine(startSecondaryFire(normalAttackLoop.clip.length / 2f));
			shoot = true;
			HUDController.Instance.OnSetOCOReticle();
		}
	}

	public override void stopShooting()
	{
		normalAttackLoop.Stop();
		StartCoroutine(stopSecondaryFire(normalAttackLoop.clip.length / 2f));
		shoot = false;
		HUDController.Instance.OnSetOCOReticle();
		resumeShootingAfterPause = false;
	}

	private IEnumerator startSecondaryFire(float delay)
	{
		yield return new WaitForSeconds(delay * Time.timeScale);
		secondaryAttackLoop.Play();
	}

	private IEnumerator stopSecondaryFire(float delay)
	{
		yield return new WaitForSeconds(delay * Time.timeScale);
		secondaryAttackLoop.Stop();
	}
}
