using System.Collections;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
	private float score;

	private float killStreak;

	public float multiplyer = 1f;

	private float temp = 5f;

	private float num;

	public void updateScore(float val)
	{
		killStreak += 1f;
		if (killStreak > 40f)
		{
			multiplyer = 5f;
		}
		else if (killStreak > 30f)
		{
			multiplyer = 4f;
		}
		else if (killStreak > 20f)
		{
			multiplyer = 3f;
		}
		else if (killStreak > 10f)
		{
			multiplyer = 2f;
		}
		else
		{
			multiplyer = 1f;
		}
		score += val;
		GameManager.Instance.hudController.updateScore(score);
		num += 1f;
		StopCoroutine("downCount");
		StartCoroutine("downCount");
	}

	public float getScore()
	{
		return score;
	}

	public void resetBonus()
	{
		killStreak = 0f;
		multiplyer = 1f;
	}

	public void subtractScore(float val)
	{
		score += -1f * val;
		killStreak = 0f;
		multiplyer = 1f;
	}

	private IEnumerator downCount()
	{
		if (multiplyer == 5f)
		{
			temp = 1.5f;
		}
		if (multiplyer == 4f)
		{
			temp = 2.5f;
		}
		if (multiplyer == 3f)
		{
			temp = 4f;
		}
		if (multiplyer <= 2f)
		{
			temp = 5f;
		}
		yield return new WaitForSeconds(temp);
		if (killStreak < 40f)
		{
			killStreak -= 10f;
		}
		else
		{
			killStreak = 40f;
			killStreak -= 10f;
		}
		multiplyer -= 1f;
	}

	public void resetScore()
	{
		score = 0f;
		killStreak = 0f;
		multiplyer = 1f;
		temp = 1f;
	}
}
