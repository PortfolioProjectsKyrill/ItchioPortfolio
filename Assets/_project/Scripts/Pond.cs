using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pond : MonoBehaviour
{
    public bool[] canCatch;
    private bool inRadius;

    private Interactable interactable;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void Update()
    {
        if (!Fishing.instance.isFishing)
        {
            //If player presses E and is in radius
            if (Input.GetKeyDown(KeyCode.E) && inRadius)
            {
                interactable.hasInteracted = true;
                //Get the bool values
                List<int> activeBools = new List<int>();
                //Loop bools
                for (int i = 0; i < canCatch.Length; i++)
                {
                    //If the bool is active
                    if (canCatch[i])
                    {
                        activeBools.Add(i);
                    }
                }
                StartCoroutine(Fishing.instance.StartCatchingFish(activeBools, this.gameObject, interactable));

                if (QuestManager.instance != null)
                {
                    if (!QuestManager.instance.questStates[3])
                    {
                        QuestManager.instance.CompleteQuest(3);
                    }
                }
            }
            else if (!inRadius)
            {
                //interactable.
            }
        }

        //If player in interactable radius
        if (interactable.inRadius)
        {
            inRadius = true;
        }
        else
        {
            inRadius = false;
        }
    }
}
