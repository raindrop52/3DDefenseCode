using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fusion_Scroll_Inven : MonoBehaviour
{
    public Skill_Data _data;
    [SerializeField] Image _icon;
    [SerializeField] Text _name;
    [SerializeField] Text _count;

    public void Init(Skill_Data data, int count)
    {
        _data = data;

        _icon.sprite = data.skill_Info.sprite;
        _name.text = data.skill_Info.name;
        ChangeCount(count);
    }

    public void ChangeCount(int count)
    {
        _count.text = string.Format("{0}", count + 1);
    }
    
    public void SelectThis()
    {
        FusionUI fUI = UIManager.I._uIIngame._fusionUI;

        if (fUI != null)
        {
            if(fUI._checkMain == false)
            {
                fUI._mainImg.sprite = _data.skill_Info.sprite;
                fUI._selectSkills[0] = this;
                fUI._checkMain = true;
            }
            else
            {
                fUI._onShow = false;
                fUI._subImg.sprite = _data.skill_Info.sprite;
                fUI._selectSkills[1] = this;
                fUI._checkSub = true;
            }
        }
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}
