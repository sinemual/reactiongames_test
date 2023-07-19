using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [ShowOdinSerializedPropertiesInInspector]
    public class RuntimeData
    {
        public bool IsPause;
        public void PauseGame(bool isPause)
        {
            IsPause = isPause;
            Time.timeScale = isPause ? 0 : 1;
        }
    }
}