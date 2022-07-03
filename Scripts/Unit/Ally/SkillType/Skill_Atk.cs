using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 스킬 공격 타입
 * 장판 타입
 * 웨이브 타입
 */

public class Skill_Atk : Skill
{
    public float _upValue = 1;        // 성장 값
    public float _value = 10;         // 고정 값

    protected override void Start()
    {
        Invoke("Disappear", _playTime);
    }

    public override void SetParameter(float parameter, float upParameter)
    {
        _value = parameter;
        _upValue = upParameter;
    }

    protected void OnDamage(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IDamages enemy = col.GetComponent<IDamages>();
            float damage = _value + (_owner._level * _upValue);
            enemy.Damage(damage);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        OnDamage(other);
    }

    protected void EnemyDamage(Collider col)
    {
        OnDamage(col);
    }

    protected GameManager CheckParent()
    {
        GameManager gm = transform.parent.GetComponent<GameManager>();

        return gm;
    }
}
