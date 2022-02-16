using System.Collections;
using UnityEngine;

public abstract class GenericCharacterController : MonoBehaviour
{
	public float moveSpeed = 15f;

	protected int control;

	public BearMovementController[] moveSchemes;

	protected Transform myTransform;

	protected CharacterController characterController;

	protected float currentMoveSpeed;

	protected Vector3 movement;

	protected Vector3 turn;

	protected Vector3 weaponDir;

	protected bool shoot;

	protected bool usingDirectAiming;

	public LayerMask aimerMask;

	public Vector3 hitPoint;

	protected Camera shoulderCam;

	protected Transform weaponTransform;

	protected AudioSource myAudio;

	protected float targetHeight;

	protected Vector3 targetHitpoint;

	public bool inputDisabled;

	protected float lastMovementSpeed;

	public float minTimeBetweenTalks = 30f;

	public float maxTimeBetweenTalks = 300f;

	public string sayingsDirectory;

	private AudioClip[] extraLines;

	private float retPos;

	public Controlls controlls;

	public Transform camRotator;

	protected virtual void Awake()
	{
		myTransform = base.transform;
		currentMoveSpeed = moveSpeed;
		myAudio = GetComponent<AudioSource>();
		myAudio.loop = true;
	}

	protected virtual void Start()
	{
		controlls = GameObject.Find("Controlls").GetComponent<Controlls>();
		shoulderCam = GetComponentInChildren(typeof(Camera)) as Camera;
		characterController = GetComponent(typeof(CharacterController)) as CharacterController;
		HUDController.Instance.linkupGenericPlayer();
		updateInputScheme();
		if (GameManager.Instance.useHighres && sayingsDirectory != "" && !GameManager.Instance.vent)
		{
			StartCoroutine(periodicTalker());
		}
	}

	private IEnumerator periodicTalker()
	{
		Object[] array = Resources.LoadAll("Music/High/" + sayingsDirectory, typeof(AudioClip));
		if (array.Length != 0)
		{
			extraLines = new AudioClip[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				extraLines[i] = array[i] as AudioClip;
			}
			while (true)
			{
				yield return new WaitForSeconds(minTimeBetweenTalks + Random.value * (maxTimeBetweenTalks - minTimeBetweenTalks));
				SoundManager.Instance.playSound(extraLines[Random.Range(0, extraLines.Length)]);
			}
		}
	}

	public void updateInputScheme()
	{
		moveSchemes[0].enabled = false;
		moveSchemes[1].enabled = false;
		moveSchemes[(int)GameManager.Instance.currentStyle].enabled = true;
		moveSchemes[(int)GameManager.Instance.currentStyle].startUp();
	}

	private void OnDrawGizmos()
	{
	}

	protected void moveReticle()
	{
		retPos = controlls.aim().y - 5f;
		if (retPos > (float)(Screen.height - 10))
		{
			retPos = Screen.height - 10;
		}
		if (retPos < 0f)
		{
			retPos = 0f;
		}
		HUDController.Instance.OnSetReticlePos(new Vector3(0f, retPos, 0f));
		Vector2 vector = HUDController.Instance.getRetPos();
		Vector3 vector2 = shoulderCam.ViewportToWorldPoint(new Vector3(0.5f, vector.y, 6f));
		Vector3 vector3 = shoulderCam.ViewportToWorldPoint(new Vector3(0.5f, vector.y, 7f));
		Vector3 vector4 = new Vector3((vector3.x - vector2.x) / 2f, (vector3.y - vector2.y) / 2f, (vector3.z - vector2.z) / 2f);
		RaycastHit hitInfo;
		Physics.Raycast(vector2, vector4, out hitInfo, float.PositiveInfinity, aimerMask);
		Debug.DrawRay(vector2, vector4 * 25f);
		if (hitInfo.distance > 1f)
		{
			aimGun(hitInfo.point);
		}
		else
		{
			aimGun(hitInfo.point - new Vector3(0f, 1f, 0f));
		}
	}

	public void rotateBear(Vector3 inputVal, float turnSpeed, bool isTopDown)
	{
		turn = inputVal * turnSpeed;
	}

	public void moveBear(Vector3 move, float speedFactor)
	{
		movement = move;
		movement.y = 0f;
		movement.Normalize();
		movement *= currentMoveSpeed * speedFactor;
	}

	public void angleGun(Vector3 inputVal, float maxTilt, float minTilt, float tiltFactor)
	{
		usingDirectAiming = false;
		weaponDir = inputVal;
		if (weaponDir.x * tiltFactor < minTilt)
		{
			weaponDir.x = minTilt;
		}
		else if (weaponDir.x * tiltFactor > maxTilt)
		{
			Debug.Log(weaponDir.x);
			weaponDir.x = maxTilt;
		}
		else
		{
			weaponDir *= tiltFactor;
		}
	}

	public void aimGun(Vector3 target)
	{
		if (!inputDisabled)
		{
			usingDirectAiming = true;
			Vector3 forward = target - weaponTransform.position;
			weaponTransform.forward = forward;
		}
	}

	public abstract void startShooting();

	public abstract void stopShooting();

	private void LateUpdate()
	{
		moveReticle();
	}
}
