using UnityEngine;

public class PuffPuff : MonoBehaviour
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
		for (short num2 = 0; num2 < vertices.Length; num2 = (short)(num2 + 1))
		{
			for (short num3 = 0; num3 < vertices.Length; num3 = (short)(num3 + 1))
			{
				if (array[num2] == 0)
				{
					num = (array[num2] = num + 1);
				}
				if (num2 != num3 && vertices[num2] == vertices[num3] && vertices[num2] != Vector3.zero)
				{
					array[num3] = array[num2];
				}
			}
		}
		finalNormals = normals;
		for (int i = 1; i <= num; i++)
		{
			Vector3 zero = Vector3.zero;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] == i)
				{
					zero += normals[j];
				}
			}
			zero.Normalize();
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k] == i)
				{
					finalNormals[k] = zero;
				}
			}
		}
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.U))
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] += finalNormals[i] * 1f * 0.0002f;
			}
			mesh.vertices = vertices;
		}
		if (Input.GetKey(KeyCode.D))
		{
			for (int j = 0; j < vertices.Length; j++)
			{
				vertices[j] -= finalNormals[j] * 1f * 0.0002f;
			}
			mesh.vertices = vertices;
		}
	}
}
