using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salsh : Skill_Atk
{
    [SerializeField] Transform _parentTrans;
    [SerializeField] Vector3 _offset;

    protected override void Start()
    {
        Invoke("Disappear", _playTime);
    }

    public override void SetTransform(Transform trans)
    {
        // trans = target
        if (_owner != null)
        {
            if (_parentTrans != null)
            {
                // 초기 위치 설정 ( 주인의 위치를 기준으로 설정 )
                _parentTrans.position = _owner.transform.position + _offset;
                // 방향 수정
                CalcRotation(trans);
            }
        }
    }

    void CalcRotation(Transform target)
    {
        Vector3 dir = target.transform.position - transform.position;
        //dir.x = 0f;
        dir.y = 0f;
        //dir.z = 0f;

        Quaternion rot = Quaternion.LookRotation(dir.normalized * -1f);
        transform.rotation = rot;
    }

    public override void Disappear()
    {
        Destroy(_parentTrans.gameObject);
    }
}
