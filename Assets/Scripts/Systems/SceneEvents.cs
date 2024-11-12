using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Made by SmallBlue at https://github.com/TheSmallBlue

/// Inspired by Facepunch's S&box and their unique interface-led event system
/// It's probably more resource intensive here than there, so dont run it on Update() or anything...

/// You can call any type of RunEvent() with any type of class, then use a method on the action to decide what to do when the event is called.
/// You can also create a class that inherits from ISceneEvent for nicer syntax, just like in S&Box!

/// Example:
/// 
/// This will make all the RigidBodies on a scene shoot up.
/// gameObject.RunEvent<Rigidbody>(x => x.AddForce(Vector3.up * 100f));

public static class SceneEvents
{
    public static void RunEvent<T>(this GameObject gameObject, Action<T> action)
    {
        ExecuteActionInScene(gameObject.scene, action);
    }

    public static void RunEvent<T>(this Scene scene, Action<T> action)
    {
        ExecuteActionInScene(scene, action);
    }

    public static void RunEvent<T>(Action<T> action)
    {
        ExecuteActionInScene(SceneManager.GetActiveScene(), action);
    }

    static void ExecuteActionInScene<T>(Scene scene, Action<T> action)
    {
        if (!scene.isLoaded || !scene.IsValid()) 
            throw new Exception("Scene in which this gameobject is in is either not loaded or not valid!");

        foreach (var rootGameObject in scene.GetRootGameObjects())
        {
            T[] components = rootGameObject.GetComponentsInChildren<T>();
            if (components == null) continue;

            foreach (var component in components)
            {
                action(component);
            }
        }
    }
}

public interface ISceneEvent<T>
{
    static void Post(Action<T> action)
    {
        SceneEvents.RunEvent(action);
    }
}
