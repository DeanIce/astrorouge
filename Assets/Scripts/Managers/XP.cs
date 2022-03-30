using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    [Serializable]
    public class XP
    {
        public List<int> XP_Levels = new();

        [Range(0, 5)] public float damageMultiplier = 1;
        [Range(0, 5)] public float killMultiplier = 1;
        [Range(0, 5)] public float bossMultiplier = 1;
    }
}