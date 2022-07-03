using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FusionUI : MonoBehaviour
{
    public Image _resultImg;

    public bool _checkMain = false;
    public bool _checkSub = false;
    [SerializeField] Sprite _defaultSprite;
    public Image _mainImg;
    public Image _subImg;

    [SerializeField] Transform _prefabParent;
    [SerializeField] GameObject _prefabInventory;
    [SerializeField] List<Skill_Data> _invenList;
    [SerializeField] List<int> _invenCountList;
    [SerializeField] List<Fusion_Scroll_Inven> _prefabList;

    public Fusion_Scroll_Inven[] _selectSkills;
    Skill_Data _resultData = null;

    public bool _onShow = false;

    private void Awake()
    {
        _invenList = new List<Skill_Data>();
        _invenCountList = new List<int>();
        _selectSkills = new Fusion_Scroll_Inven[2];
    }

    private void OnEnable()
    {
        if (UIManager.I == null)
            return;

        if(UIManager.I._uIIngame != null)
        {
            InventoryUI invenUI = UIManager.I._uIIngame._inventoryUI;
            if(invenUI != null && _invenList != null)
            {
                if(invenUI._skillList.Count > 0)
                {
                    _invenList = invenUI._skillList;
                    _invenCountList = invenUI._countList;

                    for (int i = 0; i < _invenList.Count; i++)
                    {
                        CreatePrefab(i);
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        foreach(Fusion_Scroll_Inven inven in _prefabList)
        {
            inven.Disappear();
        }

        _prefabList.Clear();
        
        ClickMainBtn();
    }

    private void Update()
    {
        // �� �� ���õ� ���
        if(_checkMain && _checkSub)
        {
            if(_selectSkills[0] != null && _selectSkills[1] != null && _onShow == false)
            {
                bool show = FusionSkill(_selectSkills[0]._data.skill_Info.name, _selectSkills[1]._data.skill_Info.name);
                if(show == true)
                {
                    if (_resultImg.gameObject.activeSelf == false)
                        _resultImg.gameObject.SetActive(true);
                }
                else
                {
                    _resultImg.gameObject.SetActive(false);
                }
            }
        }
        // �ϳ��� ���þ� �� ���
        else
        {
            // ���â �����
            _resultData = null;
            _onShow = false;
            if (_resultImg.gameObject.activeSelf == true)
                _resultImg.gameObject.SetActive(false);
        }
    }

    void CreatePrefab(int index)
    {
        GameObject prefab = Instantiate(_prefabInventory);
        prefab.transform.SetParent(_prefabParent);
        prefab.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Fusion_Scroll_Inven fsi = prefab.GetComponent<Fusion_Scroll_Inven>();
        fsi.Init(_invenList[index], _invenCountList[index]);
        _prefabList.Add(fsi);
    }

    bool FusionSkill(string main = "", string sub = "")
    {
        Skill_Recipe curRecipe = new Skill_Recipe(main, sub);
        SkillRecipe recipe = SkillManager.I._skillRecipe;
        if (recipe != null)
        {
            // ��ũ�� ������� ������
            recipe.GetRecipeResult(curRecipe);
            // ������� �´� ��ų ������ ������
            if(curRecipe._rankResult != null && curRecipe._result != null)
            {
                Skill_Data data = SkillManager.I.GetSkill(curRecipe._rankResult, curRecipe._result);
                if (data != null)
                {
                    _resultData = data;
                    _onShow = true;
                    _resultImg.sprite = _resultData.skill_Info.sprite;
                }

                return true;
            }

        }

        return false;
    }

    public void ClickMainBtn()
    {
        if(_checkMain == true)
        {
            _selectSkills[0] = null;

            _mainImg.sprite = _defaultSprite;

            _checkMain = false;

            ClickSubBtn();
        }
    }

    public void ClickSubBtn()
    {
        if (_checkSub == true)
        {
            _selectSkills[1] = null;

            _subImg.sprite = _defaultSprite;

            _checkSub = false;
        }
    }

    public void ClickFusion()
    {
        if(_resultData != null)
        {
            InventoryUI invenUI = UIManager.I._uIIngame._inventoryUI;
            // �κ��丮 UI ����
            if (invenUI != null)
            {
                // ���տ� ���� ��ų�� ����
                // ��ũ�� �� ��������
                {
                    int index1 = _invenList.IndexOf(_selectSkills[0]._data);
                    bool destroy1 = invenUI.MinusCount(index1);
                    if (destroy1 == true)
                    {
                        _prefabList[index1].Disappear();
                        _prefabList.RemoveAt(index1);
                    }
                    else
                        _prefabList[index1].ChangeCount(_invenCountList[index1]);

                    int index2 = _invenList.IndexOf(_selectSkills[1]._data);
                    bool destroy2 = invenUI.MinusCount(index2);
                    if (destroy2 == true)
                    {
                        _prefabList[index2].Disappear();
                        _prefabList.RemoveAt(index2);
                    }
                    else
                        _prefabList[index2].ChangeCount(_invenCountList[index2]);

                    // �ռ� ������ �ʱ�ȭ
                    ClickMainBtn();
                }

                // ���յ� ��ų �߰�
                // �̹� �����ϴ� ��ų�� ��� ���� �߰�
                {
                    // ������� ����Ʈ�� ����
                    bool isCreate = invenUI.CreateNewSkil(_resultData);

                    // ������� ��ũ�� �ٿ� �߰�
                    if(isCreate)
                        CreatePrefab(_prefabList.Count);
                    else
                    {
                        int index = _invenList.IndexOf(_resultData);
                        _prefabList[index].ChangeCount(_invenCountList[index]);
                    }
                }
            }
        }
    }
}
