using System.Collections.Generic;

namespace Managers
{
    public class RunStats
    {
        public readonly List<string> itemsCollected = new();
        public float damageDealt;
        public float damageTaken;
        public int enemiesKilled;
    }
}