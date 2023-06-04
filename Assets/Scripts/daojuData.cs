using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class daojuData
{
    public GameObject prop;
    public int cost;
    public int innerCD;
    public PropType type;
}

public enum PropType
{
    slowDown,
    frenzy,
    gainmoney
}
