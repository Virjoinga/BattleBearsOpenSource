using UnityEngine;

public class TextureChooser : MonoBehaviour
{
	private Texture2D tex;

	private bool hasLoaded;

	public void OnLevelWasLoaded()
	{
		Object[] array = Resources.LoadAll("Materials/LoadingScene", typeof(Material));
		Object[] array2 = Resources.LoadAll("Materials/MainMenu", typeof(Material));
		string loadedLevelName = Application.loadedLevelName;
		switch (loadedLevelName)
		{
		case "MechaBossFight":
			array = Resources.LoadAll("Materials/OliverCampaignLevel2/" + loadedLevelName, typeof(Material));
			break;
		case "OliverCampaignLevel1":
			array = Resources.LoadAll("Materials/" + loadedLevelName, typeof(Material));
			break;
		case "OliverCampaignLevel2":
			array = Resources.LoadAll("Materials/" + loadedLevelName, typeof(Material));
			break;
		case "OliverCampaignLevel3":
			array = Resources.LoadAll("Materials/" + loadedLevelName, typeof(Material));
			break;
		case "RiggsCampaignLevel1":
			array = Resources.LoadAll("Materials/" + loadedLevelName, typeof(Material));
			break;
		case "RiggsCampaignLevel2":
			array = Resources.LoadAll("Materials/" + loadedLevelName, typeof(Material));
			break;
		case "RiggsCampaignLevel3":
			array = Resources.LoadAll("Materials/" + loadedLevelName, typeof(Material));
			break;
		case "SpacebossFight":
			array = Resources.LoadAll("Materials/" + loadedLevelName, typeof(Material));
			break;
		case "Survival":
			array = Resources.LoadAll("Materials/" + loadedLevelName, typeof(Material));
			break;
		case "TentacleeseFight":
			array = Resources.LoadAll("Materials/RiggsCampaignLevel3/" + loadedLevelName, typeof(Material));
			break;
		case "WilCampaignLevel1":
			array = Resources.LoadAll("Materials/" + loadedLevelName, typeof(Material));
			break;
		case "WilCampaignLevel2":
			array = Resources.LoadAll("Materials/" + loadedLevelName, typeof(Material));
			break;
		case "WilCampaignLevel3":
			array = Resources.LoadAll("Materials/" + loadedLevelName, typeof(Material));
			break;
		case "WilCampaignLevel4":
			array = Resources.LoadAll("Materials/WilCampaignLevel3", typeof(Material));
			break;
		case "SurvivalZombies":
			array = Resources.LoadAll("Materials/Survival", typeof(Material));
			break;
		}
		if (array == null && array2 == null)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			Material material = array[i] as Material;
			string[] array3 = material.name.Split('_');
			if (GameManager.Instance != null && GameManager.Instance.useHighres)
			{
				tex = Resources.Load("Textures/High/" + array3[0]) as Texture2D;
			}
			else
			{
				tex = Resources.Load("Textures/Low/" + array3[0]) as Texture2D;
			}
			material.mainTexture = tex;
		}
	}
}
