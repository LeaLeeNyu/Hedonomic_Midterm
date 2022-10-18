using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HaloLight : MonoBehaviour
{
#if UNITY_EDITOR

    public Light LightSource;
    public Renderer Base;
    public GameObject[] Parts;
    public int PartIndex;
    public Material Mat;
    [ColorUsage(false, true)]
    public Color LightColor;
    public float LightIntensity;
    public bool IsInstanced;

    [SerializeField]
    private string ThisTargetName;
    [SerializeField]
    private Material MatCopy;

    private void Start()
    {
        Destroy(this);
    }

    public void FirstSetup()
    {
        // Unpack prefab to allow for modification
        PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);

        AssignMaterials(true);

        // Configure light  
        LightColor = MatCopy.GetColor("_EmissionColor");
        LightSource.color = LightColor;
        LightIntensity = LightSource.intensity;

        PartIndex = 1;

        IsInstanced = true;
    }

    public void DuplicateSetup()
    {
        AssignMaterials(true);
        UpdateName();
    }

    public void ChangePart()
    {
        // Store current part transform data then destroy it
        GameObject part = transform.GetChild(0).GetChild(0).gameObject;
        Vector3 position = part.transform.localPosition;
        Quaternion rotation = part.transform.localRotation;
        Vector3 scale = part.transform.localScale;
        Undo.DestroyObjectImmediate(part);

        // Create and new base prefab and set transform data
        part = PrefabUtility.InstantiatePrefab(Parts[PartIndex]) as GameObject;
        part.transform.parent = transform.GetChild(0);
        part.transform.localPosition = position;
        part.transform.localRotation = rotation;
        part.transform.localScale = scale;

        // Save to undo stack 
        Undo.RegisterCreatedObjectUndo(part, "Change Light Part");
        Undo.RecordObject(this, "Change Light Component");

        // Store new references
        LightSource = part.GetComponentInChildren<Light>();
        Base = part.GetComponentInChildren<Renderer>();

        // Configure new light
        LightSource.color = LightColor;
        LightSource.intensity = LightIntensity;

        AssignMaterials(false);
    }

    public void AssignMaterials(bool createNewMaterials)
    {
        // Create new material instances if required
        if (createNewMaterials)
        {
            if (!MatCopy)
            {
                MatCopy = new Material(Mat);
            }
            else
            {
                MatCopy = new Material(MatCopy);
            }
        }

        // Assign the materials
        Base.sharedMaterial = MatCopy;
    }

    public void ChangeLightColor()
    {
        // Save to undo stack 
        Undo.RecordObject(LightSource, "Change Light Color");
        Undo.RecordObject(MatCopy, "Change Material");

        MatCopy.SetColor("_EmissionColor", LightColor);
        LightSource.color = LightColor;
    }

    public void ChangeLightIntensity()
    {
        // Save to undo stack 
        Undo.RecordObject(LightSource, "Change Light Intensity");

        LightSource.intensity = LightIntensity;
    }

    public void UpdateName()
    {
        ThisTargetName = gameObject.name;
    }

#endif
}
