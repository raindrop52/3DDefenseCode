using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    Canvas _canvas;
    Camera _hpCamera;
    RectTransform _rectCanvas;
    RectTransform _rectHp;

    Image _hpGague;

    public Transform _target;
    public Vector3 _offset;

    [SerializeField] List<Image> _debufIcons;

    public void Init()
    {
        _canvas = GetComponentInParent<Canvas>();
        _hpCamera = _canvas.worldCamera;
        _rectCanvas = _canvas.GetComponent<RectTransform>();
        _rectHp = GetComponent<RectTransform>();
        
        _hpGague = transform.Find("HPGague").GetComponent<Image>();

        ClearIcon();
    }

    void ClearIcon()
    {
        foreach (Image img in _debufIcons)
        {
            img.gameObject.SetActive(false);
        }
    }

    public void SetGague(float percent)
    {
        _hpGague.fillAmount = percent;
    }

    public void ShowDebuf(Image image)
    {
        foreach (Image img in _debufIcons)
        {
            if(img.gameObject.activeSelf == false)
            {
                img.sprite = image.sprite;
                img.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void Disappear()
    {
        SetGague(1f);
        ObjectPoolManager.ReturnObject(this);

        ClearIcon();
        //Destroy(gameObject);
    }

    private void LateUpdate()
    {
        if(_rectCanvas != null && _hpCamera != null)
        {
            // 몬스터의 월드 3d좌표를 스크린좌표로 변환
            Vector3 screenPos = Camera.main.WorldToScreenPoint(_target.position + _offset);

            if (screenPos.z < 0.0f)
            {
                screenPos *= -1.0f;
            }

            Vector2 localPos = Vector2.zero;

            // 스크린 좌표를 다시 체력바 UI 캔버스 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectCanvas, screenPos, _hpCamera, out localPos);

            // 체력바 위치조정
            _rectHp.localPosition = localPos;
        }        
    }
}
