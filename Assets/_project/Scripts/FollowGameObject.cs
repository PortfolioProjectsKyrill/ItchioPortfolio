using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    public GameObject followObject;
    public Vector3 offset;
    public Vector3 startPos;

    private void Start()
    {
        startPos = followObject.transform.position;
        offset = (transform.position - followObject.transform.position);
    }

    void Update()
    {
        Vector3 tempPos = followObject.transform.position;
        tempPos = followObject.transform.position + offset;
        transform.position = tempPos;
    }
}
