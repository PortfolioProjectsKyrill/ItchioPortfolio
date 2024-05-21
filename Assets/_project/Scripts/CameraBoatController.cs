using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;

public class CameraBoatController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject camObj;
    [SerializeField] private GameObject camObjChild;

    [SerializeField] private float carDegrees;
    [SerializeField] private float camDegrees;

    [SerializeField] private float rotAmount;

    [SerializeField] private float turnbackTime;
    [SerializeField] private float t;

    [Header("Clamp values")]
    [SerializeField] private float minClamp;
    [SerializeField] private float maxClamp;

    [SerializeField] private float minClampBoost;
    [SerializeField] private float maxClampBoost;


    private void FixedUpdate()
    {
        SetDegrees();
        DynamicClamps();

        //wanneer de speler geen input geeft
        if (!Input.GetMouseButton(0))
        {
            //Lerp carDegrees to start value
            if (!AtStartRotation())
            {
                TurnBackCam();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            CamTurning();
        }
    }

    private void SetDegrees()
    {
        carDegrees = Mathf.Clamp(carDegrees, minClamp, maxClamp);
        Vector3 quaternion = camObj.transform.eulerAngles;
        quaternion.y = carDegrees + camDegrees;
        camObj.transform.eulerAngles = quaternion;

        carDegrees = target.transform.eulerAngles.y;
    }

    private void DynamicClamps()
    {
        minClamp = carDegrees + minClampBoost;
        maxClamp = carDegrees + maxClampBoost;
    }

    private void CamTurning()
    {
        if (Input.GetAxis("Mouse X") < 0)
        {
            camDegrees -= rotAmount;
        }
        else if (Input.GetAxis("Mouse X") > 0)
        {
            camDegrees += rotAmount;
        }
        camDegrees = Mathf.Clamp(camDegrees, minClampBoost, maxClampBoost);
    }

    private void TurnBackCam()
    {
        if (t < turnbackTime)
        {
            t += Time.deltaTime;
        }
        else
        {
            if (t > turnbackTime) { t = turnbackTime; }

            camDegrees = Mathf.Lerp(camDegrees, 0f, Time.deltaTime * 6);

            if (AtStartRotation())
            {
                t = 0;
            }
        }
    }

    private bool AtStartRotation()
    {
        if (camDegrees > -0.3f && camDegrees < 0.3f)
        {
            return true;
        }
        else { return false; }
    }
}