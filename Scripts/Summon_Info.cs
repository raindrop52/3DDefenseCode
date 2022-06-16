using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon_Info : MonoBehaviour
{
    public string _name;                             // �̸�
    public int _summonIndex = 0;                     // ��ȯ�Ǵ� ���� ��ȣ
    public bool _isSummon = false;                   // ��ȯ ����

    public Skill_Inventory _skillInventory;          // ���� ��ų
    [SerializeField] GameObject _clickObj;

    Ally _owner = null;
    public Ally Owner
    {
       get { return _owner; }
    }

    void Start()
    {
        DefaultInfo();
        OffClick();
    }

    public void DefaultInfo()
    {
        if (_owner != null)
            ClearSummoner();

        _name = "��ȯ������";
        _isSummon = false;
    }

    public void SetOwner(Ally owner)
    {
        if (_owner == null)
        {
            _owner = owner;
            _skillInventory = owner.GetComponent<Skill_Inventory>();
        }
    }

    public void OnClick()
    {
        _clickObj.SetActive(true);
    }

    public void OffClick()
    {
        _clickObj.SetActive(false);
    }

    public void ClearSummoner()
    {
        Destroy(_owner.gameObject);
        _skillInventory = null;
        _owner = null;
    }
}
