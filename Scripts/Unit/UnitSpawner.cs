using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPrefabInfo
{
    public GameObject prefab;
    public int createCount;
}

public class UnitSpawner : MonoBehaviour
{
    // �� ���� ��ġ
    [SerializeField]
    Transform _spawnTransEnemy;
    // ������ ����
    [SerializeField]
    List<GameObject> _unitPrefab;
    [SerializeField]
    List<EnemyPrefabInfo> _enemyPrefab;
    // �θ� ��ü ����
    [SerializeField]
    Transform _unitParent;
    [SerializeField]
    Transform _enemyParent;

    // �� ���� ����
    public Enemy SpawnEnemy(int level, int hp)
    {
        int index = level % _enemyPrefab.Count; // ������Ʈ Ǯ�� �� ����
        GameObject obj = ObjectPoolManager.GetEnemyObject(index);
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.Index = index;
        enemy.transform.SetParent(_enemyParent);
        enemy.transform.localPosition = _spawnTransEnemy.position;
        enemy.name = string.Format("Level {0}", level);
        enemy._maxHp = hp;

        enemy.Init();

        return enemy;
    }

    public void ObjectPoolEnemys()
    {
        ObjectPoolManager.I.InitializeEnemy(_enemyPrefab);
    }

    public GameObject GetEnemy(int index)
    {
        return _enemyPrefab[index].prefab;
    }

    // �Ʊ� ���� ����
    public Ally SpawnUnit(int index, Vector3 pos)
    {
        if (index < 0)
            return null;

        GameObject unitObj = Instantiate(_unitPrefab[index]);

        unitObj.transform.SetParent(_unitParent);
        unitObj.transform.localPosition = pos;

        Ally ally = unitObj.GetComponent<Ally>();
        ally.Init();

        return ally;
    }
}
