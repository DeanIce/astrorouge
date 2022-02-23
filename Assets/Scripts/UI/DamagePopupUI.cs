using UnityEngine;
using TMPro;

public class DamagePopupUI : MonoBehaviour
{
    private static GameObject DamagePopupPF {
        get { 
            if (_damagePopupPF == null)
                _damagePopupPF = Resources.Load("prefabs/DamagePopup", typeof(GameObject)) as GameObject;
            return _damagePopupPF;
        }
    }
    private static GameObject _damagePopupPF;
    public Transform enemyTrans {get; set;}
    
    // create a Damage Popup
    public static DamagePopupUI Create(Transform enemyTransform, Quaternion rotation, int damageAmount, bool isCriticalHit) {
        GameObject damagePopupInstance = Instantiate(DamagePopupPF, enemyTransform.position, rotation);

        DamagePopupUI damagePopupUI = damagePopupInstance.GetComponent<DamagePopupUI>();
        damagePopupUI.Setup(damageAmount, isCriticalHit);

        damagePopupUI.enemyTrans = enemyTransform;

        return damagePopupUI;
    }

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Start() {

    }

    public void Setup(int damageAmount, bool isCriticalHit) {
        textMesh.SetText(damageAmount.ToString());
        
        
        if (isCriticalHit) {
            textMesh.color = Color.red;
            textMesh.fontSize = 25;
        } else {
            textMesh.color = Color.white;
            textMesh.fontSize = 10;
        }

        textColor = textMesh.color;
        disappearTimer = 0.5f;
        
    }

    private void Update() {
        if (enemyTrans != null) {
            float moveSpeed = 10f;
            transform.position +=  enemyTrans.up * moveSpeed * Time.deltaTime;
            
            Quaternion rotation = Quaternion.LookRotation(transform.position - GameObject.Find("PlayerDefault").transform.position, transform.up);
            transform.rotation = rotation;

            disappearTimer -= Time.deltaTime;
            if (disappearTimer < 0) {
                // start disappearing
                float disappearSpeed = 3f;
                textColor.a -= disappearSpeed * Time.deltaTime;
                textMesh.color = textColor;
                if (textColor.a < 0) {
                    Destroy(gameObject);
                }

            }
        } else {
            Destroy(gameObject);
        }
    }
}
