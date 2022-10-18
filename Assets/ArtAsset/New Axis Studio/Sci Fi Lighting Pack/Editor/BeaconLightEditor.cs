using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BeaconLight))]
[CanEditMultipleObjects]
public class BeaconLightEditor : Editor
{
    #region Private Properties
    private BeaconLight beaconLight;
    private bool showObjectReferences;
    #endregion

    #region Serialized Properties
    SerializedProperty ThisTargetName;
    SerializedProperty LightSource;
    SerializedProperty Bulb;
    SerializedProperty Fixture;
    SerializedProperty Shade;
    SerializedProperty ShadowCaster;
    SerializedProperty Parts;
    SerializedProperty BaseMat;
    SerializedProperty ShadeMat;
    SerializedProperty LightColor;
    SerializedProperty LightIntensity;
    SerializedProperty PartIndex;
    SerializedProperty IsInstanced;
    #endregion

    private void OnEnable()
    {
        beaconLight = (BeaconLight)target;

        #region Get Serialized Properties
        ThisTargetName = serializedObject.FindProperty("ThisTargetName");
        LightSource = serializedObject.FindProperty("LightSource");
        Bulb = serializedObject.FindProperty("Bulb");
        Fixture = serializedObject.FindProperty("Fixture");
        Shade = serializedObject.FindProperty("Shade");
        ShadowCaster = serializedObject.FindProperty("ShadowCaster");
        Parts = serializedObject.FindProperty("Parts");
        BaseMat = serializedObject.FindProperty("BaseMat");
        ShadeMat = serializedObject.FindProperty("ShadeMat");
        LightColor = serializedObject.FindProperty("LightColor");
        LightIntensity = serializedObject.FindProperty("LightIntensity");
        PartIndex = serializedObject.FindProperty("PartIndex");
        IsInstanced = serializedObject.FindProperty("IsInstanced");
        #endregion

        #region First Setup
        if (PrefabUtility.IsAnyPrefabInstanceRoot(beaconLight.gameObject))
        {
            beaconLight.FirstSetup();
        }
        #endregion

        #region Subsequent Setup
        else if (IsInstanced.boolValue == true)
        {
            // Is this a new instance? (Duplicated)
            if (ThisTargetName.stringValue != beaconLight.gameObject.name)
            {
                foreach (BeaconLight script in targets)
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
            EditorGUILayout.PropertyField(Parts, true);
            EditorGUILayout.PropertyField(BaseMat);
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
                foreach (BeaconLight script in targets)
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
                foreach (BeaconLight script in targets)
                {
                    script.ChangeLightColor();
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(LightIntensity, new GUIContent("Light Intensity Multiplier"));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                foreach (BeaconLight script in targets)
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
        if (beaconLight && beaconLight.IsInstanced)
        {
            beaconLight.UpdateName();
        }
    }
}