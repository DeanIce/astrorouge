using UnityEngine;

public class MoustacheTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MoustacheEnabler.moustache = gameObject;
        MoustacheEnabler.needsApply = true;
    }
}
