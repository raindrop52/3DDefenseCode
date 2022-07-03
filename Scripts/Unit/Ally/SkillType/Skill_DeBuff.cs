using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_DeBuff : Skill
{
    public float _upValue = 0.1f;         // 성장값
    public float _value = 1.0f;             // 고정값

    public override void SetParameter(float parameter, float upParameter)
    {
        _value = parameter;
        _upValue = upParameter;
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
