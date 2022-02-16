using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
	private Transform myTransform;

	private float currentTurnSpeed = 10f;

	private float currentMoveSpeed = 10f;

	private bool kickout;

	private Vector3 tar;

	private float startingY;

	private void Awake()
	{
		myTransform = base.transform;
		kickout = true;
	}

	public void doneMoving()
	{
		myTransform.SendMessage("OffAutoPilot", SendMessageOptions.DontRequireReceiver);
		Object.Destroy(this);
	}

	public void move(Vector3 target)
	{
		startingY = myTransform.position.y + 0.1f;
		myTransform.SendMessage("OnAutoPilot", SendMessageOptions.DontRequireReceiver);
		tar = target;
		kickout = false;
	}

	public void move(Vector3 target, float moveSpeed, float turnSpeed)
	{
		currentMoveSpeed = moveSpeed;
		currentTurnSpeed = turnSpeed;
		move(target);
	}

	private void rotateTowards(Vector3 position)
	{
		Vector3 forward = position - myTransform.position;
		forward.y = 0f;
		if (!(forward.magnitude < 0.1f))
		{
			myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(forward), currentTurnSpeed * Time.deltaTime);
			myTransform.eulerAngles = new Vector3(0f, myTransform.eulerAngles.y, 0f);
		}
	}

	private bool moveTowards(Vector3 targetPos)
	{
		Vector3 position = myTransform.position;
		Vector3 vector = targetPos - position;
		vector.y = 0f;
		if (vector.magnitude < currentMoveSpeed * Time.deltaTime)
		{
			targetPos.y = startingY;
			myTransform.position = targetPos;
			return true;
		}
		vector.Normalize();
		position += vector * currentMoveSpeed * Time.deltaTime;
		position.y = startingY;
		myTransform.position = position;
		return false;
	}

	private void Update()
	{
		if (!kickout)
		{
			rotateTowards(tar);
			if (moveTowards(tar))
			{
				doneMoving();
			}
		}
	}
}
