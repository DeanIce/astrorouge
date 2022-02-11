using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarUI : MonoBehaviour
{

    [SerializeField] private Transform TargetFollow;
    private VisualElement healthBar;
    private VisualElement health;
    private TextElement healthText;

    private Camera mainCamera;

    private float maxHealth =100;

    // timer
    float timer = 0;
    bool timerReached = false;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        healthBar = GetComponent<UIDocument>().rootVisualElement.Q("HealthBar");
        health = healthBar.Q<VisualElement>("Health");
        healthText = healthBar.Q<TextElement>("HealthText");

        maxHealth = gameObject.GetComponent<BasicEnemyAgent>().health;

        healthBar.style.display = DisplayStyle.None;
        
        //Disables health text display
        healthText.text = "";

        SetPosition();
    }

    //sets the position of the health bar
    public void SetPosition()
    {
        Vector2 newPos = RuntimePanelUtils.CameraTransformWorldToPanel(healthBar.panel, TargetFollow.position, mainCamera);
        Vector3 adjustedPos = newPos;
        adjustedPos.x = newPos.x - healthBar.layout.width / 2;
        healthBar.transform.position = adjustedPos;
    }

    //call SetHealth whenever health needs adjusted
    public void SetHealth(float hp, float maxHp)
    {
        maxHealth = maxHp;     
        health.style.width = new StyleLength(Length.Percent((hp / maxHealth) * 100));
        // start timer
        StartTimer();
    }

    public void SetHealth(float hp)
    {
        healthBar.style.display = DisplayStyle.Flex;
        health.style.width = new StyleLength(Length.Percent((hp / maxHealth) * 100));
        // start timer
        StartTimer();
    }

    private void HideHealth() {
        healthBar.style.display = DisplayStyle.None;
        print("hiding healthbar");
    }

    private void StartTimer() {
        if (timerReached) {
            timerReached = false;
            timer = 0;
            print("starting healthbar timer");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (TargetFollow != null)
        {
            SetPosition();
        }

        if (!timerReached) {
            timer += Time.deltaTime;
        }
        if (!timerReached && timer > 10) {
            HideHealth();
            print("healthbar timer reached");
            timerReached = true;
        }
    }
}
