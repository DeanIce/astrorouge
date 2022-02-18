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
    
    public static DamagePopupUI Create(Vector3 position, int damageAmount) {
        GameObject damagePopupInstance = Instantiate(DamagePopupPF);

        // TODO (Sonja): figure out which of the following update the position
        damagePopupInstance.transform.position = position;
        //damagePopupInstance.GetComponent<RectTransform>().position = position;

        DamagePopupUI damagePopupUI = damagePopupInstance.GetComponent<DamagePopupUI>();
        damagePopupUI.Setup(damageAmount);

        return damagePopupUI;
    }
    private TextMeshPro textMesh;
    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Start() {
        // Instantiate(damagePopup, Vector3.zero, Quaternion.identity);
    }

    public void Setup(int damageAmount) {
        textMesh.SetText(damageAmount.ToString());
    }
}
