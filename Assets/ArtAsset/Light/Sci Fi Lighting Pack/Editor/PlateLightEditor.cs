using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlateLight))]
[CanEditMultipleObjects]
public class PlateLightEditor : Editor
{
    #region Private Properties
    private PlateLight plateLight;
    private bool showObjectReferences;
    #endregion

    #region Serialized Properties
    SerializedProperty ThisTargetName;
    SerializedProperty LightSource;
    SerializedProperty BasePart;
    SerializedProperty ShadeParts;
    SerializedProperty BulbHeight;
    SerializedProperty ShadeHeights;
    SerializedProperty BaseMat;
    SerializedProperty ShadeMat;
    SerializedProperty LightColor;
    SerializedProperty LightIntensity;
    SerializedProperty IsInstanced;
    #endregion

    private void OnEnable()
    {
        plateLight = (PlateLight)target;

        #region Get Serialized Properties
        ThisTargetName = serializedObject.FindProperty("ThisTargetName");
        LightSource = serializedObject.FindProperty("LightSource");
        BasePart = serializedObject.FindProperty("BasePart");
        ShadeParts = serializedObject.FindProperty("ShadeParts");
        BulbHeight = serializedObject.FindProperty("BulbHeight");
        ShadeHeights = serializedObject.FindProperty("ShadeHeights");
        BaseMat = serializedObject.FindProperty("BaseMat");
        ShadeMat = serializedObject.FindProperty("ShadeMat");
        LightColor = serializedObject.FindProperty("LightColor");
        LightIntensity = serializedObject.FindProperty("LightIntensity");
        IsInstanced = serializedObject.FindProperty("IsInstanced");
        #endregion

        #region First Setup
        if (PrefabUtility.IsAnyPrefabInstanceRoot(plateLight.gameObject))
        {
            plateLight.FirstSetup();
        }
        #endregion

        #region Subsequent Setup
        else if (IsInstanced.boolValue == true)
        {
            // Is this a new instance? (Duplicated)
            if (ThisTargetName.stringValue != plateLight.gameObject.name)
            {
                foreach (PlateLight script in targets)
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
            EditorGUILayout.PropertyField(BasePart);
            EditorGUILayout.PropertyField(ShadeParts, true);
            EditorGUILayout.PropertyField(BaseMat);
            EditorGUILayout.PropertyField(ShadeMat);
        }
        #endregion

        if (IsInstanced.boolValue == true)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Slider(BulbHeight, 0, 1, "Bulb Height");
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (PlateLight script in targets)
                {
                    script.SetHangDistance();
                }
            }

            #region Shade Heights
            for (int i = 0; i < ShadeParts.arraySize; i++)
            {
                ShadeHeightConfiguration(i, "Shade " + (i + 1) + " Height");
            }
            #endregion

            #region Light Configuration     
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightColor);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (PlateLight script in targets)
                {
                    script.ChangeLightColor();
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightIntensity, new GUIContent("Light Intensity Multiplier"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (PlateLight script in targets)
                {
                    script.ChangeLightIntensity();
                }
            }
            #endregion
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ShadeHeightConfiguration(int index, string label)
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Slider(ShadeHeights.GetArrayElementAtIndex(index), 0, 1, label);
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            foreach (PlateLight script in targets)
            {
                script.ChangeShadeHeights(index);
            }
        }
    }

    private void OnDisable()
    {
        if (plateLight && plateLight.IsInstanced)
        {
            plateLight.UpdateName();
        }
    }
}
