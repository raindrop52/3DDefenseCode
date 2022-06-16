using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Data", menuName = "GameData/SkillData", order = 1)]
public class SkillData : GameData
{
    public List<Skill_Info> _dataList;

    public Skill_Info GetData(string name)
    {
        Skill_Info info = null;

        foreach (Skill_Info i in _dataList)
        {
            if (i.name == name)
            {
                info = i;
                break;
            }
        }

        return info;
    }

    public void SetSkillManager()
    {
        if(SkillManager.I == null)
            return;

        foreach(Skill_Info info in _dataList)
        {
            SkillManager.I.DivisionSkill(info);
        }
    }

    // 유니티 에디터 함수는 #if UNITY_EDITOR #endif 로 감싸줘야 함
#if UNITY_EDITOR
    public override void Parse(System.Object[] objList)
    {
        _dataList = new List<Skill_Info>();

        foreach (System.Object csvObj in objList)
        {
            Skill_Info info = new Skill_Info();

            ParseObject(info, csvObj);
            if(info != null)
            {
                // 이미지 아이콘 적용
                if (info.sprite == null)
                {
                    string path = string.Format("Assets/Icon/{0}.PNG", info.iconPath);

                    Sprite sprites = AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
                    if (sprites != null)
                    {
                        info.sprite = sprites;
                    }
                }
            }

            _dataList.Add(info);
        }
    }
#endif
}