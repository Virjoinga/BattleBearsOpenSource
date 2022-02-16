using System.Collections;
using UnityEngine;

public class LaserController : MonoBehaviour
{
	public float damagePerSec;

	public LayerMask hitMask;

	private Vector3[] hitPositions;

	private Transform myTransform;

	public float jitterInDegrees = 1f;

	private PlayerController playerController;

	public bool playerOwned = true;

	public int maxBounces = 5;

	public int numBounces;

	public GameObject laserBeamPrefab;

	private GameObject[] laserBeams;

	private Transform[] laserBeamStarts;

	private Transform[] laserBeamEnds;

	private void Awake()
	{
		myTransform = base.transform;
		playerController = base.transform.root.gameObject.GetComponent(typeof(PlayerController)) as PlayerController;
		laserBeams = new GameObject[maxBounces + 1];
		laserBeamStarts = new Transform[maxBounces + 1];
		laserBeamEnds = new Transform[maxBounces + 1];
		hitPositions = new Vector3[maxBounces + 1];
		for (int i = 0; i < maxBounces + 1; i++)
		{
			GameObject gameObject = Object.Instantiate(laserBeamPrefab);
			gameObject.transform.parent = myTransform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
			laserBeams[i] = gameObject;
			laserBeamStarts[i] = gameObject.transform.Find("joint1");
			laserBeamEnds[i] = gameObject.transform.Find("joint2");
			gameObject.SetActiveRecursively(false);
		}
	}

	public void toggleBeams(bool toggle)
	{
		int num = (toggle ? numBounces : maxBounces);
		for (int i = 0; i < num + 1; i++)
		{
			if (laserBeams[i].active != toggle)
			{
				laserBeams[i].SetActiveRecursively(toggle);
			}
		}
	}

	public void Fire(float timeDiff)
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		Vector3 zero = Vector3.zero;
		Vector3 vector = Vector3.zero;
		for (int i = 0; i < numBounces + 1; i++)
		{
			if (i == 0)
			{
				vector = myTransform.eulerAngles;
				Vector3 eulerAngles = vector;
				if (numBounces == 0)
				{
					vector.x += Random.Range((0f - jitterInDegrees) / 2f, jitterInDegrees / 2f);
					vector.y += Random.Range((0f - jitterInDegrees) / 2f, jitterInDegrees / 2f);
				}
				myTransform.eulerAngles = vector;
				vector = myTransform.forward;
				myTransform.eulerAngles = eulerAngles;
				zero = myTransform.position;
			}
			else
			{
				laserBeamStarts[i].position = hitPositions[i - 1];
				zero = hitPositions[i - 1];
			}
			RaycastHit[] array = Physics.RaycastAll(zero, vector, float.PositiveInfinity, hitMask);
			float num = float.PositiveInfinity;
			Vector3 vector2 = Vector3.zero;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			for (int j = 0; j < array.Length; j++)
			{
				GameObject gameObject = array[j].transform.gameObject;
				if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
				{
					arrayList.Add(gameObject);
					arrayList2.Add(array[j].distance);
				}
				else if (array[j].distance < num)
				{
					num = array[j].distance;
					vector2 = array[j].point;
					vector = -2f * (vector.x * array[j].normal.x + vector.y * array[j].normal.y + vector.z * array[j].normal.z) * array[j].normal + vector;
				}
			}
			for (int k = 0; k < arrayList2.Count; k++)
			{
				if ((float)arrayList2[k] < num)
				{
					((GameObject)arrayList[k]).SendMessage("OnHit", damagePerSec * timeDiff, SendMessageOptions.DontRequireReceiver);
				}
			}
			hitPositions[i] = vector2;
		}
		if (playerOwned)
		{
			HUDController.Instance.addAmmo(WeaponType.LASER, 0f - timeDiff);
		}
	}

	private void LateUpdate()
	{
		for (int i = 0; i < numBounces + 1; i++)
		{
			if (hitPositions[i] != Vector3.zero)
			{
				laserBeamEnds[i].position = hitPositions[i];
			}
		}
	}
}
