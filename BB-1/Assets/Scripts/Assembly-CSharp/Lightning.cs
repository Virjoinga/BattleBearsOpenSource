using System.Collections;
using UnityEngine;

public class Lightning : MonoBehaviour
{
	public ParticleSystem[] lightningPS;

	private void Start()
	{
		StartCoroutine(LightningEvent());
	}

	private IEnumerator LightningEvent()
	{
		lightningPS[0].enableEmission = true;
		lightningPS[1].enableEmission = true;
		yield return new WaitForSeconds(Random.Range(5, 8));
		lightningPS[0].enableEmission = true;
		lightningPS[1].enableEmission = true;
		yield return new WaitForSeconds(Random.Range(6, 10));
		StartCoroutine(LightningEvent());
	}
}
