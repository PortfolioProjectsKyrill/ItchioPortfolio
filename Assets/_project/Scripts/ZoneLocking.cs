using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ZoneLocking : MonoBehaviour
{
    public static ZoneLocking instance;

    [SerializeField] private List<Collider> zones;
    [SerializeField] private bool[] playerZoneLocation;

    [SerializeField] private Transform player;
    [SerializeField] private PlayerStateMachine playerRef;

    [SerializeField] private bool allowedToBeInZone;
    [SerializeField] private bool testing;
    [SerializeField] private Coroutine c;

    [SerializeField] private int zoneCounter;

    [Header("Wrong Zone Timers")]
    [SerializeField] private float timer;
    [SerializeField] private float maxTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        playerZoneLocation = new bool[zones.Count];
    }

    private void FixedUpdate()
    {
        CheckWhichZone();
        allowedToBeInZone = CheckIfZoneUnlocked();

        if (!allowedToBeInZone && !testing)
        {
            if (timer >= maxTime)
            {
                c ??= StartCoroutine(RemovePlayerFromDisallowedZone());
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private void CheckWhichZone()
    {
        for (int i = 0; i < zones.Count; i++)
        {
            if (zones[i].bounds.Contains(player.transform.position))
            {
                playerZoneLocation[i] = true;
                playerRef.currentZone = zones[i];
            }
            else
            {
                playerZoneLocation[i] = false;
            }
        }
    }

    private bool CheckIfZoneUnlocked()
    {
        //als de zone waar de speler in zit niet unlocked is
        var zone = playerRef.currentZone;
        int currentZoneIndex = zones.IndexOf(zone);
        for (int i = 0; i < playerZoneLocation.Length; i++)
        {
            if (playerZoneLocation[i] == true && zones[currentZoneIndex].GetComponent<Zone>().unlocked == true)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator RemovePlayerFromDisallowedZone()
    {
        print("started removing player");
        GameManager.Instance.StartEndScreenFadeIn("OUT OF BOUNDS");
        //fade out
        yield return new WaitForSeconds(1.3f);

        if (playerRef.GetComponentInParent<BoatInteraction>())
            playerRef.GetComponentInParent<BoatInteraction>().GetOutBoat();

        SpawnPointing.instance.SpawnPlayerAtPoint(SpawnPointing.instance.MostRecentSpawnPoint());
        timer = 0;
        c = null;
    }

    public void UnlockNextZone()
    {
        zoneCounter = 0;

        for (int i = 0; i < zones.Count; i++)
        {
            if (zones[i].GetComponent<Zone>().unlocked)
            {
                zoneCounter++;
            }
        }

        zones[zoneCounter].GetComponent<Zone>().unlocked = true;
    }
}
