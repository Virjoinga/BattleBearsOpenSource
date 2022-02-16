using System.Collections;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
	public float timeToLive;

	private void Start()
	{
		if (timeToLive == 0f)
		{
			timeToLive = 1f;
		}
		StartCoroutine("WaitToDestroy");
	}

	private IEnumerator WaitToDestroy()
	{
		yield return new WaitForSeconds(timeToLive);
		Object.Destroy(GetComponent<NewBit>());
		EnemySpawner.Instance.freeMeshEmitObj(base.gameObject);
		Object.Destroy(this);
	}
}
