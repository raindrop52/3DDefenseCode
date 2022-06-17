using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeItemUI : MonoBehaviour
{
    Image _imgBase;
    Image _imgSub;
    Image _imgResult;

    Text _txtBase;
    Text _txtSub;
    Text _txtResult;

    public void Init()
    {
        _imgBase = transform.Find("Icon_Base").GetComponent<Image>();
        _imgSub = transform.Find("Icon_Sub").GetComponent<Image>();
        _imgResult = transform.Find("Icon_Result").GetComponent<Image>();

        _txtBase = transform.Find("Text_Base").GetComponent<Text>();
        _txtSub = transform.Find("Text_Sub").GetComponent<Text>();
        _txtResult = transform.Find("Text_Result").GetComponent<Text>();
    }
    
    public void SetRecipe(Skill_Info infoBase, Skill_Info infoSub, Skill_Info infoResult)
    {
        _imgBase.sprite = infoBase.sprite;
        _txtBase.text = infoBase.name;

        _imgSub.sprite = infoSub.sprite;
        _txtSub.text = infoSub.name;

        _imgResult.sprite = infoResult.sprite;
        _txtResult.text = infoResult.name;
    }
}
