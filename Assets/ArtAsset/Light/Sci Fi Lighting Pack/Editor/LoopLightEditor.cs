using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoopLight))]
[CanEditMultipleObjects]
public class LoopLightEditor : Editor
{
    #region Private Properties
    private LoopLight loopLight;
    private bool showObjectReferences;
    #endregion

    #region Serialized Properties
    SerializedProperty ThisTargetName;
    SerializedProperty LightSource;
    SerializedProperty Base;
    SerializedProperty Shade;
    SerializedProperty Parts;
    SerializedProperty PartIndex;
    SerializedProperty HangDistance;
    SerializedProperty Mat;
    SerializedProperty LightColor;
    SerializedProperty LightIntensity;
    SerializedProperty IsInstanced;
    #endregion

    private void OnEnable()
    {
        loopLight = (LoopLight)target;

        #region Get Serialized Properties
        ThisTargetName = serializedObject.FindProperty("ThisTargetName");
        LightSource = serializedObject.FindProperty("LightSource");
        Base = serializedObject.FindProperty("Base");
        Shade = serializedObject.FindProperty("Shade");
        Parts = serializedObject.FindProperty("Parts");
        PartIndex = serializedObject.FindProperty("PartIndex");
        HangDistance = serializedObject.FindProperty("HangDistance");
        Mat = serializedObject.FindProperty("Mat");
        LightColor = serializedObject.FindProperty("LightColor");
        LightIntensity = serializedObject.FindProperty("LightIntensity");
        IsInstanced = serializedObject.FindProperty("IsInstanced");
        #endregion

        #region First Setup
        if (PrefabUtility.IsAnyPrefabInstanceRoot(loopLight.gameObject))
        {
            loopLight.FirstSetup();
        }
        #endregion

        #region Subsequent Setup
        else if (IsInstanced.boolValue == true)
        {
            // Is this a new instance? (Duplicated)
            if (ThisTargetName.stringValue != loopLight.gameObject.name)
            {
                foreach (LoopLight script in targets)
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
            EditorGUILayout.PropertyField(Base);
            EditorGUILayout.PropertyField(Shade);
            EditorGUILayout.PropertyField(Parts, true);
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
                foreach (LoopLight script in targets)
                {
                    script.SetHangDistance();
                }
            }

            #region Shape Configuration
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.IntSlider(PartIndex, 0, Parts.arraySize - 1, "Shape Variation");
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (LoopLight script in targets)
                {
                    script.ChangePart();
                }
            }
            #endregion

            #region Light Configuration     
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightColor);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (LoopLight script in targets)
                {
                    script.ChangeLightColor();
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightIntensity, new GUIContent("Light Intensity Multiplier"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (LoopLight script in targets)
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
        if (loopLight && loopLight.IsInstanced)
        {
            foreach (LoopLight script in targets)
            {
                if (!script.IsBaked)
                {
                    script.BakeMesh();
                }
            }

            loopLight.UpdateName();
        }
    }
}
