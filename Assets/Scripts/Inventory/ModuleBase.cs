using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleBase : MonoBehaviour
{
    public void UpdateModuleInfo(InventoryItem[] modules)
    {
        foreach (var module in modules)
            module.mainObject.GetComponent<ModuleScript>().Activate(true);
    }
}
