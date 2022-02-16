using System.Collections;
using UnityEngine;

public class ProjectileControllerRaycast : ProjectileController
{
	public float damage;

	public LayerMask hitMask;

	public GameObject bulletTracer;

	public Transform tracerStart;

	public Transform tracerEnd;

	public GameObject bulletSplash;

	private Vector3 hitPos;

	protected override void Awake()
	{
		base.Awake();
	}

	public override void FireProjectile(Transform t, int numberOfBullets)
	{
		Vector3 eulerAngles = myTransform.eulerAngles;
		Vector3 eulerAngles2 = eulerAngles;
		eulerAngles.x += Random.Range(-1f, 1f);
		eulerAngles.y += Random.Range(-1f, 1f);
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
