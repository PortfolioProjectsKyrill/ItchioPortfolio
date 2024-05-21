using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPlatformsVisibility : MonoBehaviour
{
    public GameObject[] landingPlatforms;
    public BoatInteraction boatInteraction;

    public bool areEnabled;

    private void Start()
    {
        DisableLandingPlatforms();

        if (boatInteraction == null)
        {
            boatInteraction = GameObject.Find("Boat (1)").GetComponent<BoatInteraction>();
        }
    }

    private void EnableLandingPlatforms()
    {
        areEnabled = true;

        for (int i = 0; i < landingPlatforms.Length; i++) 
        {
            landingPlatforms[i].SetActive(true);
            landingPlatforms[i].GetComponentInParent<LandingPlatform>().canStart = true;
        }
    }

    private void DisableLandingPlatforms()
    {
        areEnabled = false;

        for (int i = 0; i < landingPlatforms.Length; i++)
        {
            landingPlatforms[i].GetComponentInParent<LandingPlatform>().canStart = false;
            landingPlatforms[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (boatInteraction.inBoat && !areEnabled)
        {
            EnableLandingPlatforms();
        }
        else if (!boatInteraction.inBoat && areEnabled) 
        {
            DisableLandingPlatforms();
        }
    }
}
