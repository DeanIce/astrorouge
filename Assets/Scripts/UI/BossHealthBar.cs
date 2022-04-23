using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class BossHealthBar : MonoBehaviour
    {
        private VisualElement healthBar;
        private VisualElement healthBarCorner;
        private TextElement healthBarText;

        private void Start()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            healthBar = root.Q<VisualElement>("Health_Bar_Fill");
            healthBarText = root.Q<TextElement>("HealthText");
            healthBarCorner = root.Q<VisualElement>("Health_Bar_Fill_Corner");
        }


        public void SetHealth(float hp, float maxHealth)
        {
            if (hp < 0) hp = 0;

            hp = Mathf.Round(hp);
            healthBarText.text = hp + " / " + maxHealth;
            float percentRemaining = hp / maxHealth * 100;
            //Temporary Corner of HUD fix
            if (percentRemaining <= 3)
            {
                if (percentRemaining < 0.2) healthBarCorner.style.width = new StyleLength(Length.Percent(0));
                percentRemaining = 3;
            }

            healthBar.style.width = new StyleLength(Length.Percent(percentRemaining - 3));
        }
    }
}