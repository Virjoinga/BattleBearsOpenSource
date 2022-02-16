using UnityEngine;

public class WeaponController : MonoBehaviour
{
	private Ray aimRay;

	private void Start()
	{
		base.enabled = false;
	}

	public void Aim(Vector2 pos)
	{
		aimRay = new Ray(Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 20f)), Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 1000f)));
		RaycastHit hitInfo;
		if (Physics.Raycast(aimRay, out hitInfo, float.PositiveInfinity))
		{
			hitInfo.collider.gameObject.SendMessage("OnHit", 1);
		}
	}
}
