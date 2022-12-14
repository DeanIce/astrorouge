using Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PersistentUpgradeMenu : MonoBehaviour
{
    private class ButtonData
    {
        public enum ButtonStatus
        {
            Available,
            Bought,
            Locked,
        }

        private const string boughtCSS = "upgrade-bought";

        private readonly List<ButtonData> dependents = new();
        private readonly Button button;
        private readonly string buttonName;
        private readonly string buttonText;
        private readonly string statName;
        private readonly float statValue;
        
        public readonly int cost;
        public ButtonStatus Status => status;
        private ButtonStatus status = ButtonStatus.Available;

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
            Lock();
        }

        public void OnClick()
        {
            if (status == ButtonStatus.Locked)
            {
                Debug.Log($"Can't buy locked upgrade {buttonName}");
                return;
            }

            if (status == ButtonStatus.Bought)
            {
                Debug.Log($"Upgrade already purchased {buttonName}");
                return;
            }

            if (PersistentUpgradeManager.Instance.AddPersistentUpgrade(buttonName, statName, statValue, cost))
            {
                SetPurchased();
                foreach (ButtonData dependent in dependents)
                    dependent.Unlock();
                Debug.Log($"Successfuly purchase of {buttonName}");
            }
            else
            {
                Debug.Log($"Failed to purchase {buttonName}");
            }
        }

        public void AddDependent(ButtonData dependent) => dependents.Add(dependent);

        public void Lock()
        {
            foreach(ButtonData dependent in dependents)
                dependent.Lock();

            status = ButtonStatus.Locked;
            button.RemoveFromClassList(boughtCSS);
            button.text = "[LOCKED]";
        }

        public void Unlock()
        {
            status = ButtonStatus.Available;
            button.RemoveFromClassList(boughtCSS);
            button.text = buttonText;
        }

        public void SetPurchased()
        {
            status = ButtonStatus.Bought;
            button.AddToClassList(boughtCSS);
        }

        public void ResolveStatus()
        {
            Unlock();

            if (PersistentUpgradeManager.Instance.NodePurchased(buttonName))
            {
                SetPurchased();
                foreach (ButtonData dependent in dependents)
                    dependent.ResolveStatus();
            }
            else
            {
                foreach (ButtonData dependent in dependents)
                    dependent.Lock();
            }
        }
    }

    private readonly ButtonData[] healthT = new ButtonData[5];
    private readonly ButtonData[] mobilityT = new ButtonData[5];
    private readonly ButtonData[] damageT = new ButtonData[5];
    private readonly ButtonData[] elementsT = new ButtonData[5];

    private VisualElement root;
    private Label balanceLabel;
    private Button escapeButton;
    private Button resetButton;

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        healthT[0] = new(root, "health-t1", "+10 Base Health", "maxHealth", 10, 1);
        healthT[1] = new(root, "health-t2", "+1 Armor", "armor", 1, 2);
        healthT[2] = new(root, "health-t3", "+3 Health Regen", "healthRegen", 3, 3);
        healthT[3] = new(root, "health-t4", "+20 Base Health", "maxHealth", 20, 4);
        healthT[4] = new(root, "health-t5", "+2 Armor", "armor", 2, 5);

        for (int i = 0; i < healthT.Length - 1; i++)
            healthT[i].AddDependent(healthT[i + 1]);

        mobilityT[0] = new(root, "mobility-t1", "+1 Movement Speed", "movementSpeed", 1, 1);
        mobilityT[1] = new(root, "mobility-t2", "+5% Sprint Multiplier", "sprintMultiplier", 0.05f, 2);
        mobilityT[2] = new(root, "mobility-t3", "+1 Jump", "maxExtraJumps", 1, 3);
        mobilityT[3] = new(root, "mobility-t4", "+2 Movement Speed", "movementSpeed", 2, 4);
        mobilityT[4] = new(root, "mobility-t5", "+10% Sprint Multiplier", "sprintMultiplier", 0.1f, 5);

        for (int i = 0; i < mobilityT.Length - 1; i++)
            mobilityT[i].AddDependent(mobilityT[i + 1]);

        damageT[0] = new(root, "damage-t1", "+5 Base Melee Damage", "meleeBaseDamage", 5, 1);
        damageT[1] = new(root, "damage-t2", "+5 Base Range Damage", "rangeBaseDamage", 5, 2);
        damageT[2] = new(root, "damage-t3", "+2 Melee Attack Range", "meleeAttackRange", 2, 3);
        damageT[3] = new(root, "damage-t4", "+5% Range Crit Chance", "rangeCritChance", 0.05f, 4);
        damageT[4] = new(root, "damage-t5", "+10% Melee Crit Chance", "meleeCritChance", 0.05f, 5);

        for (int i = 0; i < damageT.Length - 1; i++)
            damageT[i].AddDependent(damageT[i + 1]);

        elementsT[0] = new(root, "elements-t1", "+1% Effect Chance", "effectChance", 0.01f, 1);
        elementsT[1] = new(root, "elements-t2", "+1% Effect Chance", "effectChance", 0.01f, 2);
        elementsT[2] = new(root, "elements-t3", "+1% Effect Chance", "effectChance", 0.01f, 3);
        elementsT[3] = new(root, "elements-t4", "+1% Effect Chance", "effectChance", 0.01f, 4);
        elementsT[4] = new(root, "elements-t5", "+1% Effect Chance", "effectChance", 0.01f, 5);

        for (int i = 0; i < elementsT.Length - 1; i++)
            elementsT[i].AddDependent(elementsT[i + 1]);

        healthT[0].ResolveStatus();
        mobilityT[0].ResolveStatus();
        damageT[0].ResolveStatus();
        elementsT[0].ResolveStatus();

        balanceLabel = root.Q<Label>("Balance");
        escapeButton = root.Q<Button>("back-button");
        resetButton = root.Q<Button>("reset-button");

        escapeButton.clicked += EventManager.Instance.Menu;
        resetButton.clicked += () =>
        {
            int currency = PersistentUpgradeManager.Instance.GetCurrency()
                + SpentCurrency(healthT)
                + SpentCurrency(mobilityT)
                + SpentCurrency(damageT)
                + SpentCurrency(elementsT);
            PersistentUpgradeManager.Instance.Reset();
            PersistentUpgradeManager.Instance.IncCurrency(currency);

            healthT[0].ResolveStatus();
            mobilityT[0].ResolveStatus();
            damageT[0].ResolveStatus();
            elementsT[0].ResolveStatus();
        };
    }

    private void Update()
    {
        balanceLabel.text = $"Balance: {PersistentUpgradeManager.Instance.GetCurrency()}";
    }

    private int SpentCurrency(ButtonData[] buttonDatas)
    {
        int spent = 0;
        foreach(ButtonData button in buttonDatas)
        {
            if (button.Status == ButtonData.ButtonStatus.Bought)
                spent += button.cost;
        }
        return spent;
    }
}
