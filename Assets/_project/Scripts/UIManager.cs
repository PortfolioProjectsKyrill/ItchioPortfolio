using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text sellText;

    [Header("Shop Item TMP")]
    [SerializeField] private GameObject shopUIParent;
    [SerializeField] private GameObject[] itemShopUIElements;

    public GameObject shopOptionButtons;

    public GameObject fishingUI;
    public GameObject fishBar;
    public RectTransform fishBarRect;
    public Image catchTimer;
    public Image fish;
    public RectTransform fishRect;

    public List<Image> fishes;

    public GameObject caughtFish;

    public Animation caughtFishAnimation;
    public GameObject BoatUI;

    [Header("In Game Menu")]
    
    [Header("Settings")]
    public GameObject SettingsMenu;
    public GameObject WholeMenu;
    
    [Header("Audio")]

    [SerializeField] private Slider Slider1;
    [SerializeField] private Slider Slider2;
    [SerializeField] private Slider Slider3;

    [SerializeField] AudioMixer Master;
    [SerializeField] AudioMixerGroup Music;
    [SerializeField] AudioMixerGroup SoundFX;
    
    [Header("Keybinds")]

    [Header("Menu conflict manager")]
    public GameObject[] menus;
    public UnityEvent activateMenu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        fishBarRect = fishBar.GetComponent<RectTransform>();
        fishRect = fish.GetComponent<RectTransform>();
    }

    private void Start()
    {
        //Get fishes from resources folder
        GameObject[] tempFishes = Resources.LoadAll("Fishes", typeof(GameObject)).Cast<GameObject>().ToArray();
        for (int i = 0; i < tempFishes.Length; i++)
        {
            fishes.Add(tempFishes[i].GetComponent<Image>());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(GameManager.Instance.GetKey("Menu")))
        {
            if (!WholeMenu.activeSelf)
            {
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }

            if (SettingsMenu.activeSelf)
            {
                CloseSettingsMenu();
            }
        }
    }


    #region Fishing Logic

    /// <summary>
    /// Updates the player currency text
    /// </summary>
    /// <param name="currency">value to show</param>
    public void UpdateCurrencyText(string currency)
    {
        currencyText.text = "Money: " + currency;
    }
    /// <summary>
    /// Shows a message when selling inventory
    /// </summary>
    /// <param name="message">What to show</param>
    /// <returns>null</returns>
    public IEnumerator SellMessage(string message)
    {
        sellText.text = message;
        sellText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        sellText.gameObject.SetActive(false);
        yield return null;
    }

    /// <summary>
    /// Shows a shop slot with the chosen values
    /// </summary>
    /// <param name="index">index of the shop slot</param>
    /// <param name="name">name for the shop slot</param>
    /// <param name="shop">shop component</param>
    /// <param name="buyorupgrade">buy or upgrade the item</param>
    public void GenerateShopSlots(int index, string name, GameObject shop, bool buyorupgrade)
    {
        itemShopUIElements[index].SetActive(true);
        //Set slot name
        TMP_Text[] textComponents;
        textComponents = itemShopUIElements[index].GetComponentsInChildren<TMP_Text>();
        textComponents[0].text = name;
        textComponents[2].text = "Cost: " + shop.GetComponent<Shop>().shopItems[index].GetComponent<ShopItem>().cost;
        itemShopUIElements[index].GetComponentInChildren<Image>().sprite = shop.GetComponent<Shop>().shopItems[index].GetComponent<Image>().sprite;
        if (buyorupgrade)
        {
            //Set slot button text to upgrade
            itemShopUIElements[index].GetComponentInChildren<Button>().GetComponentInChildren<TMP_Text>().text = "Upgrade";
            //Add function to run when button clicked
            itemShopUIElements[index].GetComponentInChildren<Button>().onClick.AddListener(() => shop.GetComponent<Shop>().UpgradeItem(index));
        }
        else
        {
            //Set slot button text to buy
            itemShopUIElements[index].GetComponentInChildren<Button>().GetComponentInChildren<TMP_Text>().text = "Buy";
            //Add function to run when button clicked
            itemShopUIElements[index].GetComponentInChildren<Button>().onClick.AddListener(() => shop.GetComponent<Shop>().BuyItem(index));
        }
    }
    /// <summary>
    /// Show shop ui
    /// </summary>
    /// <param name="active">Enable/Disable</param>
    public void ShowShopUI(bool active, ref Interactable interact)
    {
        interact.interactEvent.Invoke();

        shopOptionButtons.SetActive(active);
    }

    /// <summary>
    /// Open shop ui
    /// </summary>
    /// <param name="active">Enable/Disable</param>
    public void OpenShopWindow(bool active)
    {
        shopUIParent.SetActive(active);
    }
    /// <summary>
    /// Show fishing ui
    /// </summary>
    /// <param name="active">Enable/Disable</param>
    public void ShowFishingUI(bool active)
    {
        fishingUI.SetActive(active);
    }
    /// <summary>
    /// The slider for the fish timer
    /// </summary>
    /// <param name="amount">amount to add or remove</param>
    public void TimerFillAmount(float amount)
    {
        fishingUI.GetComponentInChildren<Slider>().value += amount;
    }
    /// <summary>
    /// Get a random fish
    /// </summary>
    /// <returns>Random integer</returns>
    public void RandomFish(int fishInt)
    {
        fish.sprite = fishes[fishInt].sprite;
    }

    public void EnableUI()
    {
        if (BoatUI.activeSelf)
        {
            BoatUI.SetActive(false);
        }
        else
        {
            BoatUI.SetActive(true);
        }
    }

    public IEnumerator CaughtFishScreen(int fishInt)
    {
        caughtFish.GetComponent<Image>().sprite = fishes[fishInt].sprite;
        caughtFish.gameObject.SetActive(true);
        caughtFishAnimation.clip = caughtFishAnimation.GetClip("FadeInFish");
        caughtFishAnimation.Play();
        yield return new WaitForSeconds(1f);
        caughtFishAnimation.clip = caughtFishAnimation.GetClip("FadeOutFish");
        caughtFishAnimation.Play();
        yield return new WaitForSeconds(caughtFishAnimation.clip.length);
        caughtFish.gameObject.SetActive(false);
    }

    #endregion 

    public void OpenMenu()
    {
        activateMenu.Invoke();
        WholeMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseMenu()
    {
        WholeMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenMainMenu()
    {
        MenuManager.instance.LoadScene(0);
    }

    public void Continue()
    {
        CloseMenu();
    }

    public void OpenSettingsMenu()
    {
        SettingsMenu.SetActive(true);
        WholeMenu.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        SettingsMenu.SetActive(false);
        WholeMenu.SetActive(true);
    }

    public void SetCheckpoint()
    {
        SpawnPointing.instance.SetSpawnPoint();
    }

    public void OpenMenuManager(GameObject currentmenu)
    {
        int currentIndex = Array.IndexOf(menus, currentmenu);
        Debug.Log(currentIndex);

        for (int i = 0; i < menus.Length; i++)
        {
            if (i != currentIndex)
            {
                menus[i].SetActive(false);
            }
        }
    }
}
