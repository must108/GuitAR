using System.Collections.Generic;
using UnityEngine;

namespace Core3lb
{
    public class TrackedSpawner : BaseSpawner
    {
        [CoreHeader("Tracked Spawner Settings")]
        public bool infiniteSpawns;
        [CoreHideIf("infiniteSpawns")]
        public int maxSpawnAmount = 1;
        [CoreReadOnly]
        public List<GameObject> spawnedObjects;

        public override void PreSpawn(GameObject what, Vector3 where, Quaternion rotation, bool bypassOverride = false)
        {
            spawnedObjects.RemoveNulls();
            if (infiniteSpawns || (maxSpawnAmount > 0 && spawnedObjects.Count < maxSpawnAmount))
            {
                base.PreSpawn(what, where, rotation, bypassOverride);
            }

        }

        public override void AfterSpawn()
        {
            spawnedObjects.Add(lastObjectSpawned);
            base.AfterSpawn();
        }

        public virtual void _DestroyAllObjects()
        {
            foreach (var obj in spawnedObjects)
            {
                Destroy(obj);
            }
        }
    }
}
