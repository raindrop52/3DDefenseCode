using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * ����
 * �Ϲ� ���� �� ��ų ����
 * ���� : ���� �� ����Ʈ ǥ��
 * �߻�ü�� �߻��Ͽ� ������ �߻�
 * 
 */

public class Attack : MonoBehaviour
{
    protected Transform _target;

    protected virtual void Start()
    {

    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
