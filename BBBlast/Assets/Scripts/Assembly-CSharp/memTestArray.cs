using System.Collections;
using UnityEngine;

public class memTestArray : MonoBehaviour
{
	private Vector3[] vertices;

	private Vector3[] normals;

	private Vector3[] org;

	private Vector3[] durp;

	private void Start()
	{
		vertices = new Vector3[GameManager.Instance.vertices.Length];
		org = new Vector3[GameManager.Instance.vertices.Length];
		normals = new Vector3[GameManager.Instance.normals.Length];
		durp = new Vector3[GameManager.Instance.normals.Length];
		for (int i = 0; i < GameManager.Instance.vertices.Length; i++)
		{
			vertices[i] = GameManager.Instance.vertices[i];
			org[i] = GameManager.Instance.vertices[i];
		}
		for (int j = 0; j < GameManager.Instance.normals.Length; j++)
		{
			normals[j] = GameManager.Instance.normals[j];
			durp[j] = GameManager.Instance.normals[j];
		}
		float num = Random.Range(10, 20);
		StartCoroutine("death", num);
	}

	private IEnumerator death(float time)
	{
		yield return new WaitForSeconds(time);
		Object.Destroy(base.gameObject);
	}
}
