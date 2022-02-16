using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineInChildren : MonoBehaviour
{
	private void Start()
	{
		MeshFilter[] componentsInChildren = GetComponentsInChildren<MeshFilter>();
		CombineInstance[] array = new CombineInstance[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			array[i].mesh = componentsInChildren[i].sharedMesh;
			array[i].transform = componentsInChildren[i].transform.localToWorldMatrix;
			//componentsInChildren[i].gameObject.active = false;
			componentsInChildren[i].gameObject.SetActive(false);
		}
		base.transform.GetComponent<MeshFilter>().mesh = new Mesh();
		base.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(array);
		//base.transform.gameObject.active = true;
		transform.gameObject.SetActive(true);
	}
}
