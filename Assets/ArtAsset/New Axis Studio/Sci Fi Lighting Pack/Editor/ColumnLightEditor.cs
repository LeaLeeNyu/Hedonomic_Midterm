using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColumnLight))]
[CanEditMultipleObjects]
public class ColumnLightEditor : Editor
{
    #region Private Properties
    private ColumnLight columnLight;
    private bool showObjectReferences;
    #endregion

    #region Serialized Properties
    SerializedProperty ThisTargetName;
    SerializedProperty LightSource;
    SerializedProperty Base;
    SerializedProperty Shade;
    SerializedProperty BaseParts;
    SerializedProperty ShadeParts;
    SerializedProperty BaseMat;
    SerializedProperty ShadeMat; 
    SerializedProperty LightColor;
    SerializedProperty LightIntensity;
    SerializedProperty BaseIndex;
    SerializedProperty ShadeIndex;
    SerializedProperty IsInstanced;
    #endregion

    private void OnEnable()
    {
        columnLight = (ColumnLight)target;

        #region Get Serialized Properties
        ThisTargetName = serializedObject.FindProperty("ThisTargetName");
        LightSource = serializedObject.FindProperty("LightSource");
        Base = serializedObject.FindProperty("Base");
        Shade = serializedObject.FindProperty("Shade");
        BaseParts = serializedObject.FindProperty("BaseParts");
        ShadeParts = serializedObject.FindProperty("ShadeParts");
        BaseMat = serializedObject.FindProperty("BaseMat");
        ShadeMat = serializedObject.FindProperty("ShadeMat");
        LightColor = serializedObject.FindProperty("LightColor");
        LightIntensity = serializedObject.FindProperty("LightIntensity");
        BaseIndex = serializedObject.FindProperty("BaseIndex");
        ShadeIndex = serializedObject.FindProperty("ShadeIndex");
        IsInstanced = serializedObject.FindProperty("IsInstanced");
        #endregion

        #region First Setup
        if (PrefabUtility.IsAnyPrefabInstanceRoot(columnLight.gameObject))
        {
            columnLight.FirstSetup();
        }
        #endregion

        #region Subsequent Setup
        else if (IsInstanced.boolValue == true)
        {
            // Is this a new instance? (Duplicated)
            if (ThisTargetName.stringValue != columnLight.gameObject.name)
            {
                foreach (ColumnLight script in targets)
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
            EditorGUILayout.PropertyField(BaseParts, true);
            EditorGUILayout.PropertyField(ShadeParts, true);
            EditorGUILayout.PropertyField(BaseMat);
            EditorGUILayout.PropertyField(ShadeMat);
        }
        #endregion

        if (IsInstanced.boolValue == true)
        {
            #region Base Configuration
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.IntSlider(BaseIndex, 0, BaseParts.arraySize - 1, "Base Height");
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (ColumnLight script in targets)
                {
                    script.ChangePart(true);
                }
            }
            #endregion

            #region Shade Configuration
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.IntSlider(ShadeIndex, 0, 2, "Shade Height");
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (ColumnLight script in targets)
                {
                    script.ChangePart(false);
                }
            }
            #endregion

            #region Light Configuration     
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightColor);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (ColumnLight script in targets)
                {
                    script.ChangeLightColor();
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightIntensity, new GUIContent("Light Intensity Multiplier"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (ColumnLight script in targets)
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
        if (columnLight && columnLight.IsInstanced)
        {
            columnLight.UpdateName();
        }
    }
}
