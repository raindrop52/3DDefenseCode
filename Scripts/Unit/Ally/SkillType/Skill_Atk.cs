using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* ��ų ���� Ÿ��
 * ���� Ÿ��
 * ���̺� Ÿ��
 */

public class Skill_Atk : Skill
{
    public int _damageValue = 1;        // ������ ��갪
    public int _damage = 10;            // ���� ������

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
