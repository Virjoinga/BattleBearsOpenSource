using UnityEngine;

public class FacebookEventListener : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
		FacebookEventManager.onLoginSuccess += onLoginSuccess;
		FacebookEventManager.onLoginError += onLoginError;
		FacebookEventManager.onLoginCancel += onLoginCancel;
		FacebookEventManager.onLogoutFinish += onLogoutFinish;
		FacebookEventManager.onLogoutBegin += onLogoutBegin;
		FacebookEventManager.onPostComplete += onPostComplete;
		FacebookEventManager.onPostError += onPostError;
		FacebookEventManager.onPostCancel += onPostCancel;
		FacebookEventManager.onGetFriendsComplete += onGetFriendsComplete;
		FacebookEventManager.onGetFriendsError += onGetFriendsError;
		FacebookEventManager.onGetPlayerNameComplete += onGetPlayerNameComplete;
		FacebookEventManager.onGetPlayerIdComplete += onGetPlayerIdComplete;
		FacebookEventManager.onGetPlayerError += onGetPlayerError;
	}

	private void OnDisable()
	{
		FacebookEventManager.onLoginSuccess -= onLoginSuccess;
		FacebookEventManager.onLoginError -= onLoginError;
		FacebookEventManager.onLoginCancel -= onLoginCancel;
		FacebookEventManager.onLogoutFinish -= onLogoutFinish;
		FacebookEventManager.onLogoutBegin -= onLogoutBegin;
		FacebookEventManager.onPostComplete -= onPostComplete;
		FacebookEventManager.onPostError -= onPostError;
		FacebookEventManager.onPostCancel -= onPostCancel;
		FacebookEventManager.onGetFriendsComplete -= onGetFriendsComplete;
		FacebookEventManager.onGetFriendsError -= onGetFriendsError;
		FacebookEventManager.onGetPlayerNameComplete -= onGetPlayerNameComplete;
		FacebookEventManager.onGetPlayerIdComplete -= onGetPlayerIdComplete;
		FacebookEventManager.onGetPlayerError -= onGetPlayerError;
	}

	private void onLoginSuccess()
	{
		debug("Facebook login ");
	}

	private void onLoginError(string error)
	{
		debug("Facebook login error : " + error);
	}

	private void onLoginCancel()
	{
		debug("Facebook login Cancel ");
	}

	private void onLogoutFinish()
	{
		debug("Facebook logout Finish ");
	}

	private void onLogoutBegin()
	{
		debug("Facebook logout Start ");
	}

	private void onPostComplete()
	{
		debug("Facebook Post Complete ");
	}

	private void onPostError(string error)
	{
		debug("Error occurred : " + error);
	}

	private void onPostCancel()
	{
		debug("post Cancelled");
	}

	private void onGetFriendsComplete(string friends)
	{
		debug("Get Friends : " + friends);
	}

	private void onGetFriendsError(string error)
	{
		debug("Get Friends Error occurred : " + error);
	}

	private void onGetPlayerNameComplete(string name)
	{
		debug("Player Name : " + name);
	}

	private void onGetPlayerIdComplete(string id)
	{
		debug("Player id : " + id);
	}

	private void onGetPlayerError(string error)
	{
		debug("Get Player id or name an Error occurred : " + error);
	}

	private void debug(string msg)
	{
	}
}
