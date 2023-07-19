using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PathProvider
{
    public List<Transform> Value;
    public bool IsLoop;
    public SpawnPointMonoProvider SpawnPointMonoProvider;
}