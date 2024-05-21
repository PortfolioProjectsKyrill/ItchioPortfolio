using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidCollider : MonoBehaviour
{
    private Coroutine coroutine;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("player died");
        coroutine ??= StartCoroutine(YouDiedScreen());
    }

    private IEnumerator YouDiedScreen()
    {
        //show you died screen like in gta
        GameManager.Instance.StartEndScreenFadeIn();
        //fade out
        yield return new WaitForSeconds(1.3f);
        SpawnPointing.instance.SpawnPlayerAtPoint(SpawnPointing.instance.MostRecentSpawnPoint());
    }
}
