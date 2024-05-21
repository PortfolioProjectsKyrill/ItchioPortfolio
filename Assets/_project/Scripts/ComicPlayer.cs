using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ComicPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _comicObj;
    [SerializeField] private Sprite _comic;
    [SerializeField] private float[] readTimes;
    [SerializeField] private float comicSizeScale;
    [SerializeField] private Vector3[] centeredPositions;

    public float TotalPlayTime;

    private void Awake()
    {
        for (int i = 0; i < readTimes.Length; i++)
        {
            TotalPlayTime += readTimes[i];
        }

        TotalPlayTime += (centeredPositions.Length - 1);
    }

    private void Start()
    {
        StartCoroutine(PlayComic());
    }

    private IEnumerator PlayComic()
    {
        float t = 0;
        float maxTime = 1;

        //set first pos
        _comicObj.transform.position = centeredPositions[0];

        for (int i = 0; i < centeredPositions.Length - 1; i++)
        {
            yield return new WaitForSeconds(readTimes[i]);

            Debug.Log("i == to: " + i);

            while (t <= maxTime)
            {
                t += Time.unscaledDeltaTime;

                if (i + 1 <= centeredPositions.Length)
                {
                    _comicObj.transform.position = Vector3.Lerp(_comicObj.transform.position, centeredPositions[i + 1], t / maxTime);
                    yield return null;
                }
            }

            t = 0;
        }
    }
}
