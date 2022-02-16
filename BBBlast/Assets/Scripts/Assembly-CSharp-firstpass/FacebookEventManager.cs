using System.Runtime.CompilerServices;
using UnityEngine;

public class FacebookEventManager : MonoBehaviour
{
	public delegate void FacebookResultEventHandler(string result);

	public delegate void FacebookEventHandler();

	[method: MethodImpl(32)]
	public static event FacebookEventHandler onLoginSuccess;

	[method: MethodImpl(32)]
	public static event FacebookResultEventHandler onLoginError;

	[method: MethodImpl(32)]
	public static event FacebookEventHandler onLoginCancel;

	[method: MethodImpl(32)]
	public static event FacebookEventHandler onLogoutFinish;

	[method: MethodImpl(32)]
	public static event FacebookEventHandler onLogoutBegin;

	[method: MethodImpl(32)]
	public static event FacebookEventHandler onPostComplete;

	[method: MethodImpl(32)]
	public static event FacebookResultEventHandler onPostError;

	[method: MethodImpl(32)]
	public static event FacebookEventHandler onPostCancel;

	[method: MethodImpl(32)]
	public static event FacebookResultEventHandler onGetFriendsComplete;

	[method: MethodImpl(32)]
	public static event FacebookResultEventHandler onGetFriendsError;

	[method: MethodImpl(32)]
	public static event FacebookResultEventHandler onGetPlayerNameComplete;

	[method: MethodImpl(32)]
	public static event FacebookResultEventHandler onGetPlayerIdComplete;

	[method: MethodImpl(32)]
	public static event FacebookResultEventHandler onGetPlayerError;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		Object.DontDestroyOnLoad(this);
	}

	public void onLoginDidSuccess(string empty)
	{
		if (FacebookEventManager.onLoginSuccess != null)
		{
			FacebookEventManager.onLoginSuccess();
		}
	}

	public void onLoginDidError(string result)
	{
		if (FacebookEventManager.onLoginError != null)
		{
			FacebookEventManager.onLoginError(result);
		}
	}

	public void onLoginDidCancel(string empty)
	{
		if (FacebookEventManager.onLoginCancel != null)
		{
			FacebookEventManager.onLoginCancel();
		}
	}

	public void onLogoutDidFinish(string empty)
	{
		if (FacebookEventManager.onLogoutFinish != null)
		{
			FacebookEventManager.onLogoutFinish();
		}
	}

	public void onLogoutDidBegin(string empty)
	{
		if (FacebookEventManager.onLogoutBegin != null)
		{
			FacebookEventManager.onLogoutBegin();
		}
	}

	public void onPostDidComplete()
	{
		if (FacebookEventManager.onPostComplete != null)
		{
			FacebookEventManager.onPostComplete();
		}
	}

	public void onPostDidError(string error)
	{
		if (FacebookEventManager.onPostError != null)
		{
			FacebookEventManager.onPostError(error);
		}
	}

	public void onPostDidCancel(string empty)
	{
		if (FacebookEventManager.onPostCancel != null)
		{
			FacebookEventManager.onPostCancel();
		}
	}

	public void onGetFriendsDidComplete(string json)
	{
		if (FacebookEventManager.onGetFriendsComplete != null)
		{
			FacebookEventManager.onGetFriendsComplete(json);
		}
	}

	public void onGetFriendsDidError(string error)
	{
		if (FacebookEventManager.onGetFriendsError != null)
		{
			FacebookEventManager.onGetFriendsError(error);
		}
	}

	public void onGetPlayerNameDidComplete(string name)
	{
		if (FacebookEventManager.onGetPlayerNameComplete != null)
		{
			FacebookEventManager.onGetPlayerNameComplete(name);
		}
	}

	public void onGetPlayerIdDidComplete(string id)
	{
		if (FacebookEventManager.onGetPlayerIdComplete != null)
		{
			FacebookEventManager.onGetPlayerIdComplete(id);
		}
	}

	public void onGetPlayerDidError(string error)
	{
		if (FacebookEventManager.onGetPlayerError != null)
		{
			FacebookEventManager.onGetPlayerError(error);
		}
	}
}
