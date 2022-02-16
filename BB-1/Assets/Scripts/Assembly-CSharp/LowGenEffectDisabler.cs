using UnityEngine;

public class LowGenEffectDisabler : MonoBehaviour
{
	public void Awake()
	{
		if (GameManager.Instance.currentGraphicsOption != GraphicsOption.ON && GetComponent<ParticleSystem>() != null)
		{
			Object.Destroy(GetComponent<ParticleSystem>());
		}
		Object.Destroy(this);
	}
}
