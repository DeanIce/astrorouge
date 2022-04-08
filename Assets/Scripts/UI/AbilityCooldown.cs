using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Managers;

public class AbilityCooldown : MonoBehaviour
{
    private VisualElement root;
    private VisualElement background;
    private VisualElement top;
    private TextElement displaySeconds;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root = root.Q<VisualElement>("SpecialAbility");
        background = root.Q<VisualElement>("Background");
        top = root.Q<VisualElement>("Top");
        displaySeconds = root.Q<TextElement>("Timer");

        top.style.display = DisplayStyle.None;
        displaySeconds.style.display = DisplayStyle.None;
    }
    private void OnEnable()
    {
        EventManager.Instance.specialUsed += AnimateCooldown;
    }
    private void OnDisable()
    {
        EventManager.Instance.specialUsed -= AnimateCooldown;
    }

    // Update is called once per frame
    private void AnimateCooldown(float cooldown)
    {
        top.style.display = DisplayStyle.Flex;
        displaySeconds.style.display = DisplayStyle.Flex;
        StartCoroutine(hideCooldown(cooldown));
        DOTween.To(() => cooldown, x => displaySeconds.text = $"{(int)x}", 0, cooldown).SetEase(Ease.Linear);

        DOTween.To(() => 100, x => top.style.height = new StyleLength(Length.Percent(x)), 0, cooldown).SetEase(Ease.Linear);
    }

    IEnumerator hideCooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        top.style.display = DisplayStyle.None;
        displaySeconds.style.display = DisplayStyle.None;
    }
}
