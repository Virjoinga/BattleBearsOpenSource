using System.Collections;
using UnityEngine;

public class TLCController : ProjectileController
{
	public float damage;

	public LayerMask hitMask;

	public Transform blast;

	private Vector3 right;

	private Vector3 left;

	protected override void Awake()
	{
		base.Awake();
		right = new Vector3(1f, -1f, 0f);
		left = new Vector3(-1f, -1f, 0f);
		StartCoroutine("Hurr");
	}

	public override void FireProjectile(Vector3 dir)
	{
		base.transform.LookAt(dir);
		blast.LookAt(dir);
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(base.transform.position, base.transform.forward, out hitInfo, 100f, hitMask);
		RaycastHit hitInfo2;
		bool flag2 = Physics.Raycast(base.transform.position, base.transform.forward + right * 0.03f, out hitInfo2, 100f, hitMask);
		RaycastHit hitInfo3;
		bool flag3 = Physics.Raycast(base.transform.position, base.transform.forward + Vector3.up * 0.03f, out hitInfo3, 100f, hitMask);
		RaycastHit hitInfo4;
		bool flag4 = Physics.Raycast(base.transform.position, base.transform.forward + left * 0.03f, out hitInfo4, 100f, hitMask);
		if (flag)
		{
			if (GameManager.Instance.currentGameMode != GameMode.MENU)
			{
				hitInfo.collider.SendMessage("OnHit", 2f, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				hitInfo.collider.SendMessage("selected", SendMessageOptions.DontRequireReceiver);
			}
		}
		if (flag2)
		{
			if (GameManager.Instance.currentGameMode != GameMode.MENU)
			{
				hitInfo2.collider.SendMessage("OnHit", 0.75f, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				hitInfo2.collider.SendMessage("selected", SendMessageOptions.DontRequireReceiver);
			}
		}
		if (flag3)
		{
			if (GameManager.Instance.currentGameMode != GameMode.MENU)
			{
				hitInfo3.collider.SendMessage("OnHit", 0.75f, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				hitInfo3.collider.SendMessage("selected", SendMessageOptions.DontRequireReceiver);
			}
		}
		if (flag4)
		{
			if (GameManager.Instance.currentGameMode != GameMode.MENU)
			{
				hitInfo4.collider.SendMessage("OnHit", 0.75f, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				hitInfo4.collider.SendMessage("selected", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public IEnumerator Hurr()
	{
		while (true)
		{
			if (GameManager.Instance.currentGameMode != GameMode.MENU && !GameManager.Instance.isOver)
			{
				base.transform.LookAt(GameManager.Instance.hitPos);
				blast.LookAt(GameManager.Instance.hitPos);
				RaycastHit hit1;
				bool one = Physics.Raycast(base.transform.position, base.transform.forward, out hit1, 100f, hitMask);
				RaycastHit hit2;
				bool two = Physics.Raycast(base.transform.position, base.transform.forward + right * 0.03f, out hit2, 100f, hitMask);
				RaycastHit hit3;
				bool three = Physics.Raycast(base.transform.position, base.transform.forward + Vector3.up * 0.03f, out hit3, 100f, hitMask);
				RaycastHit hit4;
				bool four = Physics.Raycast(base.transform.position, base.transform.forward + left * 0.03f, out hit4, 100f, hitMask);
				if (Input.touchCount > 0 || Input.GetMouseButton(0))
				{
					if (one)
					{
						hit1.collider.SendMessage("OnHit", 2f, SendMessageOptions.DontRequireReceiver);
					}
					if (two)
					{
						hit2.collider.SendMessage("OnHit", 0.75f, SendMessageOptions.DontRequireReceiver);
					}
					if (three)
					{
						hit3.collider.SendMessage("OnHit", 0.75f, SendMessageOptions.DontRequireReceiver);
					}
					if (four)
					{
						hit4.collider.SendMessage("OnHit", 0.75f, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
			yield return new WaitForSeconds(0.0001f);
		}
	}
}
