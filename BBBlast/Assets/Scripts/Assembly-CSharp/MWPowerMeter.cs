using UnityEngine;

public class MWPowerMeter : MonoBehaviour
{
	public enum MWMeterState
	{
		OFF,
		DRAIN,
		FILL
	}

	public MWMeterState state;

	public float offset;

	public float initUV;

	public float finalUV;

	public bool drainVertical;

	private Mesh mesh;

	private Vector2 minUVXY;

	private Vector2 maxUVXY;

	private Vector2[] uvs;

	private float distance;

	public float rate;

	private float impAmount;

	private float hitAmount;

	private float leftOver;

	private float impTime;

	private void Start()
	{
		state = MWMeterState.OFF;
		mesh = GetComponent<MeshFilter>().mesh;
		uvs = mesh.uv;
		minUVXY = FindSmallestUVCoord(uvs);
		maxUVXY = FindLargestUVCoord(uvs);
		offset = 0f;
		if (drainVertical)
		{
			initUV = minUVXY.y;
			finalUV = maxUVXY.y;
		}
		else
		{
			initUV = minUVXY.x;
			finalUV = maxUVXY.x;
		}
		impAmount = 0f;
		distance = finalUV - initUV + 0.001f;
	}

	private void Update()
	{
		float amount = 0f;
		if (impAmount != 0f)
		{
			if (Time.time < impTime + 0.1f)
			{
				amount = (rate + impAmount) * Time.deltaTime;
			}
			else
			{
				impAmount = 0f;
			}
		}
		else
		{
			amount = rate * Time.deltaTime;
		}
		switch (state)
		{
		case MWMeterState.OFF:
			break;
		case MWMeterState.DRAIN:
			Drain(amount);
			break;
		case MWMeterState.FILL:
			Fill(amount);
			break;
		}
	}

	private void Drain(float amount)
	{
		Vector2[] uv = mesh.uv;
		offset += amount;
		if (offset > distance)
		{
			offset = distance;
			state = MWMeterState.OFF;
			SendMessageUpwards("PM_Empty", this);
			return;
		}
		for (int i = 0; i < uvs.Length; i++)
		{
			if (drainVertical)
			{
				uv[i] = new Vector2(uvs[i].x, uvs[i].y - offset);
			}
			else
			{
				mesh.uv[i] = new Vector2(uvs[i].x - offset, uvs[i].y);
			}
		}
		mesh.uv = uv;
	}

	private void Fill(float amount)
	{
		Vector2[] uv = mesh.uv;
		offset -= amount;
		if (offset < 0f)
		{
			offset = 0f;
			state = MWMeterState.OFF;
			SendMessageUpwards("PM_Full", this);
			return;
		}
		for (int i = 0; i < uvs.Length; i++)
		{
			if (drainVertical)
			{
				uv[i] = new Vector2(uvs[i].x, uvs[i].y - offset);
			}
			else
			{
				mesh.uv[i] = new Vector2(uvs[i].x + offset, uvs[i].y);
			}
		}
		mesh.uv = uv;
	}

	public void Impulse(float percentPerSecond)
	{
		impAmount = percentPerSecond / 100f * distance;
		impTime = Time.time;
	}

	public void SetRatePercent(float percentPerSecond)
	{
		rate = percentPerSecond / 100f * distance;
	}

	public void SetRateTime(float timeTakenToCompletelyDrainOrFill)
	{
		SetRatePercent(100f / timeTakenToCompletelyDrainOrFill);
	}

	public void Reset()
	{
		mesh.uv = uvs;
	}

	private Vector2 FindSmallestUVCoord(Vector2[] UVArray)
	{
		float num = 1f;
		float num2 = 1f;
		for (int i = 0; i < UVArray.Length; i++)
		{
			Vector2 vector = UVArray[i];
			if (vector.x < num)
			{
				num = vector.x;
			}
			if (vector.y < num2)
			{
				num2 = vector.y;
			}
		}
		return new Vector2(num, num2);
	}

	private Vector2 FindLargestUVCoord(Vector2[] UVArray)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < UVArray.Length; i++)
		{
			Vector2 vector = UVArray[i];
			if (vector.x > num)
			{
				num = vector.x;
			}
			if (vector.y > num2)
			{
				num2 = vector.y;
			}
		}
		return new Vector2(num, num2);
	}

	private void InitializeUVs()
	{
		for (int i = 0; i < uvs.Length; i++)
		{
			mesh.uv[i] = new Vector2(mesh.uv[i].x, mesh.uv[i].y + (finalUV - initUV));
		}
	}

	private void DisplayUVCoords()
	{
		for (int i = 0; i < mesh.uv.Length; i++)
		{
		}
	}
}
