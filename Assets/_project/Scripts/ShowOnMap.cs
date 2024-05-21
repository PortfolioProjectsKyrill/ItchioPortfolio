using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ShowOnMap : MonoBehaviour
{
    [Header("Needed")]
    [SerializeField] private Camera Camera;
    [Space]
    [Tooltip("Sprite parent")]
    [SerializeField] private GameObject spriteHolder;
    [Tooltip("Object the cam has to follow")]
    [SerializeField] private GameObject followObject;
    [Tooltip("Instance of the prefab in the scene")]
    [SerializeField] private GameObject spriteHolderInstance;
    [Tooltip("Cam offset from the player to the sprite")]
    [SerializeField] private Vector3 playerOffset;
    [Tooltip("Offset from the player to the camera")]
    [SerializeField] private Vector3 camOffset;
    [Tooltip("Sprite (assign from project files)")]
    [SerializeField] private Material chosenMaterial;
    [SerializeField] private bool hasTextInstead;
    [SerializeField] private bool addToZoneList;

    [Header("Situational")]
    [Tooltip("Only enable for the script on the player")]
    [SerializeField] private bool TopDownCamFollowPlayer;

    [SerializeField] private float camSizeVar;

    [SerializeField] private MapScrollingBehaviour mapScrollingBehaviour;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        followObject = gameObject;
        spriteHolderInstance = Instantiate(spriteHolder, null);//spawn without parent

        if (hasTextInstead && addToZoneList)
        {
            GameObject child = spriteHolderInstance.GetComponentInChildren<MeshRenderer>().gameObject;

            child.AddComponent<TextMeshPro>();
            //child.GetComponent<MeshRenderer>().enabled = false;

            TextMeshPro tmp = child.GetComponent<TextMeshPro>();
            tmp.font = MapController.instance.font;
            tmp.fontSize = MapController.instance.fontSize;
            tmp.color = MapController.instance.textColor;
            tmp.alignment = TextAlignmentOptions.Center;
            Vector3 temp = tmp.gameObject.transform.eulerAngles;
            temp.x = MapController.instance.textRotation.x;
            temp.y = MapController.instance.textRotation.y;
            temp.z = MapController.instance.textRotation.z;
            tmp.gameObject.transform.eulerAngles = temp;
            tmp.GetComponent<RectTransform>().sizeDelta = MapController.instance.textBoxSize;
            MapController.instance.zones.Add(GetComponent<Zone>());
            MapController.instance.zoneTexts.Add(tmp);
        }
        else
        {
            spriteHolderInstance.GetComponentInChildren<MeshRenderer>().material = chosenMaterial;

            spriteHolderInstance.name = transform.name + " mapIcon";

        }

        MapController.instance.spriteObjects.Add(spriteHolderInstance);
    }

    private void FixedUpdate()
    {
        if (TopDownCamFollowPlayer == false && hasTextInstead)
        {
            spriteHolderInstance.GetComponentInChildren<Transform>().localScale = new Vector3(Camera.orthographicSize, Camera.orthographicSize, Camera.orthographicSize);
        }
        else
            spriteHolderInstance.transform.localScale = new Vector3(Camera.orthographicSize / camSizeVar, Camera.orthographicSize / camSizeVar, Camera.orthographicSize / camSizeVar);

        spriteHolderInstance.transform.position = followObject.transform.position + playerOffset;

        //terug gaan van camera na 4 sec
        if (mapScrollingBehaviour != null)
        {
            if (TopDownCamFollowPlayer == true && !mapScrollingBehaviour.isTouching)
            {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, followObject.transform.position + camOffset, Time.deltaTime * 5);
            }
        }
    }
}
