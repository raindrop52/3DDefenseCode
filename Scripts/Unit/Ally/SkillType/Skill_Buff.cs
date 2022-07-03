using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Buff : Skill
{
    public float _value = 1.0f;         // 고정값
    public float _upValue = 0.1f;       // 성장값

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Creature"))
        {
            InBuffTypeEvent(other.gameObject);
        }
    }

    public override void SetParameter(float parameter, float upParameter)
    {
        _value = parameter;
        _upValue = upParameter;
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Creature"))
        {
            OutBuffTypeEvent(other.gameObject);
        }
    }

    protected virtual void InBuffTypeEvent(GameObject obj)
    {
    }

    protected virtual void OutBuffTypeEvent(GameObject obj)
    {
        
    }
}
