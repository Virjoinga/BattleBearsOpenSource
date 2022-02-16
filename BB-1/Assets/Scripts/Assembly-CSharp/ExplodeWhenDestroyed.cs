using UnityEngine;

public class ExplodeWhenDestroyed : MonoBehaviour
{
	public GameObject explosion;

	private Transform myTransform;

	private bool hasExploded;

	public AudioClip explosionSound;

	private void Awake()
	{
		myTransform = base.transform;
	}

	private void OnObjectDestroyed(GameObject obj)
	{
		if (!hasExploded)
		{
			hasExploded = true;
			if ((bool)explosion)
			{
				Object.Instantiate(explosion).transform.position = myTransform.position;
			}
			if (explosionSound != null)
			{
				SoundManager.Instance.playSound(explosionSound);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
		{
			OnObjectDestroyed(base.gameObject);
			Object.Destroy(base.gameObject);
		}
		else if (other.gameObject.tag == "Player")
		{
			(other.transform.root.GetComponentInChildren(typeof(OCOController)) as OCOController).OnStun(1f);
			OnObjectDestroyed(base.gameObject);
			Object.Destroy(base.gameObject);
		}
		else if (other.gameObject.name == "UrsaMajor")
		{
			OnObjectDestroyed(base.gameObject);
			Object.Destroy(base.gameObject);
			other.gameObject.SendMessage("OnDamageShip", 2f, SendMessageOptions.DontRequireReceiver);
		}
	}
}
