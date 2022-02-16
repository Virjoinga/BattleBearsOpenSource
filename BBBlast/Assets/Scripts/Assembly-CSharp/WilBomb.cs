using System;
using System.Collections;
using UnityEngine;

public class WilBomb : MonoBehaviour
{
	[HideInInspector]
	public float life;

	[HideInInspector]
	public bool isExplode;

	[HideInInspector]
	public bool hit;

	public bool replacment;

	private string spawnName;

	public ParticleEmitter smoke;

	public ParticleEmitter spark;

	private float dmg;

	private Mesh mesh;

	public Mesh orgMesh;

	private Vector3[] orgMeshVerts;

	private Vector3[] vertices;

	private Vector3[] normals;

	public bool isTut;

	private void Awake()
	{
		GetComponentInChildren<MeshFilter>().mesh = UnityEngine.Object.Instantiate(orgMesh) as Mesh;
		mesh = GetComponentInChildren<MeshFilter>().mesh;
		orgMeshVerts = orgMesh.vertices;
	}

	private void Start()
	{
		if (!replacment)
		{
			life = 12f;
			isExplode = false;
			hit = false;
		}
		smoke.emit = false;
		spark.emit = false;
		vertices = new Vector3[GameManager.Instance.bombVert.Length];
		normals = new Vector3[GameManager.Instance.bombNorm.Length];
		for (short num = 0; num < vertices.Length; num = (short)(num + 1))
		{
			vertices[num] = GameManager.Instance.bombVert[num];
		}
		for (short num2 = 0; num2 < normals.Length; num2 = (short)(num2 + 1))
		{
			normals[num2] = GameManager.Instance.bombNorm[num2];
		}
		Objectives.Instance.bombCount();
	}

	public void OnHit(float damage)
	{
		if (!hit && !isExplode)
		{
			dmg = damage;
			hit = true;
		}
	}

	private void Update()
	{
		if (hit)
		{
			for (short num = 0; num < vertices.Length; num = (short)(num + 1))
			{
				vertices[num] += normals[num] * dmg * 0.002f;
			}
			mesh.vertices = vertices;
			hit = false;
			smoke.emit = true;
			spark.emit = true;
			if (Math.Abs(vertices[0].x) > 0.035f && !isExplode)
			{
				StartCoroutine("explodeDeath");
			}
		}
		else if (Math.Abs(vertices[0].x) > Math.Abs(orgMeshVerts[0].x))
		{
			for (short num2 = 0; num2 < vertices.Length; num2 = (short)(num2 + 1))
			{
				vertices[num2] -= normals[num2] * 0.001f;
			}
			mesh.vertices = vertices;
			smoke.emit = false;
			spark.emit = false;
		}
	}

	public void explodeDeath()
	{
		isExplode = true;
		GetComponent<MeshRenderer>().enabled = false;
		smoke.emit = false;
		spark.emit = false;
		if (!SoundController.Instance.allOff)
		{
			base.GetComponent<AudioSource>().PlayOneShot(Resources.Load("bomb" + UnityEngine.Random.Range(1, 3)) as AudioClip);
		}
		if (GameManager.Instance.isHighEnd)
		{
			UnityEngine.Object.Instantiate(Resources.Load("BombExplosionHigh", typeof(GameObject)), base.transform.position, base.transform.rotation);
		}
		else
		{
			UnityEngine.Object.Instantiate(Resources.Load("BombExplosionLow"), base.transform.position, base.transform.rotation);
		}
		if (isTut)
		{
			StartCoroutine("countDown");
			GameObject.Find("Tutorial").SendMessage("bombDown", SendMessageOptions.DontRequireReceiver);
			return;
		}
		EnemySpawner.Instance.clearSpawn(spawnName);
		Objectives.Instance.Detonations();
		GameManager.Instance.statBombDetonations += 1f;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
		for (byte b = 0; b < array.Length; b = (byte)(b + 1))
		{
			array[b].SendMessage("explode", SendMessageOptions.DontRequireReceiver);
		}
		StartCoroutine("countDown");
	}

	public void getInfo(float health, string posName)
	{
		life = health;
		spawnName = posName;
	}

	public float giveInfo()
	{
		return life;
	}

	public IEnumerator countDown()
	{
		yield return new WaitForSeconds(0.5f);
		SendMessageUpwards("myBombExploded", SendMessageOptions.DontRequireReceiver);
	}
}
