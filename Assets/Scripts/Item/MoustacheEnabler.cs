using Managers;
using UnityEngine;

public class MoustacheEnabler : MonoBehaviour
{
    private bool mousEnabled;
    private Vector3 scale;

    public static GameObject moustache;
    public static bool needsApply;

    private void Start()
    {
        ResetValues();
        EventManager.Instance.playGame += ResetValues;
        PlayerStats.Instance.MoustacheEnable += ManageMoustache;
    }

    private void Update()
    {
        if (needsApply)
        {
            needsApply = false;
            ApplyStatus();
        }
    }

    private void OnDisable()
    {
        EventManager.Instance.playGame -= ResetValues;
        PlayerStats.Instance.MoustacheEnable -= ManageMoustache;
    }

    public void ApplyStatus()
    {
        var mr = moustache.GetComponent<MeshRenderer>();
        mr.enabled = mousEnabled;
        moustache.transform.localScale = scale;
    }

    public void ResetValues()
    {
        mousEnabled = false;
        scale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void ManageMoustache()
    {
        if (!mousEnabled)
            mousEnabled = true;
        else
            scale = new Vector3(scale.x + 0.1f, scale.y + 0.1f, scale.z);

        ApplyStatus();
    }
}
