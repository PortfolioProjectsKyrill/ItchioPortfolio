using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SellManager : MonoBehaviour
{
    public Inventory activeInventory;

    public GameObject[] fishTypes;
    [SerializeField] private List<float> fishValues;
    public int[] rarityChances;

    private void Start()
    {
        //Load fish types from folder
        fishTypes = Resources.LoadAll("Fishes", typeof(GameObject)).Cast<GameObject>().ToArray();
        //get values and prepare inventory
        for (int i = 0; i < fishTypes.Length; i++)
        {
            fishValues.Add(fishTypes[i].GetComponent<FishValue>().value);
            fishTypes[i].GetComponent<FishValue>().chance = rarityChances[(int)fishTypes[i].GetComponent<FishValue>().fishRarity];
            activeInventory.fishInventory.Add(0);
        }
    }
    /// <summary>
    /// Handles selling fish from inventory
    /// </summary>
    public void SellFish()
    {
        float total = 0;
        //Loop fish amount
        for (int i = 0; i < activeInventory.fishInventory.Count; i++)
        {
            if (activeInventory.fishInventory[i] > 0)
            {
                //Update money
                CurrencyManager.instance.UpdateMoney(activeInventory.fishInventory[i] * fishValues[i]);
                total += activeInventory.fishInventory[i] * fishValues[i];
                //Update text
                UIManager.instance.UpdateCurrencyText(CurrencyManager.instance.money.ToString());
                //Set inventory value to 0
                activeInventory.fishInventory[i] = 0;
                //Update text
                AudioManager.instance.PlayShopClip(AudioManager.instance.buyAndSell);
                StartCoroutine(UIManager.instance.SellMessage("Sold inventory for $" + total));
            }
        }
    }
}
