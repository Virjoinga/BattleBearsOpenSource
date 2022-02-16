using System;
using UnityEngine;

public class FacebookPlugin : IDisposable
{
	private AndroidJavaObject cls_fb;

	public void Init(string FB_ID)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			cls_fb = new AndroidJavaObject("com.MidaMobile.Facebook.FacebookPlugin");
			cls_fb.Call("init", FB_ID);
		}
	}

	public void login()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			cls_fb.Call("login");
		}
	}

	public void logout()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			cls_fb.Call("logout");
		}
	}

	public void postMessageOnWall(string message, string picture, string link, string name, string caption, string description)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			cls_fb.Call("postMessageOnWall", message, picture, link, name, caption, description);
		}
	}

	public void postMessageOnWallDialog(string message, string picture, string link, string name, string caption, string description)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			cls_fb.Call("postMessageOnWallDialog", message, picture, link, name, caption, description);
		}
	}

	public void postImageOnWall(byte[] picture, string caption)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			cls_fb.Call("postImageOnWall", picture, caption);
		}
	}

	public void postImageOnWallPath(string pathImage, string caption)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			cls_fb.Call("postImageOnWallPath", pathImage, caption);
		}
	}

	public bool isLoggedIn()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return cls_fb.Call<bool>("isLoggedIn", new object[0]);
	}

	public void getFriend()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			cls_fb.Call("getFriend");
		}
	}

	public void getPlayerNameId()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			cls_fb.Call("getPlayerNameId");
		}
	}

	public void Dispose()
	{
		cls_fb.Dispose();
	}
}
