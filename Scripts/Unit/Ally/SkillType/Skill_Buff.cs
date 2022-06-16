using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Buff : Skill
{
    public float _value = 1.0f;      // 기본값
    public float _calcValue = 0.1f;         // 수치 비율

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Creature"))
        {
            InBuffTypeEvent(other.gameObject);
        }
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
