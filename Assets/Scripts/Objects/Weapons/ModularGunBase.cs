using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A weapon that's built from various components.
/// The result projectile is the result of the combined actions of said components.
/// </summary>
public class ModularGunBase : Weapon
{
    [Header("Module settings")]
    [SerializeField] int maxModuleAmount = 5;
    [SerializeField] List<GunModule> modules = new List<GunModule>();

    [Header("Visual settings")]
    [SerializeField] Transform modulesParent;

    [Header("Editor Settings")]
    [SerializeField] Mesh previewModuleMesh;
    [SerializeField] float previewModuleSize = 1;

    public bool AddModule(GunModule module)
    {
        if(modules.Count >= maxModuleAmount) return false;
        if(modules.Contains(module)) return false;

        module.transform.parent = modulesParent;
        module.transform.localRotation = Quaternion.identity;
        module.transform.localPosition = Vector3.forward * module.transform.localScale.x * modules.Count;

        modules.Add(module);

        return true;
    }
    public void RemoveModule(GunModule module)
    {
        if(!modules.Contains(module)) return;

        modules.Remove(module);
    }

    protected override void Fire(Vector3 direction)
    {
        Fireable result = null;

        foreach (var item in modules)
        {
            result = item.Apply(result);
        }

        result.Create(Source, modules[modules.Count - 1].transform.position + modules[modules.Count - 1].transform.forward * 0.1f, direction);
    }

    private void OnDrawGizmos() 
    {
        if(modulesParent == null) return;

        for (int i = 0; i < maxModuleAmount; i++)
        {
            float spacing = previewModuleSize * 0.2f;
            if(previewModuleMesh == null)
                Gizmos.DrawWireCube(modulesParent.transform.position + modulesParent.forward * spacing * i, Vector3.one * previewModuleSize);
            else
                Gizmos.DrawWireMesh(previewModuleMesh, 0, modulesParent.transform.position + modulesParent.forward * spacing * i, modulesParent.rotation, Vector3.one * previewModuleSize);
        }
    }
}
