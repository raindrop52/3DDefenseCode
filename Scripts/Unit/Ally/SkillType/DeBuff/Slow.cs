using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : Skill_DeBuff
{
    protected override void InDeBuffTypeEvent(GameObject obj)
    {
        // 부모 클래스에서 감소 퍼센트 값 가져옴
        //float percent = base.InDeBuffTypeEvent(obj);

        //Enemy enemy = obj.GetComponent<Enemy>();
        //if (enemy != null)
        //{
        //    enemy._tmpMoveSpeed = percent;
        //}
    }

    protected override void OutDeBuffTypeEvent(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy._tmpMoveSpeed = 0f;
        }
    }
}
