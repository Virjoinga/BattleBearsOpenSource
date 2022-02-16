using UnityEngine;

public class PlayerAnimationTester : MonoBehaviour
{
	public string[] walkAnimations;

	public string[] armAnimations;

	private Animation myAnimator;

	public string walkAnimation;

	public string armAnimation;

	private int walkAnimationIndex;

	private int armAnimationIndex;

	private void Awake()
	{
		myAnimator = GetComponent<Animation>();
		for (int i = 0; i < walkAnimations.Length; i++)
		{
			myAnimator[walkAnimations[i]].layer = 0;
			myAnimator[walkAnimations[i]].wrapMode = WrapMode.Loop;
		}
		for (int j = 0; j < armAnimations.Length; j++)
		{
			myAnimator[armAnimations[j]].layer = 1;
			myAnimator[armAnimations[j]].wrapMode = WrapMode.Loop;
		}
		if (walkAnimation != "")
		{
			myAnimator[walkAnimation].layer = 0;
			myAnimator[walkAnimation].wrapMode = WrapMode.Loop;
			myAnimator.CrossFade(walkAnimation);
		}
		if (armAnimation != "")
		{
			myAnimator[armAnimation].layer = 1;
			myAnimator[armAnimation].wrapMode = WrapMode.Loop;
			myAnimator.CrossFade(armAnimation);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			walkAnimationIndex++;
			if (walkAnimationIndex >= walkAnimations.Length)
			{
				walkAnimationIndex = 0;
			}
			walkAnimation = walkAnimations[walkAnimationIndex];
			myAnimator.wrapMode = WrapMode.Loop;
			myAnimator.CrossFade(walkAnimations[walkAnimationIndex]);
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			armAnimationIndex++;
			if (armAnimationIndex >= armAnimations.Length)
			{
				armAnimationIndex = 0;
			}
			armAnimation = armAnimations[armAnimationIndex];
			myAnimator.wrapMode = WrapMode.Loop;
			myAnimator.CrossFade(armAnimations[armAnimationIndex]);
		}
	}
}
