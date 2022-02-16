using UnityEngine;

public class FPSController : MonoBehaviour
{
	private Camera hudCamera;

	public LayerMask layerMask;

	public Touch t0;

	public Touch t1;

	public GameObject followCam;

	public GameObject followCamfps;

	public Transform rotator;

	public void Update()
	{
		fps();
	}

	public void Awake()
	{
		hudCamera = GameObject.Find("HUD/Camera").GetComponent(typeof(Camera)) as Camera;
		followCam.SetActiveRecursively(true);
		followCamfps.SetActiveRecursively(false);
		GameManager.Instance.FPSMode = false;
	}

	public void fps()
	{
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			Ray ray = hudCamera.ScreenPointToRay(new Vector3(Input.touches[i].position.x, Input.touches[i].position.y, 0f));
			RaycastHit hitInfo;
			if (touch.position.x < 340f && touch.position.x > 140f && Physics.Raycast(ray.origin, ray.direction, out hitInfo, 2000f, layerMask.value) && (hitInfo.collider.name != "Rotate_Plate" || hitInfo.collider.name != "Move_Plate"))
			{
				if (i == 0)
				{
					t0 = Input.GetTouch(i);
				}
				if (i == 1)
				{
					t1 = Input.GetTouch(i);
				}
			}
		}
		if (Input.touchCount == 2)
		{
			if (t0.deltaPosition.y + t1.deltaPosition.y > 90f)
			{
				followCam.SetActiveRecursively(false);
				followCamfps.SetActiveRecursively(true);
				rotator.eulerAngles = new Vector3(0f, rotator.eulerAngles.y, 0f);
				GameManager.Instance.FPSMode = true;
			}
			else if (t0.deltaPosition.y + t1.deltaPosition.y < -100f)
			{
				followCam.SetActiveRecursively(true);
				followCamfps.SetActiveRecursively(false);
				GameManager.Instance.FPSMode = false;
				rotator.eulerAngles = new Vector3(0f, rotator.eulerAngles.y, 0f);
			}
		}
	}
}
