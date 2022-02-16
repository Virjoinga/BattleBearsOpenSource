using UnityEngine;

public class NewBit : MonoBehaviour
{
	public Vector3 velocity;

	public Vector3 constantAxisForce;

	public Vector3 rotationVelocity;

	private void Start()
	{
	}

	private void Update()
	{
		velocity = new Vector3(velocity.x + constantAxisForce.x, velocity.y + constantAxisForce.y, velocity.z + constantAxisForce.z);
		base.gameObject.transform.position = new Vector3(base.gameObject.transform.position.x + velocity.x * Time.deltaTime, base.gameObject.transform.position.y + velocity.y * Time.deltaTime, base.gameObject.transform.position.z + velocity.z * Time.deltaTime);
		base.gameObject.transform.Rotate(new Vector3(rotationVelocity.x * Time.deltaTime, rotationVelocity.y * Time.deltaTime, rotationVelocity.z * Time.deltaTime));
	}
}
