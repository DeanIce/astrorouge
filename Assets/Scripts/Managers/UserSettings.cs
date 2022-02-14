using System;

namespace Managers
{
    [Serializable]
    public class UserSettings
    {
        public string displayMode = "Fullscreen";
        public float fov = 60;
        public string resolution = "TODO";
        public float screenShake = 1; // [0,1]
        public float volumeGame = 1; // [0,1]
        public float volumeMaster = 1; // [0,1]
        public float volumeMusic = 1; // [0,1]
    }
}