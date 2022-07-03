using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClick_Inventory : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] Text _name;
    [SerializeField] Text _desc;
    [SerializeField] Button _equipBtn;
    RectTransform rectTs;
    [SerializeField] float offsetX;
    [SerializeField] float offsetY;

    static float LIMIT_X_MAX = 198.0f;


    private void Awake()
    {
        rectTs = GetComponent<RectTransform>();
    }

    public void SetUI(Sprite sprite, string name, string desc, bool btnShow)
    {
        _icon.sprite = sprite;
        _name.text = name;
        _desc.text = desc;

        if(btnShow == true)
        {
            if (UIManager.I._uIIngame._towerUI.IsViewInventory == true)
                _equipBtn.gameObject.SetActive(true);
            else
                _equipBtn.gameObject.SetActive(false);

            return;
        }

        _equipBtn.gameObject.SetActive(btnShow);
    }

    public void SetPos(Vector2 pos)
    {
        Vector2 anchorPos = pos + new Vector2(offsetX, offsetY);
        if(anchorPos.x > LIMIT_X_MAX)
        {
            anchorPos.x = LIMIT_X_MAX;
        }

        rectTs.anchoredPosition = anchorPos;

        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
