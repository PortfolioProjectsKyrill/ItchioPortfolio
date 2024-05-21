using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public bool buyOrUpgrade;
    public int cost;
    public bool item;
    public string upgradeName;
    public int level;

    private void Start()
    {
        level = 0;
    }
}
