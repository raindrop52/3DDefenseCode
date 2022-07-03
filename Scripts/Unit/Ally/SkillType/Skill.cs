using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill_Recipe
{
    public string _rankBase;
    public string _rankSub;
    public string _rankResult;
    public string _base;
    public string _sub;
    public string _result;

    public Skill_Recipe()
    {

    }

    public Skill_Recipe(string main, string sub)
    {
        _base = main;
        _sub = sub;
    }
}

// csv에서 불러올 스킬 정보 (임시)
[Serializable]
public class Skill_Info
{
    public string rank;                 // 랭크 (C, B, A, S, Oringin)
    public int type;                    // 종류 (0 공격, 1 버프, 2 디버프)
    public string name;                 // 이름
    public string desc;                 // 설명
    public float value;                 // 고정값
    public float upValue;               // 성장값
    public float percent;               // 확률
    public string iconPath;             // 아이콘 이미지 경로
    public string prefabObjName;        // 프리팹 오브젝트 명

    public Sprite sprite;

    public Skill_Info()
    {

    }

    public Skill_Info(string rank, int type, string name, string desc, float value, float upValue, float percent, string iconPath, string objName)
    {
        this.rank = rank;
        this.type = type;
        this.name = name;
        this.desc = desc;
        this.value = value;
        this.upValue = upValue;
        this.percent = percent;
        this.iconPath = iconPath;
        prefabObjName = objName;
    }
}

public enum Skil_Rank
{
    C = 0,
    B,
    A,
    S,
    Original,
}

public class Skill : MonoBehaviour
{
    [SerializeField] protected float _playTime = 5.0f;
    public Ally _owner;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {

    }

    public virtual void SetParameter(float parameter, float upParameter)
    {
        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {

    }

    public virtual void SetTransform(Transform trans)
    {
        transform.position = trans.position;
    }

    public virtual void Disappear()
    {
        Destroy(gameObject);
    }
}
