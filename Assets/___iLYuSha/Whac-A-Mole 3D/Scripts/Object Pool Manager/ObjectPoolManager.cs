using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public delegate void RecoverEventHandler<TEventArgs>(object sender, TEventArgs e) where TEventArgs : System.EventArgs;
/// <summary>
/// 回收事件
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public delegate void RecoverEventHandler(object sender, RecoverEventArgs e);
public class RecoverEventArgs : System.EventArgs
{
    public readonly GameObject objRecovery;
    public RecoverEventArgs(GameObject obj)
    {
        objRecovery = obj;
    }
}
/// <summary>
/// 物件池
/// </summary>
public class ObjPoolData
{
    public Queue<GameObject> pool;
    public Transform container;
    public int index;

    public GameObject Reuse(Vector3 position, Quaternion rotation)
    {
        if (pool.Count > 0)
        {
            GameObject reuse = pool.Dequeue();
            reuse.transform.position = position;
            reuse.transform.rotation = rotation;
            //if (reuse.name.Contains("bullet_rocket"))
            //{
            //    if (reuse.activeSelf)
            //        Debug.LogError("ERR:" + reuse.name);
            //    else
            //        Debug.Log("disable:" + reuse.name);
            //}
            reuse.SetActive(true);
            return reuse;
        }
        else
            return null;
        //else
        //{
        //    GameObject go = Instantiate(obj) as GameObject;
        //    go.SetActive(false);
        //    go.transform.SetParent(container);
        //    go.name = go.name + " " + pool.Count + 1;
        //    go.transform.position = position;
        //    go.transform.rotation = rotation;
        //    go.SetActive(true);
        //    return go;
        //}
    }
    // 方案1
    public void Recovery(object sender, RecoverEventArgs e)
    {
        GameObject gameObject = e.objRecovery;
        gameObject.transform.SetParent(container);
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }
    // 方案2
    public void Recovery(GameObject obj)
    {
        obj.transform.SetParent(container);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
/// <summary>
/// 物件池管理
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    private Dictionary<string, ObjPoolData> objPoolIndex = new Dictionary<string, ObjPoolData>(); // 物件池索引
    public int countMaximum;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // 建立物件池
    public ObjPoolData CreatObjPool(GameObject obj, int number)
    {
        // 查詢物件池索引有無相同物件
        if (objPoolIndex.ContainsKey(obj.name))
        {
            ObjPoolData poolData = objPoolIndex[obj.name];
            //Debug.Log(poolData.index + " / " + AirEarlyWarning.instance.numBattlePilot * 2 * number);
            if (poolData.index < countMaximum * number)
            {
                for (int i = 0; i < number; i++)
                {
                    GameObject go = Instantiate(obj) as GameObject;
                    go.transform.SetParent(poolData.container);
                    if (poolData.index <= poolData.pool.Count)
                        go.name = go.name + " " + poolData.pool.Count;
                    else
                        go.name = go.name + " " + poolData.index;
                    //go.GetComponent<ObjectPoolRecoverySystem>().RecoverCallee += poolData.Recovery;
                    go.GetComponent<ObjectPoolRecoverySystem>().OPD = poolData;
                    go.SetActive(false);

                    poolData.pool.Enqueue(go);
                    poolData.index++;
                }
            }
            return poolData;
        }
        // 若無
        else
        {
            // 創建新物件池資料
            ObjPoolData newPoolData = new ObjPoolData();
            newPoolData.pool = new Queue<GameObject>();
            newPoolData.container = new GameObject().transform;
            newPoolData.container.transform.SetParent(transform);
            newPoolData.container.name = obj.name;

            for (int i = 0; i < number; i++)
            {
                GameObject go = Instantiate(obj) as GameObject;
                go.transform.SetParent(newPoolData.container);
                if (newPoolData.index <= newPoolData.pool.Count)
                    go.name = go.name + " " + newPoolData.pool.Count;
                else
                    go.name = go.name + " " + newPoolData.index;
                // go.GetComponent<ObjectPoolRecoverySystem>().RecoverCallee += newPoolData.Recovery;
                go.GetComponent<ObjectPoolRecoverySystem>().OPD = newPoolData;
                go.SetActive(false);

                newPoolData.pool.Enqueue(go);
                newPoolData.index++;
            }
            // 將資料加入物件池總管
            objPoolIndex.Add(obj.name, newPoolData);
            return newPoolData;
        }
    }
    public ObjPoolData CreatObjPoolWhac(GameObject obj, int number)
    {
        // 查詢物件池索引有無相同物件
        if (objPoolIndex.ContainsKey(obj.name))
        {
            ObjPoolData poolData = objPoolIndex[obj.name];

            for (int i = 0; i < number; i++)
            {
                GameObject go = Instantiate(obj) as GameObject;
                go.transform.SetParent(poolData.container);
                if (poolData.index <= poolData.pool.Count)
                    go.name = go.name + " " + poolData.pool.Count;
                else
                    go.name = go.name + " " + poolData.index;
                //go.GetComponent<ObjectPoolRecoverySystem>().RecoverCallee += poolData.Recovery;
                go.GetComponent<ObjectPoolRecoverySystem>().OPD = poolData;
                go.SetActive(false);

                poolData.pool.Enqueue(go);
                poolData.index++;
            }

            return poolData;
        }
        // 若無
        else
        {
            // 創建新物件池資料
            ObjPoolData newPoolData = new ObjPoolData();
            newPoolData.pool = new Queue<GameObject>();
            newPoolData.container = new GameObject().transform;
            newPoolData.container.transform.SetParent(transform);
            newPoolData.container.name = obj.name;

            for (int i = 0; i < number; i++)
            {
                GameObject go = Instantiate(obj) as GameObject;
                go.transform.SetParent(newPoolData.container);
                if (newPoolData.index <= newPoolData.pool.Count)
                    go.name = go.name + " " + newPoolData.pool.Count;
                else
                    go.name = go.name + " " + newPoolData.index;
                // go.GetComponent<ObjectPoolRecoverySystem>().RecoverCallee += newPoolData.Recovery;
                go.GetComponent<ObjectPoolRecoverySystem>().OPD = newPoolData;
                go.SetActive(false);

                newPoolData.pool.Enqueue(go);
                newPoolData.index++;
            }
            // 將資料加入物件池總管
            objPoolIndex.Add(obj.name, newPoolData);
            return newPoolData;
        }
    }


}