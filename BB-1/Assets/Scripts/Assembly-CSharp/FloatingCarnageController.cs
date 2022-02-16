using UnityEngine;

public class FloatingCarnageController : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnEnable()
	{
		Animator component = GetComponent<Animator>();
		if (component != null)
		{
			component.Play(component.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, Random.Range(0f, 0.99f));
		}
	}
}
