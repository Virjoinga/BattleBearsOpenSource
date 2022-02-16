using UnityEngine;

public class MissileLauncherController : MonoBehaviour
{
	public GameObject missile;

	private Transform myTransform;

	private void Awake()
	{
		myTransform = base.transform;
	}

	public void fireMissile(Transform target)
	{
		if (!(target == null))
		{
			GameObject gameObject = Object.Instantiate(missile);
			gameObject.transform.position = myTransform.position;
			Component[] componentsInChildren = myTransform.root.GetComponentsInChildren(typeof(Collider));
			Collider component = gameObject.GetComponent<Collider>();
			Component[] array = componentsInChildren;
			foreach (Component component2 in array)
			{
				Physics.IgnoreCollision(component, component2 as Collider);
			}
			float num = Mathf.Sqrt(2f * (target.position.y - myTransform.position.y) / Physics.gravity.y);
			float num2 = Mathf.Sqrt(Mathf.Pow(myTransform.position.x - target.position.x, 2f) + Mathf.Pow(myTransform.position.z - target.position.z, 2f));
			if (num == 0f)
			{
				num = 0.1f;
			}
			float num3 = num2 / num;
			Vector3 vector = target.position - myTransform.position;
			vector.Normalize();
			vector.y = 0f;
			gameObject.GetComponent<Rigidbody>().velocity = vector * num3 * 0.85f;
		}
	}
}
