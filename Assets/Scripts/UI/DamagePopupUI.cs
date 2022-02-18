using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupUI : MonoBehaviour
{
    
    public static DamagePopupUI Create(Vector3 position, int damageAmount) {
        GameObject damagePopup = Resources.Load("prefabs/DamagePopup", GameObject) as GameObject;
        Transform damagePopupTransform = Instantiate(damagePopup, position, Quaternion.identity);

        DamagePopupUI damagePopupUI = damagePopupTransform.GetComponent<DamagePopupUI>();
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
