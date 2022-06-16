using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  [플레이어블 캐릭터 특징] (포괄적)
 *  1. 적을 공격 할 수 있다.
 *  2. 애니메이션을 여러가지 활용할 수 있다.
 *  3. 랭크를 가진다.
 *  4. 고유의 스킬이 있다. (상속해서 개별 기술 가질 수 있도록 - Default는 기술 없음)
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

    #region 공격
    // 스킬
    public Skill_Inventory _skill_Inven;

    [Header("공격 관련")]
    Enemy _atkTarget = null;

    // 공격 데미지
    [SerializeField] protected int _attackDmg = 5;          // 기본 데미지
    [SerializeField] protected int _attackDmgTmp = 1;       // 레벨에 따라 증가하는 데미지

    // 공격 사거리
    [SerializeField] protected float _attackRange = 1.0f;
    [SerializeField] GameObject _rangeObj;

    // 공격 속도
    [SerializeField] protected float _attackSpeed = 1.0f;
    public float _tmpSpeedParty = 0.0f;     // 공격 속도 파티 버프용
    public float _tmpSpeedOnly = 0.0f;      // 공격 속도 개인 버프용

    // 강화 시 사용
    public float AtkSpeed
    {
        get { return _attackSpeed; }
        set { _attackSpeed = value; }
    }
    #endregion

    #region 마나
    [Header("마나 관련")]
    public float _mp = 0f;          // 현재 마나
    public float _maxMp = 100f;     // 최대 마나
    public float _resMp = 1f;       // 공격 시 회복 마나
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
        _tmpSpeedParty = 0.0f;  // 공격 속도 버프용
        _mp = 0f;          // 현재 마나
        _resMp = 1f;       // 공격 시 회복 마나
    }

    IEnumerator _CheckAttack()
    {
        while(true)
        {
            Vector3 pos = new Vector3(transform.position.x, 0f, transform.position.z);
            Enemy target = null;

            if (_atkTarget == null)
            {
                // 공격을 하지 않아도 파티 버프 스킬은 동작하도록
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
                    /*// 목표를 향해 방향 전환
                    {
                        Vector3 to = target.transform.position;
                        Vector3 my = transform.position;
                        to.y = 0.0f;
                        my.y = 0.0f;

                        Vector3 to_enemy = to - my;
                        transform.rotation = Quaternion.FromToRotation(Vector3.forward, to_enemy);
                    }*/

                    // 공격 애니메이션 동작
                    Attack();

                    // 목표를 향해 공격
                    IDamages enemy = _atkTarget.GetComponent<IDamages>();
                    int damage = (_attackDmg + (_attackDmgTmp * _level));
                    enemy.Damage(damage);

                    _skill_Inven.OperateSkill(_atkTarget.transform);

                    // 마나 회복
                    _mp += _resMp;

                    // 쿨타임 : 공격 속도
                    float speed = _attackSpeed * ((1-_tmpSpeedParty)*(1-_tmpSpeedOnly));
                    if(speed <= 0)
                    {
                        Debug.Log("공속 에러 : " + speed);
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
