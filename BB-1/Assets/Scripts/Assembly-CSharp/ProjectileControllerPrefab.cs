using UnityEngine;

public class ProjectileControllerPrefab : ProjectileController
{
	public GameObject prefab;

	public float firingForce = 10f;

	public override void FireProjectile(Transform shooterRoot, int numberOfBullets)
	{
		GameObject gameObject = Object.Instantiate(prefab);
		gameObject.transform.position = myTransform.position;
		gameObject.transform.LookAt(myTransform.position + myTransform.forward);
		Component[] componentsInChildren = shooterRoot.GetComponentsInChildren(typeof(Collider));
		Collider component = gameObject.GetComponent<Collider>();
		Component[] array = componentsInChildren;
		foreach (Component component2 in array)
		{
			Physics.IgnoreCollision(component, component2 as Collider);
		}
		gameObject.GetComponent<Rigidbody>().velocity = myTransform.forward * firingForce;
	}
}
