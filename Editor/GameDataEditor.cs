using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using csv;

public class GameDataEditor : EditorWindow
{
    GameData _gameData;

    [MenuItem("Window/GameDataEditor")]
    static public void OpenGameDataEditor()
    {
        GetWindow<GameDataEditor>(false, "GameData Editor", true);
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("== GAME DATA EDITOR ==");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        _gameData = EditorGUILayout.ObjectField(_gameData, typeof(GameData), false) as GameData;

        if (GUILayout.Button("Import from csv"))
        {
            string gameDataType = _gameData.GetType().ToString();
            string filePath = "GameData/CSV/" + gameDataType + ".csv";
            bool hasFieldName = true;
            char seperator = ',';
            object[] objList = CsvLoader.LoadCsvToObjectList(filePath, hasFieldName, seperator);

            _gameData.Parse(objList);

            EditorUtility.SetDirty(_gameData);
        }
    }
}
