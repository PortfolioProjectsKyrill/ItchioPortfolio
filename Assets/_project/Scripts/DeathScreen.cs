using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private float fadeTime;

    public GameObject quests;
    private void Start()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        CanvasGroup.alpha = 0f;
        textMeshProUGUI.gameObject.SetActive(false);
    }

    public IEnumerator FadeInScreen(string text = null)
    {
        print("running");
        if (text != null) 
            textMeshProUGUI.text = text;

        textMeshProUGUI.gameObject.SetActive(true);
        gameObject.SetActive(true);

        for (int i = 0; i < 255; i++)
        {
            CanvasGroup.alpha += 0.012f;
            yield return new WaitForSeconds(fadeTime / 255);
        }

        CanvasGroup.alpha = 1f;

        for (int i = 0; i < 255; i++)
        {
            CanvasGroup.alpha -= 0.012f;
            yield return new WaitForSeconds(fadeTime / 255);
        }

        CanvasGroup.alpha = 0f;

        textMeshProUGUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
        textMeshProUGUI.text = "YOU DIED";
        GameManager.Instance.respawnCoroutine = null;
    }
}
