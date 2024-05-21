using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LandingPlatform : MonoBehaviour
{
    public Vector3 landPos;
    public Quaternion landRot;

    private float minA = 0;
    private float maxA;

    private Transform player;
    private MeshRenderer meshRenderer;
    private Coroutine coroutine;
    private Coroutine coroutine2;

    public bool canStart;

    BoatController boatController;

    private void Start()
    {
        Transform childTransform = GetComponentInChildren<Transform>();
        landPos = childTransform.position;
        landRot = childTransform.rotation;

        player = SpawnPointing.instance.player.transform;
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        maxA = meshRenderer.material.color.a;
    }

    private void OnTriggerEnter(Collider other)
    {
        boatController = other.GetComponent<BoatController>();

        //let player script know that it can land
        if (boatController)
        {
            boatController.canLand = true;
            boatController.currentPlatform = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BoatController>())
        {
            other.GetComponent<BoatController>().canLand = false;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.position) > GameManager.Instance.landingPlatformActDist && canStart)
        {
            if (meshRenderer.material.color.a > 0)
                coroutine ??= StartCoroutine(FadeOutBox());
        }
        else if (Vector3.Distance(transform.position, player.position) <= GameManager.Instance.landingPlatformActDist)
        {
            if (meshRenderer.material.color.a <= 0.1f)
                coroutine2 ??= StartCoroutine(FadeInBox());
        }
    }

    public IEnumerator FadeOutBox()
    {
        Color color = meshRenderer.material.color;

        maxA = color.a;

        for (int i = 0; i < 100; i++)
        {
            color.a -= 0.00338f;
            meshRenderer.material.color = color;
            yield return new WaitForSeconds(0.01f);
        }

        color.a = minA;
        meshRenderer.material.color = color;
    }

    public IEnumerator FadeInBox()
    {
        Color color = meshRenderer.material.color;


        minA = color.a;

        for (int i = 0; i < 100; i++)
        {
            color.a += 0.00338f;
            meshRenderer.material.color = color;
            yield return new WaitForSeconds(0.01f);
        }

        color.a = maxA;
        meshRenderer.material.color = color;
    }
}
