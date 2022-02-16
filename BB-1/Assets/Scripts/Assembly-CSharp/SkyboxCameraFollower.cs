using UnityEngine;

public class SkyboxCameraFollower : MonoBehaviour
{
	private Transform transformToFollow;

	private Transform myTransform;

	private Transform newRotation;

	public bool isTwo;

	public bool is34;

	public bool is14;

	private void Awake()
	{
		myTransform = base.transform;
	}

	private void Start()
	{
		transformToFollow = GameObject.FindWithTag("Player").transform.parent;
		isTwo = false;
	}

	private void LateUpdate()
	{
		myTransform.rotation = transformToFollow.rotation;
	}
}
