using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
	public GameObject[] powerup_Rings;

	public int LEFT_SIDE = -1;

	public int RIGHT_SIDE = 1;

	private Dictionary<Nuggs, int> powerups;

	private float tLeft;

	private float tRight;

	private GameObject left;

	private GameObject right;

	private Nuggs pLeft;

	private Nuggs pRight;

	private static PowerupManager instance;

	public static PowerupManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void Start()
	{
		instance = this;
		powerups = new Dictionary<Nuggs, int>();
		powerups.Add(Nuggs.FREEZE, 0);
		powerups.Add(Nuggs.MULTIPLIER, 1);
		powerups.Add(Nuggs.WHITE, 4);
	}

	private void Update()
	{
		if (GameManager.Instance.isPaused && GameManager.Instance.isLevel)
		{
			if (left != null)
			{
				//left.active = false;
				left.SetActive(false);
			}
			if (right != null)
			{
				//right.active = false;
				right.SetActive(false);
			}
		}
		else if (GameManager.Instance.isLevel)
		{
			if (left != null)
			{
				//left.active = true;
				left.SetActive(true);
			}
			if (right != null)
			{
				//right.active = true;
				right.SetActive(true);
			}
		}
	}

	private void newPowerup(ref GameObject side, Nuggs type, int scale)
	{
		if (!GameManager.Instance.isOver)
		{
			side = Object.Instantiate(powerup_Rings[powerups[type]], GameObject.Find("Crosshair_Armed").transform.position, GameObject.Find("Crosshair_Armed").transform.rotation) as GameObject;
			side.GetComponent<PowerupMeter>().type = type;
			side.transform.parent = GameObject.Find("Crosshair_Armed").transform;
			side.transform.localScale = new Vector3(scale, 1f, 1f);
		}
	}

	public void create(Nuggs pUpName)
	{
		switch (pUpName)
		{
		case Nuggs.FREEZE:
			if (left == null)
			{
				if (right != null && right.GetComponent<PowerupMeter>().type == Nuggs.FREEZE)
				{
					tRight = Time.time;
					right.GetComponent<PowerupMeter>().Kill();
					newPowerup(ref right, Nuggs.FREEZE, RIGHT_SIDE);
				}
				else
				{
					tLeft = Time.time;
					newPowerup(ref left, Nuggs.FREEZE, LEFT_SIDE);
				}
			}
			else if (right == null && left.GetComponent<PowerupMeter>().type != Nuggs.FREEZE)
			{
				if (left != null && left.GetComponent<PowerupMeter>().type == Nuggs.FREEZE)
				{
					tLeft = Time.time;
					left.GetComponent<PowerupMeter>().Kill();
					newPowerup(ref left, Nuggs.FREEZE, LEFT_SIDE);
				}
				else
				{
					newPowerup(ref right, Nuggs.FREEZE, RIGHT_SIDE);
				}
			}
			else if (left.GetComponent<PowerupMeter>().type == Nuggs.FREEZE)
			{
				tLeft = Time.time;
				left.GetComponent<PowerupMeter>().Kill();
				newPowerup(ref left, Nuggs.FREEZE, LEFT_SIDE);
			}
			else if (right.GetComponent<PowerupMeter>().type == Nuggs.FREEZE)
			{
				tRight = Time.time;
				right.GetComponent<PowerupMeter>().Kill();
				newPowerup(ref right, Nuggs.FREEZE, RIGHT_SIDE);
			}
			else if (tLeft < tRight && left != null)
			{
				tLeft = Time.time;
				left.GetComponent<PowerupMeter>().Kill();
				newPowerup(ref left, Nuggs.FREEZE, LEFT_SIDE);
			}
			else if (tRight < tLeft && right != null)
			{
				tRight = Time.time;
				right.GetComponent<PowerupMeter>().Kill();
				newPowerup(ref right, Nuggs.FREEZE, RIGHT_SIDE);
			}
			break;
		case Nuggs.MULTIPLIER:
			GameManager.Instance.multiplier += 2f;
			if (left == null)
			{
				tLeft = Time.time;
				newPowerup(ref left, Nuggs.MULTIPLIER, LEFT_SIDE);
			}
			else if (right == null)
			{
				tRight = Time.time;
				newPowerup(ref right, Nuggs.MULTIPLIER, RIGHT_SIDE);
			}
			else if (tLeft < tRight)
			{
				tLeft = Time.time;
				left.GetComponent<PowerupMeter>().Kill();
				newPowerup(ref left, Nuggs.MULTIPLIER, LEFT_SIDE);
			}
			else if (tRight < tLeft)
			{
				tRight = Time.time;
				right.GetComponent<PowerupMeter>().Kill();
				newPowerup(ref right, Nuggs.MULTIPLIER, RIGHT_SIDE);
			}
			break;
		case Nuggs.WHITE:
			if (left == null)
			{
				if (right != null && right.GetComponent<PowerupMeter>().type == Nuggs.WHITE)
				{
					tRight = Time.time;
					right.GetComponent<PowerupMeter>().Kill();
					newPowerup(ref right, Nuggs.WHITE, RIGHT_SIDE);
				}
				else
				{
					tLeft = Time.time;
					newPowerup(ref left, Nuggs.WHITE, LEFT_SIDE);
				}
			}
			else if (right == null && left.GetComponent<PowerupMeter>().type != Nuggs.WHITE)
			{
				if (left != null && left.GetComponent<PowerupMeter>().type == Nuggs.WHITE)
				{
					tLeft = Time.time;
					left.GetComponent<PowerupMeter>().Kill();
					newPowerup(ref left, Nuggs.WHITE, LEFT_SIDE);
				}
				else
				{
					newPowerup(ref right, Nuggs.WHITE, RIGHT_SIDE);
				}
			}
			else if (left.GetComponent<PowerupMeter>().type == Nuggs.WHITE)
			{
				tLeft = Time.time;
				left.GetComponent<PowerupMeter>().Kill();
				newPowerup(ref left, Nuggs.WHITE, LEFT_SIDE);
			}
			else if (right.GetComponent<PowerupMeter>().type == Nuggs.WHITE)
			{
				tRight = Time.time;
				right.GetComponent<PowerupMeter>().Kill();
				newPowerup(ref right, Nuggs.WHITE, RIGHT_SIDE);
			}
			else if (tLeft < tRight && left != null)
			{
				tLeft = Time.time;
				left.GetComponent<PowerupMeter>().Kill();
				newPowerup(ref left, Nuggs.WHITE, LEFT_SIDE);
			}
			else if (tRight < tLeft && right != null)
			{
				tRight = Time.time;
				right.GetComponent<PowerupMeter>().Kill();
				newPowerup(ref right, Nuggs.WHITE, RIGHT_SIDE);
			}
			break;
		}
	}

	private void disable(GameObject go)
	{
		PowerupMeter component = go.GetComponent<PowerupMeter>();
		if (component.type == Nuggs.MULTIPLIER)
		{
			GameManager.Instance.multiplier -= 2f;
		}
		go.active = false;
	}

	public void reset()
	{
		if (left != null)
		{
			left.GetComponent<PowerupMeter>().Kill();
		}
		if (right != null)
		{
			right.GetComponent<PowerupMeter>().Kill();
		}
	}
}
