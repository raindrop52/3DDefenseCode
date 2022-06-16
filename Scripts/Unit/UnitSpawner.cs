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
    // 적 스폰 위치
    [SerializeField]
    Transform _spawnTransEnemy;
    // 프리팹 정보
    [SerializeField]
    List<GameObject> _unitPrefab;
    [SerializeField]
    List<EnemyPrefabInfo> _enemyPrefab;
    // 부모 객체 설정
    [SerializeField]
    Transform _unitParent;
    [SerializeField]
    Transform _enemyParent;

    // 적 유닛 스폰
    public Enemy SpawnEnemy(int level, int hp)
    {
        int index = level % _enemyPrefab.Count; // 오브젝트 풀링 적 순서
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

    // 아군 유닛 스폰
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
