using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("UI ����")]
    public TowerUI _towerUI;
    public InventoryUI _inventoryUI;
    public FusionUI _fusionUI;
    public RecipeUI _recipeUI;
    public UIClick_Inventory _inventoryClickUI;

    #region ��� UI �κ�
    [Header("��� UI �κ�")]
    [SerializeField] Circle_Rotate _circleRound;
    [SerializeField] Circle_ChagneColor _circleCount;
    [SerializeField] List<Color> _circleColor;

    [SerializeField] Text _textRound;
    [SerializeField] Text _textCount;
    [SerializeField] Text _textGold;
    #endregion

    #region �Ϻ� UI �κ�
    [Header("�Ϻ� UI �κ�")]
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

        // 80% �ʰ� �� ��� ǥ��
        if (percent > 80)
        {
            _circleCount.ChangeColor(_circleColor[2]);
        }
        // 50% �ʰ� �� ���� ǥ��
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

        // ������ư�� ���� ������ ���
        _towerUI.HideTowerInfo();
    }
}
