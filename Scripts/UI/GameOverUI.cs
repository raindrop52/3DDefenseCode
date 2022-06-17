using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] EasyTween _easyTween;
    
    public void OnShow()
    {
        _easyTween.OpenCloseObjectAnimation();
    }

    public void OnClickToTitle()
    {
        OnShow();
        UIManager.I._curState = UI_State.Lobby;
    }
}
