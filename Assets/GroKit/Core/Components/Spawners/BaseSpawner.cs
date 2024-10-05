using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core3lb
{
    public class BaseSpawner : MonoBehaviour
    {
        [CoreEmphasize]
        [Tooltip("If positioner is not set it will spawn on itself")]
        public BasePositioner positioner;
        public GameObject[] objectsToSpawn;
        [CoreReadOnly]
        public GameObject lastObjectSpawned;

        public UnityEvent<GameObject> objectSpawned;

        [HideInInspector]
        public bool isOverridden = false;
        public bool canSpawn = true;


        //Spawner's steps are 

        //Call SpawnObject/

        public Action<GameObject, Vector3, Quaternion> OnSpawnObjectOverride;

        public virtual void _SpawnObject()
        {
            PreSpawn(PickObject(), Position, Rotation);
        }

        public virtual void _SpawnObjectIndex(int index)
        {
            PreSpawn(objectsToSpawn[index], Position, Rotation);
        }

        public virtual void _SpawnObjectAmount(int index)
        {
            for (int i = 0; i < index; i++)
            {
                _SpawnObject();
            }
            
        }

        public virtual void _SpawnObject(Transform where)
        {
            PreSpawn(PickObject(), where.position, where.rotation);
        }

        public virtual void _SpawnObject(Vector3 where,Quaternion rotation)
        {
            PreSpawn(PickObject(), where, rotation);
        }

        public virtual Vector3 Position
        {
            get
            {
                if (positioner)
                {
                    return positioner.WhatPosition(transform);
                }
                else
                {
                    return transform.position;
                }
            }
        }

        public virtual Quaternion Rotation
        {
            get
            {
                if (positioner)
                {
                    return positioner.WhatRotation(transform);
                }
                else
                {
                    return transform.rotation;
                }
            }
        }

        public virtual GameObject PickObject()
        {
            return objectsToSpawn.RandomItem();
        }

        public virtual void PreSpawn(GameObject what, Vector3 where, Quaternion rotation,bool bypassOverride = false)
        {
            if(!canSpawn)
            {
                return;
            }
            if (!bypassOverride)
            {
                if (isOverridden) //Networking Override
                {
                    OnSpawnObjectOverride?.Invoke(what, where, rotation);
                    return;
                }
            }
            SpawnActual(what, where, rotation);
        }

        //For RPCS!
        public virtual void SpawnActual(GameObject what, Vector3 where, Quaternion rotation)
        {
            lastObjectSpawned = Instantiate(what, where, rotation);
            AfterSpawn();
        }

        public virtual void AfterSpawn()
        {
            objectSpawned.Invoke(lastObjectSpawned);
        }

    }
}
