using UnityEngine;

public class AddRandomRotation : MonoBehaviour
{
	public float amountOfSpin = 1f;

	public void Awake()
	{
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * amountOfSpin;
		Object.Destroy(this);
	}
}
