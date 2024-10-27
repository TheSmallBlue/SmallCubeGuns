using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentExtensionMethods
{
    /// <summary>
    /// Checks the given component variable to see if its null.
    /// <para> If it isn't, it does a GetComponent(), sets the variable to the result </para>
    /// </summary>
    /// <typeparam name="T"> The component type. </typeparam>
    /// <param name="source"> The source gameobject. </param>
    /// <param name="componentVar"> The variable that will contain the component. </param>
    /// <returns> The value held in the variable componentVar </returns>
    public static T GetComponentIfVarNull<T>(this MonoBehaviour source, ref T var) where T : MonoBehaviour
    {
        if(var == null) var = source.GetComponent<T>();

        return var;
    }
}
