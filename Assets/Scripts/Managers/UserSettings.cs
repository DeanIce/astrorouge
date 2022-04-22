using System;
using UnityEngine;

namespace Managers
{
    [Serializable]
    public class UserSettings
    {
        public FullScreenMode displayMode = FullScreenMode.FullScreenWindow;
        public float fov = 60;
        public float screenShake = 1; // [0,1]
        public float volumeGame = 1; // [0,1]
        public float volumeMaster = 1; // [0,1]
        public float volumeMusic = 1; // [0,1]
        public float brightness = 1; // [0,1]

        public Resolution resolution;

        public void Init()
        {
            resolution = Screen.currentResolution;
        }
    }
}