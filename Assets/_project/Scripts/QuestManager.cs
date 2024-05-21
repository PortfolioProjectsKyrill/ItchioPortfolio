using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public TextMeshProUGUI[] textMeshProUGUI;
    public bool[] questStates;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        questStates = new bool[textMeshProUGUI.Length];

        for (int i = 0; i < textMeshProUGUI.Length; i++)
        {
            textMeshProUGUI[i].color =  Color.red;
        }
    }

    public void CompleteQuest(int boolIndex)
    {
        if (questStates[boolIndex])
            return;

        questStates[boolIndex] = true;
        textMeshProUGUI[boolIndex].color = Color.green;
    }
}
