using UnityEngine;

public class AsteroidController : MonoBehaviour
{
	public GameObject explosion;

	public GameObject[] asteroidPieces;

	private Transform myTransform;

	public int minPiecesToSpawn = 3;

	public int maxPiecesToSpawn = 6;

	private bool hasDied;

	private bool splitWhenDestroyed = true;

	public AudioClip destroySound;

	private void Awake()
	{
		myTransform = base.transform;
	}

	private void OnObjectDestroyed(GameObject obj)
	{
		if (hasDied)
		{
			return;
		}
		hasDied = true;
		if ((bool)explosion)
		{
			Object.Instantiate(explosion).transform.position = myTransform.position;
		}
		if (destroySound != null)
		{
			SoundManager.Instance.playSound(destroySound);
		}
		if (splitWhenDestroyed)
		{
			int num = Random.Range(minPiecesToSpawn, maxPiecesToSpawn + 1);
			if (GameManager.Instance.currentDifficulty == GameDifficulty.EASY)
			{
				num = 2;
			}
			else if (GameManager.Instance.currentDifficulty == GameDifficulty.MEDIUM)
			{
				num = 3;
			}
			for (int i = 0; i < num; i++)
			{
				Vector3 vector = Random.insideUnitSphere * 20f;
				GameObject obj2 = Object.Instantiate(asteroidPieces[Random.Range(0, asteroidPieces.Length)]);
				obj2.transform.position = myTransform.position + vector;
				obj2.GetComponent<Rigidbody>().velocity = new Vector3(-4f + Random.value * 8f, Random.value * 4f, 10f + (-2f + Random.value * 4f) + 2f * (float)GameManager.Instance.currentDifficulty);
				obj2.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 10f;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
		{
			splitWhenDestroyed = false;
			OnObjectDestroyed(base.gameObject);
			Object.Destroy(base.gameObject);
		}
		else if (other.gameObject.tag == "Player")
		{
			(other.transform.root.GetComponentInChildren(typeof(OCOController)) as OCOController).OnStun(3f);
			splitWhenDestroyed = false;
			OnObjectDestroyed(base.gameObject);
			Object.Destroy(base.gameObject);
		}
		else if (other.gameObject.name == "UrsaMajor")
		{
			splitWhenDestroyed = false;
			OnObjectDestroyed(base.gameObject);
			Object.Destroy(base.gameObject);
			other.gameObject.SendMessage("OnDamageShip", 5f, SendMessageOptions.DontRequireReceiver);
		}
	}
}
