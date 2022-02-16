using System.Collections;
using UnityEngine;

public class HugabullController : HuggableController
{
	public float chargeInterval = 5f;

	private float lastChargeTime;

	public GameObject hornAttack;

	public GameObject impactPrefab;

	public AudioClip chargeSound;

	public AudioClip impactSound;

	public AudioClip runSound;

	public Vector3 ChargePos;

	private bool _inCharge;

	protected override void Start()
	{
		base.Start();
		hornAttack.gameObject.active = false;
	}

	protected override void Update()
	{
		if (!_inCharge)
		{
			base.Update();
		}
	}

	protected override IEnumerator mainBehaviour()
	{
		yield return new WaitForSeconds(0.1f);
		while (currentTarget != null && !isDying)
		{
			if (Time.time > lastChargeTime + chargeInterval)
			{
				Vector3 center = myCollider.bounds.center;
				center.y = currentTarget.position.y;
				float num = Vector3.Distance(center, currentTarget.position);
				Vector3 vector = currentTarget.position - center;
				vector.Normalize();
				RaycastHit hitInfo;
				if (Physics.Raycast(center, vector, out hitInfo, num, shootMask))
				{
					if (hitInfo.transform == currentTarget)
					{
						Vector3 zero = Vector3.zero;
						if (Physics.Raycast(center, vector, out hitInfo, 1000f, chaseMask))
						{
							Vector3 targetPos = hitInfo.point - vector * 3f;
							yield return StartCoroutine(charge(vector, targetPos));
						}
						else if (num > attackDistance)
						{
							yield return StartCoroutine(chase(GameManager.Instance.chaseTime));
						}
						else
						{
							yield return new WaitForSeconds(0.1f);
						}
					}
					else if (num > attackDistance)
					{
						yield return StartCoroutine(chase(GameManager.Instance.chaseTime));
					}
					else
					{
						yield return new WaitForSeconds(0.1f);
					}
				}
				else if (num > attackDistance)
				{
					yield return StartCoroutine(chase(GameManager.Instance.chaseTime));
				}
				else
				{
					yield return new WaitForSeconds(0.1f);
				}
			}
			else if (Vector3.Distance(myCollider.bounds.center, currentTarget.position) > attackDistance)
			{
				yield return StartCoroutine(chase(GameManager.Instance.chaseTime));
			}
			else
			{
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	private IEnumerator charge(Vector3 dir, Vector3 targetPos)
	{
		_inCharge = true;
		myCollider.isTrigger = true;
		ChargePos = targetPos;
		currentMoveSpeed = 0f;
		currentWaypointIndex = -1;
		currentTurnSpeed = 0f;
		myTransform.LookAt(targetPos);
		Vector3 eulerAngles = myTransform.eulerAngles;
		eulerAngles.x = 0f;
		eulerAngles.z = 0f;
		eulerAngles.y += 180f;
		myTransform.eulerAngles = eulerAngles;
		myRigidbody.velocity = Vector3.zero;
		if (chargeSound != null)
		{
			SoundManager.Instance.playSound(chargeSound);
		}
		myAnimation["Huggabull_ChargeUp"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("Huggabull_ChargeUp");
		yield return new WaitForSeconds(myAnimation["Huggabull_ChargeUp"].length);
		if (runSound != null)
		{
			SoundManager.Instance.playSound(runSound);
		}
		hornAttack.gameObject.SetActive(true);
		myAnimation["Huggabull_Run"].wrapMode = WrapMode.Loop;
		myAnimation.CrossFade("Huggabull_Run");
		myRigidbody.velocity = dir * 30f;
		targetPos.y = 0f;
		float checkTime = 0f;
		bool haveImpact = true;
		while (Vector3.Distance(new Vector3(myTransform.position.x, 0f, myTransform.position.z), targetPos) > 3f)
		{
			yield return new WaitForSeconds(0.1f);
			checkTime += 0.1f;
			if (checkTime > 2f)
			{
				haveImpact = false;
				break;
			}
		}
		hornAttack.gameObject.SetActive(false);
		myRigidbody.velocity = Vector3.zero;
		if (haveImpact)
		{
			Object.Instantiate(impactPrefab).transform.position = myTransform.position;
			if (impactSound != null)
			{
				SoundManager.Instance.playSound(impactSound);
			}
		}
		myAnimation["Huggabull_Impact"].wrapMode = WrapMode.Once;
		myAnimation.CrossFade("Huggabull_Impact");
		yield return new WaitForSeconds(myAnimation["Huggabull_Impact"].length);
		lastChargeTime = Time.time;
		currentMoveSpeed = moveSpeed;
		currentTurnSpeed = turnSpeed;
		myCollider.isTrigger = false;
		_inCharge = false;
	}
}
