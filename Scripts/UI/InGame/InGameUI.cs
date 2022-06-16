using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("UI 모음")]
    public TowerUI _towerUI;
    public InventoryUI _inventoryUI;
    public FusionUI _fusionUI;
    public RecipeUI _recipeUI;
    public UIClick_Inventory _inventoryClickUI;

    #region 상부 UI 부분
    [Header("상부 UI 부분")]
    [SerializeField] Circle_Rotate _circleRound;
    [SerializeField] Circle_ChagneColor _circleCount;
    [SerializeField] List<Color> _circleColor;

    [SerializeField] Text _textRound;
    [SerializeField] Text _textCount;
    [SerializeField] Text _textGold;
    #endregion

    #region 하부 UI 부분
    [Header("하부 UI 부분")]
    [SerializeField] List<GameObject> _tabList;
    int _curTabNo = 1;
    public int CurTabNo
    {
        get { return _curTabNo; }
    }
    #endregion

    private void Awake()
    {
        if(_inventoryClickUI != null)
            _inventoryClickUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(RoundManager.I.Start == true)
        {
            int count = RoundManager.I.Count;

            _textRound.text = string.Format("{0}", RoundManager.I.Round);
            _textCount.text = string.Format("{0}", count);
            _textGold.text = string.Format("{0}", RoundManager.I.Gold);

            CheckUnitCount(count);
        }
    }

    public void Clear()
    {        
        _textRound.text = "1";
        _textCount.text = "0";
        _textGold.text = "50";

        if (_circleRound != null)
        {
            _circleRound.DefaultRotate();
            _circleRound.Stop = false;
        }

        if (_circleCount != null)
        {
            CheckUnitCount(0);
        }

        foreach(GameObject go in _tabList)
        {
            go.SetActive(false);
        }

        ChangeTab(0);
    }

    void CheckUnitCount(int count)
    {
        int percent = (int)(((float)count / (float)RoundManager.I.FailCount) * 100);

        // 80% 초과 시 경고 표시
        if (percent > 80)
        {
            _circleCount.ChangeColor(_circleColor[2]);
        }
        // 50% 초과 시 주의 표시
        else if (percent > 50)
        {
            _circleCount.ChangeColor(_circleColor[1]);
        }
        else
        {
            _circleCount.ChangeColor(_circleColor[0]);
        }
    }

    public void ChangeTab(int tabNo)
    {
        if (_curTabNo == tabNo)
            return;

        _tabList[_curTabNo].SetActive(false);
        _tabList[tabNo].SetActive(true);
        
        _curTabNo = tabNo;

        // 장착버튼을 누른 상태일 경우
        _towerUI.HideTowerInfo();
    }
}
