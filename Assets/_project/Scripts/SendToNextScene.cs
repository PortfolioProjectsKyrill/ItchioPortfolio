using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SendToNextScene : MonoBehaviour
{
    float length;
    [SerializeField] private float t;
    [SerializeField] private int tutorialSceneIndex;
    [SerializeField] private int mainSceneIndex;

    private bool canSwitch;

    private void Start()
    {
        length = GetComponentInParent<ComicPlayer>().TotalPlayTime;
        canSwitch = true;
    }

    private void Update()
    {
        //if (MenuManager.instance == null)
        //{
        //    Debug.Log("Menumanager is null");
        //}

        if (t <= length + 1)
        {
            t += Time.unscaledDeltaTime;
        }
        else if (t > length + 1 && canSwitch)
        {
            canSwitch = false;
            DecideWhichSceneNext();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DecideWhichSceneNext();
        }
    }

    private void DecideWhichSceneNext()
    {
        if (MenuManager.instance.tutorial)
            MenuManager.instance.LoadScene(tutorialSceneIndex);
        else
            MenuManager.instance.LoadScene(mainSceneIndex);
    }
}
