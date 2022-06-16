using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoingUpText : MonoBehaviour
{
    [SerializeField] float _ableTime = 3f;
    [SerializeField] RectTransform _rect;
    [SerializeField] float _offsetY = 0f;

    Text _text;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void OnUIMove(Vector2 target)
    {
        _rect.anchoredPosition = target;
    }

    public void ShowGold(int gold, bool isGet = true)
    {
        if(isGet == true)
            _text.text = string.Format("+{0}", gold);
        else
            _text.text = string.Format("-{0}", gold);

        Vector2 startPos = Vector2.zero;
        Vector2 endPos = startPos + new Vector2(0f, _offsetY);

        Hashtable hashtable = iTween.Hash("from", 1.0f, "to", 0.0f, "time", _ableTime, "easytype", "linear", "onUpdate", "FadeOutUpdate", "oncomplete", "FadeOutComplete");
        iTween.ValueTo(gameObject, hashtable);
        Hashtable hashtable2 = iTween.Hash("from", startPos, "to", endPos, "time", _ableTime, "easytype", iTween.EaseType.easeInOutBack, "onUpdate", "OnUIMove", "oncomplete", "Disappear");
        iTween.ValueTo(gameObject, hashtable2);
    }

    void FadeOutUpdate(float alpha)
    {
        Color color = _text.color;
        color.a = alpha;

        _text.color = color;
    }

    void FadeOutComplete()
    {
        Color color = _text.color;
        color.a = 0.0f;

        _text.color = color;
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
