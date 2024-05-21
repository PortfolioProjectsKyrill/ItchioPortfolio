using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScrollingBehaviour : MonoBehaviour
{
    [SerializeField] private Camera orthoCam;

    [Header("Vector3 Touch")]
    [SerializeField] private Vector3 _touchstart;

    public bool isTouching;

    [SerializeField] private float timeSinceTouch;
    [SerializeField] private float timeSinceTouchMax;
    private void Update()
    {
        orthoCam.orthographicSize -= Input.mouseScrollDelta.y * 10;
        orthoCam.orthographicSize = Mathf.Clamp(orthoCam.orthographicSize, 10, 300);

        if (Input.GetMouseButtonDown(0))
        {
            isTouching = true;
            timeSinceTouch = 0;
            _touchstart = orthoCam.ScreenToWorldPoint(Input.mousePosition);
        }
        //when user keeps holding
        else if (Input.GetMouseButton(0))
        {
            //calculate move amount by subtracting ,mouse position on screen, off touch start set before
            Vector3 direction = _touchstart - orthoCam.ScreenToWorldPoint(Input.mousePosition);
            //apply new position
            orthoCam.transform.position += direction;
        }
        else
        {
            if (timeSinceTouch >= timeSinceTouchMax && isTouching)
            {
                timeSinceTouch = 0;
                isTouching = false;
            }
            else
            {
                timeSinceTouch += Time.deltaTime;
            }
        }
    }
}
