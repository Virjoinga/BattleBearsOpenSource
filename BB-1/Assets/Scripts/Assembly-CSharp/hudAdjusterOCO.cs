using UnityEngine;

public class hudAdjusterOCO : MonoBehaviour
{
	public Transform sc;

	public Transform time;

	public Transform lazor;

	public Transform UM;

	private float aspect;

	private void Update()
	{
		float num = Screen.height;
		float num2 = Screen.width;
		aspect = num2 / num;
		adjust();
	}

	private void adjust()
	{
		if (aspect > 1.24f && aspect < 1.3333334f)
		{
			sc.position = new Vector3(45.74724f, 0f, -3.992725f);
			time.position = new Vector3(15.94177f, 0f, -5.720814f);
			lazor.position = new Vector3(41.48502f, 0f, 60.4624f);
			UM.position = new Vector3(-46.61166f, 0f, 0f);
		}
		else if (aspect > 1.2222222f && aspect < 1.5f)
		{
			sc.position = new Vector3(31.83797f, 0f, -3.992725f);
			time.position = new Vector3(2.032501f, 0f, -5.720814f);
			lazor.position = new Vector3(27.57574f, 0f, 60.4624f);
			UM.position = new Vector3(-33.15745f, 0f, 0f);
		}
		else if ((double)aspect > 1.49 && (double)aspect < 1.58)
		{
			sc.position = new Vector3(4.619736f, 0f, -3.992725f);
			time.position = new Vector3(-25.18573f, 0f, -5.720814f);
			lazor.position = new Vector3(0.3575134f, 0f, 60.4624f);
			UM.position = new Vector3(-7.254662f, 0f, 0f);
		}
		else if ((double)aspect > 1.5 && (double)aspect < 1.777777)
		{
			sc.position = new Vector3(-10.30899f, 0f, -3.992725f);
			time.position = new Vector3(-40.11446f, 0f, -5.720814f);
			lazor.position = new Vector3(-14.57121f, 0f, 60.4624f);
			UM.position = new Vector3(9.599434f, 0f, 0f);
		}
		else if (aspect > 1.6666666f)
		{
			sc.position = new Vector3(-38.52612f, 0f, -3.992725f);
			time.position = new Vector3(-68.33159f, 0f, -5.720814f);
			lazor.position = new Vector3(-42.78835f, 0f, 60.4624f);
			UM.position = new Vector3(37.88467f, 0f, 0f);
		}
		else
		{
			sc.position = new Vector3(-10.30899f, 0f, -3.992725f);
			time.position = new Vector3(-40.11446f, 0f, -5.720814f);
			lazor.position = new Vector3(-14.57121f, 0f, 60.4624f);
			UM.position = new Vector3(9.599434f, 0f, 0f);
		}
	}

	public void getMiniMap(GameObject m)
	{
	}
}
