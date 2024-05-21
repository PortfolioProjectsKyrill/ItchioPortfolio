using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedOMeter : MonoBehaviour
{
    [SerializeField] private GameObject pointer;
    [SerializeField] private float minDeg;
    [SerializeField] private float maxDeg;

    [SerializeField] private BoatController boat;

    [SerializeField] private TextMeshProUGUI speed;
    private void FixedUpdate()
    {
        speed.text = Mathf.Clamp(boat.speed, 0, 10).ToString("0");
        float rotationAngle = Mathf.Lerp(minDeg, maxDeg, boat.speed / 10);

        // Rotate the needle
        pointer.transform.localEulerAngles = new Vector3(0f, 0f, rotationAngle);
    }
}
