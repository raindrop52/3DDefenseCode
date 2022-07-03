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

    Round_Difficulty _difficulty = Round_Difficulty.START;       // 난이도

    #region 유닛 수
    int _failUnitCount = 100;           // 최대 유닛 갯수 ( 게임 난이도 별 패배 갯수 )
    public int FailCount
    {
        get { return _failUnitCount; }
    }
    int _stopCount = 15;                // 스폰이 되지 않는 상태로 만들어줄 갯수(초단위)
    #endregion

    // 게임 시작 관리
    bool _start = false;                // 로비에서 시작 누를 시 적용
    public bool Start
    {
        get { return _start; }
    }
    
    // 스폰 임시 관리용(테스터용)
    [SerializeField] bool _noSpawn = false;

    public UnitSpawner _spawner;

    [Header("필드")]
    [SerializeField] List<GameObject> _fields;

    #region 적 이동 경로
    [Header("적 경로")]
    public List<Transform> _goalList;
    public Goal[] _steps;
    #endregion

    #region 라운드
    [SerializeField] GameObject _eftDust;               // 게임 라운드 시작 시 발생
    [SerializeField] Transform _summonParent;           // Summon_Info 부모 객체
    [SerializeField] List<Summon_Info> _summon_Infos;   // 재시작 시 동작

    // 라운드
    int _roundLevel = 1;
    public int Round
    {
        get { return _roundLevel; }
    }

    int _maxRound = 100;

    // 시간
    float _time = 0f;
    float _oneMin = 60.0f;
    [SerializeField] Text _timer;

    // 적 스폰
    int _enemyCurCount = 0;
    public int Count
    {
        get { return _enemyCurCount; }
    }
    bool _onSpawn = false;
    [SerializeField] List<Enemy> _enemyList;

    // 골드
    [SerializeField] Transform _prefabParent;
    [SerializeField] GameObject _prefabGold;
    [SerializeField] int _gold = 50;
    public int Gold
    {
        get { return _gold; }
    }
    int _roundClearGold = 20;
    int _skillBonusGold = 0;

    // 라운드 적 체력 커브
    [SerializeField] long _maxHp = 50000;
    public AnimationCurve _enemyHpCurve;

    // 소환수 성장 소모 골드 커브
    [SerializeField] long _levelUpGold = 100;
    public AnimationCurve _levelUpCurve;
    #endregion

    // 아군 스폰
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

        // 적 이동 경로 설정
        if(_goalList.Count > 0)
        {
            _steps = _goalList[(int)index].GetComponentsInChildren<Goal>();
        }

        // 해당 난이도에 맞는 필드 활성화
        SwitchField((int)index);
        _eftDust.SetActive(true);

        // 코루틴 동작
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
        // 아군 객체 삭제
        foreach (Summon_Info info in _summon_Infos)
        {
            info.DefaultInfo();
        }
        // 적 객체 삭제
        foreach (Enemy enemy in _enemyList)
        {
            enemy.OnDie();
        }
        // 타임 스케일 초기화
        SetTimeNormal();
        // 타이머 초기화
        _time = 0f;
        _timer.text = string.Format("00 : 00");
        
        // 게임 초기화
        _roundLevel = 1;
        _enemyCurCount = 0;
        _gold = 500;
        UIManager.I._uIIngame.Clear();

        _start = true;
    }

    // 공격 거리 체크
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

    // 다수의 적 체크
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

    // 적 리스트 제거
    public void RemoveEnemy(Enemy enemyCT)
    {
        _enemyList.Remove(enemyCT);
    }

    // 라운드 타이머
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

            // 초가 정지 갯수 아래인 경우
            if(sec < _stopCount)
            {
                // 스폰 상태인 경우
                if(_onSpawn)
                {
                    // 스폰 상태 해제
                    _onSpawn = false;
                }

                // 1초보다 작은 경우
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
                    // 몬스터 스폰
                    int maxHp = (int)((float)(_maxHp * _enemyHpCurve.Evaluate((float)_roundLevel / (float)_maxRound)));
                    // Default Level 이 1이라 -1해줌
                    Enemy enemy = _spawner.SpawnEnemy(_roundLevel-1, maxHp);
                    _enemyList.Add(enemy);
                }
            }

            _enemyCurCount = _enemyList.Count;
            if (_enemyCurCount >= _failUnitCount)
                _alive = false;

            yield return null;
        }

        // 실패 UI 표시
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
        // 100렙 맥스
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

    // 골드 소모
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

    // 골드 획득
    public void GetGold(int mount)
    {
        CreateUseGold(mount);

        _gold += mount;
    }

    #region 아군 스폰
    // 유닛 스폰
    public void SpawnUnit(Summon_Info summonInfo)
    {
        int index = summonInfo._summonIndex;
        Vector3 pos = summonInfo.transform.position;
        Ally ally = _spawner.SpawnUnit(index, pos);
        ally._level = 1;
        summonInfo.SetOwner(ally);
    }

    #endregion

    [Header("타임 Scale")]
    [SerializeField] float _timeMaxLimit = 5f;
    [SerializeField] Text _txtTimeScale;

    public void SetTimeSpeed()
    {
        // 시간 배속 변경
        float time = float.Parse(_txtTimeScale.text);
        Time.timeScale = time;

        // 최대 수치에 도달 시
        if (time == _timeMaxLimit)
        {
            // 1로 초기화 한다. ( 뒤에서 숫자를 1 증가시키기 때문에 )
            time = 1f;
        }

        // 배속 텍스트를 다음 숫자로 바꾸기
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
