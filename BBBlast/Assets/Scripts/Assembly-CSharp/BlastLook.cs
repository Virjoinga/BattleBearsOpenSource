using UnityEngine;

public class BlastLook : MonoBehaviour
{
	public Transform tlc;

	private Transform myTransform;

	private void Start()
	{
		myTransform = base.transform;
	}

	private void Update()
	{
		myTransform.rotation = tlc.rotation;
	}
}
