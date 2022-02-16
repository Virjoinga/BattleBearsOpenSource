using System.Collections;
using UnityEngine;

public class PanoramaController : MonoBehaviour
{
	public Collider midCollider;

	public Collider leftCollider;

	public Collider rightCollider;

	private bool isDead;

	public GameObject fallenPanorama;

	private void Start()
	{
		if (!GameManager.Instance.inTutorialRoom)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			fallenPanorama.SetActiveRecursively(false);
		}
	}

	private void OnObjectDestroyed(GameObject obj)
	{
		if (!isDead)
		{
			isDead = true;
			StartCoroutine(delayedColliderDestroy());
		}
	}

	private IEnumerator delayedColliderDestroy()
	{
		GetComponent<Animation>()["PanoramaFall"].speed = 0.5f;
		GetComponent<Animation>().Play("PanoramaFall");
		yield return new WaitForSeconds(GetComponent<Animation>()["PanoramaFall"].length * 2f);
		Object.Destroy(midCollider.gameObject);
		Object.Destroy(leftCollider.gameObject);
		Object.Destroy(rightCollider.gameObject);
	}
}
