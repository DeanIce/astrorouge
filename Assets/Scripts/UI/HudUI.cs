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

    public int level = 0;
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        healthBar = root.Q<VisualElement>("Health_Bar_Fill");
        healthBarText = root.Q<TextElement>("HealthText");
        healthBarCorner = root.Q<VisualElement>("Health_Bar_Fill_Corner");


        expBar = root.Q<VisualElement>("Exp_Bar_Fill");
        expLevelText = root.Q<TextElement>("LevelText");

        LevelUp();
        SetHealth(99, 100);
        SetExp(10, 100);
    }

    public void SetHealth(float hp, float maxHp)
    {
        healthBarText.text = hp + " / " + maxHp;
        float percentRemaining = (hp / maxHp) * 100;
        //Temporary Corner of HUD fix
        if(percentRemaining <= 3)
        {
            if (percentRemaining < 0.2)
            {
                healthBarCorner.style.width = new StyleLength(Length.Percent(0));
            }
            percentRemaining = 3;
        }
        healthBar.style.width = new StyleLength(Length.Percent(percentRemaining-3));
    }

    public void SetExp(float exp, float maxExp)
    {
        if(exp >= maxExp)
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
}
