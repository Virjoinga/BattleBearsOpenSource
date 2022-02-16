using UnityEngine;

public class NewOptions : MonoBehaviour
{
	public TextMesh ExtraGraphicsText;

	public TextMesh InvertAimText;

	public TextMesh SensitivityValueText;

	public GameObject OpenFeintPressed;

	public GameObject ExtraGraphicsPressed;

	public GameObject InvertAimPressed;

	private bool isSlider;

	public Transform sliderTransform;

	public Camera hudCamera;

	public LayerMask layerMask;

	private void Start()
	{
		if (!PlayerPrefs.HasKey("InvertAim"))
		{
			PlayerPrefs.SetInt("InvertAim", 0);
		}
		if (!PlayerPrefs.HasKey("Sensitivity"))
		{
			PlayerPrefs.SetFloat("Sensitivity", 0.5f);
		}
		if (GameManager.Instance.currentGraphicsOption == GraphicsOption.OFF)
		{
			ExtraGraphicsText.text = "OFF";
		}
		else
		{
			ExtraGraphicsText.text = "ON";
		}
		if (PlayerPrefs.GetInt("InvertAim") == 0)
		{
			InvertAimText.text = "OFF";
		}
		else
		{
			InvertAimText.text = "ON";
		}
		OpenFeintPressed.SetActiveRecursively(false);
		ExtraGraphicsPressed.SetActiveRecursively(false);
		InvertAimPressed.SetActiveRecursively(false);
		if (Application.loadedLevel == 1)
		{
			hudCamera = GameObject.Find("MenuCamera").GetComponent("Camera") as Camera;
		}
		else
		{
			hudCamera = GameObject.Find("HUD/Camera").GetComponent("Camera") as Camera;
		}
		SensitivityValueText.text = string.Concat((int)(PlayerPrefs.GetFloat("Sensitivity") * 100f));
		sliderTransform.position = new Vector3(sliderTransform.position.x, sliderTransform.position.y, PlayerPrefs.GetFloat("Sensitivity") * -134.99997f + 90f + 1.376572f - 90f);
	}

	private void Update()
	{
		if (Input.touchCount == 0)
		{
			OpenFeintPressed.SetActiveRecursively(false);
			ExtraGraphicsPressed.SetActiveRecursively(false);
			InvertAimPressed.SetActiveRecursively(false);
		}
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			Ray ray = hudCamera.ScreenPointToRay(new Vector3(Input.touches[i].position.x, Input.touches[i].position.y, 0f));
			RaycastHit hitInfo;
			if (touch.phase == TouchPhase.Began)
			{
				if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, 2000f, layerMask))
				{
					if (hitInfo.collider.name == "up 2")
					{
						OpenFeintPressed.SetActiveRecursively(true);
					}
					else if (hitInfo.collider.name == "up")
					{
						ExtraGraphicsPressed.SetActiveRecursively(true);
					}
					else if (hitInfo.collider.name == "up 1")
					{
						InvertAimPressed.SetActiveRecursively(true);
					}
					else if (hitInfo.collider.name == "Backers4")
					{
						isSlider = true;
					}
				}
			}
			else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
			{
				if (!Physics.Raycast(ray.origin, ray.direction, out hitInfo, 2000f, layerMask))
				{
					continue;
				}
				if (hitInfo.collider.name != "up 2")
				{
					OpenFeintPressed.SetActiveRecursively(false);
				}
				if (hitInfo.collider.name != "up")
				{
					ExtraGraphicsPressed.SetActiveRecursively(false);
				}
				if (hitInfo.collider.name != "up 1")
				{
					InvertAimPressed.SetActiveRecursively(false);
				}
				if (isSlider)
				{
					Vector3 point = hitInfo.point;
					if (point.z - 90f > 1.376572f)
					{
						point.z = 91.37657f;
					}
					if (point.z - 90f < -133.6234f)
					{
						point.z = -43.623398f;
					}
					PlayerPrefs.SetFloat("Sensitivity", (point.z - 90f - 1.376572f) / -134.99997f);
					SensitivityValueText.text = string.Concat((int)(PlayerPrefs.GetFloat("Sensitivity") * 100f));
					sliderTransform.position = new Vector3(sliderTransform.position.x, sliderTransform.position.y, point.z - 90f);
				}
			}
			else
			{
				if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
				{
					continue;
				}
				isSlider = false;
				if (!Physics.Raycast(ray.origin, ray.direction, out hitInfo, 2000f, layerMask))
				{
					continue;
				}
				if (hitInfo.collider.name == "up 2")
				{
					if (!OpenFeintPressed.active)
					{
					}
				}
				else if (hitInfo.collider.name == "up")
				{
					if (ExtraGraphicsPressed.active)
					{
						ExtraGraphicsPressed.SetActiveRecursively(false);
						if (GameManager.Instance.currentGraphicsOption == GraphicsOption.OFF)
						{
							GameManager.Instance.currentGraphicsOption = GraphicsOption.ON;
							ExtraGraphicsText.text = "ON";
						}
						else
						{
							GameManager.Instance.currentGraphicsOption = GraphicsOption.OFF;
							ExtraGraphicsText.text = "OFF";
						}
					}
				}
				else if (hitInfo.collider.name == "up 1")
				{
					if (InvertAimPressed.active)
					{
						InvertAimPressed.SetActiveRecursively(false);
						if (PlayerPrefs.GetInt("InvertAim") == 0)
						{
							InvertAimText.text = "ON";
							PlayerPrefs.SetInt("InvertAim", 1);
						}
						else
						{
							InvertAimText.text = "OFF";
							PlayerPrefs.SetInt("InvertAim", 0);
						}
					}
				}
				else
				{
					bool flag = hitInfo.collider.name == "Backers4";
				}
			}
		}
	}
}
