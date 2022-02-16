using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public enum EMoveState
	{
		MoveIn,
		MoveOut
	}

	[SerializeField]
	private GameObject _targetObj;

	[SerializeField]
	private float _minRange = 3f;

	[SerializeField]
	private float _moveInSpeed = 10f;

	[SerializeField]
	private float _moveOutSpeed = 4f;

	private float DELAY_CHECK = 0.1f;

	private float _targetRange;

	private float _normalRange;

	private float _delayCheck;

	private float _delayCheckGoOut;

	private float _moveSpeed = 2f;

	private bool _updatePos;

	private Vector3 _localDir = Vector3.zero;

	private EMoveState _moveState;

	private GameObject _shieldObj;

	public void SetShieldObj(GameObject obj)
	{
		_shieldObj = obj;
		UpdateScaleShield();
	}

	private void UpdateScaleShield()
	{
		if (_shieldObj != null)
		{
			float num = 1f + 2f * (1f - (base.transform.position - _targetObj.transform.position).magnitude / _normalRange);
			_shieldObj.transform.localScale = new Vector3(num, num, num);
		}
	}

	private void Init()
	{
		if (_targetObj != null)
		{
			_normalRange = (base.transform.position - _targetObj.transform.position).magnitude;
			_normalRange = (float)Math.Round(_normalRange, 2);
			_minRange = ((_normalRange < _minRange) ? (_normalRange * 0.8f) : _minRange);
			Vector3 normalized = (base.transform.position - _targetObj.transform.position).normalized;
			_localDir = _targetObj.transform.InverseTransformVector(normalized);
			Vector3 vector = _targetObj.transform.TransformDirection(_localDir);
			Vector3 vector2 = _targetObj.transform.TransformVector(_localDir);
			Vector3 vector3 = _targetObj.transform.position + vector.normalized * _normalRange;
			Vector3 vector4 = _targetObj.transform.position + vector2.normalized * _normalRange;
		}
	}

	private void CheckCollisionWall()
	{
		if (!(_targetObj == null))
		{
			Vector3 position = _targetObj.transform.position;
			Vector3 direction = _targetObj.transform.TransformVector(_localDir);
			LayerMask layerMask = LayerMask.GetMask("Wall");
			float normalRange = _normalRange;
			RaycastHit hitInfo = default(RaycastHit);
			normalRange = ((!Physics.Raycast(position, direction, out hitInfo, _normalRange, layerMask)) ? _normalRange : ((!(hitInfo.distance < _minRange)) ? ((float)Math.Round(hitInfo.distance * 0.9f, 2)) : _minRange));
			float magnitude = (_targetObj.transform.position - base.transform.position).magnitude;
			magnitude = (float)Math.Round(magnitude, 2);
			if (normalRange < magnitude)
			{
				_updatePos = true;
				_targetRange = normalRange;
				_moveSpeed = _moveInSpeed;
				_moveState = EMoveState.MoveIn;
				_delayCheckGoOut = 0.4f;
			}
			else if (normalRange > magnitude && (!_updatePos || (_updatePos && _moveState == EMoveState.MoveOut)) && normalRange != _targetRange && _delayCheckGoOut <= 0f)
			{
				_updatePos = true;
				_targetRange = normalRange;
				_moveSpeed = _moveOutSpeed;
				_moveState = EMoveState.MoveOut;
			}
		}
	}

	private void UpdatePos()
	{
		if (!(_targetObj == null))
		{
			Vector3 vector = _targetObj.transform.TransformVector(_localDir);
			Vector3 vector2 = vector.normalized * _moveSpeed * Time.deltaTime;
			if (_moveState == EMoveState.MoveIn)
			{
				vector2 = -vector2;
			}
			Vector3 vector3 = base.transform.position + vector2;
			Vector3 position = _targetObj.transform.position + vector.normalized * _targetRange;
			float magnitude = (vector3 - _targetObj.transform.position).magnitude;
			if ((magnitude < _targetRange && _moveState == EMoveState.MoveOut) || (magnitude > _targetRange && _moveState == EMoveState.MoveIn))
			{
				base.transform.position = vector3;
			}
			else
			{
				base.transform.position = position;
				_updatePos = false;
			}
			UpdateScaleShield();
		}
	}

	private void Awake()
	{
		Init();
	}

	private void Start()
	{
	}

	private void Update()
	{
		_delayCheck -= Time.deltaTime;
		if (_delayCheck <= 0f)
		{
			_delayCheck = DELAY_CHECK;
			CheckCollisionWall();
		}
		if (_delayCheckGoOut > 0f)
		{
			_delayCheckGoOut -= Time.deltaTime;
		}
		if (_updatePos)
		{
			UpdatePos();
		}
	}
}
