using UnityEngine;

public class SatelliteController : MonoBehaviour
{
	private Transform myTransform;

	private Transform satellite;

	private Transform rootTransform;

	public RangedWeaponController machinegunShooter;

	public RangedWeaponController spreadshotShooter;

	public RangedWeaponController bearzookaShooter;

	public LaserController laserShooter;

	public RangedWeaponController shotgunShooter;

	private Transform machinegunTransform;

	private Transform spreadshotTransform;

	private Transform bearzookaTransform;

	private Transform laserTransform;

	private Transform shotgunTransform;

	private GameObject machinegunObject;

	private GameObject spreadshotObject;

	private GameObject bearzookaObject;

	private GameObject laserObject;

	private GameObject shotgunObject;

	public GameObject satelliteExplosion;

	private WeaponType lastWeaponType;

	public AudioClip orbitSound;

	public AudioClip explosionSound;

	private AudioSource myAudio;

	private void Awake()
	{
		myTransform = base.transform;
		satellite = myTransform.Find("satellite");
		rootTransform = GameObject.FindWithTag("Player").transform.parent;
		machinegunTransform = machinegunShooter.transform;
		spreadshotTransform = spreadshotShooter.transform;
		bearzookaTransform = bearzookaShooter.transform;
		laserTransform = laserShooter.transform;
		shotgunTransform = shotgunShooter.transform;
		machinegunObject = machinegunShooter.gameObject;
		spreadshotObject = spreadshotShooter.gameObject;
		bearzookaObject = bearzookaShooter.gameObject;
		laserObject = laserShooter.gameObject;
		shotgunObject = shotgunShooter.gameObject;
		myAudio = GetComponent<AudioSource>();
		myAudio.clip = orbitSound;
		myAudio.loop = true;
		myAudio.volume = SoundManager.Instance.getEffectsVolume() * 0.75f;
		myAudio.Play();
	}

	private void Start()
	{
		disableWeapons();
		machinegunObject.active = true;
		lastWeaponType = WeaponType.MACHINEGUN;
	}

	private void OnObjectDestroyed()
	{
		if (explosionSound != null)
		{
			SoundManager.Instance.playSound(explosionSound);
		}
		Object.Instantiate(satelliteExplosion).transform.position = satellite.position;
	}

	private void LateUpdate()
	{
		myTransform.position = rootTransform.position + new Vector3(0f, 2f, 0f);
		myTransform.Rotate(new Vector3(0f, 180f * Time.deltaTime, 0f));
		satellite.eulerAngles = rootTransform.eulerAngles;
	}

	private void disableWeapons()
	{
		machinegunObject.SetActiveRecursively(false);
		bearzookaObject.SetActiveRecursively(false);
		spreadshotObject.SetActiveRecursively(false);
		laserObject.SetActiveRecursively(false);
		shotgunObject.SetActiveRecursively(false);
	}

	public void shootMachineGun(Vector3 rot)
	{
		if (lastWeaponType != 0)
		{
			lastWeaponType = WeaponType.MACHINEGUN;
			disableWeapons();
			machinegunObject.active = true;
		}
		machinegunTransform.localEulerAngles = rot;
		machinegunShooter.Shoot();
	}

	public void shootBearzooka(Vector3 rot)
	{
		if (lastWeaponType != WeaponType.BEARZOOKA)
		{
			lastWeaponType = WeaponType.BEARZOOKA;
			disableWeapons();
			bearzookaObject.active = true;
		}
		bearzookaTransform.localEulerAngles = rot;
		bearzookaShooter.Shoot();
	}

	public void shootSpreadshot(Vector3 rot)
	{
		if (lastWeaponType != WeaponType.SPREADSHOT)
		{
			lastWeaponType = WeaponType.SPREADSHOT;
			disableWeapons();
			spreadshotObject.active = true;
		}
		spreadshotTransform.localEulerAngles = rot;
		spreadshotShooter.Shoot();
	}

	public void shootLaser(Vector3 rot, float timeDiff)
	{
		if (lastWeaponType != WeaponType.LASER)
		{
			lastWeaponType = WeaponType.LASER;
			disableWeapons();
		}
		if (!laserObject.active)
		{
			laserObject.SetActiveRecursively(true);
		}
		laserTransform.localEulerAngles = rot;
		laserShooter.Fire(timeDiff);
	}

	public void turnOffLaser()
	{
		laserObject.SetActiveRecursively(false);
	}

	public void shootShotgun(Vector3 rot)
	{
		if (lastWeaponType != WeaponType.SHOTGUN)
		{
			lastWeaponType = WeaponType.SHOTGUN;
			disableWeapons();
			shotgunObject.active = true;
		}
		shotgunTransform.localEulerAngles = rot;
		shotgunShooter.Shoot();
	}
}
