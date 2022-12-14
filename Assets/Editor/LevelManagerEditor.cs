using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

[CustomEditor(typeof(LevelManagerBase), true)]
public class LevelManagerEditor : Editor
{
    SerializedProperty levelOrder;

    ReorderableList levelOrderReordarableList;

    private void OnEnable()
    {
        levelOrder = serializedObject.FindProperty("levelOrder");
        levelOrderReordarableList = new ReorderableList(serializedObject, levelOrder, true, true, true, true);

        levelOrderReordarableList.drawElementCallback = DrawListItems;
        levelOrderReordarableList.drawHeaderCallback = DrawHeader;
    }

    private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty listItem = levelOrderReordarableList.serializedProperty.GetArrayElementAtIndex(index);
        
        EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "LevelID");
        EditorGUI.PropertyField(new Rect(rect.x + 60, rect.y, 50, EditorGUIUtility.singleLineHeight), listItem.FindPropertyRelative("LevelID"), GUIContent.none);

        EditorGUI.LabelField(new Rect(rect.x + 120, rect.y, 100, EditorGUIUtility.singleLineHeight), "LevelName");
        EditorGUI.PropertyField(new Rect(rect.x + 200, rect.y, 150, EditorGUIUtility.singleLineHeight), listItem.FindPropertyRelative("LevelName"), GUIContent.none);

        //EditorGUI.LabelField(new Rect(rect.x + 5, rect.y, 100, EditorGUIUtility.singleLineHeight), "LevelID");
        //EditorGUI.PropertyField(new Rect(rect.x + 65, rect.y, 50, EditorGUIUtility.singleLineHeight), listItem, GUIContent.none);
    }

    private void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Level Order");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        levelOrderReordarableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}