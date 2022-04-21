using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Transform TargetFollow;
    private VisualElement health;
    private VisualElement healthBar;
    private TextElement healthText;
    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
        healthBar = GetComponent<UIDocument>().rootVisualElement.Q("HealthBar");
        health = healthBar.Q<VisualElement>("Health");
        healthText = healthBar.Q<TextElement>("HealthText");

        healthBar.style.display = DisplayStyle.None;

        // Disables health text display
        healthText.text = "";

        SetHealthBarPosition();
    }

    private void LateUpdate()
    {
        if (TargetFollow != null) SetHealthBarPosition();
    }

    /// <summary>
    ///     Call whenever healthbar needs updating.
    /// </summary>
    /// <param name="hp">BasicEnemyAgent.health usually</param>
    /// <param name="maxHp">BasicEnemyAgent.maxHealth usually</param>
    public void SetHealth(float hp, float maxHp)
    {
        healthBar.style.display = DisplayStyle.Flex;
        health.style.width = new StyleLength(Length.Percent(hp / maxHp * 100));
        healthText.text = $"{Mathf.Round(hp)}";
        StartCoroutine(HealthBarTimer());
    }

    private void SetHealthBarPosition()
    {
        Vector2 newPos =
            RuntimePanelUtils.CameraTransformWorldToPanel(healthBar.panel, TargetFollow.position, mainCamera);
        Vector3 adjustedPos = newPos;
        adjustedPos.x = newPos.x - healthBar.layout.width / 2;
        healthBar.transform.position = adjustedPos;
    }

    private IEnumerator HealthBarTimer()
    {
        yield return new WaitForSeconds(10);
        HideHealth();
    }

    public void HideHealth()
    {
        healthBar.style.display = DisplayStyle.None;
    }
}