using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class HudUI : MonoBehaviour
    {
        public int level;
        private readonly float crosshairSize = 60;

        private readonly float maxHealth = 100;

        private VisualElement crosshair;

        private VisualElement expBar;

        private TextElement expLevelText;

        // Start is called before the first frame update
        private VisualElement healthBar;
        private VisualElement healthBarCorner;
        private TextElement healthBarText;

        private void Start()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            healthBar = root.Q<VisualElement>("Health_Bar_Fill");
            healthBarText = root.Q<TextElement>("HealthText");
            healthBarCorner = root.Q<VisualElement>("Health_Bar_Fill_Corner");

            crosshair = root.Q<VisualElement>("Crosshair");


            expBar = root.Q<VisualElement>("Exp_Bar_Fill");
            expLevelText = root.Q<TextElement>("LevelText");

            SetHealth(PlayerStats.Instance.currentHealth);

            LevelUp();
            SetExp(10, 100);
        }

        private void OnEnable()
        {
            EventManager.Instance.playerStatsUpdated += UpdateBars;
            EventManager.Instance.crosshairSpread += AdjustCrosshair;
        }

        private void OnDisable()
        {
            EventManager.Instance.playerStatsUpdated -= UpdateBars;
        }

        private void UpdateBars()
        {
            SetHealth(PlayerStats.Instance.currentHealth);
        }

        public void SetHealth(float hp)
        {
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

        public void SetExp(float exp, float maxExp)
        {
            if (exp >= maxExp)
            {
                LevelUp();
                exp -= maxExp;
            }

            expBar.style.width = new StyleLength(Length.Percent(exp / maxExp * 100));
        }

        public void LevelUp()
        {
            level++;
            expLevelText.text = level.ToString();
        }

        public void AdjustCrosshair(float spread)
        {
            crosshair.style.width = new StyleLength(crosshairSize + spread);
            crosshair.style.height = new StyleLength(crosshairSize + spread);
        }
    }
}