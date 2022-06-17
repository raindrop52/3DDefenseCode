using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    public Transform _projCollector;

    public Camera _mainCam;

    public Transform _enemyHpBarTrans;

    public SkillData _skillData;

    private void Awake()
    {
        I = this;
    }

    void Start()
    {
        UIManager.I.Init();
        RoundManager.I.Init();

        _skillData = Resources.Load("GameData/SkillData") as SkillData;
        _skillData.SetSkillManager();

        SkillManager.I._skillRecipe = Resources.Load("GameData/SkillRecipe") as SkillRecipe;

        // ��ų ������ UI �ۼ�
        UIManager.I._uIIngame.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        //TODO : �޴�â ����� �����ϵ���
        Application.Quit();
#endif
    }
}
