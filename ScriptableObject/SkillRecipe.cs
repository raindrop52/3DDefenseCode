using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Data", menuName = "GameData/SkillRecipe", order = 2)]
public class SkillRecipe : GameData
{
    public List<Skill_Recipe> _dataList;

    public int GetListCount()
    {
        return _dataList.Count;
    }

    public Skill_Recipe GetRecipe(int index)
    {
        if(index < _dataList.Count && index >= 0)
            return _dataList[index];

        return null;
    }

    public void GetRecipeResult(Skill_Recipe recipe)
    {
        foreach (Skill_Recipe r in _dataList)
        {
            if (r._base == recipe._base && r._sub == recipe._sub)
            {
                recipe._result = r._result;
                recipe._rankResult = r._rankResult;
                break;
            }
        }
    }

    // 유니티 에디터 함수는 #if UNITY_EDITOR #endif 로 감싸줘야 함
#if UNITY_EDITOR
    public override void Parse(System.Object[] objList)
    {
        _dataList = new List<Skill_Recipe>();

        foreach (System.Object csvObj in objList)
        {
            Skill_Recipe recipe = new Skill_Recipe();

            ParseObject(recipe, csvObj);
            
            _dataList.Add(recipe);
        }
    }
#endif
}