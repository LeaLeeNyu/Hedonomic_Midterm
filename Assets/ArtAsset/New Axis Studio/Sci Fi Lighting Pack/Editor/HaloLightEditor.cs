using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HaloLight))]
[CanEditMultipleObjects]
public class HaloLightEditor : Editor
{
    #region Private Properties
    private HaloLight haloLight;
    private bool showObjectReferences;
    #endregion

    #region Serialized Properties
    SerializedProperty ThisTargetName;
    SerializedProperty LightSource;
    SerializedProperty Base;
    SerializedProperty Parts;
    SerializedProperty Mat;
    SerializedProperty LightColor;
    SerializedProperty LightIntensity;
    SerializedProperty PartIndex;
    SerializedProperty IsInstanced;
    #endregion

    private void OnEnable()
    {
        haloLight = (HaloLight)target;

        #region Get Serialized Properties
        ThisTargetName = serializedObject.FindProperty("ThisTargetName");
        LightSource = serializedObject.FindProperty("LightSource");
        Base = serializedObject.FindProperty("Base");
        Parts = serializedObject.FindProperty("Parts");
        Mat = serializedObject.FindProperty("Mat");
        LightColor = serializedObject.FindProperty("LightColor");
        LightIntensity = serializedObject.FindProperty("LightIntensity");
        PartIndex = serializedObject.FindProperty("PartIndex");
        IsInstanced = serializedObject.FindProperty("IsInstanced");
        #endregion

        #region First Setup
        if (PrefabUtility.IsAnyPrefabInstanceRoot(haloLight.gameObject))
        {
            haloLight.FirstSetup();
        }
        #endregion

        #region Subsequent Setup
        else if (IsInstanced.boolValue == true)
        {
            // Is this a new instance? (Duplicated)
            if (ThisTargetName.stringValue != haloLight.gameObject.name)
            {
                foreach (HaloLight script in targets)
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
            EditorGUILayout.PropertyField(Parts, true);
            EditorGUILayout.PropertyField(Mat);
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
                foreach (HaloLight script in targets)
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
                foreach (HaloLight script in targets)
                {
                    script.ChangeLightColor();
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightIntensity, new GUIContent("Light Intensity Multiplier"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (HaloLight script in targets)
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
        if (haloLight && haloLight.IsInstanced)
        {
            haloLight.UpdateName();
        }
    }
}