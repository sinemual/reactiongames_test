using System;
using UnityEngine;

[Serializable]
public struct PlayerUnitProvider
{
    public GameObject Pointer;
    [HideInInspector] public int Number;
}