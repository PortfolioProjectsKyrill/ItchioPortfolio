using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    /// <summary>
    /// You place this script on the object you want the player to interact with.
    /// interactEvent UnityEvent gets invoked in the script/place you toggle the canvas/interaction on/off
    /// inRadiusEvent UnityEvent gets invoked in the Playerinteraction script, you don't have to interact with this.
    /// </summary>


    public bool inRadius;
    public UnityEvent inRadiusEvent;
    public UnityEvent interactEvent;

    [SerializeField] private GameObject InteractionKey;
    public bool hasInteracted;
    public Canvas canvas;

    public GameObject WorldObject;
    public RectTransform UI_Element;
    public static bool canSet;

    private void Start()
    {
        if (InteractionKey != null)
            HideInteraction();
    }

    public void EventFunction()
    {
        if (!inRadius)
        {
            HideInteraction();
        }
        else if (inRadius && hasInteracted)
        {
            HideInteraction();
        }
        else if (inRadius && !hasInteracted)
        {
            ShowInteraction();
        }
    }

    private void ShowInteraction()
    {
        UI_Element.gameObject.SetActive(true);
    }

    private void HideInteraction()
    {
        UI_Element.gameObject.SetActive(false);
    }

    private void FollowWorldToCanvas()
    {
        //first you need the RectTransform component of your canvas
        RectTransform CanvasRect = canvas.GetComponent<RectTransform>();

        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(WorldObject.transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        UI_Element.anchoredPosition = WorldObject_ScreenPosition;
    }

    public void HasInteracted()
    {
        if (hasInteracted)
        {
            hasInteracted = false;
        }
        else
        {
            hasInteracted = true;
        }
    }

    private void FixedUpdate()
    {
        FollowWorldToCanvas();
    }
}
