using UnityEngine;

public class MeshEmitter : MonoBehaviour
{
	public bool start;

	public bool loop;

	public float timeBetweenExplosions = 0.5f;

	public GameObject armToEmit;

	public Vector3 minInitArmVelocity = new Vector3(20f, 100f, 20f);

	public Vector3 maxInitArmVelocity = new Vector3(20f, 100f, 20f);

	public Vector3 constantAxisForceArm = new Vector3(0f, -9.8f, 0f);

	public Vector3 armRotationVelocity = new Vector3(830f, 660f, 660f);

	public int minArmsToEmit;

	public int maxArmsToEmit = 2;

	public GameObject legToEmit;

	public Vector3 minInitLegVelocity = new Vector3(20f, 100f, 20f);

	public Vector3 maxInitLegVelocity = new Vector3(20f, 100f, 20f);

	public Vector3 constantAxisForceLeg = new Vector3(0f, -9.8f, 0f);

	public Vector3 legRotationVelocity = new Vector3(830f, 660f, 660f);

	public int minLegsToEmit;

	public int maxLegsToEmit = 2;

	public GameObject headToEmit;

	public Vector3 minInitHeadVelocity = new Vector3(20f, 100f, 20f);

	public Vector3 maxInitHeadVelocity = new Vector3(20f, 100f, 20f);

	public Vector3 constantAxisForceHead = new Vector3(0f, -9.8f, 0f);

	public Vector3 headRotationVelocity = new Vector3(830f, 660f, 660f);

	public int minHeadsToEmit;

	public int maxHeadsToEmit = 1;

	private NewBit[] bits;

	private float lastExplosion;

	private GameObject[] newMeshInstances;

	private Vector3 emitPos;

	private Vector3 initialPosition;

	private void Start()
	{
		start = false;
		if (GameManager.Instance.isHighEnd)
		{
			minHeadsToEmit = 1;
		}
	}

	private void Update()
	{
		if (!start)
		{
			return;
		}
		if (loop)
		{
			if (Time.time > lastExplosion + timeBetweenExplosions)
			{
				lastExplosion = Time.time + timeBetweenExplosions;
				Explode();
			}
		}
		else
		{
			Explode();
			start = false;
		}
	}

	public void emit(Vector3 pos, bool exploding)
	{
		emitPos = pos;
		if (!exploding)
		{
			Explode();
		}
		else
		{
			head();
		}
	}

	public void Explode()
	{
		head();
		if (GameManager.Instance.isLevel)
		{
			arms();
			legs();
		}
	}

	public void arms()
	{
		int num = Random.Range(minArmsToEmit, maxArmsToEmit + 1);
		for (int i = 0; i < num; i++)
		{
			emitObj(EnemySpawner.Instance.freeArm, emitPos, minInitArmVelocity, maxInitArmVelocity, constantAxisForceArm, armRotationVelocity);
		}
	}

	public void legs()
	{
		int num = Random.Range(minLegsToEmit, maxLegsToEmit + 1);
		for (int i = 0; i < num; i++)
		{
			emitObj(EnemySpawner.Instance.freeLeg, emitPos, minInitLegVelocity, maxInitLegVelocity, constantAxisForceLeg, legRotationVelocity);
		}
	}

	public void head()
	{
		int num = Random.Range(minHeadsToEmit, maxHeadsToEmit + 1);
		for (int i = 0; i < num; i++)
		{
			emitObj(EnemySpawner.Instance.freeHead, emitPos, minInitHeadVelocity, maxInitHeadVelocity, constantAxisForceHead, headRotationVelocity);
		}
	}

	public void emitObj(GameObject meshObj, Vector3 pos, Vector3 minVel, Vector3 maxVel, Vector3 constAxisForce, Vector3 rotVel)
	{
		Quaternion rotation = default(Quaternion);
		rotation.eulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
		meshObj.transform.position = pos;
		meshObj.transform.rotation = rotation;
		NewBit newBit = meshObj.AddComponent<NewBit>();
		newBit.velocity = randomVelocity(minVel, maxVel);
		newBit.constantAxisForce = constAxisForce;
		newBit.rotationVelocity = rotVel;
		TimedDestroy timedDestroy = meshObj.AddComponent<TimedDestroy>();
		timedDestroy.timeToLive = 2f;
	}

	private Vector3 randomVelocity(Vector3 min, Vector3 max)
	{
		int num = Random.Range(0, 2);
		if (num == 1)
		{
			return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
		}
		return new Vector3(-1f * Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
	}
}
