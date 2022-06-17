using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : BottomUIBase
{
    public List<int> _countList;                        // ���� ��ų�� ���� �����
    public List<Skill_Data> _skillList;                 // ���� ��ų�� ���̴� ���
    public List<PickSkillUI> _pickSkillList;            // ���� ��ų�� ������ ������

    [SerializeField] Transform _skillPrefabParent;
    [SerializeField] GameObject _pickSkillPrefab;
    
    public GameObject _clickObj = null;

    protected override void OnEnable()
    {
        base.OnEnable();

        ClearClickObj();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Debug.Log("������");

        ClearClickObj();
    }

    public void ClearClickObj()
    {
        if (_clickObj != null)
        {
            int index = FindClickIndex();
            _pickSkillList[index].Click();
        }
    }

    public bool CreateNewSkil(Skill_Data data)
    {
        bool isCreate = false;
        // �ߺ� ���� üũ ( �ߺ� �� ������ ���� )
        if(_skillList.Contains(data) == true)
        {
            int index = _skillList.IndexOf(data);
            _countList[index]++;
            _pickSkillList[index].ChangeCount(_countList[index]);

            isCreate = false;
        }
        else
        {
            _skillList.Add(data);
            _countList.Add(0);

            GameObject prefab = Instantiate(_pickSkillPrefab);
            prefab.transform.SetParent(_skillPrefabParent);
            prefab.transform.localScale = new Vector3(1f, 1f, 1f);

            PickSkillUI pickSkillUI = prefab.GetComponent<PickSkillUI>();
            if (pickSkillUI != null)
            {
                _pickSkillList.Add(pickSkillUI);
                pickSkillUI.Init(data, this);
            }

            isCreate = true;
        }

        return isCreate;
    }

    #region ��ư ��ɺ�
    // ��ų �̱� ��ư
    public void SummonMaterial()
    {
        if (RoundManager.I.UseGold(10))
        {
            // ��ų �̱�
            if (SkillManager.I != null)
            {
                Skill_Data data = SkillManager.I.GetMaterial();
                if (data != null)
                {
                    int findNo = _skillList.IndexOf(data);

                    if (findNo >= 0)
                    {
                        _countList[findNo]++;
                        _pickSkillList[findNo].ChangeCount(_countList[findNo]);
                    }
                    else
                    {
                        CreateNewSkil(data);
                    }
                }
            }
        }
    }

    public int FindClickIndex()
    {
        if(_clickObj != null)
        {
            PickSkillUI ui = _clickObj.GetComponent<PickSkillUI>();
            int index = _pickSkillList.IndexOf(ui);

            return index;
        }

        return -1;
    }

    // ��ų �Ǹ� ��ư
    public void SellSkill()
    {
        if(_skillList.Count > 0)
        {
            if (_clickObj == null)
                return;

            // �Ǹ� �ݾ� üũ
            int index = FindClickIndex();
            Skill_Data data = _skillList[index];
            int getGold = HowMuchSkill(data.skill_Info);
            RoundManager.I.GetGold(getGold);
            // TODO : ���� �Ǹ� ����
            MinusCount(index);
        }
    }

    // ��� ������ true, ���� �� false
    public bool MinusCount(int index)
    {
        int count = _countList[index] - 1;
        if (count < 0)
        {
            // ��� ���� -> ����Ʈ���� ����
            _skillList.RemoveAt(index);
            _countList.RemoveAt(index);
            _pickSkillList[index].Disappear();
            _pickSkillList.RemoveAt(index);

            _clickObj = null;
            return true;
        }
        else
        {
            _pickSkillList[index].ChangeCount(count);
            _countList[index]--;

            return false;
        }
    }

    int HowMuchSkill(Skill_Info info)
    {
        int result = 0;

        switch(info.rank)
        {
            case "s":
                {
                    result = 80;
                    break;
                }

            case "a":
                {
                    result = 40;
                    break;
                }

            case "b":
                {
                    result = 20;
                    break;
                }

            case "c":
                {
                    result = 10;
                    break;
                }

            case "m":
                {
                    result = 5;
                    break;
                }
        }

        return result;
    }
    #endregion
}
