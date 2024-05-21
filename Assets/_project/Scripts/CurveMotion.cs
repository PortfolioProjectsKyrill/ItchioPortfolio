using System.Collections;
using UnityEngine;

public class CurveMotion : MonoBehaviour
{
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
    public Transform startPoint;
    public Transform endPoint;
    public float rotationAngle = 40f; // Set the rotation angle in the Inspector
    public float duration = 2f;

    private bool _isNormalCam = true;
    private PlayerStateMachine _playerStateMachine;
    private Vector3 initialPosition;

    private void Awake()
    {
        _playerStateMachine = FindObjectOfType<PlayerStateMachine>();
    }

    void Start()
    {
        // Store the initial position of the camera
        initialPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(MoveObject());
        }
    }

    IEnumerator MoveObject()
    {
        float currentTime = 0f;
        bool _lerpIsDone = false;

        while (currentTime < duration)
        {
            float t = currentTime / duration;
            float curveValue = curve.Evaluate(t);

            if (_isNormalCam)
            {
                // Close Up
                transform.position = Vector3.Lerp(initialPosition, endPoint.position, curveValue);

                // Get the player's y rotation
                float playerYRotation = _playerStateMachine.transform.rotation.eulerAngles.y;

                // Rotate the camera based on the curve and the player's y rotation
                transform.rotation = Quaternion.Euler((1 - curveValue) * rotationAngle, playerYRotation,
                    transform.rotation.eulerAngles.z);

                _playerStateMachine.enabled = false;
            }
            else
            {
                // Back to normal camera
                transform.position = Vector3.Lerp(endPoint.position, initialPosition, curveValue);

                // Get the player's y rotation
                float playerYRotation = _playerStateMachine.transform.rotation.eulerAngles.y;
                // Rotate the camera based on the curve and the player's y rotation
                transform.rotation = Quaternion.Euler(curveValue * rotationAngle, playerYRotation,
                    transform.rotation.eulerAngles.z);
                _lerpIsDone = true;
            }

            currentTime += Time.deltaTime;
            yield return null;
        }

        if (_lerpIsDone)
        {
            _playerStateMachine.enabled = true;
        }

        _isNormalCam = !_isNormalCam;
    }
}
