using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPool
    {
        // Gameobject to pool
        public GameObject prefab;

        // Maximum instances of the gameobject
        public int maximumInstances;

        // Name of the pool
        public Pools.Types poolType;
        
        [HideInInspector]
        public Dictionary<int, GameObject> passiveObjectsDictionary;

        [HideInInspector]
        public GameObject pool;

        /// <summary>
        /// Initialize the pool with creating instances of the gameobject and a container
        /// for the hieararchy
        /// </summary>
        public void InitializePool()
        {
            //activeList = new List<GameObject>();
            passiveObjectsDictionary = new Dictionary<int, GameObject>();
            pool = new GameObject("[" + poolType + "]");
            DontDestroyOnLoad(pool);

            // Reference to the created instance
            GameObject clone;

            for (int i = 0; i < maximumInstances; i++)
            {
                // Create the gameobject
                clone = Instantiate(prefab);

                // Deactivate and add to the container and list
                clone.SetActive(false);
                clone.transform.SetParent(pool.transform);

                passiveObjectsDictionary.Add(clone.GetInstanceID(), clone);
            }
        }

        /// <summary>
        /// Get the next gameobject that can be spawned from the pool
        /// </summary>
        /// <returns>Next gameobject to spawn</returns>
        GameObject tempObject;
        public GameObject GetNextObject()
        {
            if (passiveObjectsDictionary.Count > 0)
            {
                tempObject = passiveObjectsDictionary.Values.ElementAt(0);
                passiveObjectsDictionary.Remove(passiveObjectsDictionary.Keys.ElementAt(0));
                return tempObject;
            }
            else
            {
                Debug.Log(string.Format("PoolManager: {0} - passiveObjectsDictionary is empty. Instantiating new one.", PoolType));
                GameObject clone; 
                clone = Instantiate(prefab);
                clone.SetActive(false);
                clone.transform.SetParent(pool.transform);
                passiveObjectsDictionary.Add(clone.GetInstanceID(), clone);

                tempObject = passiveObjectsDictionary.Values.ElementAt(0);
                passiveObjectsDictionary.Remove(passiveObjectsDictionary.Keys.ElementAt(0));
                return tempObject;
            }
        }

        // Properties
        public int MaximumInstances { get { return maximumInstances; } }
        public Pools.Types PoolType { get { return poolType; } set { poolType = value; } }
    }

    // List to hold all the pools for the game
    public List<ObjectPool> pools;

    public static PoolManager Instance;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize all the pools
        for (int i = 0; i < pools.Count; i++)
        {
            pools[i].InitializePool();
        }
    }

    /// <summary>
    /// Spawn the next gameobject at its current place 
    /// </summary>
    /// <param name="poolName">Name of the pool to get the gameobject</param>
    /// <returns>Spawned gameobject</returns>
    public GameObject Spawn(Pools.Types poolType, Transform parent = null)
    {
        return Spawn(poolType, null, null, parent);
    }

    public GameObject Spawn(Pools.Types poolType, Vector3? position, Quaternion? rotation, Transform parent = null)
    {
        // Get the pool with the given pool name
        ObjectPool pool = GetObjectPool(poolType);

        if (pool == null)
        {
            //Debug.LogErrorFormat("Cannot find the object pool with name %s", poolName);

            return null;
        }

        // Get the next object from the pool
        GameObject clone = pool.GetNextObject();

        if (clone == null)
        {
            //Debug.LogError("Scene contains maximum number of instances.");

            return null;
        }

        if (parent != null)
        {
            clone.transform.SetParent(parent);
        }

        if (position != null)
        {
            clone.transform.position = (Vector3)position;
        }

        if (rotation != null)
        {
            clone.transform.rotation = (Quaternion)rotation;
        }

        // Spawn the gameobject
        clone.SetActive(true);

        //pool.activeList.Add(clone);

        //pool.passiveList.RemoveAt(pool.passiveList.Count - 1);

        return clone;
    }

    /// <summary>
    /// Spawn the next gameobject from the given pool with the specified position and
    /// rotation
    /// </summary>
    /// <param name="poolName">Name of the pool</param>
    /// <param name="position">Position of the spawned gameobject</param>
    /// <param name="rotation">Rotation of the spawned gameobject</param>
    /// <returns>Spawned gameobject</returns>
    //public GameObject Spawn(Pools.Types poolType, Vector3 position, Quaternion rotation)
    //{
    //    // Spawn the gameobject
    //    GameObject clone = Spawn(poolType);

    //    // Set its position and rotation
    //    if (clone != null)
    //    {
    //        clone.transform.position = position;
    //        clone.transform.rotation = rotation;

    //        return clone;
    //    }

    //    return null;
    //}

    /// <summary>
    /// Spawn the next gameobject from the given pool to the random location between two
    /// vectors and given rotation
    /// </summary>
    /// <param name="poolName">Name of the pool</param>
    /// <param name="minVector">Minimum vector position for the spawned gameobject</param>
    /// <param name="maxVector">Maximum vector position for the spawned gameobject</param>
    /// <param name="rotation">Rotation of the spawned gameobject</param>
    /// <returns>Spawned gameobject</returns>
    public GameObject Spawn(Pools.Types poolType, Vector3 minVector, Vector3 maxVector, Quaternion rotation)
    {
        // Determine the random position
        float x = Random.Range(minVector.x, maxVector.x);
        float y = Random.Range(minVector.y, maxVector.y);
        float z = Random.Range(minVector.z, maxVector.z);
        Vector3 newPosition = new Vector3(x, y, z);

        // Spawn the next gameobject
        return Spawn(poolType, newPosition, rotation);
    }

    /// <summary>
    /// Despawn the given gameobject from the scene
    /// </summary>
    /// <param name="obj">Gameobject to despawn</param>
    public void Despawn(Pools.Types poolType, GameObject obj)
    {
        ObjectPool poolObject = GetObjectPool(poolType);

        obj.transform.SetParent(poolObject.pool.transform);

        //pool.activeList.Remove(obj);

        if (!poolObject.passiveObjectsDictionary.ContainsKey(obj.GetInstanceID()))
        {
            poolObject.passiveObjectsDictionary.Add(obj.GetInstanceID(), obj);
        }

        obj.SetActive(false);
    }

    /// <summary>
    /// Get the object pool reference from the pool list with the given pool name
    /// </summary>
    /// <param name="poolName">Name of the pool</param>
    /// <returns>ObjectPool object with the given name</returns>
    public ObjectPool GetObjectPool(Pools.Types poolType)
    {
        string poolName = Pools.GetTypeStr(poolType);
        // Find the pool with the given name
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].PoolType == poolType)
            {
                return pools[i];
            }
        }

        return null;  
    } 

}
