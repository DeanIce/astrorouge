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
        private readonly float hitmarkerScreenDuration = 0.2f;

        private VisualElement crosshair;
        private VisualElement hitmarker;

        private VisualElement expBar;

        private float hitmarkerTimer;
        private bool hitmarkerDisplayed = false;

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
            hitmarker = root.Q<VisualElement>("Hitmarker");


            expBar = root.Q<VisualElement>("Exp_Bar_Fill");
            expLevelText = root.Q<TextElement>("LevelText");

            SetHealth(PlayerStats.Instance.currentHealth);
            hitmarker.style.display = DisplayStyle.None;

            LevelUp();
            SetExp(10, 100);
        }

        private void OnEnable()
        {
            EventManager.Instance.playerStatsUpdated += UpdateBars;
            EventManager.Instance.crosshairSpread += AdjustCrosshair;
            EventManager.Instance.enemyDamaged += DisplayHitmarker;
        }

        private void OnDisable()
        {
            EventManager.Instance.playerStatsUpdated -= UpdateBars;
            EventManager.Instance.crosshairSpread -= AdjustCrosshair;
        }

        private void UpdateBars()
        {
            SetHealth(PlayerStats.Instance.currentHealth);
        }

        private void Update()
        {
            if (hitmarkerDisplayed)
            {
                hitmarkerTimer -= Time.deltaTime;
                if(hitmarkerTimer <= 0)
                {
                    HideHitmarker();
                }
            }
        }

        private void DisplayHitmarker()
        {
            hitmarkerDisplayed = true;
            hitmarkerTimer = hitmarkerScreenDuration;
            hitmarker.style.display = DisplayStyle.Flex;
        }

        private void HideHitmarker()
        {
            hitmarkerDisplayed = false;
            hitmarkerTimer = 0;
            hitmarker.style.display = DisplayStyle.None;
        }

        public void SetHealth(float hp)
        {
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