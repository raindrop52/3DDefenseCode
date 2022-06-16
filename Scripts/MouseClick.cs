using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    public static MouseClick I;

    [SerializeField] LayerMask _layerTower;
    [SerializeField] LayerMask _layerGround;
    Camera      _mainCam;
    Summon_Info _curClickInfo;
    
    void Awake()
    {
        _mainCam = Camera.main;
        I = this;
    }

    public void OffClickInfo()
    {
        if (_curClickInfo != null)
        {
            _curClickInfo.OffClick();
            _curClickInfo = null;
        }
    }

    void Update()
    {
        // ���콺 ���� Ŭ������ ����, ����
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);

            // ������ �ε����� ������Ʈ�� ���� �� (= Ÿ���� Ŭ������ ��)
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, _layerTower))
            {
                // UI ǥ��
                if(UIManager.I._uIIngame != null)
                {
                    Summon_Info info = hit.transform.GetComponent<Summon_Info>();
                    // ������ Ŭ���� ������ �ִ��� Ȯ�� ( ������ ���ش� )
                    OffClickInfo();

                    _curClickInfo = info;
                    _curClickInfo.OnClick();

                    UIManager.I._uIIngame._towerUI.SelectTower(info);

                    UIManager.I._uIIngame._inventoryUI.ClearClickObj();
                }
            }
            // ������ �ε��� ������Ʈ�� ���� ��� (= Ÿ���� �ƴ� ���� ���� ���)
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerGround))
            {
                // UI ǥ�� ���
                if (UIManager.I._uIIngame != null)
                {
                    // ���� ��ư�� ���� ������ ��� ����
                    UIManager.I._uIIngame._towerUI.HideTowerInfo();

                    UIManager.I._uIIngame._towerUI.DeSelectTower();

                    UIManager.I._uIIngame._inventoryUI.ClearClickObj();

                    OffClickInfo();
                }
            }
        }
    }
}
