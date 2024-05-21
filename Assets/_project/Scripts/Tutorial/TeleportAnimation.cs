using System.Collections;
using UnityEngine;

public class TeleportAnimation : MonoBehaviour
{
    public float spinSpeed = 1020f;
    public float buildupTime = 2f;
    public float lerpingTime = 2f;
    public Transform teleportTarget;

    private bool isTeleporting = false;

    private void Update()
    {
        
        if (isTeleporting)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Teleport();
        }
        
        SpinWithBuildupEffect();
    }

    private void SpinWithBuildupEffect()
    {
        float currentTime = Time.time;
        float normalizedTime = Mathf.Clamp01(currentTime / buildupTime);
        float currentSpinSpeed = Mathf.Lerp(0f, spinSpeed, normalizedTime);

        transform.Rotate(Vector3.up, currentSpinSpeed * Time.deltaTime);
    }

    public void Teleport()
    {
        StartCoroutine(TeleportCoroutine());
    }

    private IEnumerator TeleportCoroutine()
    {
        isTeleporting = true;

        // Lerping out of the screen
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + 10f, initialPosition.z); // Adjust the y value as needed

        while (elapsedTime < lerpingTime)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / lerpingTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Teleporting to the specified coordinates
        transform.position = teleportTarget.position;

        // Lerping into the new position
        elapsedTime = 0f;
        initialPosition = transform.position;
        targetPosition = new Vector3(initialPosition.x, initialPosition.y - 10f, initialPosition.z); // Adjust the y value as needed

        while (elapsedTime < lerpingTime)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / lerpingTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Stop spinning with build-off
        float currentSpinSpeed = spinSpeed;
        while (currentSpinSpeed > 0f)
        {
            currentSpinSpeed = Mathf.Lerp(0f, spinSpeed, elapsedTime / buildupTime);
            transform.Rotate(Vector3.up, currentSpinSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isTeleporting = false;
    }
}
