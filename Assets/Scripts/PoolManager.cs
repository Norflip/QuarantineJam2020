using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    [System.Serializable]
    public struct Pool
    {
        public GameObject key;
        public int initializeAmount;
    }

    [NaughtyAttributes.ReorderableList]
    //Listan som ansvarar för poolerna som finns
    public List<Pool> items = new List<Pool>();

    //Dicts för att behålla O(1) tidskomplexitet
    Dictionary<GameObject, Pool> pools;
    Dictionary<GameObject, List<GameObject>> poolItems;
    
    //Rooten där alla objekt finns
    Transform root;

    private void Awake()
    {
        //Sätt gameobjectet till att inte vara active
        root = new GameObject("PoolRoot").transform;
        root.gameObject.SetActive(false);
        
        //init av poolerna
        pools = new Dictionary<GameObject, Pool>();
        poolItems = new Dictionary<GameObject, List<GameObject>>();
        
        for (int i = 0; i < items.Count; i++)
        {
            CreatePool(items[i]);
        }
    }
    
    public void CreatePool (Pool pool)
    {
        GameObject go;

        for (int i = 0; i < pool.initializeAmount; i++)
        {
            go = Instantiate(pool.key);
            go.gameObject.SetActive(false);
            go.transform.SetParent(root);
        }
    }

    public static T Fetch <T> (GameObject key, Transform parent = null, bool returnActive = true ) 
        where T : Component
    {
        return PoolManager.Instance.m_Fetch(key, parent, returnActive).GetComponent<T>();
    }
    
    public static GameObject Fetch (GameObject key)
    {
        return PoolManager.Instance.m_Fetch(key);
    }

    public static void Return (GameObject key, GameObject obj)
    {
        PoolManager.Instance.m_Return(key, obj);
    }

    public static void Return (GameObject key, GameObject obj, float time)
    {
        PoolManager.Instance.m_Return(key, obj, time);
    }

    GameObject m_Fetch (GameObject key, Transform parent = null, bool returnActive = true)
    {
        GameObject ret = null;

        if (poolItems.TryGetValue(key, out List<GameObject> items) && items.Count > 0)
        {
            //Ta fram sista objektet och ta bort det från verkliga poolitems-dicten
            ret = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            
            //Uppdatera parenten och sätt gameobjektet som aktivt
            ret.transform.SetParent(parent);
            ret.SetActive(returnActive);
        }
        else
        {
            ret = Instantiate(key);
            ret.transform.SetParent(parent);
            ret.SetActive(returnActive);
        }

        return ret;
    }

    void m_Return(GameObject key, GameObject obj)
    {
        if (poolItems.ContainsKey(key))
        {
            poolItems[key].Add(obj);
            obj.transform.SetParent(root);
        }
        else
        {
            Destroy(obj);
        }
    }

    void m_Return(GameObject key, GameObject obj, float time)
    {
        StartCoroutine(ReturnEnumerator(key, obj, time));
    }

    IEnumerator ReturnEnumerator (GameObject key, GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        m_Return(key, obj);
    }
}
