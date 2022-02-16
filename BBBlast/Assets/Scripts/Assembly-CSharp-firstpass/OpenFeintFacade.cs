using System;
using UnityEngine;

public class OpenFeintFacade : IDisposable
{
	private bool calledInit;

	private AndroidJavaClass cls_OpenFeintFacade = new AndroidJavaClass("com.unity3d.Plugin.OpenFeintFacade");

	private AndroidJavaClass cls_OpenFeint = new AndroidJavaClass("com.openfeint.api.OpenFeint");

	private AndroidJavaClass cls_Dashboard = new AndroidJavaClass("com.openfeint.api.ui.Dashboard");

	public void Init(string name, string key, string secret, string id)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("com.openfeint.api.OpenFeintSettings", name, key, secret, id))
				{
					cls_OpenFeintFacade.CallStatic("Init", androidJavaObject, androidJavaObject2);
				}
			}
		}
		calledInit = true;
	}

	public bool isInitialized()
	{
		return calledInit;
	}

	public bool isUserLoggedIn()
	{
		bool result = cls_OpenFeint.CallStatic<bool>("isUserLoggedIn", new object[0]);
		if (AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
		{
			AndroidJNI.ExceptionClear();
			return false;
		}
		return result;
	}

	public void OpenDashboard()
	{
		cls_Dashboard.CallStatic("open");
	}

	public void CloseDashboard()
	{
		cls_Dashboard.CallStatic("close");
	}

	public void OpenAchievements()
	{
		cls_Dashboard.CallStatic("openAchievements");
	}

	public void OpenLeaderboards()
	{
		cls_Dashboard.CallStatic("openLeaderboards");
	}

	public void SubmitScore(string leaderboard, int score)
	{
		cls_OpenFeintFacade.CallStatic("SubmitScore", leaderboard, score);
	}

	public void SubmitAchievement(int achievementID)
	{
		cls_OpenFeintFacade.CallStatic("SubmitAchievement", achievementID);
	}

	public string GetUserID()
	{
		AndroidJavaObject androidJavaObject = cls_OpenFeint.CallStatic<AndroidJavaObject>("getCurrentUser", new object[0]);
		return androidJavaObject.Call<string>("userID", new object[0]);
	}

	public void Dispose()
	{
		cls_OpenFeintFacade.Dispose();
		cls_OpenFeint.Dispose();
		cls_Dashboard.Dispose();
	}
}
