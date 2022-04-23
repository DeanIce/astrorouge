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
    public static DamagePopupUI Create(Transform enemyTransform, Quaternion rotation, int damageAmount, int type) {
        Vector3 randOffset = new Vector3(Random.Range(-.75f, .75f), Random.Range(-.75f, .75f), Random.Range(-.75f, .75f));
        GameObject damagePopupInstance = Instantiate(DamagePopupPF, enemyTransform.position + randOffset, rotation);

        DamagePopupUI damagePopupUI = damagePopupInstance.GetComponent<DamagePopupUI>();
        damagePopupUI.Setup(damageAmount, type);

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

    public void Setup(int damageAmount, int type) {
        textMesh.SetText(damageAmount.ToString());
        
        
        if (type == 1) {
            textMesh.color = Color.red;
            textMesh.fontSize = 6;
        } else {
            textMesh.color = Color.white;
            textMesh.fontSize = 6;
        }

        switch (type)
        {
            case 1:
                textMesh.color = Color.Lerp(Color.yellow, Color.red, 0.5f);
                textMesh.fontSize = 6;
                break;
            case 2:
                textMesh.color = Color.magenta;
                textMesh.fontSize = 6;
                break;
            case 3:
                textMesh.color = Color.yellow;
                textMesh.fontSize = 6;
                break;
            case 4:
                textMesh.color = Color.green;
                textMesh.fontSize = 6;
                break;
            case 5:
                textMesh.color = Color.blue;
                textMesh.fontSize = 6;
                break;
            default:
                textMesh.color = Color.white;
                textMesh.fontSize = 6;
                break;
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
