using System.Collections;
using UnityEngine;

public class MissileController : MonoBehaviour
{
	public GameObject explosion;

	public ParticleSystem trailPS;

	private Transform myTransform;

	public string[] colliderLayers;

	public AudioClip explosionSound;

	private Rigidbody myRigidbody;

	private void Awake()
	{
		myTransform = base.transform;
		myRigidbody = GetComponent<Rigidbody>();
	}

	private void OnTriggerEnter(Collider c)
	{
		handleCollision(c.gameObject);
	}

	private void OnCollisionEnter(Collision c)
	{
		handleCollision(c.gameObject);
	}

	private void handleCollision(GameObject c)
	{
		if (c.layer == LayerMask.NameToLayer("PlayerDefence"))
		{
			myRigidbody.velocity = new Vector3(0f, -2f, 0f);
			myRigidbody.detectCollisions = false;
			c.SendMessage("OnBlock");
			StartCoroutine(delayedDestroy());
			return;
		}
		for (int i = 0; i < colliderLayers.Length; i++)
		{
			if (LayerMask.NameToLayer(colliderLayers[i]) == c.layer)
			{
				destroy();
				break;
			}
		}
	}

	private IEnumerator delayedDestroy()
	{
		yield return new WaitForSeconds(4f);
		Object.Destroy(base.gameObject);
	}

	private void destroy()
	{
		if (explosionSound != null)
		{
			SoundManager.Instance.playSound(explosionSound);
		}
		if (explosion != null)
		{
			Object.Instantiate(explosion).transform.position = myTransform.position - myRigidbody.velocity * 2f * Time.deltaTime;
		}
		if (trailPS != null)
		{
			trailPS.transform.parent = null;
			trailPS.enableEmission = false;
		}
		Object.Destroy(base.gameObject);
	}
}
