using UnityEngine;

public class BearMovementController : MonoBehaviour
{
	protected GenericCharacterController player;

	public Vector2 leftVec;

	public Vector2 rightVec;

	public Controlls controlls;

	private void Awake()
	{
		controlls = GameObject.Find("Controlls").GetComponent<Controlls>();
		player = GetComponent(typeof(GenericCharacterController)) as GenericCharacterController;
	}

	public virtual void bearMovement(Vector2 Vec)
	{
	}

	public virtual void bearTurning(Vector2 Vec)
	{
	}

	public virtual void bearFiring(bool fire)
	{
	}

	public virtual void startUp()
	{
	}
}
