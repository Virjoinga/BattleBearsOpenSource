using UnityEngine;

public class hudAdjuster : MonoBehaviour
{
	public Transform life;

	public Transform ammo;

	private Transform map;

	public Transform wep;

	public Transform score;

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
			life.position = new Vector3(-57.46706f, 0f, 0f);
			ammo.position = new Vector3(57.46706f, 0f, 0f);
			wep.position = new Vector3(38.87503f, 0f, 0.6001587f);
			score.position = new Vector3(197.9605f, 100f, -144.0256f);
			if (map != null)
			{
				map.transform.localScale = new Vector3(0.9f, 1f, 1f);
			}
		}
		else if (aspect > 1.2222222f && aspect < 1.5f)
		{
			life.position = new Vector3(-44.08948f, 0f, 0f);
			ammo.position = new Vector3(44.08948f, 0f, 0f);
			wep.position = new Vector3(24.57782f, 0f, 0f);
			score.position = new Vector3(212.2682f, 100f, -144.0256f);
		}
		else if ((double)aspect > 1.49 && (double)aspect < 1.58)
		{
			life.position = new Vector3(-17.6012f, 0f, 0f);
			ammo.position = new Vector3(17.6012f, 0f, 0f);
			wep.position = new Vector3(-1.565216f, 0f, 0f);
			score.position = new Vector3(238.6202f, 100f, -144.0256f);
		}
		else if ((double)aspect > 1.5 && (double)aspect < 1.777777)
		{
			life.position = new Vector3(-1.981232f, 0f, -1.661957f);
			ammo.position = new Vector3(1.981232f, 0f, -1.661957f);
			wep.position = new Vector3(-16.71887f, 0f, 5.740723f);
			score.position = new Vector3(256.0153f, 100f, -144.0256f);
		}
		else if (aspect > 1.6666666f)
		{
			life.position = new Vector3(26.27681f, 0f, 0f);
			ammo.position = new Vector3(-26.27681f, 0f, 0f);
			wep.position = new Vector3(-45.52672f, 0f, 0f);
			score.position = new Vector3(283.6631f, 100f, -144.0256f);
		}
		else
		{
			life.position = new Vector3(-1.981232f, 0f, -1.661957f);
			ammo.position = new Vector3(1.981232f, 0f, -1.661957f);
			wep.position = new Vector3(-16.71887f, 0f, 5.740723f);
			score.position = new Vector3(256.0153f, 100f, -144.0256f);
		}
	}

	public void getMiniMap(GameObject m)
	{
		map = m.transform;
	}
}
