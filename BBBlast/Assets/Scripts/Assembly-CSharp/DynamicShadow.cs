using UnityEngine;

public class DynamicShadow : MonoBehaviour
{
	private Transform body;

	private float startTime;

	private Transform myTransform;

	private void Start()
	{
		base.GetComponent<Renderer>().enabled = false;
		startTime = Time.time;
		Transform[] componentsInChildren = base.transform.parent.GetComponentsInChildren<Transform>();
		myTransform = base.transform;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject.name == "Body")
			{
				body = componentsInChildren[i];
			}
		}
	}

	private void Update()
	{
		if (Time.time > startTime + 0.1f)
		{
			base.GetComponent<Renderer>().enabled = true;
		}
		if (body != null)
		{
			myTransform.position = new Vector3(body.position.x, base.transform.position.y, body.position.z);
		}
	}
}
