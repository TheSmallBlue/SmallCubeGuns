using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeGuns.ObjectPool
{
    public class Factory<T> where T : MonoBehaviour
    {
        public T CreateObject(T objectToCreate)
        {
            return GameObject.Instantiate(objectToCreate);
        }
    }

    public class ObjectPool<T> where T : MonoBehaviour, IPoolableObject
    {
        T pooledObject;
        Factory<T> myFactory;

        List<T> pool = new List<T>();
        

        public ObjectPool(T objectToPool, int poolSize)
        {
            myFactory = new Factory<T>();

            pooledObject = objectToPool;

            for (int i = 0; i < poolSize; i++)
            {
                T thingToAdd = myFactory.CreateObject(pooledObject);
                thingToAdd.DeActivateObject();
                pool.Add(thingToAdd);
            }
        }

        public T GetObject()
        {
            T returnValue = default;

            if (pool.Count > 0)
            {
                returnValue = pool[0];
                pool.RemoveAt(0);
            }
            else
            {
                returnValue = myFactory.CreateObject(pooledObject);
            }
            returnValue.ActivateObject(this);

            return returnValue;
        }

        public void AddBackToPool(T objectToReAdd)
        {
            objectToReAdd.DeActivateObject();
            pool.Add(objectToReAdd);
        }
    }

    public interface IPoolableObject
    {
        public abstract void ActivateObject<T>(ObjectPool<T> source) where T : MonoBehaviour, IPoolableObject;
        public abstract void DeActivateObject();
    }

}
