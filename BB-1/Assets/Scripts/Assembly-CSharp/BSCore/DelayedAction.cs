using System;
using System.Collections;
using UnityEngine;

namespace BSCore
{
	public class DelayedAction : MonoBehaviour
	{
		public struct DelayedActionHandle
		{
			public Coroutine _coroutine;

			public DelayedActionHandle(Coroutine coroutine)
			{
				_coroutine = coroutine;
			}

			public void Kill()
			{
				if (_coroutine != null && _instanceCache != null)
				{
					_instance.StopCoroutine(_coroutine);
				}
			}
		}

		private static DelayedAction _instanceCache;

		private static DelayedAction _instance
		{
			get
			{
				if (_instanceCache == null)
				{
					_instanceCache = new GameObject("DelayedAction").AddComponent<DelayedAction>();
				}
				return _instanceCache;
			}
		}

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		public static DelayedActionHandle RunAfterSeconds(float seconds, Action callback)
		{
			return new DelayedActionHandle(_instance.StartCoroutine(_instance.RunAfterSecondsRoutine(seconds, callback)));
		}

		public static DelayedActionHandle RunWhen(Func<bool> conditional, Action callback)
		{
			return new DelayedActionHandle(_instance.StartCoroutine(_instance.RunWhenRoutine(conditional, callback)));
		}

		public static DelayedActionHandle RunNextFrame(Action callback)
		{
			return new DelayedActionHandle(_instance.StartCoroutine(_instance.RunNextFrameRoutine(callback)));
		}

		public static DelayedActionHandle RunCoroutine(IEnumerator iEnumerator)
		{
			return new DelayedActionHandle(_instance.StartCoroutine(iEnumerator));
		}

		public static void KillCoroutine(DelayedActionHandle handle)
		{
			handle.Kill();
		}

		private IEnumerator RunAfterSecondsRoutine(float seconds, Action callback)
		{
			yield return new WaitForSeconds(seconds);
			callback();
		}

		private IEnumerator RunWhenRoutine(Func<bool> conditional, Action callback)
		{
			yield return new WaitUntil(conditional);
			callback();
		}

		private IEnumerator RunNextFrameRoutine(Action callback)
		{
			yield return null;
			callback();
		}
	}
}
