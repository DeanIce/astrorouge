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

    private TextElement damage;

    private Camera mainCamera;

    private float maxHealth =100;

    // timer
    float healthBarTimer = 0;
    bool healthBarTimerReached = false;

    float damageTimer = 0;
    bool damageTimerReached = false;

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

        damage = healthBar.Q<TextElement>("DamageText");
        damage.text = "";

        SetHealthBarPosition();
        SetDamagePosition();
    }

    //sets the position of the health bar
    private void SetHealthBarPosition()
    {
        Vector2 newPos = RuntimePanelUtils.CameraTransformWorldToPanel(healthBar.panel, TargetFollow.position, mainCamera);
        Vector3 adjustedPos = newPos;
        adjustedPos.x = newPos.x - healthBar.layout.width / 2;
        healthBar.transform.position = adjustedPos;
    }

    //sets the position of the damage indicator
    private void SetDamagePosition()
    {
        Vector2 newPos = RuntimePanelUtils.CameraTransformWorldToPanel(healthBar.panel, TargetFollow.position, mainCamera);
        Vector3 adjustedPos = newPos;
        adjustedPos.x = newPos.x - healthBar.layout.width / 2;
        adjustedPos.y = newPos.y + (float) 0.5;
        damage.transform.position = adjustedPos;
    }

    //call SetHealth whenever health needs adjusted
    public void SetHealth(float hp, float maxHp)
    {
        maxHealth = maxHp;     
        health.style.width = new StyleLength(Length.Percent((hp / maxHealth) * 100));
        // start timer
        StartHealthBarTimer();
    }

    public void SetHealth(float hp)
    {
        healthBar.style.display = DisplayStyle.Flex;
        health.style.width = new StyleLength(Length.Percent((hp / maxHealth) * 100));
        // start timer
        StartHealthBarTimer();
    }

    public void SetDamage(float dmg)
    {
        print("Setting damage!");
        damage.style.display = DisplayStyle.Flex;    
        damage.text = dmg.ToString();
        // start timer
        StartDamageTimer();
    }

    private void HideHealth() {
        healthBar.style.display = DisplayStyle.None;
        //print("hiding healthbar");
    }

    private void HideDamage() {
        damage.style.display = DisplayStyle.None;
        print("hiding damage");
    }


    private void StartHealthBarTimer() {
        if (!healthBarTimerReached && healthBarTimer > 0) {
            healthBarTimer = 0;
        }
        if (healthBarTimerReached) {
            healthBarTimerReached = false;
            healthBarTimer = 0;
            //print("starting healthbar timer");
        }
    }

    private void StartDamageTimer() {
        if (!damageTimerReached && damageTimer > 0) {
            damageTimer = 0;
        }
        if (damageTimerReached) {
            damageTimerReached = false;
            damageTimer = 0;
            print("starting damage timer");
        }
    }

    private void CheckHealthBarTimer() {
        if (!healthBarTimerReached) {
            healthBarTimer += Time.deltaTime;
        }
        if (!healthBarTimerReached && healthBarTimer > 10) {
            HideHealth();
            // print("healthbar timer reached");
            healthBarTimerReached = true;
        }
    }

    private void CheckDamageTimer() {
        if (!damageTimerReached) {
            damageTimer += Time.deltaTime;
        }
        if (!damageTimerReached && damageTimer > 10) {
            HideDamage();
            print("damage timer reached");
            damageTimerReached = true;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (TargetFollow != null)
        {
            SetHealthBarPosition();
            SetDamagePosition();
        }

        CheckHealthBarTimer();
        CheckDamageTimer();
    }
}
