using UnityEngine;

public class BearControlsThirdPersonStyle1 : BearMovementController
{
	private Transform bearTrans;

	public Transform rotator;

	private float turnSpeed = 180f;

	public float minTilt = -15f;

	public float maxTilt = 30f;

	private float tiltFactor = 18f;

	public float deadZoneAmount = 0.2f;

	private Vector2 initSlidePos;

	private Vector2 curSlidePos;

	private Vector3 sliderPosition;

	public float sensitivityJoystick = 0.7f;

	public Transform weaponDir;

	private void Start()
	{
		controlls.Initialize();
		bearTrans = base.transform;
		if (GameManager.Instance.currentCharacter == Character.RIGGS)
		{
			tiltFactor -= 7f;
		}
	}

	private void Update()
	{
		bearMovement(controlls.move());
		bearTurning(controlls.aim() * sensitivityJoystick);
		bearAiming(controlls.aim() * sensitivityJoystick);
		bearFiring(controlls.fire());
	}

	public override void bearMovement(Vector2 Vec)
	{
		Vector3 vector = Vec.x * bearTrans.right;
		Vector3 vector2 = Vec.y * bearTrans.forward;
		Vector3 move = vector + vector2;
		float magnitude = Vec.magnitude;
		player.moveBear(move, magnitude);
	}

	public override void bearTurning(Vector2 Vec)
	{
		Vector3 zero = Vector3.zero;
		zero.y = Vec.x;
		player.rotateBear(zero, turnSpeed, false);
	}

	public void bearAiming(Vector2 Vec)
	{
		Vector3 zero = Vector3.zero;
		zero.x = Vec.y;
	}

	public override void bearFiring(bool fire)
	{
		if (!fire)
		{
			player.stopShooting();
		}
		else
		{
			player.startShooting();
		}
	}
}
