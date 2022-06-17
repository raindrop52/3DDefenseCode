using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeUI : BottomUIBase
{
    [SerializeField] Transform _prefabParent;
    [SerializeField] GameObject _prefabGo;
    List<GameObject> _rankCRecipe;
    List<GameObject> _rankBRecipe;
    List<GameObject> _rankARecipe;
    List<GameObject> _rankSRecipe;

    public void Init()
    {
        // 리스트 할당
        _rankCRecipe = new List<GameObject>();
        _rankBRecipe = new List<GameObject>();
        _rankARecipe = new List<GameObject>();
        _rankSRecipe = new List<GameObject>();

        // 레시피 생성
        SkillRecipe skillRecipe = SkillManager.I._skillRecipe;
        if(skillRecipe != null)
        {
            int max = skillRecipe.GetListCount();

            for (int i = 0; i < max; i++)
            {
                Skill_Recipe recipe = skillRecipe.GetRecipe(i);
                if(recipe != null)
                {
                    Skill_Data dataMain = SkillManager.I.GetSkill(recipe._rankBase, recipe._base);
                    Skill_Data dataSub = SkillManager.I.GetSkill(recipe._rankSub, recipe._sub);
                    Skill_Data dataResult = SkillManager.I.GetSkill(recipe._rankResult, recipe._result);
                    if (dataMain != null && dataSub != null && dataResult != null)
                    {
                        GameObject recipeGo = Instantiate(_prefabGo);
                        recipeGo.transform.SetParent(_prefabParent);
                        recipeGo.transform.localScale = new Vector3(1f, 1f, 1f);

                        RecipeItemUI itemUI = recipeGo.GetComponent<RecipeItemUI>();
                        if (itemUI != null)
                        {
                            itemUI.Init();
                            itemUI.SetRecipe(dataMain.skill_Info, dataSub.skill_Info, dataResult.skill_Info);
                            PutRankRecipe(dataResult.skill_Info.rank, recipeGo);
                        }
                    }
                }
            }
        }
    }

    void PutRankRecipe(string rank, GameObject go)
    {
        switch(rank)
        {
            case "s":
                {
                    _rankSRecipe.Add(go);
                    break;
                }

            case "a":
                {
                    _rankARecipe.Add(go);
                    break;
                }

            case "b":
                {
                    _rankBRecipe.Add(go);
                    break;
                }

            case "c":
                {
                    _rankCRecipe.Add(go);
                    break;
                }
        }
    }
}
