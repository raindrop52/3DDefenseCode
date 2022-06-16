using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager I;

    List<Queue<GameObject>> _enemyObjQueue = new List<Queue<GameObject>>();

    [SerializeField] Transform _enemyHpParent;
    [SerializeField] GameObject _enemyHpPrefab;
    Queue<HPBar> _enemyHpObjQueue = new Queue<HPBar>();

    void Awake()
    {
        I = this;

        InitializeEnemyHp(100);
    }

    #region 풀링 - HPBar
    public void InitializeEnemyHp(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _enemyHpObjQueue.Enqueue(CreateEnemyHpObj());
        }
    }

    HPBar CreateEnemyHpObj()
    {
        var newObj = Instantiate(_enemyHpPrefab).GetComponent<HPBar>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(_enemyHpParent);
        newObj.transform.localScale = new Vector3(1f, 1f, 1f);
        return newObj;
    }

    public static HPBar GetEnemyHpObject()
    {
        if (I._enemyHpObjQueue.Count > 0)
        {
            var obj = I._enemyHpObjQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = I.CreateEnemyHpObj();
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    public static void ReturnObject(HPBar obj)
    {
        obj.gameObject.SetActive(false);
        I._enemyHpObjQueue.Enqueue(obj);
    }
    #endregion

    #region 몬스터
    public void InitializeEnemy(List<EnemyPrefabInfo> infos)
    {
        for (int i = 0; i < infos.Count; i++)
        {
            _enemyObjQueue.Add(InsertQueue(infos[i]));
        }
    }

    GameObject CreateEnemyObj(int index)
    {
        var newObj = Instantiate(RoundManager.I._spawner.GetEnemy(index));
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        newObj.transform.localScale = new Vector3(1f, 1f, 1f);
        return newObj;
    }

    Queue<GameObject> InsertQueue(EnemyPrefabInfo info)
    {
        Queue<GameObject> queue = new Queue<GameObject>();

        for (int i = 0; i < info.createCount; i++)
        {
            GameObject objectClone = Instantiate(info.prefab);
            objectClone.SetActive(false);
            objectClone.transform.SetParent(transform);

            queue.Enqueue(objectClone);
        }

        return queue;
    }

    public static GameObject GetEnemyObject(int index)
    {
        if (I._enemyObjQueue.Count > 0)
        {
            var obj = I._enemyObjQueue[index].Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = I.CreateEnemyObj(index);
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static void ReturnObject(Enemy obj)
    {
        int index = obj.Index;
        obj.transform.position = new Vector3(-100f, -100f, -100f);
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(I.transform);
        I._enemyObjQueue[index].Enqueue(obj.gameObject);
    }
    #endregion
}
