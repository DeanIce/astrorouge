using System;
using UnityEngine;

namespace Managers
{
    [Serializable]
    public class UserSettings
    {
        public FullScreenMode displayMode = FullScreenMode.FullScreenWindow;
        public float volumeGame = 1; // [0,1]
        public float volumeMusic = 1; // [0,1]
        public int msaa = 4; // {1, 2, 4, 8}

        public float lookSensitivity = 1; // [0,5]

        public Resolution resolution;

        public void Init()
        {
            resolution = Screen.currentResolution;
        }
    }
}