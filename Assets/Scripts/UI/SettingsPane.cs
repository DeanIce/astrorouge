using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class SettingsPane : MonoBehaviour
    {
        public UserSettings userSettings;
        private DropdownMenu displayMode;
        private Slider fov;
        private Slider gameVolume;
        private Slider masterVolume;
        private Slider musicVolume;
        private DropdownMenu resolution;
        private VisualElement root;
        private Slider screenShake;

        private void Start()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            fov = root.Q<Slider>("fov");
            screenShake = root.Q<Slider>("screen-shake");
            masterVolume = root.Q<Slider>("volumeMaster");

            userSettings = new UserSettings();

            fov.RegisterCallback<ChangeEvent<float>>(e => userSettings.fov = e.newValue);
        }

        private void Update()
        {
            print(userSettings.fov);
        }
    }
}