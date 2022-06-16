using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoUI : MonoBehaviour
{
    [SerializeField] Image _imgIcon;
    [SerializeField] Image _imgFrame;
    [SerializeField] Text _txtName;
    [SerializeField] Text _txtDesc;
    [SerializeField] Sprite _defaultImg;
    Button _btnClick;
    [SerializeField] int _index;
    [SerializeField] Color _colorDefault;
    [SerializeField] Color _colorClick;

    void Awake()
    {
        _imgIcon = transform.Find("Icon").GetComponent<Image>();
        _imgFrame = transform.Find("Frame").GetComponent<Image>();
        _txtName = transform.Find("Name").GetComponent<Text>();
        _txtDesc = transform.Find("Desc").GetComponent<Text>();
        _btnClick = GetComponent<Button>();

        DefaultSetting();
    }

    private void Start()
    {
        _btnClick.onClick.AddListener(delegate ()
        {
            if (UIManager.I != null)
            {
                if (UIManager.I._uIIngame != null)
                {
                    // UI에 번호 매김
                    if(_index >= 0)
                    {
                        UIManager.I._uIIngame._towerUI.ClickSkillUI(_index);
                    }
                }
                else
                {
                    Debug.Log("버튼 기능 추가 실패");
                }
            }
        });
    }

    public void ChangeFrameColor(bool change = false)
    {
        if (change)
            _imgFrame.color = _colorClick;
        else
            _imgFrame.color = _colorDefault;
    }

    public void DefaultSetting()
    {
        _txtName.text = _defaultImg.name + "랭크";
        _txtDesc.text = "스킬 설명란";

        _imgIcon.sprite = _defaultImg;

        ChangeFrameColor();
    }

    public void SetInfo(Skill_Info info)
    {
        _txtName.text = info.name;
        _txtDesc.text = info.desc;

        _imgIcon.sprite = info.sprite;
    }
}
