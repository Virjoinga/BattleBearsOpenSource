using System.Collections;
using UnityEngine;

public class ProjectileControllerRaycastRiggs : ProjectileController
{
	public float damage;

	public LayerMask hitMask;

	private GameObject bulletTracer;

	public GameObject bulletTracerTPS;

	public GameObject bulletTracerFPS;

	private Transform tracerStart;

	public Transform tracerStartTPS;

	public Transform tracerStartFPS;

	private Transform tracerEnd;

	public Transform tracerEndTPS;

	public Transform tracerEndFPS;

	public GameObject bulletSplash;

	private Vector3 hitPos;

	private RaycastHit hit;

	protected override void Awake()
	{
		base.Awake();
	}

	private void Update()
	{
		if (!GameManager.Instance.FPSMode)
		{
			bulletTracer = bulletTracerTPS;
			tracerStart = tracerStartTPS;
			tracerEnd = tracerEndTPS;
			bulletTracerFPS.SetActiveRecursively(false);
		}
		else if (GameManager.Instance.FPSMode)
		{
			bulletTracer = bulletTracerFPS;
			tracerStart = tracerStartFPS;
			tracerEnd = tracerEndFPS;
			bulletTracerTPS.SetActiveRecursively(false);
		}
	}

	public override void FireProjectile(Transform t, int numberOfBullets)
	{
		Vector3 eulerAngles = myTransform.eulerAngles;
		Vector3 eulerAngles2 = eulerAngles;
		eulerAngles.x += Random.Range(-2f, 2f);
		eulerAngles.y += Random.Range(-2f, 2f);
		if (GameManager.Instance.FPSMode)
		{
			eulerAngles.x += Random.Range(-0.5f, 0.5f);
			eulerAngles.y += Random.Range(-0.5f, 0.5f);
		}
		myTransform.eulerAngles = eulerAngles;
		RaycastHit hitInfo;
		if (Physics.Raycast(myTransform.position, myTransform.forward, out hitInfo, float.PositiveInfinity, hitMask))
		{
			GameObject gameObject = hitInfo.transform.gameObject;
			if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
			{
				gameObject.SendMessage("OnHit", damage, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				Transform obj = Object.Instantiate(bulletSplash).transform;
				obj.position = hitInfo.point;
				obj.LookAt(obj.position + hitInfo.normal);
			}
			hitPos = hitInfo.point;
			StartCoroutine(hideTracer());
		}
		myTransform.eulerAngles = eulerAngles2;
	}

	private IEnumerator hideTracer()
	{
		bulletTracer.SetActiveRecursively(true);
		yield return new WaitForSeconds(0.05f);
		bulletTracer.SetActiveRecursively(false);
	}

	private void LateUpdate()
	{
		tracerEnd.position = hitPos;
	}
}
