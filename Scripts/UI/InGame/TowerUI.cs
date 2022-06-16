using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour
{
    [SerializeField]
    Summon_Info _curSummonInfo;           // ���� ���õ� Ÿ�� ����
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
            // ��½�� ������ ���� ������ �̵���Ŵ
            gameObject.transform.position = new Vector3(-2500f, 0f, 0f);
            _isViewInventory = false;

            DeSelectTower();

            if(MouseClick.I != null)
            {
                MouseClick.I.OffClickInfo();
            }
        }

        
    }

    // Ÿ�� Ŭ�� �� �������� ���� ����
    public void ShowTowerInfo(bool show)
    {
        if(show == true)
        {
            // Ȱ��ȭ �� ���õ� Ÿ�� ���� ǥ��
            if(_curSummonInfo != null)
            {
                ShowLevelUI(_curSummonInfo._isSummon);

                SetUIData();
            }
        }

        // UI ǥ�� �� ��ǥ��
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

    // Ÿ�� ����
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
            // ���� ��Ÿ� ǥ�� ����
            ShowRange(false);
        }

        _curSummonInfo = info;

        ShowTowerInfo(true);

        ShowRange(true);

        // Ÿ�� ���� �� �ٽ� Ÿ�� �����ϴ� ��츦 ���� �ʱ�ȭ
        _curSelecetIndex = -1;
    }

    // Ÿ�� ���� ����
    public void DeSelectTower()
    {
        ShowRange(false);

        ShowTowerInfo(false);

        _curSummonInfo = null;

        _curSelecetIndex = -1;
    }

    // ���� �Ÿ� ǥ��
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
        // �̸� ǥ��
        _textName.text = _curSummonInfo._name;
        // ����, ���� ��ư Ȱ��ȭ
        ShowButtonEquipClear(_curSummonInfo._isSummon);
        // �������� ��ų ǥ��
        ShowOriginSkill();
        // ���� ��ų ǥ��
        ShowRankSkills();
    }

    void ShowOriginSkill()
    {
        if (_curSummonInfo._isSummon == true)
        {
            // ��ȯ�� �� ���¿����� ���� ǥ��
            _textLevel.text = string.Format("LV.{0}", _curSummonInfo.Owner._level);
            // �������� ��ų ���� ����
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
        // ���� ��ų ���� ����
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

    #region ��ư ��� ������
    // ��ȯ ��ư
    public void SummonCreature()
    {
        if (RoundManager.I.UseGold(50))      //TODO : ��� �Ҹ�
        {
            // Prefab �κ�
            RoundManager.I.SpawnUnit(_curSummonInfo);

            // UI �κ�
            _curSummonInfo._isSummon = true;

            // �̸� ����
            _curSummonInfo._name = _curSummonInfo.Owner._name;
            // TODO : ���� ��ų ���� ( �����Ϳ��� ���� ��ų ã�ƿ� )
            Skill_Data skill = new Skill_Data();
            Original origin = _curSummonInfo.Owner.transform.GetComponent<Original>();
            if (origin != null)
            {
                // TODO : ���� ��ų ���� ( �����Ϳ��� ���� ��ų ã�ƿ� )
            }
            else
            {
                // �ӽ� ��ų
                skill.skill_Obj = null;
                skill.skill_Info = new Skill_Info("original", 0, "�ӽ�", "�۷� �ۼ��� ��ų �������� ����.", 0.0f, 0.1f, "5", "");
            }

            _curSummonInfo._skillInventory.SetOriginSkill(skill);
            // ��ȯ ��ư ���� �� ���� ǥ��
            ShowLevelUI(true);
            SetUIData();

            // �����Ÿ� ǥ��
            ShowRange(true);
        }
    }

    // ���� ��ư
    public void LevelUpCreature()
    {
        // ��ȯ �� ������ ���� ��������
        if (_curSummonInfo != null && _curSummonInfo._isSummon == true)
        {
            if (RoundManager.I.UseGold(0))  // TODO : ��� �Ҹ�
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

                // ��ȯ������ ��ų ���̱�
                // true �� ���� ����
                if (_curSummonInfo._skillInventory.SetSkill(data) == true)
                {
                    // �κ��丮UI�� �ִ� �ش� ��ų�� ����.
                    invenUI.MinusCount(index);
                    // ��ų �� ���� â�� ���ش�.
                    UIManager.I._uIIngame._inventoryClickUI.Hide();
                    // ��ų ������ �����Ѵ�.
                    ShowRankSkills();
                }
            }
        }
    }

    // ��ų ���� ��ư ( �κ��丮 UI�� ���� )
    public void ShowInventoryUI()
    {
        InventoryUI invenUI = UIManager.I._uIIngame._inventoryUI;
        if(invenUI != null)
        {
            // �κ��丮 UI�� �����ش�. (��Ȱ�� ���� ��)
            if(invenUI.gameObject.activeSelf == false)
            {
                // �κ��丮 ������ ����
                UIManager.I._uIIngame.ChangeTab(0);
            }

            // TowerUI�� ġ���.
            gameObject.SetActive(false);
            _isViewInventory = true;

            if (_btnEquipCancle.gameObject.activeSelf == false)
                _btnEquipCancle.gameObject.SetActive(true);

            _btnEquip.gameObject.SetActive(false);
        }
    }

    public void ShowThis()
    {
        // ���� ��ư�� ���� â�� ������ ���
        if (gameObject.activeSelf == false)
        {
            // TowerUI�� �ҷ��´�.
            gameObject.SetActive(true);
            _isViewInventory = false;

            if (_btnEquip.gameObject.activeSelf == false)
                _btnEquip.gameObject.SetActive(true);

            _btnEquipCancle.gameObject.SetActive(false);
        }
    }

    // ��ų ����
    public void ClearSkill()
    {
        // ���õ� ��ų�� �ִ��� üũ
        if(_curSelecetIndex >= 0)
        {
            Debug.Log("���� ��ư Ŭ��");

            Skill_Data data = _curSummonInfo._skillInventory.GetSkillData(_curSelecetIndex);
            if (data != null)
            {
                // UI���� ����
                _skillUIs[_curSelecetIndex].DefaultSetting();

                // �κ��丮UI�� �߰�
                UIManager.I._uIIngame._inventoryUI.CreateNewSkil(data);

                // ��ų �κ��丮���� ����
                _curSummonInfo._skillInventory.ClearSkillInfo(_curSelecetIndex);
            }
            else
                Debug.Log("��ų ���µ� �����Ѵٰ� ��");
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
