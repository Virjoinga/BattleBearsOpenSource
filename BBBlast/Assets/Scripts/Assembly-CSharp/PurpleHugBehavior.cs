using System;
using System.Collections;
using UnityEngine;

public class PurpleHugBehavior : MonoBehaviour
{
	private enum HuggyState
	{
		IDLE,
		INTRO,
		WALK,
		ATTACK,
		DEATH
	}

	public LayerMask chaseMask;

	public LayerMask fallMask;

	public float attackDistance;

	private HuggyState state;

	private float walkAnimSpeed;

	private BoxCollider myCollider;

	private Vector3 startPoint;

	private Transform myTransform;

	private Transform target;

	private float prevTime;

	private int num;

	private string walkAnim;

	private GameObject oliverBody;

	private GameObject shadow;

	private bool die;

	private string[] spawnPoint;

	private float damage;

	private Vector3[] vertices;

	private Vector3[] orgMeshVertices;

	private Vector3[] normals;

	private Mesh mesh;

	private bool hit;

	public Enemy myColor;

	private bool isPlaying;

	private float myTime;

	private int liveTime;

	private float decRateMod;

	private float incRateMod;

	private float pointMod;

	private float puffMod;

	private float speedMod;

	public Mesh orgMesh;

	public GameObject BOOM;

	public Transform meshPeParent;

	public TextMesh deathScore;

	private bool hasHurtOliver;

	private MeshEmitter myMeshEmitter;

	private GameObject myGameObject;

	private bool didExplode;

	private bool isCurrentlyExpanded;

	private void Awake()
	{
		if (GameManager.Instance.beemColor != Enemy.PUR)
		{
			orgMeshVertices = orgMesh.vertices;
			GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = UnityEngine.Object.Instantiate(orgMesh) as Mesh;
			mesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
		}
	}

	private void Start()
	{
		if (GameManager.Instance.beemColor != Enemy.PUR)
		{
			hasHurtOliver = false;
			speedMod = 1f;
			puffMod = 1f;
			pointMod = 1f;
			decRateMod = 1f;
			incRateMod = 1f;
			spawnPoint = base.name.Split('_');
			shadow = GetComponentInChildren<DynamicShadow>().gameObject;
			oliverBody = GameObject.Find("Oliver/oliverAnim/Oliver");
			target = GameObject.Find("Oliver").transform;
			myCollider = GetComponentInChildren<BoxCollider>();
			this.num = UnityEngine.Random.Range(0, 100);
			if (this.num >= 50)
			{
				walkAnim = "frontFlip";
			}
			else
			{
				walkAnim = "jump";
			}
			walkAnimSpeed = 1f;
			if (spawnPoint[1] == "Drop")
			{
				spawnPoint[1] = spawnPoint[1] + UnityEngine.Random.Range(1, 4);
			}
			base.GetComponent<Animation>().Play(spawnPoint[1]);
			state = HuggyState.INTRO;
			startPoint = myCollider.bounds.center;
			myTransform = base.transform;
			myGameObject = base.gameObject;
			myMeshEmitter = myGameObject.GetComponent<MeshEmitter>();
			if (base.gameObject.name.Contains("purple"))
			{
				myColor = Enemy.PUR;
			}
			vertices = new Vector3[GameManager.Instance.vertices.Length];
			normals = new Vector3[GameManager.Instance.normals.Length];
			for (short num = 0; num < GameManager.Instance.vertices.Length; num = (short)(num + 1))
			{
				vertices[num] = GameManager.Instance.vertices[num];
			}
			for (short num2 = 0; num2 < GameManager.Instance.normals.Length; num2 = (short)(num2 + 1))
			{
				normals[num2] = GameManager.Instance.normals[num2];
			}
			base.GetComponent<Animation>()[walkAnim].wrapMode = WrapMode.Once;
			base.GetComponent<Animation>()[walkAnim].speed = walkAnimSpeed * GameManager.Instance.enemySpeed;
			die = false;
		}
	}

	public void gotHit(float dmg)
	{
		if (!base.enabled || die)
		{
			return;
		}
		damage = dmg * 3f;
		switch (GameManager.Instance.beemColor)
		{
		case Enemy.PIN:
			if (myColor == Enemy.PIN)
			{
				damage = dmg * 3f * 1.4f;
			}
			break;
		case Enemy.GRN:
			decRateMod = 0.5f;
			break;
		case Enemy.RED:
			incRateMod = 1.1f;
			pointMod = 0.85f;
			break;
		case Enemy.YEL:
			pointMod = 0.75f;
			break;
		case Enemy.BLU:
			puffMod = 1.2f;
			speedMod = 0.8f;
			break;
		case Enemy.ORG:
			speedMod = 1.2f;
			pointMod = 1.2f;
			break;
		case Enemy.PUR:
			if (myColor != Enemy.PUR)
			{
			}
			break;
		default:
			damage = dmg * 3f;
			break;
		}
		hit = true;
	}

	public IEnumerator delayedHitSound()
	{
		isPlaying = true;
		SoundController.Instance.playClip(Resources.Load("huggable_stretch" + UnityEngine.Random.Range(1, 3)) as AudioClip);
		yield return new WaitForSeconds(0.5f);
		isPlaying = false;
	}

	private void Update()
	{
		if (!GameManager.Instance.isOver)
		{
			if (hit)
			{
				for (short num = 0; num < vertices.Length; num = (short)(num + 1))
				{
					vertices[num] += normals[num] * damage * incRateMod * Time.deltaTime * GameManager.Instance.firingSpeed;
				}
				mesh.vertices = vertices;
				hit = false;
				isCurrentlyExpanded = true;
				if (!isPlaying)
				{
					StartCoroutine("delayedHitSound");
				}
				if (Math.Abs(vertices[0].x) > Math.Abs(orgMeshVertices[0].x) * 1.2f * puffMod)
				{
					death();
				}
			}
			else if (isCurrentlyExpanded)
			{
				if (Math.Abs(vertices[0].x) - Math.Abs(orgMeshVertices[0].x) > 0f)
				{
					for (short num2 = 0; num2 < vertices.Length; num2 = (short)(num2 + 1))
					{
						vertices[num2] -= normals[num2] * 2f * decRateMod * Time.deltaTime;
					}
					mesh.vertices = vertices;
				}
				else
				{
					vertices = orgMesh.vertices;
					mesh.vertices = vertices;
					isCurrentlyExpanded = false;
				}
			}
			base.GetComponent<Animation>()[walkAnim].speed = walkAnimSpeed * GameManager.Instance.enemySpeed;
			switch (state)
			{
			case HuggyState.IDLE:
				break;
			case HuggyState.INTRO:
				intro();
				break;
			case HuggyState.WALK:
				walk();
				break;
			case HuggyState.ATTACK:
				attack();
				break;
			case HuggyState.DEATH:
				headDeath();
				break;
			}
		}
		else
		{
			base.GetComponent<Animation>()[walkAnim].speed = 0f;
		}
	}

	private void intro()
	{
		if (!base.GetComponent<Animation>().isPlaying)
		{
			state = HuggyState.WALK;
		}
	}

	private void walk()
	{
		if (!base.enabled)
		{
			return;
		}
		myTransform.LookAt(GameObject.Find("Oliver/oliverAnim/Root/Hips/Spine1/Spine2/Spine3/Head").transform.position);
		if (base.GetComponent<Animation>().isPlaying)
		{
			base.GetComponent<Animation>()[walkAnim].speed = walkAnimSpeed * GameManager.Instance.enemySpeed;
			RaycastHit hitInfo;
			if (Physics.Raycast(myTransform.position, Vector3.down, out hitInfo, 6f, fallMask))
			{
				myTransform.position = new Vector3(myTransform.position.x, Mathf.Lerp(myTransform.position.y, hitInfo.point.y, 0.1f), myTransform.position.z);
			}
			if (hitInfo.collider != null && myCollider.transform.position.y < hitInfo.point.y + 5f)
			{
				myTransform.position = new Vector3(myTransform.position.x, Mathf.Lerp(myTransform.position.y, hitInfo.point.y, 0.1f), myTransform.position.z);
				return;
			}
			(new Vector3(target.position.x, 3.1f, target.position.z) - myCollider.bounds.center).Normalize();
			myTransform.position = Vector3.MoveTowards(base.gameObject.transform.position, target.position, 0.12f);
		}
		else if (!base.GetComponent<Animation>().isPlaying)
		{
			float num = Vector3.Distance(myTransform.position, target.position);
			if (num < attackDistance)
			{
				state = HuggyState.ATTACK;
				return;
			}
			SoundController.Instance.playClip(Resources.Load("huggable_jump") as AudioClip);
			int num2 = UnityEngine.Random.Range(0, 100);
			string text = ((num2 < 50) ? "jump" : "frontFlip");
			base.GetComponent<Animation>().Play(text);
		}
	}

	private void attack()
	{
		if (!base.enabled || die)
		{
			return;
		}
		if (base.GetComponent<Animation>().isPlaying)
		{
			if (shadow != null && shadow.active)
			{
				shadow.active = false;
			}
			Vector3 vector = new Vector3(oliverBody.transform.position.x, oliverBody.transform.position.y + 5.5f, target.position.z + 3f);
			RaycastHit hitInfo;
			Physics.Raycast(myTransform.position, Vector3.down, out hitInfo, 6f, fallMask);
			if (!(hitInfo.collider != null) || !(myCollider.transform.position.y < hitInfo.point.y + 5f))
			{
				myTransform.position = Vector3.MoveTowards(base.gameObject.transform.position, vector, 0.3f);
			}
		}
		else
		{
			SoundController.Instance.playClip(Resources.Load("huggable_jump") as AudioClip);
			base.GetComponent<Animation>().Play(walkAnim);
			if (!shadow.active)
			{
				state = HuggyState.DEATH;
			}
		}
	}

	private void headDeath()
	{
		if (!base.enabled)
		{
			return;
		}
		if (!base.GetComponent<Animation>().isPlaying && !die)
		{
			SoundController.Instance.playClip(Resources.Load("huggable_jump") as AudioClip);
			base.GetComponent<Animation>().Play(walkAnim);
			die = true;
		}
		else if (base.GetComponent<Animation>().isPlaying)
		{
			myTransform.position = Vector3.MoveTowards(base.gameObject.transform.position, base.gameObject.transform.position - Vector3.one * -6f, 0.2f);
			if (!hasHurtOliver && Vector3.Distance(myTransform.position, myTransform.position) < 1f)
			{
				GameManager.Instance.playerController.onHit();
				hasHurtOliver = true;
			}
		}
		else if (die)
		{
			EnemySpawner.Instance.enemyCount -= 1f;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void death()
	{
		if (!base.enabled)
		{
			return;
		}
		if (!die)
		{
			if (shadow != null)
			{
				UnityEngine.Object.Destroy(shadow);
			}
			die = true;
			//myCollider.gameObject.active = false;
			myCollider.gameObject.SetActive(false);
			base.GetComponent<Animation>().Stop();
			GameManager.Instance.playerController.notGetHit();
			GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
			SoundController.Instance.deathScale(GameManager.Instance.colorComboAmt);
			if (BOOM != null)
			{
				UnityEngine.Object.Instantiate(BOOM, meshPeParent.transform.position, BOOM.transform.rotation);
			}
			if (myMeshEmitter != null)
			{
				myMeshEmitter.emit(meshPeParent.position, didExplode);
			}
			if (myColor == GameManager.Instance.prev || myColor == Enemy.WHI || GameManager.Instance.prev == Enemy.WHI || GameManager.Instance.whiteOut)
			{
				GameManager.Instance.colorComboAmt++;
				if ((float)GameManager.Instance.colorComboAmt > GameManager.Instance.statMultiplier)
				{
					GameManager.Instance.statMultiplier += 1f;
				}
				if (GameManager.Instance.whiteOut)
				{
					myColor = Enemy.WHI;
				}
				GameManager.Instance.prev = myColor;
				Objectives.Instance.killCombo(1f, myColor);
				switch (myColor)
				{
				case Enemy.BLU:
					GameManager.Instance.statBlueCombo += 1f;
					break;
				case Enemy.GRN:
					GameManager.Instance.statGreenCombo += 1f;
					break;
				case Enemy.ORG:
					GameManager.Instance.statOrangeCombo += 1f;
					break;
				case Enemy.PIN:
					GameManager.Instance.statPinkCombo += 1f;
					break;
				case Enemy.RED:
					GameManager.Instance.statRedCombo += 1f;
					break;
				case Enemy.YEL:
					GameManager.Instance.statYellowCombo += 1f;
					break;
				case Enemy.PUR:
					GameManager.Instance.statPurpleCombo += 1f;
					break;
				}
			}
			else
			{
				GameManager.Instance.colorComboAmt = 1;
				GameManager.Instance.prev = myColor;
			}
			switch (myColor)
			{
			case Enemy.BLU:
				GameManager.Instance.statBlueKills += 1f;
				break;
			case Enemy.GRN:
				GameManager.Instance.statGreenKills += 1f;
				break;
			case Enemy.ORG:
				GameManager.Instance.statOrangeKills += 1f;
				break;
			case Enemy.PIN:
				GameManager.Instance.statPinkKills += 1f;
				break;
			case Enemy.RED:
				GameManager.Instance.statRedKills += 1f;
				break;
			case Enemy.YEL:
				GameManager.Instance.statYellowKills += 1f;
				break;
			case Enemy.PUR:
				GameManager.Instance.statPurpleKills += 1f;
				break;
			}
			EnemySpawner.Instance.removeEnemy();
			float num = 1f;
			if (GameManager.Instance.multiplier > 0f)
			{
				num = GameManager.Instance.multiplier;
			}
			float num2 = 0f;
			num2 = ((GameManager.Instance.beemColor != Enemy.YEL) ? Mathf.Round(pointMod * (50f * Mathf.Pow(2f, GameManager.Instance.colorComboAmt - 1) * (float)Objectives.Instance.setCompletions)) : Mathf.Round(pointMod * (50f * Mathf.Pow(3f, GameManager.Instance.colorComboAmt - 1) * (float)Objectives.Instance.setCompletions)));
			if (num2 > 1337f)
			{
				num2 = 1337f;
			}
			GameManager.Instance.hudController.updateScore(num2 * num);
			GameObject gameObject = UnityEngine.Object.Instantiate(deathScore.gameObject, meshPeParent.transform.position, BOOM.transform.rotation) as GameObject;
			gameObject.SendMessage("getPoints", num2, SendMessageOptions.DontRequireReceiver);
			Objectives.Instance.bake(base.gameObject.name, Vector3.one, myColor, liveTime);
			Objectives.Instance.setCheck(myColor);
			UnityEngine.Object.Destroy(mesh);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		EnemySpawner.Instance.removeEnemy();
		Objectives.Instance.limitCheck(myColor);
	}

	public void explode()
	{
		if (base.enabled && !die)
		{
			GameManager.Instance.statBombKills += 1f;
			didExplode = true;
			Objectives.Instance.explosions();
			StartCoroutine("death");
		}
	}
}
