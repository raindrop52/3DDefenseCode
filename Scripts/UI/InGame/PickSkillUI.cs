using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PickSkillUI : MonoBehaviour
{
    Image _img;
    [SerializeField] Image _frame;
    [SerializeField] Text _txtCount;
    InventoryUI _owner = null;
    Skill_Data _data;

    Color _selectColor = Color.red;
    Color _deSelectColor = Color.white;

    [SerializeField] bool _isClick = false;

    public void Init(Skill_Data data, InventoryUI owner)
    {
        if(_img == null)
            _img = GetComponent<Image>();

        _owner = owner;
        _data = data;
        _img.sprite = data.skill_Info.sprite;
        _frame.color = _deSelectColor;
        ChangeCount(0);
    }

    public void ChangeCount(int count)
    {
        _txtCount.text = string.Format("{0}", count + 1);
    }

    public void ChangeColor(bool select)
    {
        _isClick = select;

        if (select == true)
        {
            _frame.color = _selectColor;
        }
        else
        {
            _frame.color = _deSelectColor;
        }
    }

    public void Click()
    {
        if(_owner != null)
        {
            if (_isClick == false)
            {
                if (_owner._clickObj != null)
                {
                    PickSkillUI pickSkillUI = _owner._clickObj.GetComponent<PickSkillUI>();
                    pickSkillUI.ChangeColor(false);
                }
            }
            else if (_isClick == true)
            {
                ChangeColor(false);
                _owner._clickObj = null;
                if (UIManager.I._uIIngame._inventoryClickUI.gameObject.activeSelf == true)
                    UIManager.I._uIIngame._inventoryClickUI.gameObject.SetActive(false);
                return;
            }

            ChangeColor(true);

            GameObject clickObject = EventSystem.current.currentSelectedGameObject;

            _owner._clickObj = clickObject;

            int index = _owner.FindClickIndex();
            if (index >= 0)
            {
                Skill_Info info = _owner._skillList[index].skill_Info;
                if(info != null)
                {
                    bool isShow = false;
                    if ( info.rank.Equals("m") == false)
                        isShow = true;
                    
                    UIManager.I._uIIngame._inventoryClickUI.SetUI(info.sprite, info.name, info.desc, isShow);
                    RectTransform rect = clickObject.GetComponent<RectTransform>();
                    UIManager.I._uIIngame._inventoryClickUI.SetPos(rect.localPosition);
                }
            }
        }
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}
