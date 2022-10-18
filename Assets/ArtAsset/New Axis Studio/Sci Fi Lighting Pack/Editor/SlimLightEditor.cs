using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SlimLight))]
[CanEditMultipleObjects]
public class SlimLightEditor : Editor
{
    #region Private Properties
    private SlimLight slimLight;
    private bool showObjectReferences;
    #endregion

    #region Serialized Properties
    SerializedProperty ThisTargetName;
    SerializedProperty LightSource;
    SerializedProperty Shade;
    SerializedProperty Parts;
    SerializedProperty ShadeMat;
    SerializedProperty LightColor;
    SerializedProperty LightIntensity;
    SerializedProperty PartIndex;
    SerializedProperty IsInstanced;
    #endregion

    private void OnEnable()
    {
        slimLight = (SlimLight)target;

        #region Get Serialized Properties
        ThisTargetName = serializedObject.FindProperty("ThisTargetName");
        LightSource = serializedObject.FindProperty("LightSource");
        Shade = serializedObject.FindProperty("Shade");
        Parts = serializedObject.FindProperty("Parts");
        ShadeMat = serializedObject.FindProperty("ShadeMat");
        LightColor = serializedObject.FindProperty("LightColor");
        LightIntensity = serializedObject.FindProperty("LightIntensity");
        PartIndex = serializedObject.FindProperty("PartIndex");
        IsInstanced = serializedObject.FindProperty("IsInstanced");
        #endregion

        #region First Setup
        if (PrefabUtility.IsAnyPrefabInstanceRoot(slimLight.gameObject))
        {
            slimLight.FirstSetup();
        }
        #endregion

        #region Subsequent Setup
        else if (IsInstanced.boolValue == true)
        {
            // Is this a new instance? (Duplicated)
            if (ThisTargetName.stringValue != slimLight.gameObject.name)
            {
                foreach (SlimLight script in targets)
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
            EditorGUILayout.PropertyField(Shade);
            EditorGUILayout.PropertyField(Parts, true);
            EditorGUILayout.PropertyField(ShadeMat);
        }
        #endregion

        if (IsInstanced.boolValue == true)
        {
            #region Shape Configuration
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.IntSlider(PartIndex, 0, Parts.arraySize - 1, "Shape Variation");
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (SlimLight script in targets)
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
                foreach (SlimLight script in targets)
                {
                    script.ChangeLightColor();
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightIntensity, new GUIContent("Light Intensity Multiplier"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (SlimLight script in targets)
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
        if (slimLight && slimLight.IsInstanced)
        {
            slimLight.UpdateName();
        }
    }
}