using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop : Skill_DeBuff
{
    protected override void Start()
    {
        Invoke("Disappear", _playTime);
    }

    protected override void InDeBuffTypeEvent(GameObject obj)
    {
        if (_owner == null)
            return;

        float value = _value + (_owner._level * _calcValue);
        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy != null)
        {
            if(enemy._isStop == true)
                enemy._timeChecker = 0f;
            else
                enemy._isStop = true;

            enemy._stopTime = value;
        }
    }

    protected override void OutDeBuffTypeEvent(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy != null)
        {
            if(enemy._isStop != false)
                enemy._isStop = false;
        }
    }
}
