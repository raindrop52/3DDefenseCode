using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UI_State
{
    None = -1,
    Lobby = 0,
    Difficulty,
    InGame,
    GameOver,
}

public class UIManager : MonoBehaviour
{
    public static UIManager I;
    [SerializeField] List<Canvas> _uis;
    public UI_State _curState = UI_State.None;
    public InGameUI _uIIngame;

    void Awake()
    {
        I = this;
    }

    public void Init()
    {
        foreach(Canvas c in _uis)
        {
            c.gameObject.SetActive(false);
            InGameUI ui = c.GetComponent<InGameUI>();
            if (ui != null)
            {
                _uIIngame = ui;
            }
        }
    }

    private void Update()
    {
        CheckUIState();
    }

    void CheckUIState()
    {
        int index = (int)_curState;
        if (index < 0)
        {
            return;
        }

        if (_uis[index].gameObject.activeSelf == false)
        {
            ShowUIs(index);

            switch (_curState)
            {
                case UI_State.Lobby:
                    {
                        RoundManager.I.ClearStage();
                        break;
                    }

                case UI_State.Difficulty:
                    {
                        break;
                    }
                case UI_State.InGame:
                    {

                        break;
                    }
                case UI_State.GameOver:
                    {
                        GameOverUI ui = _uis[index].GetComponent<GameOverUI>();
                        ui.OnShow();
                        break;
                    }
            }
        }
        
    }

    void ShowUIs(int index)
    {
        for (int i = 0; i < _uis.Count; i++)
        {
            if(i == index)
            {
                _uis[i].gameObject.SetActive(true);
            }
            else
            {
                _uis[i].gameObject.SetActive(false);
            }
        }
    }
}
