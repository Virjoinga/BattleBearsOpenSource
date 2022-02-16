using UnityEngine;

public class TargetController : MonoBehaviour
{
	private bool hit;

	private float dmg;

	private bool isDead;

	private void Awake()
	{
		hit = false;
		isDead = false;
	}

	public void OnHit(float damage)
	{
		if (!hit)
		{
			dmg = damage;
			hit = true;
		}
	}

	private void Update()
	{
		if (hit)
		{
			base.transform.localScale += Vector3.one * dmg * 0.01f;
			if (base.transform.localScale.x >= 2f && !isDead)
			{
				isDead = true;
				GameManager.Instance.isLevel = true;
				SendMessageUpwards("ButtonPress", base.gameObject.name);
			}
			hit = false;
		}
		else if (base.transform.localScale.x > 1f)
		{
			base.transform.localScale -= Vector3.one * 2f * 0.01f;
		}
	}
}
