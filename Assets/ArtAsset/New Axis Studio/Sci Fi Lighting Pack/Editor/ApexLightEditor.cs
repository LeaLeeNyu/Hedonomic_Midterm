using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ApexLight))]
[CanEditMultipleObjects]
public class ApexLightEditor : Editor
{
    #region Private Properties
    private ApexLight apexLight;
    private bool showObjectReferences;
    #endregion

    #region Serialized Properties
    SerializedProperty ThisTargetName;
    SerializedProperty LightSource;
    SerializedProperty Bulb;
    SerializedProperty Fixture;
    SerializedProperty Shade;
    SerializedProperty ShadowCaster;
    SerializedProperty BaseMat;
    SerializedProperty ShadeMat;
    SerializedProperty ShadeEmissiveTextures;
    SerializedProperty ShadeTransparencyTextures;
    SerializedProperty ShadeTexIndex;
    SerializedProperty LightColor;
    SerializedProperty LightIntensity;
    SerializedProperty IsInstanced;
    #endregion

    private void OnEnable()
    {
        apexLight = (ApexLight)target;

        #region Get Serialized Properties
        ThisTargetName = serializedObject.FindProperty("ThisTargetName");
        LightSource = serializedObject.FindProperty("LightSource");
        Bulb = serializedObject.FindProperty("Bulb");
        Fixture = serializedObject.FindProperty("Fixture");
        Shade = serializedObject.FindProperty("Shade");
        ShadowCaster = serializedObject.FindProperty("ShadowCaster");
        BaseMat = serializedObject.FindProperty("BaseMat");
        ShadeMat = serializedObject.FindProperty("ShadeMat");
        ShadeEmissiveTextures = serializedObject.FindProperty("ShadeEmissiveTextures");
        ShadeTransparencyTextures = serializedObject.FindProperty("ShadeTransparencyTextures");
        ShadeTexIndex = serializedObject.FindProperty("ShadeTexIndex");
        LightColor = serializedObject.FindProperty("LightColor");
        LightIntensity = serializedObject.FindProperty("LightIntensity");
        IsInstanced = serializedObject.FindProperty("IsInstanced");
        #endregion

        #region First Setup
        if (PrefabUtility.IsAnyPrefabInstanceRoot(apexLight.gameObject))
        {
            apexLight.FirstSetup();
        }
        #endregion

        #region Subsequent Setup
        else if (IsInstanced.boolValue == true)
        {
            // Is this a new instance? (Duplicated)
            if (ThisTargetName.stringValue != apexLight.gameObject.name)
            {
                foreach (ApexLight script in targets)
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
            EditorGUILayout.PropertyField(Bulb);
            EditorGUILayout.PropertyField(Fixture);
            EditorGUILayout.PropertyField(Shade);
            EditorGUILayout.PropertyField(ShadowCaster);
            EditorGUILayout.PropertyField(BaseMat);
            EditorGUILayout.PropertyField(ShadeMat);
            EditorGUILayout.PropertyField(ShadeEmissiveTextures, true);
            EditorGUILayout.PropertyField(ShadeTransparencyTextures, true);
        }
        #endregion

        if (IsInstanced.boolValue == true)
        {
            #region Shade Texture Configuration
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.IntSlider(ShadeTexIndex, 0, ShadeEmissiveTextures.arraySize - 1, "Shade Texture Variants");
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (ApexLight script in targets)
                {
                    script.ChangeTexture();
                }
            }
            #endregion

            #region Light Configuration     
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightColor);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (ApexLight script in targets)
                {
                    script.ChangeLightColor();
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightIntensity, new GUIContent("Light Intensity Multiplier"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (ApexLight script in targets)
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
        if (apexLight && apexLight.IsInstanced)
        {
            apexLight.UpdateName();
        }
    }
}
