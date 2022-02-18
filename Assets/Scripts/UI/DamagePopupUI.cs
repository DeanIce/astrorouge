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
    
    // create a Damage Popup
    public static DamagePopupUI Create(Vector3 position, int damageAmount, bool isCriticalHit) {
        GameObject damagePopupInstance = Instantiate(DamagePopupPF);

        // TODO (Sonja): figure out which of the following update the position
        damagePopupInstance.transform.position = position;
        //damagePopupInstance.GetComponent<RectTransform>().position = position;

        DamagePopupUI damagePopupUI = damagePopupInstance.GetComponent<DamagePopupUI>();
        damagePopupUI.Setup(damageAmount, isCriticalHit);

        return damagePopupUI;
    }

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Start() {
        // Instantiate(damagePopup, Vector3.zero, Quaternion.identity);
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
        float moveYSpeed = 20f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

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
    }
}
