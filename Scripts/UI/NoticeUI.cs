using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeUI : MonoBehaviour
{
    public static NoticeUI I;
    [SerializeField] GameObject _hideObj;
    [SerializeField] Text _textComent;
    float _nowTime = 0f;
    float _goalTime = 5f;

    bool _isCoroutineRun = false;

    private void Awake()
    {
        I = this;
        Hide();
    }

    public void OnShow(string coment)
    {
        if(coment.Length > 0)
        {
            _textComent.text = coment;
            _nowTime = 0f;
            _hideObj.SetActive(true);

            // 이미 하나가 돌아가고 있으면 정지하고 시작
            if(_isCoroutineRun == true)
                StopCoroutine(_Hide());
            
            StartCoroutine(_Hide());
        }
    }

    IEnumerator _Hide()
    {
        _isCoroutineRun = true;
        while (_nowTime < _goalTime)
        {
            _nowTime += Time.deltaTime;
            yield return null;
        }

        _isCoroutineRun = false;
        Hide();
    }

    public void OnClickHide()
    {
        Hide();
    }

    void Hide()
    {
        _hideObj.SetActive(false);
    }
}
