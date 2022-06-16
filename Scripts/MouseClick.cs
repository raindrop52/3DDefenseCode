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
        // 마우스 왼쪽 클릭으로 선택, 해제
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);

            // 광선에 부딪히는 오브젝트가 있을 때 (= 타워가 클릭됐을 때)
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, _layerTower))
            {
                // UI 표시
                if(UIManager.I._uIIngame != null)
                {
                    Summon_Info info = hit.transform.GetComponent<Summon_Info>();
                    // 이전에 클릭한 정보가 있는지 확인 ( 있으면 꺼준다 )
                    OffClickInfo();

                    _curClickInfo = info;
                    _curClickInfo.OnClick();

                    UIManager.I._uIIngame._towerUI.SelectTower(info);

                    UIManager.I._uIIngame._inventoryUI.ClearClickObj();
                }
            }
            // 광선에 부딪힌 오브젝트가 없을 경우 (= 타워가 아닌 곳을 찍은 경우)
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerGround))
            {
                // UI 표시 취소
                if (UIManager.I._uIIngame != null)
                {
                    // 장착 버튼을 누른 상태일 경우 동작
                    UIManager.I._uIIngame._towerUI.HideTowerInfo();

                    UIManager.I._uIIngame._towerUI.DeSelectTower();

                    UIManager.I._uIIngame._inventoryUI.ClearClickObj();

                    OffClickInfo();
                }
            }
        }
    }
}
