using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum FishStates
{
    up,
    down
}

public class Fishing : MonoBehaviour
{
    public static Fishing instance;

    [SerializeField] private SellManager sellManager;
    private PlayerStateMachine playerStateMachine;
    private FishStates fishState = FishStates.up;
    private Rarities fishRarity;

    public bool isFishing;
    [SerializeField] private float fishBarSpeed;
    [SerializeField] private float fishSpeed;
    public float movingFishTimer;
    public int currentFish;

    [SerializeField] private RectTransform fishBarRect;
    [SerializeField] private RectTransform fishRect;

    [SerializeField] private GameObject dobber;
    [SerializeField] private GameObject fishingRod;

    [SerializeField] private Interactable tempInteractable;
    [SerializeField] private bool tutorial;
    public UnityEvent tutorialEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        fishBarRect = UIManager.instance.fishBarRect;
        fishRect = UIManager.instance.fishRect;
    }

    public void FixedUpdate()
    {
        //If the player is fishing
        if (isFishing)
        {
            //If the timer for chaning the direction is 0
            if (movingFishTimer <= 0)
            {
                if (fishRect.anchoredPosition.y > 125)
                {
                    fishState = FishStates.down;
                }
                else if (fishRect.anchoredPosition.y < -125)
                {
                    fishState = FishStates.up;
                }
                else
                {
                    int random = UnityEngine.Random.Range(0, 2);
                    fishState = ((FishStates)random);
                }
                //Set random time
                movingFishTimer = UnityEngine.Random.Range(0.8f, 1.2f);
            }
            else
            {
                //Set temporary variable
                Vector2 tempfish = Vector2.zero;
                tempfish = fishRect.anchoredPosition;
                switch (fishState)
                {
                    //If the fish direction is up, move it towards the direction
                    case FishStates.up:
                        tempfish = new Vector2(fishRect.anchoredPosition.x, fishRect.anchoredPosition.y + fishSpeed);
                        break;
                    //If the fish direction is down, move it towards the direction
                    case FishStates.down:
                        tempfish = new Vector2(fishRect.anchoredPosition.x, fishRect.anchoredPosition.y - fishSpeed);
                        break;
                }
                //Clamp the fish
                tempfish.y = Mathf.Clamp(tempfish.y, -150, 150);
                //Set fish to position of temp fish
                fishRect.anchoredPosition = tempfish;
            }
            //Remove time from the itmer
            movingFishTimer -= Time.deltaTime;
            //Set temporary variable
            Vector2 temp = Vector2.zero;
            temp = fishBarRect.anchoredPosition;
            //If player is holding mouse button 0
            if (Input.GetMouseButton(0))
            {
                //Move the bar up
                temp = new Vector2(fishBarRect.anchoredPosition.x, fishBarRect.anchoredPosition.y + fishBarSpeed);
            }
            else
            {
                //Move the bar down
                temp = new Vector2(fishBarRect.anchoredPosition.x, fishBarRect.anchoredPosition.y - fishBarSpeed);
            }
            //Set the clamp
            temp.y = Mathf.Clamp(temp.y, -200, 200);
            //Set bar position to temp location
            fishBarRect.anchoredPosition = temp;
            //Check if fish is on the bar
            if (fishRect.anchoredPosition.y < fishBarRect.anchoredPosition.y + fishBarRect.rect.height / 2 && fishRect.anchoredPosition.y > fishBarRect.anchoredPosition.y - fishBarRect.rect.height / 2)
            {
                //If the slider is not full yet
                if (UIManager.instance.fishingUI.GetComponentInChildren<Slider>().value < 1)
                {
                    //Add value to slider
                    UIManager.instance.TimerFillAmount(0.001f / (int)fishRarity);
                    print(0.001f / (int)fishRarity);
                }
                else
                {
                    StartCoroutine(StopCatchingFish());
                }
            }
            else
            {
                //If the slider above 0
                if (UIManager.instance.fishingUI.GetComponentInChildren<Slider>().value > 0)
                {
                    //Remove value from slider
                    UIManager.instance.TimerFillAmount(-0.00025f * (int)fishRarity);
                }
                else
                {
                    //Disable fishing ui
                    UIManager.instance.ShowFishingUI(false);
                    dobber.transform.position = new Vector3(10000, 10000, 10000);
                    //Disable fishing
                    isFishing = false;
                    playerStateMachine.isFishing = false;
                    fishingRod.SetActive(false);
                    playerStateMachine.EnablePlayerMovement();
                }
            }
        }
    }
    /// <summary>
    /// Start Fishing
    /// </summary>
    public IEnumerator StartCatchingFish(List<int> activeFishes, GameObject obj, Interactable interactable)
    {
        tempInteractable = interactable;
        fishingRod.SetActive(true);
        playerStateMachine.isFishing = true;
        dobber.transform.position = obj.transform.GetComponent<BoxCollider>().bounds.center;
        yield return new WaitForSeconds(1f);
        //Enable fishing ui
        UIManager.instance.ShowFishingUI(true);
        //Enable fishing
        isFishing = true;
        //Set fish to catch
        currentFish = RandomFishInt(activeFishes);
        fishRarity = sellManager.fishTypes[currentFish].GetComponent<FishValue>().fishRarity;
        //Set sprite to chosen fish
        UIManager.instance.RandomFish(currentFish);
        //Reset slider value
        UIManager.instance.fishingUI.GetComponentInChildren<Slider>().value = 0;
        //Reset fish and bar positions
        fishRect.anchoredPosition = new Vector2(fishBarRect.anchoredPosition.x, 0);
        fishBarRect.anchoredPosition = new Vector2(fishBarRect.anchoredPosition.x, 0);
        //Reset Timer
        movingFishTimer = 0.1f;
        UIManager.instance.TimerFillAmount(0.05f);
        tempInteractable.inRadiusEvent.Invoke();
        yield return null;
    }

    public IEnumerator StopCatchingFish()
    {
        tempInteractable.interactEvent.Invoke();
        //Disable fishing ui
        UIManager.instance.ShowFishingUI(false);
        //Disable fishing
        isFishing = false;
        StartCoroutine(UIManager.instance.CaughtFishScreen(currentFish));
        if (tutorial)
        {
            tutorialEvent.Invoke();
        }
        //Add to inventory
        if (sellManager != null)
            sellManager.activeInventory.AddToInventory(currentFish);

        yield return new WaitForSeconds(UIManager.instance.caughtFishAnimation.clip.length);
        dobber.transform.position = new Vector3(10000, 10000, 10000);
        playerStateMachine.isFishing = false;
        fishingRod.SetActive(false);
        playerStateMachine.EnablePlayerMovement();

        yield return null;
    }
    /// <summary>
    /// Get a random int for the fish array
    /// </summary>
    /// <param name="activeFishes">List with all fishes that you can catch at an island.</param>
    /// <returns>Random Integer</returns>
    public int RandomFishInt(List<int> activeFishes)
    {
        int random = UnityEngine.Random.Range(0, 100);
        Debug.Log(random);
        int cumulativeChance = 0;
        int finalChance = 0;
        List<int> potentionalFishes = new List<int>();
        foreach (var rarityChance in sellManager.rarityChances)
        {
            cumulativeChance += rarityChance;
            if (random <= cumulativeChance)
            {
                finalChance = rarityChance;
            }
        }
        for (int i = 0; i < activeFishes.Count; i++)
        {
            if (sellManager.fishTypes[activeFishes[i]].GetComponent<FishValue>().chance >= finalChance)
            {
                potentionalFishes.Add(activeFishes[i]);
            }
        }
        int rand = UnityEngine.Random.Range(0, potentionalFishes.Count);
        Debug.Log(rand);
        return activeFishes[rand];
    }
}
