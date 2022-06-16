using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //TODO : 마우스 클릭 시 버튼 글씨 변경 ( 성장 시 필요한 골드 용 )
    bool _change = false;
    public string _changeText;

    Text _text;
    Button _btn;

    [SerializeField] int _defaultSize = 72;
    [SerializeField] int _upSize = 84;
    [SerializeField] Round_Difficulty _difficulty;

    private void Start()
    {
        _text = GetComponentInChildren<Text>();

        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(delegate ()
        {
            RoundManager.I.SelectDifficulty(_difficulty);
            UIManager.I._curState = UI_State.InGame;
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_text != null)
        {
            if(_change == true)
            {

            }
            else
                _text.fontSize = _upSize;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_text != null)
        {
            if (_change == true)
            {

            }
            else
                _text.fontSize = _defaultSize;
        }
    }
}
