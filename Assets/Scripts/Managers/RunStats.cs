using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    
    public class RunStats
    {
        public int enemies_killed;
        public float damage_dealt;
        public float damage_taken;

        public RunStats() {
            enemies_killed = 0;
            damage_dealt = 0;
            damage_taken = 0;
        }

    }
}