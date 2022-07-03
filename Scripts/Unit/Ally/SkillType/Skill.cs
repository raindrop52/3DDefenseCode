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

// csv���� �ҷ��� ��ų ���� (�ӽ�)
[Serializable]
public class Skill_Info
{
    public string rank;                 // ��ũ (C, B, A, S, Oringin)
    public int type;                    // ���� (0 ����, 1 ����, 2 �����)
    public string name;                 // �̸�
    public string desc;                 // ����
    public float value;                 // ������
    public float upValue;               // ���尪
    public float percent;               // Ȯ��
    public string iconPath;             // ������ �̹��� ���
    public string prefabObjName;        // ������ ������Ʈ ��

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
