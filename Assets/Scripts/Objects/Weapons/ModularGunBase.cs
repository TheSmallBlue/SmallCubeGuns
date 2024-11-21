using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A weapon that's built from various components.
/// The result projectile is the result of the combined actions of said components.
/// </summary>
public class ModularGunBase : Weapon
{
    public List<GunModule> Modules => modules;
    
    [Header("Module settings")]
    [SerializeField] int maxModuleAmount = 5;
    [SerializeField] List<GunModule> modules = new List<GunModule>();

    [Header("Visual settings")]
    [SerializeField] Transform modulesParent;
    [SerializeField] float moduleSize = 1;

    [Header("Editor Settings")]
    [SerializeField] Mesh previewModuleMesh;

    private void Awake() 
    {
        foreach (var module in modules)
        {
            Debug.Log(transform.name);
            var newModule = module;

            // If the module is a prefab, instantiate it
            if(module.gameObject.scene.isLoaded == false)
                newModule = Instantiate(module);

            AddModule(newModule, defaultModule: true);
        }
    }

    public bool AddModule(GunModule module, bool defaultModule = false)
    {
        if(!defaultModule && modules.Count >= maxModuleAmount) return false;
        if(!defaultModule && modules.Contains(module)) return false;

        if (!defaultModule) modules.Add(module);

        module.DisableRigidBody();

        module.transform.parent = modulesParent;
        module.transform.localRotation = Quaternion.identity;
        module.transform.localPosition = Vector3.forward * moduleSize * 0.2f * (modules.Count - 1);
        module.transform.localScale = Vector3.one;

        return true;
    }
    public void RemoveModule(GunModule module)
    {
        if(!modules.Contains(module)) return;

        modules.Remove(module);
    }

    protected override void Fire(Vector3 direction)
    {
        if(modules.Count == 0) return;
        
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
            float spacing = moduleSize * 0.2f;
            if(previewModuleMesh == null)
                Gizmos.DrawWireCube(modulesParent.transform.position + modulesParent.forward * spacing * i, Vector3.one * moduleSize);
            else
                Gizmos.DrawWireMesh(previewModuleMesh, 0, modulesParent.transform.position + modulesParent.forward * spacing * i, modulesParent.rotation, Vector3.one * moduleSize);
        }
    }
}
