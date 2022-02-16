using System;
using System.Collections;
using UnityEngine;

public class NuggsController : MonoBehaviour
{
	public Nuggs type;

	public float movementSpeed;

	public Mesh orgMesh;

	private Vector3[] orgMeshVerts;

	private float damage;

	private bool hit;

	private bool isDead;

	private Vector3[] vertices;

	private Vector3[] normals;

	private Mesh mesh;

	public TextMesh pointText;

	private bool isPlaying;

	private BoxCollider myCollider;

	private Transform body;

	private float points;

	private float orgMovementSpeed;

	private Transform myTransform;

	private string flyAnim;

	private GameObject weatherEffect;

	private bool isPlayingAudio;

	private void Awake()
	{
		myCollider = GetComponentInChildren<BoxCollider>();
		orgMeshVerts = orgMesh.vertices;
		body = GetComponentInChildren<SkinnedMeshRenderer>().transform;
	}

	private void Start()
	{
		myTransform = base.transform;
		string[] array = base.gameObject.name.Split('_');
		orgMovementSpeed = movementSpeed;
		flyAnim = array[1];
		base.GetComponent<Animation>().Play(array[1]);
		SoundController.Instance.PlayMySound("nugg_fly" + UnityEngine.Random.Range(1, 5));
		hit = false;
		isDead = false;
		GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = UnityEngine.Object.Instantiate(orgMesh) as Mesh;
		mesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
		if (type == Nuggs.NONE)
		{
			vertices = new Vector3[GameManager.Instance.pointNuggsVert.Length];
			normals = new Vector3[GameManager.Instance.pointNuggsNorm.Length];
			for (short num = 0; num < GameManager.Instance.pointNuggsVert.Length; num = (short)(num + 1))
			{
				vertices[num] = GameManager.Instance.pointNuggsVert[num];
			}
			for (short num2 = 0; num2 < GameManager.Instance.pointNuggsNorm.Length; num2 = (short)(num2 + 1))
			{
				normals[num2] = GameManager.Instance.pointNuggsNorm[num2];
			}
			return;
		}
		if (type != Nuggs.POINTS)
		{
			vertices = new Vector3[GameManager.Instance.nuggsVert.Length];
			normals = new Vector3[GameManager.Instance.nuggsNorm.Length];
			for (short num3 = 0; num3 < GameManager.Instance.nuggsVert.Length; num3 = (short)(num3 + 1))
			{
				vertices[num3] = GameManager.Instance.nuggsVert[num3];
			}
			for (short num4 = 0; num4 < GameManager.Instance.nuggsNorm.Length; num4 = (short)(num4 + 1))
			{
				normals[num4] = GameManager.Instance.nuggsNorm[num4];
			}
		}
		else
		{
			vertices = new Vector3[GameManager.Instance.pointNuggsVert.Length];
			normals = new Vector3[GameManager.Instance.pointNuggsNorm.Length];
			for (short num5 = 0; num5 < GameManager.Instance.pointNuggsVert.Length; num5 = (short)(num5 + 1))
			{
				vertices[num5] = GameManager.Instance.pointNuggsVert[num5];
			}
			for (short num6 = 0; num6 < GameManager.Instance.pointNuggsNorm.Length; num6 = (short)(num6 + 1))
			{
				normals[num6] = GameManager.Instance.pointNuggsNorm[num6];
			}
		}
		if (type != Nuggs.POINTS)
		{
			StartCoroutine(delayedDeath(array[1]));
		}
	}

	public IEnumerator delayedDeath(string anim)
	{
		yield return new WaitForSeconds(base.GetComponent<Animation>().GetClip(anim).length + 1f);
		isDead = true;
		GameManager.Instance.canHasNuggs = true;
		UnityEngine.Object.Destroy(mesh);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Update()
	{
		if (isDead)
		{
			return;
		}
		if (type == Nuggs.NONE)
		{
			myTransform.position -= Vector3.right * movementSpeed * Time.deltaTime * GameManager.Instance.enemySpeed;
			base.GetComponent<Animation>()[flyAnim].speed = GameManager.Instance.enemySpeed;
			base.GetComponent<Animation>().Play(flyAnim);
			if ((double)myTransform.position.x < -18.75553)
			{
				GameObject.Find("Tutorial").SendMessage("hit", SendMessageOptions.DontRequireReceiver);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (hit)
			{
				if (Math.Abs(vertices[1].y) < Math.Abs(orgMeshVerts[1].y * 2f))
				{
					for (short num = 0; num < vertices.Length; num = (short)(num + 1))
					{
						vertices[num] += normals[num] * damage * Time.deltaTime * 2f;
					}
				}
				if (myCollider.size.x < 0.06f)
				{
					myCollider.size *= 1.5f;
				}
				mesh.vertices = vertices;
				hit = false;
				if (!isPlaying)
				{
					StartCoroutine("delayhitSound");
				}
				if (Math.Abs(vertices[0].y) > Math.Abs(orgMeshVerts[0].y) * 1.2f)
				{
					StartCoroutine("death");
				}
			}
			else if (Math.Abs(vertices[0].y) > Math.Abs(orgMeshVerts[0].y))
			{
				for (short num2 = 0; num2 < vertices.Length; num2 = (short)(num2 + 1))
				{
					vertices[num2] -= normals[num2] * 1f * Time.deltaTime;
				}
				mesh.vertices = vertices;
			}
		}
		if (type != Nuggs.POINTS)
		{
			if (hit)
			{
				for (short num3 = 0; num3 < vertices.Length; num3 = (short)(num3 + 1))
				{
					vertices[num3] += normals[num3] * damage * Time.deltaTime * 0.05f;
				}
				if (myCollider.size.x < 0.06f)
				{
					myCollider.size *= 1.5f;
				}
				mesh.vertices = vertices;
				hit = false;
				if (!isPlaying)
				{
					StartCoroutine("delayhitSound");
				}
				if (Math.Abs(vertices[0].y) > 0.035f)
				{
					StartCoroutine("death");
				}
			}
			else if (Math.Abs(vertices[0].y) > Math.Abs(orgMeshVerts[0].y))
			{
				for (short num4 = 0; num4 < vertices.Length; num4 = (short)(num4 + 1))
				{
					vertices[num4] -= normals[num4] * 0.01f * Time.deltaTime;
				}
				mesh.vertices = vertices;
			}
		}
		else
		{
			myTransform.position -= Vector3.right * movementSpeed * Time.deltaTime;
			if (myTransform.position.x < -30f)
			{
				GameManager.Instance.canHasNuggs = true;
				UnityEngine.Object.Destroy(base.gameObject);
				UnityEngine.Object.Destroy(mesh);
			}
			if (hit)
			{
				if (Math.Abs(vertices[0].y) < Math.Abs(orgMeshVerts[0].y * 2f))
				{
					for (short num5 = 0; num5 < vertices.Length; num5 = (short)(num5 + 1))
					{
						vertices[num5] += normals[num5] * damage * Time.deltaTime * 2f;
					}
				}
				else
				{
					StopCoroutine("pointThingy");
					StartCoroutine("pointThingy", 1000);
					StartCoroutine("death");
				}
				if (myCollider.size.x < 0.06f)
				{
					myCollider.size *= 1.5f;
				}
				mesh.vertices = vertices;
				hit = false;
				if (movementSpeed > orgMovementSpeed * 0.75f)
				{
					movementSpeed -= orgMovementSpeed * 0.1f;
				}
				if (!isPlaying)
				{
					StartCoroutine("delayhitSound");
				}
				if (Math.Abs(vertices[0].y) > Math.Abs(orgMeshVerts[0].y) * 1.1f)
				{
					StopCoroutine("pointThingy");
					StartCoroutine("pointThingy", 3);
					if (!isPlayingAudio)
					{
						isPlayingAudio = true;
						StartCoroutine("ResetAudioBool", SoundController.Instance.cacheDic["nugg_points_loop"].length);
						SoundController.Instance.PlayMySound("nugg_points_loop");
					}
				}
			}
			else if (Math.Abs(vertices[0].y) > Math.Abs(orgMeshVerts[0].y))
			{
				for (short num6 = 0; num6 < vertices.Length; num6 = (short)(num6 + 1))
				{
					vertices[num6] -= normals[num6] * 1f * Time.deltaTime;
				}
				mesh.vertices = vertices;
				if (movementSpeed < orgMovementSpeed)
				{
					movementSpeed += orgMovementSpeed * 0.1f;
				}
			}
		}
		if (myCollider.size.x > 0.03f && myCollider.size.x < 0.03f)
		{
			myCollider.size *= 0.02f;
		}
	}

	private IEnumerator delayhitSound()
	{
		isPlaying = true;
		SoundController.Instance.PlayMySound("nugg_imp" + UnityEngine.Random.Range(1, 3));
		yield return new WaitForSeconds(0.5f);
		isPlaying = false;
	}

	private IEnumerator pointThingy(float val)
	{
		float newAmt = Mathf.Round(val * (float)Objectives.Instance.setCompletions);
		points += newAmt;
		float tempMult = 1f;
		if (GameManager.Instance.multiplier > 0f)
		{
			tempMult = GameManager.Instance.multiplier;
		}
		if (GameManager.Instance.multiplier == 4f)
		{
			pointText.text = numberConverter(points) + "*";
		}
		else if (GameManager.Instance.multiplier == 2f)
		{
			pointText.text = numberConverter(points) + "!";
		}
		else
		{
			pointText.text = numberConverter(points);
		}
		pointText.transform.LookAt(GameManager.Instance.playerController.transform.position);
		pointText.transform.Rotate(Vector3.up * 180f);
		GameManager.Instance.hudController.updateScore(newAmt * tempMult);
		yield return new WaitForSeconds(1f);
	}

	private string numberConverter(float number)
	{
		number -= 0.1f;
		number = Mathf.Round(number);
		string text = number.ToString();
		string text2 = string.Empty;
		for (int i = 0; i < text.Length; i++)
		{
			text2 += (char)(text[i] + 49);
		}
		return text2;
	}

	public void gotHit(float dmg)
	{
		if (!isDead)
		{
			damage = dmg;
			hit = true;
		}
	}

	private IEnumerator death()
	{
		GameManager.Instance.NuggsDeath(type);
		if (!isDead)
		{
			isDead = true;
			GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
			switch (type)
			{
			case Nuggs.FREEZE:
				UnityEngine.Object.Instantiate(Resources.Load("NuggsCyanExplosion"), body.position, body.rotation);
				break;
			case Nuggs.MULTIPLIER:
				UnityEngine.Object.Instantiate(Resources.Load("NuggsPurpleExplosion"), body.position, body.rotation);
				break;
			case Nuggs.WHITE:
				UnityEngine.Object.Instantiate(Resources.Load("NuggsWhiteExplosion"), body.position, body.rotation);
				break;
			case Nuggs.NONE:
				UnityEngine.Object.Instantiate(Resources.Load("NuggsPurpleExplosion"), body.position, body.rotation);
				break;
			case Nuggs.POINTS:
				UnityEngine.Object.Instantiate(Resources.Load("NuggsYellowExplosion"), body.position, body.rotation);
				break;
			}
			SoundController.Instance.PlayMySound("nugg_destroy");
			GameManager.Instance.canHasNuggs = true;
			if (type != 0)
			{
				GameManager.Instance.statPowerups += 1f;
				Objectives.Instance.bake(base.gameObject.name, myTransform.localPosition, Enemy.NONE, 0);
			}
			else
			{
				GameObject.Find("Tutorial").SendMessage("nuggsDead", SendMessageOptions.DontRequireReceiver);
			}
			yield return new WaitForSeconds(0.6f);
			UnityEngine.Object.Destroy(weatherEffect);
			UnityEngine.Object.Destroy(mesh);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void delete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public IEnumerator ResetAudioBool(float sec)
	{
		yield return new WaitForSeconds(0.6f);
		isPlayingAudio = false;
	}
}
