using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	private bool isCurrentlyExpanded;

	private string[] spawnPoint;

	private bool isDead;

	private Transform target;

	public float attackDistance;

	public bool explodeAttack;

	public float movementSpeed;

	public LayerMask chaseMask;

	public LayerMask fallMask;

	public float recalcTime;

	private bool playerLocked;

	private bool isAttacking;

	private bool hit;

	private Collider myCollider;

	private float movementSpeedOrg;

	public Enemy myColor;

	private float damage;

	public TextMesh deathScore;

	private bool isPlaying;

	private Vector3[] vertices;

	private Vector3[] orgVertAbs;

	private Vector3[] normals;

	private Mesh mesh;

	private float downCount;

	public Mesh orgMesh;

	public GameObject BOOM;

	public Transform meshPeParent;

	private int liveTime;

	private float decRateMod;

	private float incRateMod;

	private float pointMod;

	private float puffMod;

	private float speedMod;

	private string walk;

	private Vector3 startPoint;

	private float walkAnimSpeed;

	private Transform myTransform;

	public bool isTut;

	private GameObject shadow;

	private Animation myAnim;

	private GameObject myGameObject;

	private bool playedHugMe;

	private AudioClip[] hugMe;

	private AudioClip[] stretch;

	private MeshEmitter myMeshEmitter;

	private bool didExplode;

	private void Awake()
	{
		myCollider = GetComponentInChildren<Collider>();
		GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = UnityEngine.Object.Instantiate(orgMesh) as Mesh;
		mesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
		orgVertAbs = orgMesh.vertices;
		myTransform = base.transform;
		myAnim = base.GetComponent<Animation>();
		myGameObject = base.gameObject;
		myMeshEmitter = myGameObject.GetComponent<MeshEmitter>();
	}

	private void Start()
	{
		if (GameManager.Instance.beemColor == Enemy.BLU)
		{
			speedMod = 0.65f;
		}
		else if (GameManager.Instance.beemColor == Enemy.ORG)
		{
			speedMod = 1.2f;
		}
		else
		{
			speedMod = 1f;
		}
		puffMod = 1f;
		pointMod = 1f;
		decRateMod = 1f;
		incRateMod = 1f;
		shadow = GetComponentInChildren<DynamicShadow>().gameObject;
		spawnPoint = base.name.Split('_');
		getTarget();
		hit = false;
		isAttacking = false;
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
		movementSpeedOrg = movementSpeed;
		walkAnimSpeed = movementSpeed / 3f;
		if (myGameObject.name.Contains("pink"))
		{
			myColor = Enemy.PIN;
		}
		else if (myGameObject.name.Contains("blue"))
		{
			myColor = Enemy.BLU;
		}
		else if (myGameObject.name.Contains("green"))
		{
			myColor = Enemy.GRN;
		}
		else if (myGameObject.name.Contains("orange"))
		{
			myColor = Enemy.ORG;
		}
		else if (myGameObject.name.Contains("red"))
		{
			myColor = Enemy.RED;
		}
		else if (myGameObject.name.Contains("yellow"))
		{
			myColor = Enemy.YEL;
		}
		else if (myGameObject.name.Contains("purple"))
		{
			myColor = Enemy.PUR;
		}
		else if (myGameObject.name.Contains("white"))
		{
			myColor = Enemy.WHI;
		}
		walk = "hugWalk";
		//deathScore.gameObject.active = false;
		deathScore.gameObject.SetActive(false);
		StartCoroutine("intro");
		liveTime = 0;
		startPoint = myCollider.bounds.center;
		StartCoroutine("myUpdate");
	}

	private IEnumerator intro()
	{
		if (spawnPoint[1].Contains("Drop"))
		{
			string dropNum = UnityEngine.Random.Range(1, 4).ToString();
			myAnim[spawnPoint[1] + dropNum].speed = GameManager.Instance.enemySpeed;
			myAnim.Play(spawnPoint[1] + dropNum);
			if (GameManager.Instance.enemySpeed == 1f)
			{
				yield return new WaitForSeconds(myAnim.GetClip(spawnPoint[1] + dropNum).length);
			}
			else
			{
				yield return new WaitForSeconds(myAnim.GetClip(spawnPoint[1] + dropNum).length * 4f);
			}
		}
		else
		{
			myAnim[spawnPoint[1]].speed = GameManager.Instance.enemySpeed;
			myAnim.Play(spawnPoint[1]);
			if (GameManager.Instance.enemySpeed == 1f)
			{
				yield return new WaitForSeconds(myAnim.GetClip(spawnPoint[1]).length);
			}
			else
			{
				yield return new WaitForSeconds(myAnim.GetClip(spawnPoint[1]).length * 4f);
			}
		}
		StartCoroutine("mainBehaviour");
	}

	private void death()
	{
		if (isDead)
		{
			return;
		}
		isDead = true;
		if (shadow != null)
		{
			shadow.active = false;
		}
		//myCollider.gameObject.active = false;
		myCollider.gameObject.SetActive(false);
		GameManager.Instance.playerController.notGetHit();
		GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
		vertices = null;
		normals = null;
		SoundController.Instance.deathScale(GameManager.Instance.colorComboAmt);
		if (BOOM != null && !didExplode)
		{
			UnityEngine.Object.Instantiate(BOOM, meshPeParent.transform.position, BOOM.transform.rotation);
		}
		if (myMeshEmitter != null && !isTut)
		{
			myMeshEmitter.emit(meshPeParent.position, didExplode);
		}
		if (isTut)
		{
			GameObject.Find("Tutorial").SendMessage("huggyDead", SendMessageOptions.DontRequireReceiver);
			UnityEngine.Object.Destroy(mesh);
			UnityEngine.Object.Destroy(myGameObject);
			return;
		}
		if (myColor == GameManager.Instance.prev || myColor == Enemy.WHI || GameManager.Instance.prev == Enemy.WHI)
		{
			GameManager.Instance.colorComboAmt++;
			if ((float)GameManager.Instance.colorComboAmt > GameManager.Instance.statMultiplier)
			{
				GameManager.Instance.statMultiplier += 1f;
			}
			if (GameManager.Instance.currentGameMode == GameMode.SCORE)
			{
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
		float num2 = Mathf.Round(pointMod * (50f * Mathf.Pow(2f, GameManager.Instance.colorComboAmt - 1) * (float)Objectives.Instance.setCompletions));
		if (num2 > 1337f)
		{
			num2 = 1337f;
		}
		GameManager.Instance.hudController.updateScore(num2 * num);
		GameObject gameObject = UnityEngine.Object.Instantiate(deathScore.gameObject, meshPeParent.transform.position, BOOM.transform.rotation) as GameObject;
		gameObject.SendMessage("getPoints", num2, SendMessageOptions.DontRequireReceiver);
		Objectives.Instance.bake(myGameObject.name, myTransform.localPosition, myColor, liveTime);
		Objectives.Instance.setCheck(myColor);
		UnityEngine.Object.Destroy(mesh);
		gameObject = null;
		hugMe = null;
		stretch = null;
		UnityEngine.Object.Destroy(gameObject);
		UnityEngine.Object.Destroy(myGameObject);
	}

	private string numberToString(float number)
	{
		string text = number.ToString();
		string text2 = string.Empty;
		for (int i = 0; i < text.Length; i++)
		{
			text2 += (char)(text[i] + 49);
		}
		return text2;
	}

	private string numberConverter(float number)
	{
		number -= 0.1f;
		number = Mathf.Round(number);
		string result = string.Empty;
		switch ((int)number)
		{
		case 0:
			result = "a";
			break;
		case 1:
			result = "b";
			break;
		case 2:
			result = "c";
			break;
		case 3:
			result = "d";
			break;
		case 4:
			result = "e";
			break;
		case 5:
			result = "f";
			break;
		case 6:
			result = "g";
			break;
		case 7:
			result = "h";
			break;
		case 8:
			result = "i";
			break;
		case 9:
			result = "j";
			break;
		}
		return result;
	}

	private IEnumerator scoreScale()
	{
		while (deathScore.transform.localScale.x < 0.9f)
		{
			deathScore.transform.localScale += Vector3.one * 0.05f * 10f;
			yield return new WaitForSeconds(0.01f);
		}
		deathScore.transform.localScale = Vector3.one;
		for (int i = 0; i < 4; i++)
		{
			if (i % 2 == 0)
			{
				deathScore.transform.localScale += Vector3.one * 0.1f;
			}
			else
			{
				deathScore.transform.localScale -= Vector3.one * 0.1f;
			}
			yield return new WaitForSeconds(0.05f);
		}
		yield return new WaitForSeconds(0.05f);
		while (deathScore.transform.localScale.x > 0.1f)
		{
			deathScore.transform.localScale -= Vector3.one * 0.05f * 10f;
			yield return new WaitForSeconds(0.01f);
		}
		deathScore.transform.localScale = Vector3.zero;
	}

	private void getTarget()
	{
		if (!playerLocked && target != null)
		{
			UnityEngine.Object.Destroy(target.gameObject);
		}
		target = GameObject.Find("Oliver/oliverAnim").transform;
		if (target != null)
		{
			playerLocked = true;
		}
	}

	private IEnumerator mainBehaviour()
	{
		yield return new WaitForSeconds(0.01f);
		while (target != null && !isDead && !GameManager.Instance.isOver)
		{
			float distance = Vector3.Distance(myCollider.bounds.center, target.position);
			if (distance < attackDistance + 1f && playerLocked)
			{
				GameManager.Instance.playerController.gonnaGetHit();
			}
			if (distance < attackDistance && playerLocked)
			{
				if (!explodeAttack)
				{
					yield return StartCoroutine(attack());
				}
			}
			else
			{
				yield return StartCoroutine(chase());
			}
		}
	}

	private IEnumerator attack()
	{
		if (!GameManager.Instance.isOver && !isAttacking)
		{
			isAttacking = true;
			myAnim.CrossFade("hugAtk");
			if (!playedHugMe)
			{
				playedHugMe = true;
				SoundController.Instance.PlayMySound("hugme" + UnityEngine.Random.Range(1, 4));
			}
			yield return new WaitForSeconds(myAnim["hugAtk"].length);
			if (!isDead && !GameManager.Instance.isOver)
			{
				GameManager.Instance.playerController.onHit();
			}
			isAttacking = false;
			if (isTut)
			{
				GameObject.Find("Tutorial").SendMessage("hit", SendMessageOptions.DontRequireReceiver);
				UnityEngine.Object.Destroy(myGameObject);
			}
		}
	}

	private IEnumerator chase()
	{
		myAnim[walk].wrapMode = WrapMode.Loop;
		myAnim[walk].speed = walkAnimSpeed * GameManager.Instance.enemySpeed;
		myAnim.CrossFade(walk);
		startPoint = myCollider.bounds.center;
		Vector3 dir = new Vector3(target.position.x, 3.1f, target.position.z) - myCollider.bounds.center;
		dir.Normalize();
		RaycastHit hit;
		if (!Physics.Raycast(myCollider.bounds.center, dir, out hit, 5f, chaseMask))
		{
			myTransform.LookAt(target.position);
			if (!isAttacking)
			{
				myTransform.position += dir * movementSpeed * Time.deltaTime;
			}
			RaycastHit groundHit;
			if (Physics.Raycast(startPoint, Vector3.down, out groundHit, 10f, fallMask))
			{
				myTransform.position = new Vector3(myTransform.position.x, Mathf.Lerp(myTransform.position.y, groundHit.point.y + myCollider.bounds.size.y * 0.1f, 0.1f), myTransform.position.z);
			}
		}
		yield return new WaitForSeconds(recalcTime);
	}

	public void explode()
	{
		if (!isDead)
		{
			GameManager.Instance.statBombKills += 1f;
			Objectives.Instance.explosions();
			didExplode = true;
			StartCoroutine("death");
		}
	}

	public void gotHit(float dmg)
	{
		if (isDead)
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
			pointMod = 2f;
			if (GameManager.Instance.colorComboAmt == 1)
			{
				pointMod = 0.75f;
			}
			break;
		case Enemy.BLU:
			pointMod = 0.8f;
			break;
		case Enemy.ORG:
			pointMod = 1.2f;
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
		SoundController.Instance.PlayMySound("stretch" + UnityEngine.Random.Range(1, 3));
		yield return new WaitForSeconds(0.5f);
		isPlaying = false;
	}

	private IEnumerator myUpdate()
	{
		while (!isDead)
		{
			if (hit)
			{
				if (Math.Abs(vertices[0].x) > Math.Abs(orgVertAbs[0].x) * 1.05f)
				{
					movementSpeed = movementSpeedOrg * 0.8f * GameManager.Instance.enemySpeed * speedMod;
				}
				float mult2 = damage * incRateMod * Time.deltaTime * (float)GameManager.Instance.firingSpeed;
				for (short j = 0; j < vertices.Length; j = (short)(j + 1))
				{
					vertices[j].x += normals[j].x * mult2;
					vertices[j].y += normals[j].y * mult2;
					vertices[j].z += normals[j].z * mult2;
				}
				isCurrentlyExpanded = true;
				mesh.vertices = vertices;
				;
				hit = false;
				if (!isPlaying)
				{
					StartCoroutine("delayedHitSound");
				}
				if (Math.Abs(vertices[0].x) > Math.Abs(orgVertAbs[0].x) * 1.3f * puffMod && !isDead)
				{
					death();
				}
			}
			else if (isCurrentlyExpanded)
			{
				float mult = 2f * decRateMod * Time.deltaTime;
				if (Math.Abs(vertices[0].x) - Math.Abs(orgVertAbs[0].x) > 0f)
				{
					for (short i = 0; i < vertices.Length; i = (short)(i + 1))
					{
						vertices[i] -= normals[i] * mult;
					}
					mesh.vertices = vertices;
				}
				else
				{
					isCurrentlyExpanded = false;
					vertices = orgMesh.vertices;
					mesh.vertices = vertices;
				}
				if (Math.Abs(vertices[0].x) < Math.Abs(orgVertAbs[0].x) * 1.05f)
				{
					movementSpeed = movementSpeedOrg * GameManager.Instance.enemySpeed * speedMod;
				}
			}
			yield return null;
		}
		yield return null;
	}

	public IEnumerator JumpOnHead()
	{
		yield return new WaitForSeconds(3f);
		EnemySpawner.Instance.removeEnemy();
	}
}
