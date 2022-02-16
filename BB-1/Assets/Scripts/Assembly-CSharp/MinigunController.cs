using System.Collections;
using UnityEngine;

public class MinigunController : MonoBehaviour
{
	public LayerMask layersToHit;

	private Transform myTransform;

	public float firingDelay = 0.1f;

	public float damagePerSec = 50f;

	public GameObject bulletSplash;

	public GameObject bulletTracer;

	public Transform bulletTracerStart;

	public Transform bulletTracerEnd;

	private void Awake()
	{
		myTransform = base.transform;
	}

	public void startFiring()
	{
		StartCoroutine("fire");
	}

	public void stopFiring()
	{
		StopCoroutine("fire");
	}

	private IEnumerator showTracer(Vector3 endPosition)
	{
		bulletTracer.SetActiveRecursively(true);
		bulletTracerEnd.position = endPosition;
		bulletTracerStart.LookAt(bulletTracerEnd);
		bulletTracerEnd.LookAt(bulletTracerStart);
		yield return new WaitForSeconds(0.05f);
		bulletTracer.SetActiveRecursively(false);
	}

	private IEnumerator fire()
	{
		yield return new WaitForSeconds(0.1f);
		while (true)
		{
			RaycastHit hitInfo;
			if (Physics.Raycast(myTransform.position, myTransform.forward, out hitInfo, float.PositiveInfinity, layersToHit))
			{
				Debug.DrawRay(myTransform.position, myTransform.forward * hitInfo.distance, Color.yellow);
				GameObject gameObject = hitInfo.transform.gameObject;
				if (gameObject.layer == LayerMask.NameToLayer("PlayerDefence"))
				{
					gameObject.SendMessage("OnBlock");
				}
				else if (gameObject.layer == LayerMask.NameToLayer("Player"))
				{
					gameObject.BroadcastMessage("OnHit", damagePerSec * firingDelay, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					Transform obj = Object.Instantiate(bulletSplash).transform;
					obj.position = hitInfo.point;
					obj.LookAt(hitInfo.point + hitInfo.normal);
				}
				StartCoroutine(showTracer(hitInfo.point));
			}
			yield return new WaitForSeconds(firingDelay);
		}
	}
}
