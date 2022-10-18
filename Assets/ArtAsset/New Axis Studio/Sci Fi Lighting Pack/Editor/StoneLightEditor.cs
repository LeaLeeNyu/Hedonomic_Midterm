using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StoneLight))]
[CanEditMultipleObjects]
public class StoneLightEditor : Editor
{
    #region Private Properties
    private StoneLight stoneLight;
    private bool showObjectReferences;
    #endregion

    #region Serialized Properties
    SerializedProperty ThisTargetName;
    SerializedProperty LightSource;
    SerializedProperty Fixture;
    SerializedProperty Shell;
    SerializedProperty HangDistance;
    SerializedProperty Mat;
    SerializedProperty LightColor;
    SerializedProperty LightIntensity;
    SerializedProperty IsInstanced;
    #endregion

    private void OnEnable()
    {
        stoneLight = (StoneLight)target;

        #region Get Serialized Properties
        ThisTargetName = serializedObject.FindProperty("ThisTargetName");
        LightSource = serializedObject.FindProperty("LightSource");
        Fixture = serializedObject.FindProperty("Fixture");
        Shell = serializedObject.FindProperty("Shell");
        HangDistance = serializedObject.FindProperty("HangDistance");
        Mat = serializedObject.FindProperty("Mat");
        LightColor = serializedObject.FindProperty("LightColor");
        LightIntensity = serializedObject.FindProperty("LightIntensity");
        IsInstanced = serializedObject.FindProperty("IsInstanced");
        #endregion

        #region First Setup
        if (PrefabUtility.IsAnyPrefabInstanceRoot(stoneLight.gameObject))
        {
            stoneLight.FirstSetup();
        }
        #endregion

        #region Subsequent Setup
        else if (IsInstanced.boolValue == true)
        {
            // Is this a new instance? (Duplicated)
            if (ThisTargetName.stringValue != stoneLight.gameObject.name)
            {
                foreach (StoneLight script in targets)
                {
                    script.DuplicateSetup();
                }
            }
        }
        #endregion
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        #region Object References
        showObjectReferences = EditorGUILayout.Foldout(showObjectReferences, "References");
        if (showObjectReferences)
        {
            EditorGUILayout.PropertyField(LightSource);
            EditorGUILayout.PropertyField(Fixture);
            EditorGUILayout.PropertyField(Shell);
            EditorGUILayout.PropertyField(Mat);
        }
        #endregion

        if (IsInstanced.boolValue == true)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Slider(HangDistance, 0, 1, "Hanging Height");
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (StoneLight script in targets)
                {
                    script.SetHangDistance();
                }
            }

            #region Light Configuration     
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightColor);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (StoneLight script in targets)
                {
                    script.ChangeLightColor();
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightIntensity, new GUIContent("Light Intensity Multiplier"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (StoneLight script in targets)
                {
                    script.ChangeLightIntensity();
                }
            }
            #endregion
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void OnDisable()
    {
        if (stoneLight && stoneLight.IsInstanced)
        {
            foreach (StoneLight script in targets)
            {
                if (!script.IsBaked)
                {
                    script.BakeMesh();
                }
            }

            stoneLight.UpdateName();
        }
    }
}
