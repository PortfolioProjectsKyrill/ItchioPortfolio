using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject camObj;
    [SerializeField] private Transform camObjChild;
    [SerializeField] private Transform characterObj;

    [Header("Lookat Offset")]
    [SerializeField] private Vector3 lookatOffset;

    [Header("Camera Position")]
    [SerializeField] private Vector3 cameraOffset;

    public bool onBoat;

    private void FixedUpdate()
    {
        if (!onBoat)
        {
            FollowOther();
        }
        else
        {
            DefaultFollow();
        }
    }

    private void DefaultFollow()
    {
        transform.position = Vector3.Lerp(transform.position, camObjChild.position, Time.deltaTime * 6);
        transform.rotation = Quaternion.Lerp(transform.rotation, camObjChild.rotation, Time.deltaTime * 6);

        transform.LookAt(target.position + lookatOffset);
    }

    private void FollowOther()
    {
        if (characterObj)
        {
            transform.position = Vector3.Lerp(transform.position, characterObj.position, Time.deltaTime * 6);
            transform.rotation = Quaternion.Lerp(transform.rotation, characterObj.rotation, Time.deltaTime * 6);
        }
    }
}