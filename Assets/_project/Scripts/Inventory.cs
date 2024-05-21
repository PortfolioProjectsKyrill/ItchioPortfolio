using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private Shop shop;
    public List<float> fishInventory;
    public List<GameObject> itemInventory;

    public TMP_Text[] fishInventoryText;

    public GameObject[] inventoryUI;

    public Image[] upgradeIcons;

    private void Start()
    {
        shop = GetComponent<Shop>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AudioManager.instance.PlayInventoryClip(AudioManager.instance.inventoryOpen);
            if (inventoryUI[0].activeSelf)
            {
                inventoryUI[0].SetActive(false);
                inventoryUI[1].SetActive(true);
                inventoryUI[2].SetActive(false);
                UpdateInventory();
            }
            else
            {
                inventoryUI[0].SetActive(true);
                inventoryUI[1].SetActive(true);
                inventoryUI[2].SetActive(false);
                UpdateInventory();
            }
        }
    }
    /// <summary>
    /// Add a fish to inventory
    /// </summary>
    /// <param name="index"></param>
    public void AddToInventory(int index)
    {
        //Add 1 to fish type
        fishInventory[index]++;
    }

    /// <summary>
    /// Clears the fish inventory
    /// </summary>
    public void ClearInventory()
    {
        //Loop fish count
        for (int i = 0; i < fishInventory.Count; i++)
        {
            //Set value back to 0
            fishInventory[i] = 0;
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < fishInventory.Count; i++)
        {
            fishInventoryText[i].text = fishInventory[i].ToString() + "x";
        }
        for (int i = 0; i < shop.shopItems.Length; i++)
        {
            for (int l = 0; l < shop.shopItems[i].GetComponent<ShopItem>().level; l++)
            {
                upgradeIcons[i * 4 + l].gameObject.SetActive(true);
            }
            
        }
    }
}
