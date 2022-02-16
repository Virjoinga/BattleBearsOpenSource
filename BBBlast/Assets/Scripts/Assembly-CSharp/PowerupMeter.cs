using UnityEngine;

public class PowerupMeter : MonoBehaviour
{
	public float initUV;

	public float curUVpos;

	public float finalUV;

	public bool slide;

	public float percentPerSecond;

	public Nuggs type;

	private GameObject weatherEffect;

	private Mesh mesh;

	private Vector2[] uvs;

	private bool done;

	private bool doPPS = true;

	private float distance;

	private float pps;

	private float prevTime;

	private void Awake()
	{
	}

	private void Start()
	{
		initUV = 0.260354f;
		curUVpos = 0.260354f;
		finalUV = 0.321198f;
		distance = finalUV - initUV;
		mesh = GetComponent<MeshFilter>().mesh;
		uvs = mesh.uv;
		GameManager.Instance.PowerUp(type);
		if (type == Nuggs.FREEZE && GameManager.Instance.isHighEnd)
		{
			weatherEffect = Object.Instantiate(Resources.Load("FreezeSleet")) as GameObject;
		}
	}

	private void Update()
	{
		if (doPPS && !GameManager.Instance.isPaused)
		{
			if (slide && curUVpos <= finalUV)
			{
				drain(Time.deltaTime * percentPerSecond * 0.01f * distance);
			}
		}
		else if (!GameManager.Instance.isPaused)
		{
		}
		if (curUVpos >= finalUV)
		{
			GameManager.Instance.PowerDown(type);
			SendMessageUpwards("disable", base.gameObject);
			Object.Destroy(weatherEffect);
			Object.Destroy(base.gameObject);
		}
	}

	private void displayUVCoords()
	{
		for (int i = 0; i < uvs.Length; i++)
		{
		}
	}

	private void drain(float amount)
	{
		for (int i = 0; i < uvs.Length; i++)
		{
			uvs[i] = new Vector2(uvs[i].x, uvs[i].y + amount);
		}
		mesh.uv = uvs;
		curUVpos += amount;
	}

	public void reset()
	{
	}

	public void Kill()
	{
		GameManager.Instance.PowerDown(type);
		SendMessageUpwards("disable", base.gameObject);
		if (weatherEffect != null)
		{
			Object.Destroy(weatherEffect);
		}
		Object.Destroy(base.gameObject);
	}
}
