using UnityEngine;

public class MWPowerBarManager : MonoBehaviour
{
	public enum RateType
	{
		ByPercent,
		OverNumSeconds
	}

	public GameObject[] redBlock;

	public GameObject[] greenBlock;

	public GameObject redArrow;

	public float pointThreshold;

	private float arrowPos;

	private int rBIndex;

	private int gBIndex;

	private int state;

	private string stateName;

	public RateType rateType;

	public float rate;

	private float fillUpTime;

	private static MWPowerBarManager instance;

	public static MWPowerBarManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		state = 0;
		rBIndex = 0;
		gBIndex = greenBlock.Length - 1;
		if (rateType == RateType.ByPercent)
		{
			GameObject[] array = greenBlock;
			foreach (GameObject gameObject in array)
			{
				gameObject.GetComponent<MWPowerMeter>().SetRatePercent(rate);
			}
		}
		else if (rateType == RateType.OverNumSeconds)
		{
			GameObject[] array2 = greenBlock;
			foreach (GameObject gameObject2 in array2)
			{
				gameObject2.GetComponent<MWPowerMeter>().SetRateTime(rate / (float)greenBlock.Length);
			}
		}
	}

	public void InitializeRedBlocks()
	{
		rBIndex = 0;
		for (int i = 0; i < redBlock.Length; i++)
		{
			//redBlock[i].active = false;
			redBlock[i].SetActive(false);
		}
	}

	public void InitializeGreenBlocks()
	{
		gBIndex = greenBlock.Length - 1;
		for (int i = 0; i < greenBlock.Length; i++)
		{
			greenBlock[i].GetComponent<MWPowerMeter>().Reset();
		}
	}

	public void BeginComboAttack()
	{
		InitializeRedBlocks();
		greenBlock[gBIndex].GetComponent<MWPowerMeter>().state = MWPowerMeter.MWMeterState.DRAIN;
	}

	public void GetHit(float amount)
	{
		state = 1;
		greenBlock[gBIndex].GetComponent<MWPowerMeter>().Impulse(amount);
		greenBlock[gBIndex].GetComponent<MWPowerMeter>().state = MWPowerMeter.MWMeterState.DRAIN;
	}

	private void Update()
	{
		if (GameManager.Instance.hudController.scoreVal > (float)(rBIndex + 1) * pointThreshold)
		{
			addRedBlock();
		}
		/*switch (state)
		{
		}*/
		if (state == 2 && Time.time > fillUpTime + 1f)
		{
			state = 1;
			greenBlock[gBIndex].GetComponent<MWPowerMeter>().state = MWPowerMeter.MWMeterState.DRAIN;
		}
	}

	public void fillUp(float amount)
	{
		state = 2;
		MWPowerMeter component = greenBlock[gBIndex].GetComponent<MWPowerMeter>();
		component.state = MWPowerMeter.MWMeterState.FILL;
		component.Impulse(amount);
		fillUpTime = Time.time;
	}

	public void setRedBlock()
	{
		for (int i = rBIndex; i < redBlock.Length; i++)
		{
			//redBlock[i].active = false;
			redBlock[i].SetActive(false);
		}
	}

	public void addRedBlock()
	{
		if (rBIndex >= redBlock.Length)
		{
			rBIndex = redBlock.Length - 1;
		}
		//redBlock[rBIndex].active = true;
		redBlock[rBIndex].SetActive(true);
		rBIndex++;
	}

	public void subRedBlock()
	{
		rBIndex--;
		if (rBIndex < 0)
		{
			rBIndex = 0;
		}
		//redBlock[rBIndex].active = false;
		redBlock[rBIndex].SetActive(false);
	}

	public void PM_Empty(MWPowerMeter current)
	{
		gBIndex--;
		if (gBIndex < rBIndex)
		{
			gBIndex++;
			state = 0;
			lose();
		}
		else
		{
			greenBlock[gBIndex].GetComponent<MWPowerMeter>().state = MWPowerMeter.MWMeterState.DRAIN;
		}
	}

	public void PM_Full(MWPowerMeter current)
	{
		gBIndex++;
		if (gBIndex >= greenBlock.Length)
		{
			state = 0;
			gBIndex--;
		}
		greenBlock[gBIndex].GetComponent<MWPowerMeter>().state = MWPowerMeter.MWMeterState.FILL;
	}

	private void lose()
	{
		rBIndex = 0;
		StartCoroutine(GameManager.Instance.playerController.afterAction());
	}

	public void Reset()
	{
		InitializeRedBlocks();
		InitializeGreenBlocks();
	}
}
