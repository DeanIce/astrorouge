using UnityEngine;
using UnityEngine.UIElements;

public class PersistentUpgradeMenu : MonoBehaviour
{
    private class ButtonData
    {
        private readonly Button button;
        private readonly string buttonName;
        private readonly string buttonText;
        private readonly string statName;
        private readonly float statValue;
        private readonly int cost;

        private bool locked = false;

        public ButtonData(VisualElement root, string buttonName, string buttonText, string statName, float statValue, int cost)
        {
            this.buttonName = buttonName;
            this.buttonText = buttonText;
            this.statName = statName;
            this.statValue = statValue;
            this.cost = cost;
            button = root.Q<Button>(buttonName);
            button.text = buttonText;

            button.clicked += OnClick;
        }

        public void OnClick()
        {
            if (PersistentUpgradeManager.Instance.NodePurchased(buttonName))
                return;
            if (PersistentUpgradeManager.Instance.AddPersistentUpgrade(buttonName, statName, statValue, cost))
                SetPurchased();
        }

        private void Lock()
        {
            if (locked) return;

            locked = true;
            button.text = "[REDACTED]";
        }

        private void Unlock()
        {
            if (!locked) return;

            locked = false;
            button.text = buttonText;
        }

        private void SetPurchased()
        {

        }
    }

    private readonly ButtonData[] healthT = new ButtonData[5];
    private readonly ButtonData[] mobilityT = new ButtonData[5];
    private readonly ButtonData[] damageT = new ButtonData[5];
    private readonly ButtonData[] elementsT = new ButtonData[5];

    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        healthT[0] = new(root, "health-t1", "+10 Base Health", "health", 10, 1);
        healthT[1] = new(root, "health-t2", "+20 Base Health", "health", 20, 1);
        healthT[2] = new(root, "health-t3", "+30 Base Health", "health", 30, 1);
        healthT[3] = new(root, "health-t4", "+40 Base Health", "health", 40, 1);
        healthT[4] = new(root, "health-t5", "+50 Base Health", "health", 50, 1);

        mobilityT[0] = new(root, "mobility-t1", "+2 Movement Speed", "baseMovementSpeed", 2, 1);
        mobilityT[1] = new(root, "mobility-t2", "+5% Sprint Multiplier", "baseSprintMultiplier", 0.05f, 1);
        mobilityT[2] = new(root, "mobility-t3", "+1 Jump", "baseMaxExtraJumps", 1, 1);
        mobilityT[3] = new(root, "mobility-t4", "+2 Movement Speed", "baseMovementSpeed", 2, 1);
        mobilityT[4] = new(root, "mobility-t5", "+5% Sprint Multiplier", "baseSprintMultiplier", 0.05f, 1);

        damageT[0] = new(root, "damage-t1", "+5 Base Melee Damage", "meleeBaseDamage", 5, 1);
        damageT[1] = new(root, "damage-t2", "+5 Base Range Damage", "rangeBaseDamage", 5, 1);
        damageT[2] = new(root, "damage-t3", "+2 Melee Attack Range", "meleeAttackRange", 2, 1);
        damageT[3] = new(root, "damage-t4", "+5% Range Crit Chance", "rangeCritChance", 0.05f, 1);
        damageT[4] = new(root, "damage-t5", "+10% Melee Crit Chance", "meleeCritChance", 0.05f, 1);

        elementsT[0] = new(root, "elements-t1", "+1% Effect Chance", "baseEffectChance", 0.01f, 1);
        elementsT[1] = new(root, "elements-t2", "+1% Effect Chance", "baseEffectChance", 0.01f, 1);
        elementsT[2] = new(root, "elements-t3", "+1% Effect Chance", "baseEffectChance", 0.01f, 1);
        elementsT[3] = new(root, "elements-t4", "+1% Effect Chance", "baseEffectChance", 0.01f, 1);
        elementsT[4] = new(root, "elements-t5", "+1% Effect Chance", "baseEffectChance", 0.01f, 1);
    }
}
