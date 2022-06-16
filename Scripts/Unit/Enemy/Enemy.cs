using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   //Navigation ��� ��� �� �ʿ�

/*  
 *  [�� ��ü Ư¡]
 *  1. �������� ������ Ʈ���� �޸���.
 *  2. ������ �޴´�.
 *  3. ü���� ���ϸ� ��������. (���� �� ����� - Destroy)
*/

public class Enemy : Unit, IDamages
{
    #region ���� ����(������)
    [SerializeField] bool _die = false;
    HPBar _hpBar;
    
    // ������Ʈ Ǯ�� �� ����
    int _index;
    public int Index
    { get { return _index; }  set { _index = value; } }

    public float _dmgAmplify = 1.0f;
    #endregion

    #region �̵� ����
    int _loop = 0;
    [SerializeField] float stopDist = 0.08f;
    [SerializeField] float _moveSpeed = 2f;
    public float _tmpMoveSpeed = 0f;    // ���ο� ��ų ���� ��
    public bool _isStop = false;
    public float _stopTime = 0f;

    // Ÿ�� üũ ��
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

    // �ð� üũ ��
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

    // ������Ʈ Ȱ��ȭ �� ����(�ʱ�ȭ)
    private void OnEnable()
    {
        _die = false;

        StartCoroutine(_Running());
    }

    #region ���� ����(������)
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

    #region �̵� ����
    IEnumerator _Running()
    {
        yield return new WaitUntil(() => RoundManager.I._steps.Length > 0);

        // �ʱ� Ÿ�� ����
        SetTarget();

        // �ױ� ������ �޸�
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
