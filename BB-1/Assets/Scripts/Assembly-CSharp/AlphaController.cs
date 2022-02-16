using System.Collections;
using UnityEngine;

public class AlphaController : MonoBehaviour
{
	public Transform controllerObj;

	public float delay;

	private void Awake()
	{
		StartCoroutine(delayedStart(delay));
	}

	private void Update()
	{
		if (controllerObj != null)
		{
			GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, controllerObj.localPosition.z);
		}
	}

	private IEnumerator delayedStart(float delay)
	{
		yield return new WaitForSeconds(delay);
		controllerObj.GetComponent<Animation>().Play("idle0");
	}
}
