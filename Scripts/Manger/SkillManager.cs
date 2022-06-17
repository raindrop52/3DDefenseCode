using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill_Data
{
    public GameObject skill_Obj;
    public Skill_Info skill_Info;
    public Skill passiveRinker;        // 패시브 스킬 생성 시 연결 ( 장착 해제 용 )
}

public class SkillManager : MonoBehaviour
{
    public static SkillManager I;

    [SerializeField] List<Skill_Data> _skillsRankS;
    [SerializeField] List<Skill_Data> _skillsRankA;
    [SerializeField] List<Skill_Data> _skillsRankB;
    [SerializeField] List<Skill_Data> _skillsRankC;
    [SerializeField] List<Skill_Data> _skillsRankM;

    public SkillRecipe _skillRecipe;

    private void Awake()
    {
        I = this;
    }

    public void DivisionSkill(Skill_Info info)
    {
        Skill_Data data = new Skill_Data();
        // 데이터 가져오면서 수치 배분
        if(info != null)
        {
            GameObject prefab = Resources.Load(info.prefabObjName) as GameObject;
            data.skill_Obj = prefab;
            data.skill_Info = info;

            string rankLow = info.rank.ToLower();

            switch (rankLow)
            {
                case "s":
                    {
                        _skillsRankS.Add(data);
                        break;
                    }

                case "a":
                    {
                        _skillsRankA.Add(data);
                        break;
                    }

                case "b":
                    {
                        _skillsRankB.Add(data);
                        break;
                    }

                case "c":
                    {
                        _skillsRankC.Add(data);
                        break;
                    }

                case "m":
                    {
                        _skillsRankM.Add(data);
                        break;
                    }
            }
        }
    }

    int RandomNo(int max)
    {
        int result = Random.Range(0, max);
        return result;
    }

    public Skill_Data GetMaterial()
    {
        Skill_Data data = null;

        int index = Random.Range(0, _skillsRankM.Count);
        data = _skillsRankM[index];

        return data;
    }

    public Skill_Data GetSkill(string rank, string name)
    {
        string rankLow = rank.ToLower();
        Skill_Data data = null;

        switch (rankLow)
        {
            case "s":
                {
                    foreach (Skill_Data item in _skillsRankS)
                    {
                        if(item.skill_Info.name.Equals(name))
                        {
                            data = item;
                            break;
                        }
                    }
                    break;
                }

            case "a":
                {
                    foreach (Skill_Data item in _skillsRankA)
                    {
                        if (item.skill_Info.name.Equals(name))
                        {
                            data = item;
                            break;
                        }
                    }
                    break;
                }

            case "b":
                {
                    foreach (Skill_Data item in _skillsRankB)
                    {
                        if (item.skill_Info.name.Equals(name))
                        {
                            data = item;
                            break;
                        }
                    }
                    break;
                }

            case "c":
                {
                    foreach (Skill_Data item in _skillsRankC)
                    {
                        if (item.skill_Info.name.Equals(name))
                        {
                            data = item;
                            break;
                        }
                    }
                    break;
                }

            case "m":
                {
                    foreach (Skill_Data item in _skillsRankM)
                    {
                        if (item.skill_Info.name.Equals(name))
                        {
                            data = item;
                            break;
                        }
                    }
                    break;
                }
        }

        return data;
    }
}
