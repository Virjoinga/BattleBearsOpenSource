using UnityEngine;

public class UVAnim : MonoBehaviour
{
	private Mesh mesh;

	private Vector2[] uvs;

	private float myTime;

	public float x;

	public float y = 0.1f;

	public float tick = 0.1f;

	private void Start()
	{
		myTime = Time.time;
		mesh = GetComponent<MeshFilter>().mesh;
		uvs = mesh.uv;
	}

	private void Update()
	{
		if (Time.time > myTime + tick)
		{
			for (int i = 0; i < uvs.Length; i++)
			{
				uvs[i] += new Vector2(x, y);
			}
			mesh.uv = uvs;
			myTime = Time.time;
		}
	}
}
