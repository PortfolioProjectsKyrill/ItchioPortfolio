using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public bool activated;
    [SerializeField] private float t;
    [SerializeField] private float maxTime;

    [SerializeField] private GameObject winScreen;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            t += Time.deltaTime;
            if (!activated && t >= maxTime)
            {
                StartCoroutine(Win());
                activated = true;
            }
        }
    }

    public IEnumerator Win()
    {
        QuestManager.instance.CompleteQuest(6);
        winScreen.SetActive(true);
        yield return new WaitForSeconds(5);
        MenuManager.instance.LoadScene(0);
    }
}
