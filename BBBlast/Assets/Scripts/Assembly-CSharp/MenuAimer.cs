using UnityEngine;

public class MenuAimer : MonoBehaviour
{
	private float x;

	private float y;

	private Ray aimRay;

	private void Update()
	{
		aimRay = new Ray(Camera.main.ScreenToWorldPoint(new Vector3(x, y, 20f)), Camera.main.ScreenToWorldPoint(new Vector3(x, y, 1000f)));
		RaycastHit hitInfo;
		if (Physics.Raycast(aimRay, out hitInfo, float.PositiveInfinity))
		{
			hitInfo.collider.gameObject.SendMessage("selected");
			if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				hitInfo.collider.gameObject.SendMessage("clicked");
			}
		}
		else
		{
			base.gameObject.BroadcastMessage("unSelected", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void getPos(Vector2 pos)
	{
		x = pos.x;
		y = pos.y;
	}
}
