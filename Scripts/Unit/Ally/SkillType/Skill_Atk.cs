using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 스킬 공격 타입
 * 장판 타입
 * 웨이브 타입
 */

public class Skill_Atk : Skill
{
    public int _damageValue = 1;        // 데미지 계산값
    public int _damage = 10;            // 실제 데미지

    protected override void Start()
    {
        Invoke("Disappear", _playTime);
    }

    public override void SetParameter(int parameter)
    {
        _damageValue = parameter;
    }

    protected override void Update()
    {
        if(_owner != null)
            _damage = _owner._level * _damageValue;
    }

    protected void OnDamage(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IDamages enemy = col.GetComponent<IDamages>();
            enemy.Damage(_damage);
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
