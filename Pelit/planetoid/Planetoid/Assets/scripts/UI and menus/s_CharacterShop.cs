using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class s_CharacterShop : MonoBehaviour
{
	public s_PlayerCharacterSkin playerMenu;
	public s_PlayerCharacterSkin playerShop;
	public Button randomBuyButton;
	public Text scrapText;
	public Text skinCostText;
	public Button[] characterSkins;

	public int skinCost;
	public bool[] boughtSkins = new bool[8];
	private int equippedSkin;
	//List<int> availableSkins;
	//int selected;
	
	void Start()
	{
		//Tää on vaan testausta varten. Lopullisessa tämä tieto haetaan savefilestä.
		//--------------------------------------------------------------------------
		/*for (int i = 0; i < boughtSkins.Length; i++)
		{
			if (i == 0 || i == 3 || i == 4)
			{
				boughtSkins[i] = true;
			}
			else
			{
				boughtSkins[i] = false;
			}

			//boughtSkins[i] = true;
		}*/
		//--------------------------------------------------------------------------

		boughtSkins = PlayerData.current.boughtSkins;
		equippedSkin = PlayerData.current.equippedSkin;

		//SetAvailableSkins();

		for (int i = 0; i < boughtSkins.Length; i++)
		{
			if (boughtSkins[i] == false)
			{
				randomBuyButton.interactable = true;
				break;
			}
		}

		scrapText.text = PlayerData.current.GetTotalScrapCollected().ToString();
		skinCostText.text = skinCost.ToString();

		//SetUpButtons();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.M)) {
			Debug.Log("Gib money");
			PlayerData.current.AddScrapPoints(1000);
		}
	}

	/*void SetUpButtons()
	{
		for (int i = 0; i < characterSkins.Length; i++)
		{
			if (i == equippedSkin)
			{
				//Tähän equipped
				characterSkins[i].GetComponentInChildren<Text>().text = "Equipped";
			}
			else
			{
				if (boughtSkins[i] == true)
				{
					//Tähän equip
					characterSkins[i].GetComponentInChildren<Text>().text = "Equip";
				}
				else
				{
					//Tähän buy
					characterSkins[i].GetComponentInChildren<Text>().text = skinCost.ToString() + "$";
				}
			}
		}
	}

	public void EquipButtonPressed(int index)
	{
		if (boughtSkins[index] == true)
		{
			characterSkins[equippedSkin].GetComponentInChildren<Text>().text = "Equip";
			characterSkins[index].GetComponentInChildren<Text>().text = "Equipped";
			equippedSkin = index;
			PlayerData.current.equippedSkin = equippedSkin;

			playerMenu.LoadWholeSkin(equippedSkin);
		}
		else
		{
			//Tähän toiminnallisuus ostolle
			if (PlayerData.current.scrapCollected >= skinCost)
			{
				PlayerData.current.RemoveScrap(skinCost);
				characterSkins[index].GetComponentInChildren<Text>().text = "Equip";
				boughtSkins[index] = true;
				scrapText.text = PlayerData.current.scrapCollected.ToString();
				PlayerData.current.boughtSkins = boughtSkins;
			}
			else
			{
				Debug.Log("Insufficiant Funds");
			}
		}

		SaveLoadData.Save();
	}*/

	public void RandomSkin()
	{
		List<int> availableSkins = new List<int>();
		
		for (int i = 0; i < boughtSkins.Length; i++)
		{
			if (boughtSkins[i] == false)
			{
				availableSkins.Add(i);
			}
		}

		if (availableSkins.Count > 0 && PlayerData.current.GetTotalScrapCollected() >= skinCost)
		{
			PlayerData.current.RemoveScrapPoints(skinCost);
			int randomSkinID = Random.Range(0, availableSkins.Count);
			PlayerData.current.equippedSkin = availableSkins[randomSkinID];
			PlayerData.current.equippedHead = availableSkins[randomSkinID];
			PlayerData.current.equippedBody = availableSkins[randomSkinID];
			PlayerData.current.equippedLegs = availableSkins[randomSkinID];
			PlayerData.current.boughtSkins[availableSkins[randomSkinID]] = true;
			playerShop.LoadWholeSkin(availableSkins[randomSkinID]);
			playerMenu.LoadWholeSkin(availableSkins[randomSkinID]);
			scrapText.text = PlayerData.current.GetTotalScrapCollected().ToString();

			if (availableSkins.Count == 1)
			{
				randomBuyButton.interactable = false;
			}
		}
		else
		{
			Debug.Log("Insufficiant Funds/All Skins Bought");
		}

		SaveLoadData.Save();
	}

	public void ChangeFullSkin(int direction)
	{
		List<int> availableSkins = new List<int>();
		int selected = 0;
		
		for (int i = 0; i < boughtSkins.Length; i++)
		{
			if (boughtSkins[i] == true)
			{
				availableSkins.Add(i);
				if (i == PlayerData.current.equippedSkin)
				{
					selected = availableSkins.Count-1;
				}
			}
		}

		int newSkin = GetNewSkin(availableSkins, selected, direction);

		PlayerData.current.equippedSkin = newSkin;
		PlayerData.current.equippedHead = newSkin;
		PlayerData.current.equippedBody = newSkin;
		PlayerData.current.equippedLegs = newSkin;

		playerShop.LoadWholeSkin(newSkin);
		playerMenu.LoadWholeSkin(newSkin);

		SaveLoadData.Save();
	}

	public void ChangeHeadSkin(int direction)
	{
		List<int> availableSkins = new List<int>();
		int selected = 0;
		
		for (int i = 0; i < boughtSkins.Length; i++)
		{
			if (boughtSkins[i] == true)
			{
				availableSkins.Add(i);
				if (i == PlayerData.current.equippedHead)
				{
					selected = availableSkins.Count-1;
				}
			}
		}

		int newSkin = GetNewSkin(availableSkins, selected, direction);
		
		PlayerData.current.equippedHead = newSkin;
		
		playerShop.ChangeHelmet(newSkin);
		playerMenu.ChangeHelmet(newSkin);
		
		SaveLoadData.Save();
	}

	public void ChangeBodySkin(int direction)
	{
		List<int> availableSkins = new List<int>();
		int selected = 0;
		
		for (int i = 0; i < boughtSkins.Length; i++)
		{
			if (boughtSkins[i] == true)
			{
				availableSkins.Add(i);
				if (i == PlayerData.current.equippedBody)
				{
					selected = availableSkins.Count-1;
				}
			}
		}


		int newSkin = GetNewSkin(availableSkins, selected, direction);
		
		PlayerData.current.equippedBody = newSkin;
		
		playerShop.ChangeBody(newSkin);
		playerMenu.ChangeBody(newSkin);
		
		SaveLoadData.Save();
	}

	public void ChangeLegsSkin(int direction)
	{
		List<int> availableSkins = new List<int>();
		int selected = 0;
		
		for (int i = 0; i < boughtSkins.Length; i++)
		{
			if (boughtSkins[i] == true)
			{
				availableSkins.Add(i);
				if (i == PlayerData.current.equippedLegs)
				{
					selected = availableSkins.Count-1;
				}
			}
		}

		int newSkin = GetNewSkin(availableSkins, selected, direction);
		
		PlayerData.current.equippedLegs = newSkin;
		
		playerShop.ChangeLegs(newSkin);
		playerMenu.ChangeLegs(newSkin);
		
		SaveLoadData.Save();
	}

	int GetNewSkin(List<int> availableSkins, int selected, int direction)
	{	
		int newSkin = 0;
		
		if (direction == -1)
		{
			newSkin = selected-1;
			
			if (newSkin < 0)
			{
				newSkin = availableSkins[availableSkins.Count-1];
			}
			else
			{
				newSkin = availableSkins[selected-1];
			}
		}
		else
		{
			newSkin = selected + 1;
			
			if (newSkin == availableSkins.Count)
			{
				newSkin = availableSkins[0];
			}
			else
			{
				newSkin = availableSkins[newSkin];
			}
		}

		return newSkin;
	}

	public void ResetCharacterShop()
	{
		playerMenu.LoadWholeSkin(0);
		playerShop.LoadWholeSkin(0);
		boughtSkins = PlayerData.current.boughtSkins;
		scrapText.text = PlayerData.current.GetTotalScrapCollected().ToString();
		randomBuyButton.interactable = true;
	}
}
