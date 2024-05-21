using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BoatInteraction : MonoBehaviour
{
    private BoatController controller;
    private CameraBoatController cameraBoatController;

    [SerializeField] private Camera cam;

    [SerializeField] private GameObject charPos;
    [SerializeField] private GameObject charPos2;
    [SerializeField] private GameObject Player;
    public bool inBoat;
    [SerializeField] private bool inBoatArea;

    public UnityEvent enableUI;

    private Rigidbody rb;
    private Interactable self;

    private void Start()
    {
        controller = GetComponent<BoatController>();
        cameraBoatController = GetComponent<CameraBoatController>();
        rb = GetComponent<Rigidbody>();
        self = GetComponent<Interactable>();
    }

    private void FixedUpdate()
    {
        if (inBoat)
        {
            Player.transform.position = charPos.transform.position;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inBoatArea && controller.hasLanded)
        {
            if (inBoat)
            {
                GetOutBoat();
            }
            else if (!inBoat)
            {
                GetInBoat();
            }
        }

        if (self.inRadius)
        {
            inBoatArea = true;
        }
        else
        {
            inBoatArea = false;
        }
    }

    private void GetInBoat()
    {
        Player.GetComponent<PlayerStateMachine>().DisablePlayerMovement(false);
        Player.GetComponent<PlayerStateMachine>().inBoat = true;
        Player.GetComponent<CapsuleCollider>().enabled = false;
        Player.transform.position = charPos.transform.position;
        Player.transform.rotation = charPos.transform.rotation;
        Player.GetComponent<Rigidbody>().useGravity = false;
        Player.transform.parent = gameObject.transform;
        inBoat = true;
        GetComponentInParent<BoatController>().canUseBoat = true;
        cam.GetComponent<CameraFollow>().onBoat = true;
        inBoatArea = true;
        GetComponent<Interactable>().interactEvent.Invoke();

        Physics.SyncTransforms();
        enableUI.Invoke();
    }

    public void GetOutBoat()
    {
        if (controller != null && !controller.hasLanded)
        {
            controller.LandBoatStart();
            controller.hasLanded = true;
            controller.speed = 0;
        }

        Player.GetComponent<PlayerStateMachine>().inBoat = false;
        Player.GetComponent<Rigidbody>().useGravity = true;

        Player.transform.position = charPos2.transform.position;
        Player.transform.rotation = charPos2.transform.rotation;

        Player.GetComponent<CapsuleCollider>().enabled = true;

        Player.GetComponent<PlayerStateMachine>().EnablePlayerMovement();

        Player.transform.parent = null;
        inBoat = false;

        GetComponentInParent<BoatController>().canUseBoat = false;
        cam.GetComponent<CameraFollow>().onBoat = false;
        inBoatArea = false;
        GetComponent<Interactable>().interactEvent.Invoke();

        Physics.SyncTransforms();
        enableUI.Invoke();
    }
}
