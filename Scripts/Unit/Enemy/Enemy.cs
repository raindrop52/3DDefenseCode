using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   //Navigation 기능 사용 시 필요

/*  
 *  [적 객체 특징]
 *  1. 쓰러지기 전까지 트랙을 달린다.
 *  2. 공격을 받는다.
 *  3. 체력이 다하면 쓰러진다. (게임 내 사라짐 - Destroy)
*/

public class Enemy : Unit, IDamages
{
    #region 생존 관련(데미지)
    [SerializeField] bool _die = false;
    HPBar _hpBar;
    
    // 오브젝트 풀링 적 순서
    int _index;
    public int Index
    { get { return _index; }  set { _index = value; } }

    public float _dmgAmplify = 1.0f;
    #endregion

    #region 이동 관련
    int _loop = 0;
    [SerializeField] float stopDist = 0.08f;
    [SerializeField] float _moveSpeed = 2f;
    public float _tmpMoveSpeed = 0f;    // 슬로우 스킬 적용 시
    public bool _isStop = false;
    public float _stopTime = 0f;

    // 타겟 체크 용
    [SerializeField] Transform _target;
    #endregion

    public override void Init()
    {
        base.Init();

        HPBar hpBar = ObjectPoolManager.GetEnemyHpObject();
        hpBar._target = transform;
        hpBar.Init();

        if (_hpBar == null)
        {
            _hpBar = hpBar;
        }
    }

    // 시간 체크 용
    public float _timeChecker = 0f;
    private void Update()
    {
        if(_isStop == true)
        {
            if(_timeChecker >= _stopTime)
            {
                _isStop = false;
                _timeChecker = 0f;
            }

            _timeChecker += Time.deltaTime;
        }
    }

    // 오브젝트 활성화 시 동작(초기화)
    private void OnEnable()
    {
        _die = false;

        StartCoroutine(_Running());
    }

    #region 생존 관련(데미지)
    public void Damage(int hitDmg)
    {
        _hp -= (int)(hitDmg * _dmgAmplify);

        if (_hpBar != null)
        {
            float percent = (float)_hp / (float)_maxHp;

            _hpBar.SetGague(percent);
        }

        if (_hp <= 0)
        {
            OnDie();
        }
    }

    public void OnDie()
    {
        _die = true;

        if (_hpBar != null)
        {
            _hpBar.Disappear();
            _hpBar = null;
        }
    }
    #endregion

    #region 이동 관련
    IEnumerator _Running()
    {
        yield return new WaitUntil(() => RoundManager.I._steps.Length > 0);

        // 초기 타겟 설정
        SetTarget();

        // 죽기 전까지 달림
        while (_die == false)
        {
            MoveToTarget();

            yield return null;
        }

        _loop = 0;

        RoundManager.I.RemoveEnemy(this);

        ObjectPoolManager.ReturnObject(this);
    }

    public void SetTarget(int index = 0)
    {
        Goal[] goals = RoundManager.I._steps;
        if (goals.Length > 0)
        {
            int coner = goals.Length;

            _target = goals[_loop % coner].transform;
            transform.LookAt(_target.position);
        }
    }

    void MoveToTarget()
    {
        if (_isStop == true)
            return;

        float percent = _tmpMoveSpeed;
        if (_tmpMoveSpeed == 0f)
        {
            percent = 1.0f;
        }

        float speed = _moveSpeed * percent;

        Vector3 target = _target.position;

        if (Vector3.Distance(transform.position, target) <= stopDist)
        {
            _loop++;
            SetTarget(_loop);
        }

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
    #endregion
}
