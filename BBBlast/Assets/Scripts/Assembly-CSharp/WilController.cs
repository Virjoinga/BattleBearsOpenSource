using System.Collections;
using UnityEngine;

public class WilController : MonoBehaviour
{
	private bool isCurrentlyExpanded;

	private bool isDead;

	private bool hit;

	public WilBomb wilBomb;

	private Vector3[] vertices;

	private Vector3[] normals;

	private Mesh mesh;

	public Mesh orgMesh;

	private Vector3[] orgMeshVerts;

	public Transform deathPos;

	private bool isPlaying;

	public bool isTut;

	private string currentAnim;

	private float degrees;

	public GameObject myBomb;

	public Material orgMat;

	private Material myMat;

	private bool check;

	private Animation myAnim;

	private float flashWil;

	private void Awake()
	{
		GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = Object.Instantiate(orgMesh) as Mesh;
		mesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
		orgMeshVerts = orgMesh.vertices;
		GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = Object.Instantiate(orgMat) as Material;
		myMat = GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial;
	}

	private void Start()
	{
		isDead = false;
		hit = false;
		vertices = new Vector3[GameManager.Instance.wilVert.Length];
		normals = new Vector3[GameManager.Instance.wilNorm.Length];
		for (short num = 0; num < vertices.Length; num = (short)(num + 1))
		{
			vertices[num] = GameManager.Instance.wilVert[num];
		}
		for (short num2 = 0; num2 < normals.Length; num2 = (short)(num2 + 1))
		{
			normals[num2] = GameManager.Instance.wilNorm[num2];
		}
		GetComponentInChildren<WilBomb>().getInfo(0f, currentAnim);
		isPlaying = false;
		currentAnim = base.gameObject.name.Split('_')[1];
		wilBomb.getInfo(3f, currentAnim);
		GameManager.Instance.isWilOut = true;
		StartCoroutine("bombDrop");
		myAnim = base.GetComponent<Animation>();
		SoundController.Instance.PlayMySound("wil_" + Random.Range(1, 9));
	}

	private IEnumerator bombDrop()
	{
		base.GetComponent<Animation>().Play(currentAnim);
		yield return new WaitForSeconds(base.GetComponent<Animation>()[currentAnim].length * 2f);
		GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
		Object.Destroy(GetComponentInChildren<Collider>());
	}

	public void gotHit(float damage)
	{
		if (!isDead && !hit)
		{
			hit = true;
		}
	}

	public IEnumerator shake()
	{
		Vector3 orgPosition = deathPos.localPosition;
		Vector3 orgRot = deathPos.localEulerAngles;
		for (byte loops = 16; loops > 0; loops = (byte)(loops - 1))
		{
			deathPos.localPosition += Vector3.one * Random.Range(-0.25f, 0.25f);
			deathPos.localEulerAngles += Vector3.one * Random.Range(-0.75f, 0.75f);
			yield return new WaitForSeconds(0.001f);
		}
		deathPos.localPosition = orgPosition;
		deathPos.localEulerAngles = orgRot;
	}

	public IEnumerator delayedDeath()
	{
		base.GetComponent<Animation>().Play(currentAnim);
		yield return new WaitForSeconds(10f);
		GameManager.Instance.canHasWil = true;
		EnemySpawner.Instance.clearSpawn(currentAnim);
		Object.Destroy(base.gameObject);
	}

	private void Update()
	{
		if (isDead)
		{
			return;
		}
		if (hit)
		{
			isCurrentlyExpanded = true;
			if (Time.time > flashWil + 0.05f)
			{
				flashWil = Time.time;
				if (myMat.color == Color.red)
				{
					myMat.SetColor("_Color", Color.white);
				}
				else
				{
					myMat.SetColor("_Color", Color.red);
				}
			}
			float num = 0.0015f;
			if (GameManager.Instance.beemColor == Enemy.WHI)
			{
				num = 1.5E-05f;
			}
			for (short num2 = 0; num2 < vertices.Length; num2 = (short)(num2 + 1))
			{
				vertices[num2] += normals[num2] * num;
			}
			mesh.vertices = vertices;
			hit = false;
			if (!isPlaying)
			{
				StartCoroutine("delayedHitSound");
			}
			if (vertices[0].x > -0.0017454f && !isDead)
			{
				isDead = true;
				hit = false;
				wilDeath();
			}
		}
		else
		{
			if (vertices[0].x > orgMeshVerts[0].x)
			{
				for (short num3 = 0; num3 < vertices.Length; num3 = (short)(num3 + 1))
				{
					vertices[num3] -= normals[num3] * 0.001f;
				}
				mesh.vertices = vertices;
			}
			else
			{
				isCurrentlyExpanded = false;
			}
			myMat.SetColor("_Color", Color.white);
		}
		if (check && !myAnim.isPlaying)
		{
			GameManager.Instance.canHasWil = true;
			GameManager.Instance.isWilOut = false;
			Object.Destroy(base.gameObject);
		}
	}

	public void wilDeath()
	{
		isDead = true;
		GameManager.Instance.canHasWil = false;
		if (isTut)
		{
			GameObject.Find("Tutorial").SendMessage("wilDown", SendMessageOptions.DontRequireReceiver);
			Object.Destroy(base.gameObject);
			return;
		}
		Objectives.Instance.bake(base.gameObject.name, base.transform.localPosition, Enemy.NONE, 0);
		Objectives.Instance.reSet(false);
		base.transform.position = deathPos.position;
		base.GetComponent<Animation>().CrossFade("idle");
		StartCoroutine("deathEffect");
		Object.Destroy(myBomb);
		myMat.SetColor("_Color", Color.white);
	}

	public IEnumerator deathEffect()
	{
		StartCoroutine(GameManager.Instance.hudController.GameOver());
		for (byte loops = 64; loops > 0; loops = (byte)(loops - 1))
		{
			myMat.SetColor("_Color", Color.white);
			base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(0f, 8.24f, -8.9f), 0.1f);
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.one * 1.1f, 0.1f);
			base.transform.LookAt(Camera.main.transform.position);
			if (vertices[0].x < 0.1f)
			{
				for (short i = 0; i < vertices.Length; i = (short)(i + 1))
				{
					vertices[i] += normals[i] * 0.0004f;
				}
			}
			mesh.vertices = vertices;
			yield return new WaitForSeconds(0.01f);
		}
		base.GetComponent<Animation>().CrossFade("wilFloatUp");
	}

	public IEnumerator delayedHitSound()
	{
		isPlaying = true;
		SoundController.Instance.PlayMySound("wil_hurt" + Random.Range(1, 11));
		yield return new WaitForSeconds(0.5f);
		isPlaying = false;
	}

	public void myBombExploded()
	{
		check = true;
	}
}
