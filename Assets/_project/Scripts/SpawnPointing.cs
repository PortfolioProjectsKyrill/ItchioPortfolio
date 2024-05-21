using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnPointing : MonoBehaviour
{
    public static SpawnPointing instance;
    public BoatController BoatController;

    [Header("Objects that need saving")]
    public GameObject player;
    public GameObject boat;

    public List<Vector3> playerSpawnPoints;

    [Header("boat")]
    public List<Vector3> boatSpawnPoints;
    public List<Quaternion> boatSpawnPointsRotations;

    public List<float> BoatFuel;

    public float autoSaveTimer;
    public float autoSaveTime;

    public LayerMask autoSaveLayerMask;

    public TextMeshProUGUI savedText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        SetPlayerCheckPoint();
        autoSaveTime = 0;
    }

    private void FixedUpdate()
    {
        if (autoSaveTime > autoSaveTimer)
        {
            if (player.GetComponent<PlayerStateMachine>().CanSetSpawnPoint(autoSaveLayerMask))
            {
                SetSpawnPoint();
            }
            autoSaveTime = 0;
        }

        autoSaveTime += Time.deltaTime;
    }

    public void SetSpawnPoint()
    {
        SetPlayerCheckPoint();
        SetBoatCheckPoint();
        Debug.Log("added spawnpoint at: " + Time.timeSinceLevelLoad + "at index: " + playerSpawnPoints.Count);
        StartCoroutine(ShowText());
        autoSaveTime = 0;
    }

    public void RemoveMostRecentSpawnpoint()
    {
        playerSpawnPoints.RemoveAt(playerSpawnPoints.Count);
        boatSpawnPoints.RemoveAt(boatSpawnPoints.Count);
        boatSpawnPointsRotations.RemoveAt(boatSpawnPointsRotations.Count);
        BoatFuel.RemoveAt(BoatFuel.Count);
        Debug.Log("added spawnpoint at: " + Time.timeSinceLevelLoad + "at index: " + playerSpawnPoints.Count);
    }

    public void SpawnPlayerAtPoint(int index)
    {
        SetPlayerPos(index);
        SetBoatPos(index);
        Physics.SyncTransforms();
    }

    /// <summary>
    /// shows the text "saved checkpoint" at bottom of screen
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShowText()
    {
        if (savedText != null)
        {
            savedText.alpha = 0f;

            savedText.gameObject.SetActive(true);

            for (int i = 0; i < 255; i++)
            {
                savedText.alpha += 0.0039f;
                yield return new WaitForSeconds(0.004f);
            }

            for (int i = 0; i < 255; i++)
            {
                savedText.alpha -= 0.0039f;
                yield return new WaitForSeconds(0.004f);
            }

            savedText.gameObject.SetActive(false);

            savedText.alpha = 1f;
        }
    }

    public int MostRecentSpawnPoint()
    {
        return playerSpawnPoints.Count - 1;//to account for index startin at 0
    }

    private void SetBoatPos(int index)
    {
        if (boat)
        {
            boat.transform.position = boatSpawnPoints[index];
            boat.transform.rotation = boatSpawnPointsRotations[index];
            BoatController.totalFuel = BoatFuel[index];
        }
    }

    private void SetPlayerPos(int index)
    {
        if (player)
        {
            player.transform.position = playerSpawnPoints[index];
        }
    }

    private void SetPlayerCheckPoint()
    {
        if (player) 
        {
            playerSpawnPoints.Add(player.transform.position);
        }
    }

    private void SetBoatCheckPoint()
    {
        if (boat)
        {
            boatSpawnPoints.Add(boat.transform.position);
            boatSpawnPointsRotations.Add(boat.transform.rotation);
            BoatFuel.Add(BoatController.totalFuel);
        }
    }
}
