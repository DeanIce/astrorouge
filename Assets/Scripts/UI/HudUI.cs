using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class HudUI : MonoBehaviour
    {
        private readonly float crosshairSize = 60;


        private readonly float hitmarkerScreenDuration = 0.2f;


        private VisualElement crosshair;

        private VisualElement expBar;

        private TextElement expLevelText;

        private VisualElement healthBar;
        private VisualElement healthBarCorner;
        private TextElement healthBarText;
        private VisualElement hitmarker;
        private bool hitmarkerDisplayed;

        private float hitmarkerTimer;
        private Label loading;

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

            expLevelText.text = PlayerStats.Instance.xpLevel.ToString();
            SetExp(PlayerStats.Instance.xp);

            loading = root.Q<Label>("loading");
        }

        private void Update()
        {
            if (hitmarkerDisplayed)
            {
                hitmarkerTimer -= Time.deltaTime;
                if (hitmarkerTimer <= 0) HideHitmarker();
            }
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

        public void DoShit(string val)
        {
            loading.text = val;
        }

        private void UpdateBars()
        {
            SetExp(PlayerStats.Instance.xp);
            SetHealth(PlayerStats.Instance.currentHealth);
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
            int maxHealth = PlayerStats.Instance.maxHealth;
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

        public void SetExp(float exp)
        {
            if (exp >= PlayerStats.Instance.xpPerLevel)
            {
                LevelUp();
                exp -= PlayerStats.Instance.xpPerLevel;
            }

            expBar.style.width = new StyleLength(Length.Percent(exp / PlayerStats.Instance.xpPerLevel * 100));
            PlayerStats.Instance.xp = exp;
        }

        public void LevelUp()
        {
            PlayerStats.Instance.xpLevel++;
            expLevelText.text = PlayerStats.Instance.xpLevel.ToString();
            PlayerStats.Instance.LevelUp();
        }

        public void AdjustCrosshair(float spread)
        {
            crosshair.style.width = new StyleLength(crosshairSize + spread);
            crosshair.style.height = new StyleLength(crosshairSize + spread);
        }
    }
}