using System.Collections;
using UnityEngine;

public class PermaUVShifter : MonoBehaviour
{
	public Vector2 scale = new Vector2(1f, 1f);

	public Vector2 offset;

	public bool skinned;

	public bool huggy;

	private Mesh myMesh;

	private Vector2[] uv;

	private void Start()
	{
		if (skinned)
		{
			if (GameManager.Instance.hasSkinnedUVs)
			{
				return;
			}
			myMesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
			StartCoroutine("wait");
		}
		else
		{
			myMesh = GetComponentInChildren<MeshFilter>().mesh;
		}
		if (huggy)
		{
			myMesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
		}
		uv = myMesh.uv;
		for (int i = 0; i < uv.Length; i++)
		{
			float x = uv[i].x * scale.x + offset.x;
			float y = uv[i].y * scale.y + offset.y;
			uv[i] = new Vector2(x, y);
		}
		myMesh.uv = uv;
	}

	private IEnumerator wait()
	{
		yield return new WaitForSeconds(0.1f);
		GameManager.Instance.hasSkinnedUVs = true;
	}
}
