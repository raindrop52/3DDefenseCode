using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : Skill_DeBuff
{
    protected override void InDeBuffTypeEvent(GameObject obj)
    {
        // �θ� Ŭ�������� ���� �ۼ�Ʈ �� ������
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
