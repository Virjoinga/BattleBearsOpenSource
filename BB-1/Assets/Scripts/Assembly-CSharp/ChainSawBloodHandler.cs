using System.Collections;
using UnityEngine;

public class ChainSawBloodHandler : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(BloodHandler());
	}

	private IEnumerator BloodHandler()
	{
		yield return new WaitForSeconds(1f);
		Object.Destroy(base.gameObject);
	}
}
