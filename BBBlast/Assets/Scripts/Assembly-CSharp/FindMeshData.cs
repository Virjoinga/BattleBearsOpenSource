using UnityEngine;

public class FindMeshData : MonoBehaviour
{
	private Mesh mesh;

	private Vector3[] vertices;

	private Vector3[] normals;

	private Vector3[] finalNormals;

	public bool nuggs;

	public bool bomb;

	public bool wil;

	public bool pointNuggs;

	private void Start()
	{
		if (!bomb)
		{
			mesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
		}
		else
		{
			mesh = GetComponentInChildren<MeshFilter>().mesh;
		}
		vertices = mesh.vertices;
		normals = mesh.normals;
		int[] array = new int[vertices.Length];
		int num = 0;
		for (int i = 0; i < vertices.Length; i++)
		{
			for (int j = 0; j < vertices.Length; j++)
			{
				if (array[i] == 0)
				{
					num = (array[i] = num + 1);
				}
				if (i != j && vertices[i] == vertices[j] && vertices[i] != Vector3.zero)
				{
					array[j] = array[i];
				}
			}
		}
		finalNormals = normals;
		for (int k = 1; k <= num; k++)
		{
			Vector3 zero = Vector3.zero;
			for (int l = 0; l < array.Length; l++)
			{
				if (array[l] == k)
				{
					zero += normals[l];
				}
			}
			zero.Normalize();
			for (int m = 0; m < array.Length; m++)
			{
				if (array[m] == k)
				{
					finalNormals[m] = zero;
				}
			}
		}
		if (nuggs)
		{
			GameManager.Instance.nuggsVert = new Vector3[vertices.Length];
			GameManager.Instance.nuggsNorm = new Vector3[finalNormals.Length];
		}
		else if (bomb)
		{
			GameManager.Instance.bombVert = new Vector3[vertices.Length];
			GameManager.Instance.bombNorm = new Vector3[finalNormals.Length];
		}
		else if (wil)
		{
			GameManager.Instance.wilVert = new Vector3[vertices.Length];
			GameManager.Instance.wilNorm = new Vector3[finalNormals.Length];
		}
		else if (pointNuggs)
		{
			GameManager.Instance.pointNuggsVert = new Vector3[vertices.Length];
			GameManager.Instance.pointNuggsNorm = new Vector3[finalNormals.Length];
		}
		else
		{
			GameManager.Instance.vertices = new Vector3[vertices.Length];
			GameManager.Instance.normals = new Vector3[finalNormals.Length];
		}
		for (int n = 0; n < vertices.Length; n++)
		{
			if (nuggs)
			{
				GameManager.Instance.nuggsVert[n] = vertices[n];
			}
			else if (bomb)
			{
				GameManager.Instance.bombVert[n] = vertices[n];
			}
			else if (wil)
			{
				GameManager.Instance.wilVert[n] = vertices[n];
			}
			else if (pointNuggs)
			{
				GameManager.Instance.pointNuggsVert[n] = vertices[n];
			}
			else
			{
				GameManager.Instance.vertices[n] = vertices[n];
			}
		}
		for (int num2 = 0; num2 < finalNormals.Length; num2++)
		{
			if (nuggs)
			{
				GameManager.Instance.nuggsNorm[num2] = finalNormals[num2];
			}
			else if (bomb)
			{
				GameManager.Instance.bombNorm[num2] = finalNormals[num2];
			}
			else if (wil)
			{
				GameManager.Instance.wilNorm[num2] = finalNormals[num2];
			}
			else if (pointNuggs)
			{
				GameManager.Instance.pointNuggsNorm[num2] = finalNormals[num2];
			}
			else
			{
				GameManager.Instance.normals[num2] = finalNormals[num2];
			}
		}
		Object.Destroy(base.gameObject);
	}
}
