using System.Collections;
using UnityEngine;

public class BossIntroController : MonoBehaviour
{
	private bool movieSet;

	public BossMode boss;

	public Transform playerPosition;

	private void Awake()
	{
		if (GameManager.Instance.currentGameMode != 0)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (GameManager.Instance.currentGameMode == GameMode.CAMPAIGN && !movieSet && c.CompareTag("Player"))
		{
			movieSet = true;
			Time.timeScale = 0f;
			if (boss == BossMode.MECHABEARZERKER)
			{
				GameManager.Instance.inOliverBossRoom = true;
				GameManager.Instance.PlayMovie("o_boss1_intro");
			}
			else if (boss == BossMode.TENTACLEESE)
			{
				GameManager.Instance.inRiggsBossRoom = true;
				GameManager.Instance.PlayMovie("r_boss1_intro");
			}
			StartCoroutine(delayedMovieActions(c));
		}
	}

	private IEnumerator delayedMovieActions(Collider c)
	{
		yield return new WaitForSeconds(0.25f);
		if (playerPosition != null)
		{
			c.transform.root.position = playerPosition.position;
			c.transform.root.eulerAngles = playerPosition.eulerAngles;
		}
		base.transform.root.BroadcastMessage("OnBossSpawn");
		Component[] componentsInChildren = base.transform.root.GetComponentsInChildren(typeof(BossIntroController));
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.Destroy(componentsInChildren[i].gameObject);
		}
	}
}
