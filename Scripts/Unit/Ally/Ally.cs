using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  [�÷��̾�� ĳ���� Ư¡] (������)
 *  1. ���� ���� �� �� �ִ�.
 *  2. �ִϸ��̼��� �������� Ȱ���� �� �ִ�.
 *  3. ��ũ�� ������.
 *  4. ������ ��ų�� �ִ�. (����ؼ� ���� ��� ���� �� �ֵ��� - Default�� ��� ����)
 */

public enum Ally_State
{
    IDLE = 0,
    ATTACK,
}

public class Ally : Unit, IIdleable
{
    protected Animator _anim;
    protected Rigidbody _rigid;

    [SerializeField] Ally_State _state = Ally_State.IDLE;

    public string _name;
    public int _level;

    #region ����
    // ��ų
    public Skill_Inventory _skill_Inven;

    [Header("���� ����")]
    Enemy _atkTarget = null;

    // ���� ������
    [SerializeField] protected int _attackDmg = 5;          // �⺻ ������
    [SerializeField] protected int _attackDmgTmp = 1;       // ������ ���� �����ϴ� ������

    // ���� ��Ÿ�
    [SerializeField] protected float _attackRange = 1.0f;
    [SerializeField] GameObject _rangeObj;

    // ���� �ӵ�
    [SerializeField] protected float _attackSpeed = 1.0f;
    public float _tmpSpeedParty = 0.0f;     // ���� �ӵ� ��Ƽ ������
    public float _tmpSpeedOnly = 0.0f;      // ���� �ӵ� ���� ������

    // ��ȭ �� ���
    public float AtkSpeed
    {
        get { return _attackSpeed; }
        set { _attackSpeed = value; }
    }
    #endregion

    #region ����
    [Header("���� ����")]
    public float _mp = 0f;          // ���� ����
    public float _maxMp = 100f;     // �ִ� ����
    public float _resMp = 1f;       // ���� �� ȸ�� ����
    #endregion

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.position, _attackRange);
    }

    public override void Init()
    {
        base.Init();

        _rigid = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _skill_Inven = GetComponent<Skill_Inventory>();

        _rangeObj = transform.Find("Range").gameObject;
        Vector3 scale = new Vector3(_attackRange / 10f, 1f, _attackRange / 10f);
        _rangeObj.transform.localScale = scale;

        ShowAttackRange(false);

        Idle();

        StartCoroutine(_CheckAttack());
    }

    public void ClearParameter()
    {
        _level = 1;
        _tmpSpeedParty = 0.0f;  // ���� �ӵ� ������
        _mp = 0f;          // ���� ����
        _resMp = 1f;       // ���� �� ȸ�� ����
    }

    IEnumerator _CheckAttack()
    {
        while(true)
        {
            Vector3 pos = new Vector3(transform.position.x, 0f, transform.position.z);
            Enemy target = null;

            if (_atkTarget == null)
            {
                // ������ ���� �ʾƵ� ��Ƽ ���� ��ų�� �����ϵ���
                if(_skill_Inven != null)
                {
                    _skill_Inven.OperateSkill(null);
                }

                target = RoundManager.I.CheckDistance(pos, _attackRange);
                if (target != null)
                {
                    _atkTarget = target;
                }
            }
            else
            {
                if (Vector3.Distance(_atkTarget.transform.position, pos) <= _attackRange)
                {
                    /*// ��ǥ�� ���� ���� ��ȯ
                    {
                        Vector3 to = target.transform.position;
                        Vector3 my = transform.position;
                        to.y = 0.0f;
                        my.y = 0.0f;

                        Vector3 to_enemy = to - my;
                        transform.rotation = Quaternion.FromToRotation(Vector3.forward, to_enemy);
                    }*/

                    // ���� �ִϸ��̼� ����
                    Attack();

                    // ��ǥ�� ���� ����
                    IDamages enemy = _atkTarget.GetComponent<IDamages>();
                    int damage = (_attackDmg + (_attackDmgTmp * _level));
                    enemy.Damage(damage);

                    _skill_Inven.OperateSkill(_atkTarget.transform);

                    // ���� ȸ��
                    _mp += _resMp;

                    // ��Ÿ�� : ���� �ӵ�
                    float speed = _attackSpeed * ((1-_tmpSpeedParty)*(1-_tmpSpeedOnly));
                    if(speed <= 0)
                    {
                        Debug.Log("���� ���� : " + speed);
                    }
                    yield return new WaitForSeconds(speed);
                }
                else
                {
                    _atkTarget = null;
                    Idle();
                }
            }
                        
            yield return null;
        }
    }

    public void Idle()
    {
        _state = Ally_State.IDLE;
    }

    void Attack()
    {
        SetAnimAttack();

        if(_state != Ally_State.ATTACK)
            _state = Ally_State.ATTACK;
    }

    void SetAnimAttack()
    {
        if (_anim != null)
        {
            _anim.SetTrigger("Attack");
        }
    }

    void SetPose()
    {
        if (_anim != null)
        {
            _anim.SetTrigger("Pose");
        }
    }

    public void ShowAttackRange(bool show)
    {
        _rangeObj.SetActive(show);
    }
}
