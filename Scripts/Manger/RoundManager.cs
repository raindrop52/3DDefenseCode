using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Round_Difficulty
{
    START = -1,
    EASY = 0,
    NORMAL,
    HARD,
    HELL,
    END
}

public class RoundManager : MonoBehaviour
{
    public static RoundManager I;

    Round_Difficulty _difficulty = Round_Difficulty.START;       // ���̵�

    #region ���� ��
    int _failUnitCount = 100;           // �ִ� ���� ���� ( ���� ���̵� �� �й� ���� )
    public int FailCount
    {
        get { return _failUnitCount; }
    }
    int _stopCount = 15;                // ������ ���� �ʴ� ���·� ������� ����(�ʴ���)
    #endregion

    // ���� ���� ����
    bool _start = false;                // �κ񿡼� ���� ���� �� ����
    public bool Start
    {
        get { return _start; }
    }
    
    // ���� �ӽ� ������(�׽��Ϳ�)
    [SerializeField] bool _noSpawn = false;

    public UnitSpawner _spawner;

    [Header("�ʵ�")]
    [SerializeField] List<GameObject> _fields;

    #region �� �̵� ���
    [Header("�� ���")]
    public List<Transform> _goalList;
    public Goal[] _steps;
    #endregion

    #region ����
    [SerializeField] GameObject _eftDust;               // ���� ���� ���� �� �߻�
    [SerializeField] Transform _summonParent;           // Summon_Info �θ� ��ü
    [SerializeField] List<Summon_Info> _summon_Infos;   // ����� �� ����

    // ����
    int _roundLevel = 1;
    public int Round
    {
        get { return _roundLevel; }
    }

    int _maxRound = 100;

    // �ð�
    float _time = 0f;
    float _oneMin = 60.0f;
    [SerializeField] Text _timer;

    // �� ����
    int _enemyCurCount = 0;
    public int Count
    {
        get { return _enemyCurCount; }
    }
    bool _onSpawn = false;
    [SerializeField] List<Enemy> _enemyList;

    // ���
    [SerializeField] Transform _prefabParent;
    [SerializeField] GameObject _prefabGold;
    [SerializeField] int _gold = 50;
    public int Gold
    {
        get { return _gold; }
    }
    int _roundClearGold = 20;
    int _skillBonusGold = 0;

    // ���� �� ü�� Ŀ��
    [SerializeField] long _maxHp = 50000;
    public AnimationCurve _enemyHpCurve;

    // ��ȯ�� ���� �Ҹ� ��� Ŀ��
    [SerializeField] long _levelUpGold = 100;
    public AnimationCurve _levelUpCurve;
    #endregion

    // �Ʊ� ����
    MouseClick _clickObj;

    bool _alive = true;

    private void Awake()
    {
        I = this;
    }

    public void Init()
    {
        _eftDust.SetActive(false);

        _summon_Infos = new List<Summon_Info>();
        Summon_Info[] summonInfos = _summonParent.GetComponentsInChildren<Summon_Info>();
        foreach (Summon_Info info in summonInfos)
        {
            _summon_Infos.Add(info);
        }

        _enemyList = new List<Enemy>();
        _spawner = GetComponentInChildren<UnitSpawner>();

        _spawner.ObjectPoolEnemys();

        _clickObj = GetComponentInChildren<MouseClick>();

        if (UIManager.I != null)
        {
            UIManager.I._curState = UI_State.Lobby;
        }
    }

    public void ClickLobbyStartBtn()
    {
        UIManager.I._curState = UI_State.Difficulty;
    }

    public void SelectDifficulty(Round_Difficulty index)
    {
        _difficulty = index;

        if (index != Round_Difficulty.START && index != Round_Difficulty.END)
        {
            switch (index)
            {
                case Round_Difficulty.EASY:
                    {
                        _failUnitCount = 100;
                        break;
                    }
                case Round_Difficulty.NORMAL:
                    {
                        _failUnitCount = 90;
                        break;
                    }
                case Round_Difficulty.HARD:
                    {
                        _failUnitCount = 80;
                        break;
                    }
                case Round_Difficulty.HELL:
                    {
                        _failUnitCount = 60;
                        break;
                    }
            }
        }

        // �� �̵� ��� ����
        if(_goalList.Count > 0)
        {
            _steps = _goalList[(int)index].GetComponentsInChildren<Goal>();
        }

        // �ش� ���̵��� �´� �ʵ� Ȱ��ȭ
        SwitchField((int)index);
        _eftDust.SetActive(true);

        // �ڷ�ƾ ����
        StartCoroutine(_RoundTimer());
    }

    void SwitchField(int index = -1)
    {
        if (index < 0)
        {
            if (_clickObj.gameObject.activeSelf == true)
                _clickObj.gameObject.SetActive(false);
        }
        else
        {
            if(_clickObj.gameObject.activeSelf == false)
                _clickObj.gameObject.SetActive(true);
        }

        for (int i = 0; i < _fields.Count; i++)
        {
            if (index == i)
            {
                if(_fields[i].activeSelf == false)
                {
                    _fields[i].SetActive(true);
                }
            }
            else
            {
                if (_fields[i].activeSelf == true)
                {
                    _fields[i].SetActive(false);
                }
            }
        }
    }

    public void ClearStage()
    {
        _alive = true;

        SwitchField();
        // �Ʊ� ��ü ����
        foreach (Summon_Info info in _summon_Infos)
        {
            info.DefaultInfo();
        }
        // �� ��ü ����
        foreach (Enemy enemy in _enemyList)
        {
            enemy.OnDie();
        }
        // Ÿ�� ������ �ʱ�ȭ
        SetTimeNormal();
        // Ÿ�̸� �ʱ�ȭ
        _time = 0f;
        _timer.text = string.Format("00 : 00");
        
        // ���� �ʱ�ȭ
        _roundLevel = 1;
        _enemyCurCount = 0;
        _gold = 500;
        UIManager.I._uIIngame.Clear();

        _start = true;
    }

    // ���� �Ÿ� üũ
    public Enemy CheckDistance(Vector3 pos, float range)
    {
        Enemy resultEnemy = null;

        foreach(Enemy enemy in _enemyList)
        {
            Vector3 enemyPos = new Vector3(enemy.transform.position.x, 0f, enemy.transform.position.z);

            if (Vector3.Distance(enemyPos, pos) <= range)
            {
                resultEnemy = enemy;
                break;
            }
        }

        return resultEnemy;
    }

    // �ټ��� �� üũ
    public List<Enemy> CheckDistanceAllEnemy(Vector3 pos, float range)
    {
        List<Enemy> enemies = new List<Enemy>();

        foreach (Enemy enemy in _enemyList)
        {
            Vector3 enemyPos = new Vector3(enemy.transform.position.x, 0f, enemy.transform.position.z);

            if (Vector3.Distance(enemyPos, pos) <= range)
            {
                enemies.Add(enemy);
            }
        }

        return enemies;
    }

    // �� ����Ʈ ����
    public void RemoveEnemy(Enemy enemyCT)
    {
        _enemyList.Remove(enemyCT);
    }

    // ���� Ÿ�̸�
    IEnumerator _RoundTimer()
    {
        int prevSec = -1;

        while(_alive)
        {
            if (_noSpawn)
            {
                break;
            }

            _time += Time.deltaTime;

            int sec = Mathf.FloorToInt(_oneMin - _time);

            _timer.text = string.Format("00 : {0:D2}", sec);

            // �ʰ� ���� ���� �Ʒ��� ���
            if(sec < _stopCount)
            {
                // ���� ������ ���
                if(_onSpawn)
                {
                    // ���� ���� ����
                    _onSpawn = false;
                }

                // 1�ʺ��� ���� ���
                if(sec < 1)
                {
                    _roundLevel++;

                    _time = 0.0f;

                    _onSpawn = true;

                    int getGold = (_roundClearGold + _skillBonusGold);

                    GetGold(getGold);

                    continue;
                }
            }
            else
            {
                if(prevSec != sec)
                {
                    prevSec = sec;
                    // ���� ����
                    int maxHp = (int)((float)(_maxHp * _enemyHpCurve.Evaluate((float)_roundLevel / (float)_maxRound)));
                    // Default Level �� 1�̶� -1����
                    Enemy enemy = _spawner.SpawnEnemy(_roundLevel-1, maxHp);
                    _enemyList.Add(enemy);
                }
            }

            _enemyCurCount = _enemyList.Count;
            if (_enemyCurCount >= _failUnitCount)
                _alive = false;

            yield return null;
        }

        // ���� UI ǥ��
        _start = false;
        UIManager.I._curState = UI_State.GameOver;
    }

    public int GetLevelUpGold(int level)
    {
        int gold = (int)((float)(_levelUpGold * _levelUpCurve.Evaluate((float)level / 100.0f)));

        return gold;
    }

    public bool LevelUpGold(int level)
    {
        // 100�� �ƽ�
        int useGold = GetLevelUpGold(level);

        if (UseGold(useGold))
            return true;
        else
            return false;
    }

    void CreateUseGold(int gold, bool isGet = true)
    {
        GameObject prefab = Instantiate(_prefabGold);
        prefab.transform.SetParent(_prefabParent);
        prefab.transform.localScale = new Vector3(1f, 1f, 1f);
        GoingUpText goText = prefab.GetComponent<GoingUpText>();

        goText.ShowGold(gold, isGet);
    }

    // ��� �Ҹ�
    public bool UseGold(int mount)
    {
        if (_gold - mount >= 0)
        {
            CreateUseGold(mount, false);
            _gold -= mount;
            return true;
        }

        return false;
    }

    // ��� ȹ��
    public void GetGold(int mount)
    {
        CreateUseGold(mount);

        _gold += mount;
    }

    #region �Ʊ� ����
    // ���� ����
    public void SpawnUnit(Summon_Info summonInfo)
    {
        int index = summonInfo._summonIndex;
        Vector3 pos = summonInfo.transform.position;
        Ally ally = _spawner.SpawnUnit(index, pos);
        ally._level = 1;
        summonInfo.SetOwner(ally);
    }

    #endregion

    [Header("Ÿ�� Scale")]
    [SerializeField] float _timeMaxLimit = 5f;
    [SerializeField] Text _txtTimeScale;

    public void SetTimeSpeed()
    {
        // �ð� ��� ����
        float time = float.Parse(_txtTimeScale.text);
        Time.timeScale = time;

        // �ִ� ��ġ�� ���� ��
        if (time == _timeMaxLimit)
        {
            // 1�� �ʱ�ȭ �Ѵ�. ( �ڿ��� ���ڸ� 1 ������Ű�� ������ )
            time = 1f;
        }

        // ��� �ؽ�Ʈ�� ���� ���ڷ� �ٲٱ�
        if (_txtTimeScale != null)
        {
            int timeScale = (int)time + 1;
            _txtTimeScale.text = string.Format("{0}", timeScale);
        }
    }

    public void SetTimeNormal()
    {
        Time.timeScale = 1.0f;
        float time = float.Parse(_txtTimeScale.text);
        if(time > 2.0f)
        {
            _txtTimeScale.text = "2";
        }
    }

    public void SetPauseTime()
    {
        Time.timeScale = 0.0f;
    }

    public void ExitGame()
    {
        GameManager.I.ExitGame();
    }
}
