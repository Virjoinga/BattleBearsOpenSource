using UnityEngine;

public class customizeController : MonoBehaviour
{
	private GameObject[] helm;

	private GameObject[] armor;

	private Texture[] skins;

	private Material mySkin;

	public BackpackController backPack;

	private void Start()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Knight");
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("Shogun");
		GameObject[] array3 = GameObject.FindGameObjectsWithTag("Pirate");
		helm = new GameObject[4];
		armor = new GameObject[4];
		helm[0] = array[1];
		helm[1] = array2[1];
		helm[2] = null;
		helm[3] = array3[1];
		armor[0] = array[0];
		armor[1] = array2[0];
		armor[2] = null;
		armor[3] = array3[0];
		skins = new Texture[3];
		skins[0] = Resources.Load("Oliver") as Texture;
		skins[1] = Resources.Load("Oco") as Texture;
		skins[2] = Resources.Load("darkOco") as Texture;
		mySkin = Resources.Load("OliverMat") as Material;
		mySkin.SetTexture("_MainTex", skins[PlayerPrefs.GetInt("skin")]);
	}

	private void Update()
	{
		for (int i = 0; i < helm.Length; i++)
		{
			if (helm[i] != null)
			{
				//helm[i].active = false;
				helm[i].SetActive(false);
			}
		}
		for (int j = 0; j < armor.Length; j++)
		{
			if (armor[j] != null)
			{
				//armor[j].active = false;
				armor[j].SetActive(false);
			}
		}
		if (backPack.armorIndex != 2)
		{
			//armor[backPack.armorIndex].active = true;
			armor[backPack.armorIndex].SetActive(true);
		}
		if (backPack.helmIndex != 2)
		{
			//helm[backPack.helmIndex].active = true;
			helm[backPack.helmIndex].SetActive(true);
		}
		mySkin.SetTexture("_MainTex", skins[backPack.skinIndex]);
	}
}
