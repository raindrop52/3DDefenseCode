using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amplify : Skill_DeBuff
{
    public Vector3 _offset;

    protected override void InDeBuffTypeEvent(GameObject obj)
    {
        float percent = (_value + (_owner._level * _upValue)) * 0.01f;
        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy._dmgAmplify += percent;
        }
    }

    protected override void OutDeBuffTypeEvent(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy._dmgAmplify = 1.0f;
    }

    public override void SetTransform(Transform trans)
    {
        transform.position = trans.position + _offset;
    }
}
