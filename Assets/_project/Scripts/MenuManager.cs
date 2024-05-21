using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public GameObject choice;

    public int cinematicSceneIndex;

    public bool tutorial;

    public Sprite[] loadingScreens;

    public Vector2[] vectors;

    public GameObject spriteInstance;

    public Canvas canvas;

    private int height;
    private int width;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(canvas);
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        height = Screen.height;
        width = Screen.width;


        vectors = new Vector2[3];
        vectors = CalculateVectorPositions();

        SetSprite();
    }

    private void SceneSwitch(int index)
    {
        if (index == 0)
        {
            Destroy(canvas.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void StartGame()
    {
        choice.SetActive(true);
    }

    public void LoadScene(int index)
    {
        StartCoroutine(PlayLoadingScreen(index));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Yes()
    {
        tutorial = true;
    }

    public void No()
    {
        tutorial = false;
    }

    private IEnumerator PlayLoadingScreen(int index)
    {
        float t = 0;
        float maxT = 1;

        while (t <= maxT)
        {
            t += Time.unscaledDeltaTime;
            spriteInstance.transform.position = Vector2.Lerp(spriteInstance.transform.position, vectors[1], t / maxT);
            yield return null;
        }

        if (t >= maxT)
        {
            spriteInstance.transform.position = vectors[1];
            t = 0;
        }

        SceneSwitch(index);

        SceneManager.LoadScene(index);

        while (t <= maxT)
        {
            t += Time.unscaledDeltaTime;
            spriteInstance.transform.position = Vector2.Lerp(spriteInstance.transform.position, vectors[2], t / maxT);
            yield return null;
        }

        if (t >= maxT)
        {
            spriteInstance.transform.position = vectors[2];
        }

        SetSprite();
    }

    public void SetSprite()
    {
        spriteInstance.GetComponent<RectTransform>().position = vectors[0];
        Sprite sprite = GrabRandomSprite();
        spriteInstance.GetComponent<Image>().sprite = sprite;
    }

    public Sprite GrabRandomSprite()
    {
        return loadingScreens[Random.Range(0, loadingScreens.Length)];
    }

    /// <summary>
    /// calculate vector positions based on screensize
    /// </summary>
    private Vector2[] CalculateVectorPositions()
    {
        Vector2[] points = new Vector2[3];

        points[0] = new Vector2(width / 2, (height / 2) + (height + 10));
        points[1] = new Vector2(width / 2, height / 2);
        points[2] = new Vector2(width / 2, (height / 2) - (height + 10));

        return points;
    }
}
