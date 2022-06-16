using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 구성
 * 일반 공격 및 스킬 관련
 * 공통 : 공격 시 이펙트 표시
 * 발사체를 발사하여 적에게 발사
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
