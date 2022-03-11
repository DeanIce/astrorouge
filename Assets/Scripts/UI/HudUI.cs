using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HudUI : MonoBehaviour
{
    // Start is called before the first frame update
    private VisualElement healthBar;
    private VisualElement healthBarCorner;
    private TextElement healthBarText;

    private VisualElement expBar;
    private TextElement expLevelText;

    private VisualElement crosshair;

    private float maxHealth = 100;
    private float crosshairSize = 60;

    public int level = 0;
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        healthBar = root.Q<VisualElement>("Health_Bar_Fill");
        healthBarText = root.Q<TextElement>("HealthText");
        healthBarCorner = root.Q<VisualElement>("Health_Bar_Fill_Corner");

        crosshair = root.Q<VisualElement>("Crosshair");


        expBar = root.Q<VisualElement>("Exp_Bar_Fill");
        expLevelText = root.Q<TextElement>("LevelText");

        maxHealth = PlayerStats.Instance.maxHealth;
        SetHealth(maxHealth);

        LevelUp();
        SetExp(10, 100);
    }


    public void SetHealth(float hp)
    {
        healthBarText.text = hp + " / " + maxHealth;
        float percentRemaining = (hp / maxHealth) * 100;
        //Temporary Corner of HUD fix
        if (percentRemaining <= 3)
        {
            if (percentRemaining < 0.2)
            {
                healthBarCorner.style.width = new StyleLength(Length.Percent(0));
            }
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
        expBar.style.width = new StyleLength(Length.Percent((exp / maxExp) * 100));
    }

    public void LevelUp()
    {
        level++;
        expLevelText.text = level.ToString();
    }

    public void AdjustCrosshair(float spread)
    {
        crosshair.style.width = new StyleLength(crosshairSize+spread);
        crosshair.style.height = new StyleLength(crosshairSize+spread);
    }
}

