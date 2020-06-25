using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class s_JetpackShop : MonoBehaviour
{
	public RectTransform jetpackPowerTransform;
	public RectTransform jetpackFuelTransform;
	public Text scrapText;
	public Button jetpackPowerButton;
	public Text powerPriceText;
	public Button jetpackFuelButton;
	public Text fuelPriceText;
	public float powerUpgradeAmount;
	public float fuelUpgradeAmount;
	public int powerPriceIncrease;
	public int fuelPriceIncrease;

	private float fullUpgradeWidth = 400f;
	private float upgradeAddition = 80f;
	private float jetpackPower;
	private float jetpackFuel;
	private int currentPowerPrice;
	private int currentFuelPrice;

	void Start()
	{
		jetpackPower = PlayerData.current.jetpackPower;
		jetpackFuel = PlayerData.current.jetpackDuration;
		powerPriceText.text = PlayerData.current.currentPowerPrice.ToString();
		fuelPriceText.text = PlayerData.current.currentFuelPrice.ToString();
		currentPowerPrice = PlayerData.current.currentPowerPrice;
		currentFuelPrice = PlayerData.current.currentFuelPrice;

		SetUpUpgradeBars();
	}

	void SetUpUpgradeBars()
	{
		float width = PlayerData.current.powerUpgradeLevel * upgradeAddition;
		float height = jetpackPowerTransform.sizeDelta.y;
		jetpackPowerTransform.sizeDelta = new Vector2(width, height);

		if (width < fullUpgradeWidth)
		{
			jetpackPowerButton.interactable = true;
		}

		width = PlayerData.current.fuelUpgradeLevel * upgradeAddition;
		height = jetpackFuelTransform.sizeDelta.y;
		jetpackFuelTransform.sizeDelta = new Vector2(width, height);

		if (width < fullUpgradeWidth)
		{
			jetpackFuelButton.interactable = true;
		}
	}

	public void UpgradeJetpack()
	{
		float width = jetpackPowerTransform.sizeDelta.x;
		float height = jetpackPowerTransform.sizeDelta.y;

		//Debug.Log("Scrap collected: " + PlayerData.current.scrapCollected);
		//Debug.Log("Current price: " + currentPowerPrice);


		if (width < fullUpgradeWidth && PlayerData.current.GetTotalScrapCollected() >= currentPowerPrice)
		{
			PlayerData.current.RemoveScrapPoints(currentPowerPrice);
			scrapText.text = PlayerData.current.GetTotalScrapCollected().ToString();
			jetpackPowerTransform.sizeDelta = new Vector2(width + upgradeAddition, height);
			jetpackPower = jetpackPower + powerUpgradeAmount;
			PlayerData.current.jetpackPower = jetpackPower;
			PlayerData.current.powerUpgradeLevel++;
			PlayerData.current.currentPowerPrice = PlayerData.current.currentPowerPrice + powerPriceIncrease;
			currentPowerPrice = PlayerData.current.currentPowerPrice;
			powerPriceText.text = PlayerData.current.currentPowerPrice.ToString();
			SaveLoadData.Save();

			if (jetpackPowerTransform.sizeDelta.x == fullUpgradeWidth)
			{
				jetpackPowerButton.interactable = false;
				Debug.Log("Fully upgraded");
			}
		}
	}
	
	public void UpgradeFuel()
	{
		float width = jetpackFuelTransform.sizeDelta.x;
		float height = jetpackFuelTransform.sizeDelta.y;
		
		if (width < fullUpgradeWidth && PlayerData.current.GetTotalScrapCollected() >= currentFuelPrice)
		{
			PlayerData.current.RemoveScrapPoints(currentFuelPrice);
			scrapText.text = PlayerData.current.GetTotalScrapCollected().ToString();
			jetpackFuelTransform.sizeDelta = new Vector2(width + upgradeAddition, height);
			jetpackFuel = jetpackFuel + fuelUpgradeAmount;
			PlayerData.current.jetpackDuration = jetpackFuel;
			PlayerData.current.fuelUpgradeLevel++;
			PlayerData.current.currentFuelPrice = PlayerData.current.currentFuelPrice + fuelPriceIncrease;
			currentFuelPrice = PlayerData.current.currentFuelPrice;
			fuelPriceText.text = PlayerData.current.currentFuelPrice.ToString();
			SaveLoadData.Save();

			if (jetpackFuelTransform.sizeDelta.x == fullUpgradeWidth)
			{
				jetpackFuelButton.interactable = false;
				Debug.Log("Fully upgraded");
			}
		}
	}

	public void ResetJetpackShop()
	{
		scrapText.text = PlayerData.current.GetTotalScrapCollected().ToString();
		jetpackFuelButton.interactable = true;
		jetpackPowerButton.interactable = true;
		powerPriceText.text = PlayerData.current.currentPowerPrice.ToString();
		fuelPriceText.text = PlayerData.current.currentFuelPrice.ToString();
		jetpackFuelTransform.sizeDelta = new Vector2(0, jetpackFuelTransform.sizeDelta.y);
		jetpackPowerTransform.sizeDelta = new Vector2(0, jetpackFuelTransform.sizeDelta.y);
		currentFuelPrice = PlayerData.current.currentFuelPrice;
		currentPowerPrice = PlayerData.current.currentPowerPrice;

	}
}
