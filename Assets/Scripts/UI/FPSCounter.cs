using Managers;
using Tayx.Graphy;
using UnityEngine;

namespace UI
{
    public class FPSCounter : MonoBehaviour
    {
        private GraphyManager gm;

        private void Start()
        {
            gm = GetComponent<GraphyManager>();
        }


        private void Update()
        {
            if (DevTools.Instance.showFPSCounter)
            {
                gm.FpsModuleState = GraphyManager.ModuleState.FULL;
                gm.RamModuleState = GraphyManager.ModuleState.FULL;
                gm.AdvancedModuleState = GraphyManager.ModuleState.FULL;
            }
            else
            {
                gm.RamModuleState = GraphyManager.ModuleState.OFF;
                gm.FpsModuleState = GraphyManager.ModuleState.OFF;
                gm.AdvancedModuleState = GraphyManager.ModuleState.OFF;
            }
        }
    }
}