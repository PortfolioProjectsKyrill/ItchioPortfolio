using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    public float money;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// Handles updating the variable for money
    /// </summary>
    /// <param name="value"></param>
    public void UpdateMoney(float value)
    {
        money += value;
        UIManager.instance.UpdateCurrencyText(money.ToString());
    }
}
