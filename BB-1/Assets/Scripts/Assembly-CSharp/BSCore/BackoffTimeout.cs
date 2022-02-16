using System;
using System.Collections;
using UnityEngine;

namespace BSCore
{
	public class BackoffTimeout
	{
		protected float _backoffDelay;

		protected readonly float _backoffDelayIncrement;

		protected readonly float _backoffDelayMax;

		public BackoffTimeout(float backoffDelayIncrement = 0.5f, float backoffDelayMax = 3f)
		{
			_backoffDelayIncrement = backoffDelayIncrement;
			_backoffDelayMax = backoffDelayMax;
		}

		public bool RunAfterBackoff(MonoBehaviour mono, Action callback)
		{
			if (!ShouldRun())
			{
				return false;
			}
			_backoffDelay += _backoffDelayIncrement;
			mono.StartCoroutine(RunAfterBackoffRoutine(callback));
			return true;
		}

		protected IEnumerator RunAfterBackoffRoutine(Action callback)
		{
			yield return new WaitForSeconds(_backoffDelay);
			callback();
		}

		public bool RunAfterBackoff(Action callback)
		{
			if (!ShouldRun())
			{
				return false;
			}
			_backoffDelay += _backoffDelayIncrement;
			DelayedAction.RunAfterSeconds(_backoffDelay, callback);
			return true;
		}

		protected virtual bool ShouldRun()
		{
			return _backoffDelay < _backoffDelayMax;
		}

		public void ResetBackoff()
		{
			_backoffDelay = 0f;
		}
	}
}
