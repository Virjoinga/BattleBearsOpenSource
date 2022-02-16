using UnityEngine;

public class GUISlider : MonoBehaviour
{
	public GameObject ScrollSlider;

	public Transform ScrollSliderTransform;

	public LayerMask sliderMask;

	public LayerMask sliderGrabMask;

	public float sliderMin = -83f;

	public float sliderMax = 120f;

	private Camera sliderCamera;

	private Transform currentSlider;

	private Controlls controlls;

	private void Start()
	{
		controlls = GameObject.Find("Controlls").GetComponent<Controlls>();
		GameObject gameObject = GameObject.Find("MenuCamera");
		if (gameObject == null)
		{
			sliderCamera = base.transform.parent.Find("Camera").GetComponent<Camera>();
		}
		else
		{
			sliderCamera = gameObject.GetComponent<Camera>();
		}
	}

	private void Update()
	{
		bool flag = false;
		if (controlls.comp)
		{
			flag = true;
		}
		else if (Input.touches.Length != 0)
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		Ray ray = sliderCamera.ScreenPointToRay(controlls.pos());
		RaycastHit hitInfo;
		if (currentSlider == null && controlls.began() && Physics.Raycast(ray.origin, ray.direction, out hitInfo, 2000f, sliderGrabMask))
		{
			if (hitInfo.transform == ScrollSliderTransform)
			{
				currentSlider = hitInfo.transform;
			}
			else
			{
				currentSlider = null;
			}
		}
		if (currentSlider != null && controlls.moved() && Physics.Raycast(ray.origin, ray.direction, out hitInfo, 10000f, sliderMask))
		{
			GameObject obj = hitInfo.transform.gameObject;
			Vector3 point = hitInfo.point;
			if (obj == ScrollSlider && currentSlider == ScrollSliderTransform)
			{
				Vector3 position = ScrollSliderTransform.position;
				position.z = point.z;
				ScrollSliderTransform.position = position;
				Debug.Log(position.z);
			}
		}
		else if (currentSlider != null && controlls.ended())
		{
			GameObject gameObject = Object.Instantiate(currentSlider.gameObject);
			gameObject.transform.parent = currentSlider.parent;
			gameObject.transform.position = currentSlider.position;
			gameObject.transform.localEulerAngles = currentSlider.localEulerAngles;
			gameObject.name = currentSlider.gameObject.name;
			Object.Destroy(currentSlider.gameObject);
			ScrollSliderTransform = gameObject.transform;
			currentSlider = null;
		}
	}
}
