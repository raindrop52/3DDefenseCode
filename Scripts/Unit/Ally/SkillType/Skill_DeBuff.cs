using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_DeBuff : Skill
{
    public float _calcValue = 0.1f;         // 수치 비율
    public float _value = 1.0f;             // 기본값

    public override void SetParameter(int parameter)
    {
        _calcValue = parameter;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            InDeBuffTypeEvent(other.gameObject);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            OutDeBuffTypeEvent(other.gameObject);
        }
    }

    protected virtual void InDeBuffTypeEvent(GameObject obj)
    {
        
    }

    protected virtual void OutDeBuffTypeEvent(GameObject obj)
    {

    }
}
