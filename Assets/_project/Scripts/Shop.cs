using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject[] shopItems;
    public List<string> boatUpgrades;

    public Inventory inventory;
    public BoatController boatController;
    public FishingRodStats fishingRodStats;
    public Interactable interactable;


    private void Start()
    {
        shopItems = Resources.LoadAll("ShopItems", typeof(GameObject)).Cast<GameObject>().ToArray();
        GenerateShopItems();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !UIManager.instance.shopOptionButtons.activeSelf && interactable.inRadius)
        {
            UIManager.instance.ShowShopUI(true, ref interactable);
            interactable.inRadiusEvent.Invoke();

            if (!QuestManager.instance.questStates[1])
            {
                QuestManager.instance.CompleteQuest(1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && UIManager.instance.shopOptionButtons.activeSelf && interactable.inRadius)
        {
            UIManager.instance.ShowShopUI(false, ref interactable);
        }
        else if (!interactable.inRadius && interactable.hasInteracted)
        {
            UIManager.instance.ShowShopUI(false, ref interactable);
        }
    }
    /// <summary>
    /// Generates items in the preset shop slots
    /// </summary>
    public void GenerateShopItems()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            //Make uimanager show the shop slot
            UIManager.instance.GenerateShopSlots(i, shopItems[i].name, gameObject, shopItems[i].GetComponent<ShopItem>().buyOrUpgrade);
        }
    }
    /// <summary>
    /// Handles buying items from the shop
    /// </summary>
    /// <param name="index">index of the target item in the list</param>
    public void BuyItem(int index)
    {
        //If player has enough money
        if (CurrencyManager.instance.money >= shopItems[index].GetComponent<ShopItem>().cost)
        {
            //If player does not already have the item
            if (!inventory.itemInventory.Contains(shopItems[index]))
            {
                //Remove the cost from the player's money
                CurrencyManager.instance.UpdateMoney(-shopItems[index].GetComponent<ShopItem>().cost);
                //Add the item to the player's inventory
                inventory.itemInventory.Add(shopItems[index]);
            }
        }
    }
    
    public void UpgradeItem(int index)
    {
        if (shopItems[index].GetComponent<ShopItem>().item) 
        {
            //If player has enough money
            if (CurrencyManager.instance.money >= shopItems[index].GetComponent<ShopItem>().cost)
            {
                if (shopItems[index].GetComponent<ShopItem>().level < 4)
                {
                    //Remove the cost from the player's money
                    CurrencyManager.instance.UpdateMoney(-shopItems[index].GetComponent<ShopItem>().cost);
                    //Add the item to the player's inventory
                    UpgradeFishingRod(shopItems[index].GetComponent<ShopItem>().upgradeName);
                    shopItems[index].GetComponent<ShopItem>().level++;
                }
            }
        }
        else
        {
            //If player has enough money
            if (CurrencyManager.instance.money >= shopItems[index].GetComponent<ShopItem>().cost)
            {
                //If player does not already have the item
                //If player does not already have the item
                if (shopItems[index].GetComponent<ShopItem>().upgradeName == "moreFuel")
                {
                    //Remove the cost from the player's money
                    CurrencyManager.instance.UpdateMoney(-shopItems[index].GetComponent<ShopItem>().cost);
                    //Add the item to the player's inventory
                    UpgradeBoat(shopItems[index].GetComponent<ShopItem>().upgradeName);
                }
                else if (shopItems[index].GetComponent<ShopItem>().level < 4)
                {
                    //Remove the cost from the player's money
                    CurrencyManager.instance.UpdateMoney(-shopItems[index].GetComponent<ShopItem>().cost);
                    //Add the item to the player's inventory
                    UpgradeBoat(shopItems[index].GetComponent<ShopItem>().upgradeName);
                    shopItems[index].GetComponent<ShopItem>().level++;
                }
            }
        }
    }

    public void UpgradeBoat(string upgrade)
    {
        if (upgrade == "fuel")
        {
            boatController.AddFuelCapacity(10);
        }
        else if (upgrade == "speed")
        {
            boatController.AddSpeedMultiplier();
        }
        else if (upgrade == "moreFuel")
        {
            boatController.AddFuel(30);
        }
    }

    public void UpgradeFishingRod(string upgrade)
    {
        if (upgrade == "luck")
        {
            fishingRodStats.luck++;
        }
    }
}
