using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Managers;

public class AbilityCooldown : MonoBehaviour
{
    private VisualElement root;

    public Texture2D ability1Icon;
    public Texture2D ability2Icon;
    public Texture2D ability3Icon;
    public Texture2D ability4Icon;

    private VisualElement ability1;
    private VisualElement background1;
    private VisualElement top1;
    private TextElement displaySeconds1;

    private VisualElement ability2;
    private VisualElement background2;
    private VisualElement top2;
    private TextElement displaySeconds2;

    private VisualElement ability3;
    private VisualElement background3;
    private VisualElement top3;
    private TextElement displaySeconds3;

    private VisualElement ability4;
    private VisualElement background4;
    private VisualElement top4;
    private TextElement displaySeconds4;


    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root = root.Q<VisualElement>("AbilityBar");
        ability1 = root.Q<VisualElement>("Ability1");
        ability2 = root.Q<VisualElement>("Ability2");
        ability3 = root.Q<VisualElement>("Ability3");
        ability4 = root.Q<VisualElement>("Ability4");

        background1 = ability1.Q<VisualElement>("Background");
        top1 = ability1.Q<VisualElement>("Top");
        displaySeconds1 = ability1.Q<TextElement>("Timer");

        background2 = ability2.Q<VisualElement>("Background");
        top2 = ability2.Q<VisualElement>("Top");
        displaySeconds2 = ability2.Q<TextElement>("Timer");

        background3 = ability3.Q<VisualElement>("Background");
        top3 = ability3.Q<VisualElement>("Top");
        displaySeconds3 = ability3.Q<TextElement>("Timer");

        background4 = ability4.Q<VisualElement>("Background");
        top4 = ability4.Q<VisualElement>("Top");
        displaySeconds4 = ability4.Q<TextElement>("Timer");

        top1.style.display = DisplayStyle.None;
        displaySeconds1.style.display = DisplayStyle.None;

        top2.style.display = DisplayStyle.None;
        displaySeconds2.style.display = DisplayStyle.None;

        top3.style.display = DisplayStyle.None;
        displaySeconds3.style.display = DisplayStyle.None;

        top4.style.display = DisplayStyle.None;
        displaySeconds4.style.display = DisplayStyle.None;

        background1.style.backgroundImage = new StyleBackground(ability1Icon);
        background2.style.backgroundImage = new StyleBackground(ability2Icon);
        background3.style.backgroundImage = new StyleBackground(ability3Icon);
        background4.style.backgroundImage = new StyleBackground(ability4Icon);
    }
    private void OnEnable()
    {
        EventManager.Instance.meleeUsed += HandleAbility1;
        EventManager.Instance.secondaryUsed += HandleAbility2;
        EventManager.Instance.specialUsed += HandleAbility3;
        EventManager.Instance.utilityUsed += HandleAbility4;
    }
    private void OnDisable()
    {
        EventManager.Instance.meleeUsed -= HandleAbility1;
        EventManager.Instance.secondaryUsed -= HandleAbility2;
        EventManager.Instance.specialUsed -= HandleAbility3;
        EventManager.Instance.utilityUsed -= HandleAbility4;
    }

    private void HandleAbility1(float cooldown)
    {
        AnimateCooldown(cooldown, top1, displaySeconds1);
    }
    private void HandleAbility2(float cooldown)
    {
        AnimateCooldown(cooldown, top2, displaySeconds2);
    }
    private void HandleAbility3(float cooldown)
    {
        AnimateCooldown(cooldown, top3, displaySeconds3);
    }
    private void HandleAbility4(float cooldown)
    {
        AnimateCooldown(cooldown, top4, displaySeconds4);
    }
    private void AnimateCooldown(float cooldown, VisualElement top, TextElement displaySeconds)
    {
        top.style.display = DisplayStyle.Flex;
        displaySeconds.style.display = DisplayStyle.Flex;
        StartCoroutine(hideCooldown(cooldown, top, displaySeconds));
        DOTween.To(() => cooldown, x => displaySeconds.text = $"{(int)(x+1)}", 0, cooldown).SetEase(Ease.Linear);

        DOTween.To(() => 100, x => top.style.height = new StyleLength(Length.Percent(x)), 0, cooldown).SetEase(Ease.Linear);
    }
    IEnumerator hideCooldown(float seconds, VisualElement top, TextElement displaySeconds)
    {
        yield return new WaitForSeconds(seconds);
        top.style.display = DisplayStyle.None;
        displaySeconds.style.display = DisplayStyle.None;
    }
}
