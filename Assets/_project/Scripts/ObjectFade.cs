using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 1.0f;
    /// <summary>
    /// Fade out object
    /// </summary>
    /// <param name="obj">Object to fade</param>
    /// <returns></returns>
    public IEnumerator FadeOut(GameObject obj)
    {
        //Get meshrenderer and color
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        Color color = meshRenderer.materials[0].color;

        //While object color not transparent
        while (color.a > 0)
        {
            //Fade color out
            color.a -= fadeSpeed * Time.deltaTime;
            //Update color
            meshRenderer.materials[0].color = color;
            yield return new WaitForSeconds(0.001f);
        }
        //Wait till color is transparent
        yield return new WaitUntil(() => meshRenderer.materials[0].color.a <= 0f);
    }
    /// <summary>
    /// Fade in object
    /// </summary>
    /// <param name="obj">Object to fade in</param>
    /// <returns></returns>
    public IEnumerator FadeIn(GameObject obj)
    {
        //Get meshrenderer and color
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        Color color = meshRenderer.materials[0].color;

        //While object is still transparent
        while (color.a < 1)
        {
            //Fade color in
            color.a += fadeSpeed * Time.deltaTime;
            //Update color
            meshRenderer.materials[0].color = color;
            yield return new WaitForSeconds(0.001f);
        }
        //Wait till color is not transparent anymore
        yield return new WaitUntil(() => meshRenderer.materials[0].color.a >= 1f);
    }
}
