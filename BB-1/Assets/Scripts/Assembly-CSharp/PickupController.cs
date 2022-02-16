using System.Collections;
using UnityEngine;

public class PickupController : MonoBehaviour
{
	private Transform myTransform;

	private Animation myAnimator;

	public int amount = 1;

	public float lifetime = 30f;

	public string type;

	public GameObject pickupEffect;

	public static ArrayList currentPickups = new ArrayList();

	private Collider myCollider;

	public AudioClip pickupSound;

	private void Awake()
	{
		if (GameManager.Instance.currentCharacter == Character.WIL)
		{
			lifetime = 300f;
		}
		myTransform = base.transform;
		myAnimator = GetComponent<Animation>();
		myCollider = GetComponent<Collider>();
		for (int i = 0; i < HuggableController.currentBaddies.Count; i++)
		{
			Physics.IgnoreCollision(HuggableController.currentBaddies[i] as Collider, myCollider);
		}
		for (int j = 0; j < RoomTile.currentRoomColliders.Count; j++)
		{
			Collider collider = RoomTile.currentRoomColliders[j] as Collider;
			if (collider != null && collider.gameObject.active)
			{
				Physics.IgnoreCollision(collider, myCollider);
			}
		}
		for (int k = 0; k < currentPickups.Count; k++)
		{
			Physics.IgnoreCollision(currentPickups[k] as Collider, myCollider);
		}
		StartCoroutine(lifetimeController());
	}

	private IEnumerator lifetimeController()
	{
		if (myAnimator != null)
		{
			myAnimator.CrossFade("In");
			yield return new WaitForSeconds(myAnimator["In"].length);
			myAnimator["idleFloat"].wrapMode = WrapMode.Loop;
			myAnimator.CrossFade("idleFloat");
		}
		yield return new WaitForSeconds(lifetime);
		if (myAnimator != null)
		{
			myAnimator.CrossFade("Out");
			yield return new WaitForSeconds(myAnimator["Out"].length);
		}
		StatsManager.Instance.pickupsMissed++;
		Object.Destroy(myTransform.parent.gameObject);
	}

	private void OnTriggerEnter(Collider c)
	{
		if (!c.CompareTag("Player"))
		{
			return;
		}
		if (pickupSound != null)
		{
			SoundManager.Instance.playSound(pickupSound);
		}
		PlayerController playerController = c.transform.parent.GetComponent(typeof(PlayerController)) as PlayerController;
		switch (type)
		{
		case "bearzooka":
			playerController.OnGetBearzooka();
			StatsManager.Instance.currentBearzookasPickedUp++;
			break;
		case "spreadshot":
			playerController.OnGetSpreadshot();
			StatsManager.Instance.currentSpreadshotsPickedUp++;
			break;
		case "extralife":
			playerController.OnGetExtraLife();
			StatsManager.Instance.currentLivesPickedUp++;
			break;
		case "food":
			playerController.OnAddFood();
			StatsManager.Instance.currentFoodsPickedUp++;
			break;
		case "satellite":
			playerController.OnAddSatellite();
			StatsManager.Instance.currentSatellitesPickedUp++;
			break;
		case "screenclear":
			playerController.OnScreenClear(amount);
			StatsManager.Instance.currentScreenclearsPickedUp++;
			break;
		case "shield":
			playerController.OnGetShield();
			StatsManager.Instance.currentShieldsPickedUp++;
			break;
		case "speed":
			playerController.OnDrinkCoffee();
			StatsManager.Instance.currentCoffeesPickedUp++;
			break;
		case "special":
			playerController.OnGetSpecial();
			StatsManager.Instance.currentSpecialsPickedUp++;
			break;
		case "special2":
			playerController.OnGetSpecial2();
			StatsManager.Instance.currentSpecialsPickedUp++;
			break;
		case "score":
			StatsManager.Instance.currentScore += amount;
			HUDController.Instance.updateScore();
			break;
		case "giftGY":
		{
			float value = Random.value;
			if (value < 0.33f)
			{
				StatsManager.Instance.toasters++;
			}
			else if (value < 0.66f)
			{
				StatsManager.Instance.boats++;
			}
			else
			{
				StatsManager.Instance.vespas++;
			}
			break;
		}
		case "giftYR":
		{
			float value = Random.value;
			if (value < 0.33f)
			{
				StatsManager.Instance.turkeys++;
			}
			else if (value < 0.66f)
			{
				StatsManager.Instance.grills++;
			}
			else
			{
				StatsManager.Instance.vespas++;
			}
			break;
		}
		}
		if (pickupEffect != null)
		{
			if (type == "screenclear")
			{
				Object.Instantiate(pickupEffect, myTransform.position, myTransform.rotation);
			}
			else
			{
				GameObject obj = Object.Instantiate(pickupEffect);
				obj.transform.parent = c.transform;
				obj.transform.localPosition = new Vector3(0f, -5f, 0f);
				obj.transform.rotation = Quaternion.identity;
			}
		}
		Object.Destroy(myTransform.parent.gameObject);
	}

	private void OnDisable()
	{
		currentPickups.Remove(myCollider);
	}
}
