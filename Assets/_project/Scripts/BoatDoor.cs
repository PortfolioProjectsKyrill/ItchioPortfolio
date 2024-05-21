using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatDoor : MonoBehaviour
{
    [SerializeField] private ObjectFade objectFade;
    [SerializeField] private GameObject objToFade;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(objectFade.FadeOut(objToFade));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(objectFade.FadeIn(objToFade));
        }
    }
}
