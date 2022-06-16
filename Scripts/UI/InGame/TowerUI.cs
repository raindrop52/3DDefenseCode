using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour
{
    [SerializeField]
    Summon_Info _curSummonInfo;           // 현재 선택된 타워 정보
    EasyTween _tweenUI;
    public EasyTween _tweenButtonUI;

    [SerializeField] Button _btnSummon;
    [SerializeField] Button _btnEquip;
    [SerializeField] Button _btnEquipCancle;
    [SerializeField] Button _btnClear;
    [SerializeField] Text _textName;
    [SerializeField] Text _textLevel;

    [SerializeField] SkillInfoUI _originUI;
    [SerializeField] Transform _skillUIParent;
    SkillInfoUI[] _skillUIs;

    bool _isViewInventory = false;
    public bool IsViewInventory
    {
        get { return _isViewInventory; }
    }

    [SerializeField] int _curSelecetIndex = -1;

    void Start()
    {
        _tweenUI = GetComponent<EasyTween>();
        _tweenUI.gameObject.SetActive(false);
        _skillUIs = _skillUIParent.GetComponentsInChildren<SkillInfoUI>(true);
    }

    public void HideTowerInfo()
    {
        if (_isViewInventory == true)
        {
            gameObject.SetActive(true);
            // 번쩍임 방지를 위해 강제로 이동시킴
            gameObject.transform.position = new Vector3(-2500f, 0f, 0f);
            _isViewInventory = false;

            DeSelectTower();

            if(MouseClick.I != null)
            {
                MouseClick.I.OffClickInfo();
            }
        }

        
    }

    // 타워 클릭 시 보여지는 정보 변경
    public void ShowTowerInfo(bool show)
    {
        if(show == true)
        {
            // 활성화 시 선택된 타워 정보 표시
            if(_curSummonInfo != null)
            {
                ShowLevelUI(_curSummonInfo._isSummon);

                SetUIData();
            }
        }

        // UI 표시 및 비표시
        if(gameObject.activeSelf != show)
        {
            ShowTween(show);
        }
    }

    void ShowTween(bool show)
    {
        if(_tweenUI != null)
            _tweenUI.ShowObjectAnimation(show);

        if (_tweenButtonUI != null)
            _tweenButtonUI.ShowObjectAnimation(show);
    }

    // 타워 선택
    public void SelectTower(Summon_Info info)
    {
        if(_curSummonInfo == info)
        {
            if(gameObject.activeSelf == false)
            {
                ShowTween(true);
            }
            return;
        }
        else if (_curSummonInfo != info)
        {
            // 기존 사거리 표시 해제
            ShowRange(false);
        }

        _curSummonInfo = info;

        ShowTowerInfo(true);

        ShowRange(true);

        // 타워 선택 후 다시 타워 선택하는 경우를 위해 초기화
        _curSelecetIndex = -1;
    }

    // 타워 선택 해제
    public void DeSelectTower()
    {
        ShowRange(false);

        ShowTowerInfo(false);

        _curSummonInfo = null;

        _curSelecetIndex = -1;
    }

    // 공격 거리 표시
    void ShowRange(bool show)
    {
        if (_curSummonInfo != null && _curSummonInfo.Owner != null)
        {
            _curSummonInfo.Owner.ShowAttackRange(show);
        }
    }

    void ShowButtonEquipClear(bool show)
    {
        if(_btnEquip.gameObject.activeSelf != show)
            _btnEquip.gameObject.SetActive(show);

        _btnEquipCancle.gameObject.SetActive(false);

        if (_btnClear.gameObject.activeSelf != show)
            _btnClear.gameObject.SetActive(show);
    }

    void SetUIData()
    {
        // 이름 표시
        _textName.text = _curSummonInfo._name;
        // 장착, 해제 버튼 활성화
        ShowButtonEquipClear(_curSummonInfo._isSummon);
        // 오리지널 스킬 표시
        ShowOriginSkill();
        // 보유 스킬 표시
        ShowRankSkills();
    }

    void ShowOriginSkill()
    {
        if (_curSummonInfo._isSummon == true)
        {
            // 소환이 된 상태에서만 레벨 표시
            _textLevel.text = string.Format("LV.{0}", _curSummonInfo.Owner._level);
            // 오리지널 스킬 정보 설정
            if (_curSummonInfo._skillInventory != null)
            {
                Skill_Info originInfo = _curSummonInfo._skillInventory.GetOriginSkillInfo();
                if (originInfo != null)
                    _originUI.SetInfo(originInfo);
            }
        }
        else
        {
            _originUI.DefaultSetting();
        }
    }

    void ShowRankSkills()
    {
        // 보유 스킬 정보 설정
        for (int i = 0; i < _skillUIs.Length; i++)
        {
            if (_curSummonInfo._skillInventory != null)
            {
                Skill_Info info = _curSummonInfo._skillInventory.GetSkillInfo(i);
                if (info != null)
                {
                    _skillUIs[i].SetInfo(info);
                    continue;
                }
            }

            _skillUIs[i].DefaultSetting();
        }
    }

    void ShowLevelUI(bool show)
    {
        if(show == true)
        {
            _btnSummon.gameObject.SetActive(false);
            _textLevel.gameObject.SetActive(true);
        }
        else
        {
            _btnSummon.gameObject.SetActive(true);
            _textLevel.gameObject.SetActive(false);
        }
    }

    #region 버튼 기능 구현부
    // 소환 버튼
    public void SummonCreature()
    {
        if (RoundManager.I.UseGold(50))      //TODO : 골드 소모
        {
            // Prefab 부분
            RoundManager.I.SpawnUnit(_curSummonInfo);

            // UI 부분
            _curSummonInfo._isSummon = true;

            // 이름 변경
            _curSummonInfo._name = _curSummonInfo.Owner._name;
            // TODO : 고유 스킬 설정 ( 데이터에서 고유 스킬 찾아옴 )
            Skill_Data skill = new Skill_Data();
            Original origin = _curSummonInfo.Owner.transform.GetComponent<Original>();
            if (origin != null)
            {
                // TODO : 고유 스킬 설정 ( 데이터에서 고유 스킬 찾아옴 )
            }
            else
            {
                // 임시 스킬
                skill.skill_Obj = null;
                skill.skill_Info = new Skill_Info("original", 0, "임시", "글로 작성한 스킬 데미지는 없다.", 0.0f, 0.1f, "5", "");
            }

            _curSummonInfo._skillInventory.SetOriginSkill(skill);
            // 소환 버튼 숨김 및 레벨 표시
            ShowLevelUI(true);
            SetUIData();

            // 사정거리 표시
            ShowRange(true);
        }
    }

    // 성장 버튼
    public void LevelUpCreature()
    {
        // 소환 된 상태일 때만 눌리도록
        if (_curSummonInfo != null && _curSummonInfo._isSummon == true)
        {
            if (RoundManager.I.UseGold(0))  // TODO : 골드 소모
            {
                _textLevel.text = string.Format("LV.{0}", ++_curSummonInfo.Owner._level);
            }
        }
    }

    public void EquipSkill()
    {
        InventoryUI invenUI = UIManager.I._uIIngame._inventoryUI;
        if (invenUI != null)
        {
            int index = invenUI.FindClickIndex();
            if (index >= 0)
            {
                Skill_Data data = invenUI._skillList[index];

                // 소환수에게 스킬 붙이기
                // true 시 장착 성공
                if (_curSummonInfo._skillInventory.SetSkill(data) == true)
                {
                    // 인벤토리UI에 있는 해당 스킬을 뺀다.
                    invenUI.MinusCount(index);
                    // 스킬 상세 정보 창을 꺼준다.
                    UIManager.I._uIIngame._inventoryClickUI.Hide();
                    // 스킬 정보를 갱신한다.
                    ShowRankSkills();
                }
            }
        }
    }

    // 스킬 장착 버튼 ( 인벤토리 UI를 띄운다 )
    public void ShowInventoryUI()
    {
        InventoryUI invenUI = UIManager.I._uIIngame._inventoryUI;
        if(invenUI != null)
        {
            // 인벤토리 UI를 보여준다. (비활성 상태 시)
            if(invenUI.gameObject.activeSelf == false)
            {
                // 인벤토리 탭으로 변경
                UIManager.I._uIIngame.ChangeTab(0);
            }

            // TowerUI를 치운다.
            gameObject.SetActive(false);
            _isViewInventory = true;

            if (_btnEquipCancle.gameObject.activeSelf == false)
                _btnEquipCancle.gameObject.SetActive(true);

            _btnEquip.gameObject.SetActive(false);
        }
    }

    public void ShowThis()
    {
        // 장착 버튼을 통해 창이 없어진 경우
        if (gameObject.activeSelf == false)
        {
            // TowerUI를 불러온다.
            gameObject.SetActive(true);
            _isViewInventory = false;

            if (_btnEquip.gameObject.activeSelf == false)
                _btnEquip.gameObject.SetActive(true);

            _btnEquipCancle.gameObject.SetActive(false);
        }
    }

    // 스킬 해제
    public void ClearSkill()
    {
        // 선택된 스킬이 있는지 체크
        if(_curSelecetIndex >= 0)
        {
            Debug.Log("해제 버튼 클릭");

            Skill_Data data = _curSummonInfo._skillInventory.GetSkillData(_curSelecetIndex);
            if (data != null)
            {
                // UI에서 빼줌
                _skillUIs[_curSelecetIndex].DefaultSetting();

                // 인벤토리UI에 추가
                UIManager.I._uIIngame._inventoryUI.CreateNewSkil(data);

                // 스킬 인벤토리에서 제거
                _curSummonInfo._skillInventory.ClearSkillInfo(_curSelecetIndex);
            }
            else
                Debug.Log("스킬 없는데 해제한다고 함");
        }
    }

    public void ClickSkillUI(int index)
    {
        if (_curSelecetIndex == index)
            return;

        if (_curSelecetIndex >= 0)
        {
            _skillUIs[_curSelecetIndex].ChangeFrameColor();
        }

        _skillUIs[index].ChangeFrameColor(true);

        _curSelecetIndex = index;
    }
    #endregion
}
