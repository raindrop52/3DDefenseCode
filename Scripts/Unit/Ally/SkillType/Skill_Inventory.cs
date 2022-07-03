using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Inventory : MonoBehaviour
{
    static int HAS_SKILL_COUNT = 4;

    [SerializeField] Skill_Data _originSkill;
    [SerializeField] Skill_Data[] _skills;
    public bool[] _skillisInstantiate;

    private void Awake()
    {
        _skills = new Skill_Data[HAS_SKILL_COUNT];
        _skillisInstantiate = new bool[HAS_SKILL_COUNT];
    }

    int CheckRank(string rank)
    {
        int result = -1;
        switch (rank)
        {
            case "s":
                {
                    result = 0;
                    break;
                }

            case "a":
                {
                    result = 1;
                    break;
                }

            case "b":
                {
                    result = 2;
                    break;
                }

            case "c":
                {
                    result = 3;
                    break;
                }

            case "m":
                {
                    result = -1;
                    break;
                }
        }

        return result;
    }

    // ��ų �߰�
    public bool SetSkill(Skill_Data data)
    {
        string rank = data.skill_Info.rank;
        if(rank.Equals("m"))
        {
            NoticeUI.I.OnShow("���� ������ �� �����ϴ�.");
            return false;
        }

        if (data.skill_Obj != null)
        {
            Skill skill = data.skill_Obj.GetComponentInChildren<Skill>();
            skill._owner = gameObject.GetComponent<Ally>();

            int rankIndex = CheckRank(rank);
            // �迭 ������ �� ���� ( m��ũ ������ ����ó�� ������ �迭 �����°� �ѹ� �� ���� ����)
            if(rankIndex >= 0)
            {
                if(_skills[rankIndex] == null)
                {
                    _skills[rankIndex] = data;
                    return true;
                }
                else
                    NoticeUI.I.OnShow(rank + "�� �̹� ������ ��ų�� �����մϴ�.");
            }
        }

        return false;
    }

    public void SetOriginSkill(Skill_Data data)
    {
        _originSkill = data;
    }

    public Skill_Info GetOriginSkillInfo()
    {
        if(_originSkill != null)
            return _originSkill.skill_Info;

        return null;
    }

    public Skill_Info GetSkillInfo(int index)
    {
        if(index >= 0)
        {
            if(_skills[index] != null)
            {
                return _skills[index].skill_Info;
            }
        }

        return null;
    }

    public Skill_Data GetSkillData(int index)
    {
        if (index >= 0)
        {
            if (_skills[index] != null)
            {
                return _skills[index];
            }
        }

        return null;
    }

    public void ClearSkillInfo(int index)
    {
        if(index >= 0)
        {
            if(_skills[index].passiveRinker != null)
            {
                _skillisInstantiate[index] = false;
                _skills[index].passiveRinker.Disappear();
            }

            System.Array.Clear(_skills, index, 1);
        }
    }

    public void OperateSkill(Transform target)
    {
        for (int i = 0; i < HAS_SKILL_COUNT; i++)
        {
            if (_skills[i] == null)
                continue;

            if (_skills[i].skill_Obj != null && _skills[i].skill_Info != null)
            {
                int index = Random.Range(0, 100);

                // ��� �ߵ��� ��ų
                if (_skills[i].skill_Info.percent == 100)
                {
                    // �̹� ���� �� ��� ��ŵ
                    if (_skillisInstantiate[i] == true)
                    {
                        continue;
                    }
                    else if (_skillisInstantiate[i] == false)
                    {
                        _skillisInstantiate[i] = true;
                    }

                    GameObject fxGo = Instantiate(_skills[i].skill_Obj);
                    fxGo.transform.SetParent(GameManager.I._projCollector);
                    Skill skill = fxGo.GetComponent<Skill>();
                    skill.SetTransform(transform);

                    skill.SetParameter(_skills[i].skill_Info.value, _skills[i].skill_Info.upValue);

                    _skills[i].passiveRinker = skill;
                }
                else
                {
                    if (index < _skills[i].skill_Info.percent && target != null)
                    {
                        GameObject fxGo = Instantiate(_skills[i].skill_Obj);
                        fxGo.transform.SetParent(GameManager.I._projCollector);
                        Skill skill = fxGo.GetComponent<Skill>();

                        if(skill == null)
                        {
                            skill = fxGo.GetComponentInChildren<Skill>();
                        }

                        skill.SetParameter(_skills[i].skill_Info.value, _skills[i].skill_Info.upValue);

                        // �ߵ��� ���� ��ų
                        if (_skills[i].skill_Info.type == 1)
                            skill.SetTransform(transform);
                        // ����, ������ ����� ��ų
                        else
                            skill.SetTransform(target);
                    }
                }
            }
        }
    }
}
