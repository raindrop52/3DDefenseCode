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
            // ������ ���� 3d��ǥ�� ��ũ����ǥ�� ��ȯ
            Vector3 screenPos = Camera.main.WorldToScreenPoint(_target.position + _offset);

            if (screenPos.z < 0.0f)
            {
                screenPos *= -1.0f;
            }

            Vector2 localPos = Vector2.zero;

            // ��ũ�� ��ǥ�� �ٽ� ü�¹� UI ĵ���� ��ǥ�� ��ȯ
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectCanvas, screenPos, _hpCamera, out localPos);

            // ü�¹� ��ġ����
            _rectHp.localPosition = localPos;
        }        
    }
}
