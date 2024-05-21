using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarities
{
    none,
    common,
    uncommon,
    rare,
    epic,
    legendary
}

public class FishValue : MonoBehaviour
{
    public float value;
    public Rarities fishRarity;
    public int chance;
}
