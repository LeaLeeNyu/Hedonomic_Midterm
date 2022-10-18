using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ColumnLight : MonoBehaviour
{
#if UNITY_EDITOR

    private float[] matTiling = new float[] { 2, 1.33f, 1 };

    public Light LightSource;
    public Renderer Base;
    public Transform Shade;
    public GameObject[] BaseParts, ShadeParts;
    public int BaseIndex, ShadeIndex;
    public Material BaseMat, ShadeMat;
    [ColorUsage(false, true)]
    public Color LightColor;
    public float LightIntensity;
    public bool IsInstanced;

    [SerializeField]
    private string ThisTargetName;
    [SerializeField]
    private Material BaseMatCopy, ShadeMatCopy;

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
        LightColor = ShadeMatCopy.GetColor("_EmissionColor");
        LightSource.color = LightColor;
        LightIntensity = LightSource.intensity;

        BaseIndex = 1;
        ShadeIndex = 1;

        IsInstanced = true;
    }

    public void DuplicateSetup()
    {
        AssignMaterials(true);
        UpdateName();
    }

    public void ChangePart(bool changeBase)
    {
        // Get part references
        GameObject currentBase = transform.GetChild(0).GetChild(0).gameObject;
        GameObject currentShade = transform.GetChild(0).GetChild(1).gameObject;
        GameObject part = changeBase ? currentBase : currentShade;

        // Store current part transform data then destroy it
        Vector3 position = part.transform.localPosition;
        Quaternion rotation = part.transform.localRotation;
        Vector3 scale = part.transform.localScale;
        Undo.DestroyObjectImmediate(part);

        // Create and new base prefab and set transform data
        part = changeBase ? PrefabUtility.InstantiatePrefab(BaseParts[BaseIndex]) as GameObject : PrefabUtility.InstantiatePrefab(ShadeParts[ShadeIndex]) as GameObject;
        part.transform.parent = transform.GetChild(0);
        part.transform.SetSiblingIndex(changeBase ? 0 : 1);
        part.transform.localPosition = position;
        part.transform.localRotation = rotation;
        part.transform.localScale = scale;

        // Save to undo stack 
        Undo.RegisterCreatedObjectUndo(part, "Change Light Part");
        Undo.RegisterFullObjectHierarchyUndo(gameObject, "Register Light Hierarchy");

        if (changeBase)
        {
            // Store new references
            LightSource = part.GetComponentInChildren<Light>();
            Base = part.transform.GetChild(0).GetComponent<Renderer>();

            // Configure new light
            LightSource.color = LightColor;
            LightSource.intensity = LightIntensity;

            // Offset position of shade
            float yOffset = part.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.y;
            currentShade.transform.localPosition = new Vector3(0, -yOffset, 0);          
        }
        else
        {
            // Save to undo stack 
            Undo.RecordObject(ShadeMatCopy, "Change Shade Material");

            // Store new references
            Shade = part.transform;
        }

        AssignMaterials(false);
    }

    public void AssignMaterials(bool createNewMaterials)
    {
        // Create new material instances if required
        if (createNewMaterials)
        {
            if (!BaseMatCopy)
            {
                BaseMatCopy = new Material(BaseMat);
                ShadeMatCopy = new Material(ShadeMat);
            }
            else
            {
                BaseMatCopy = new Material(BaseMatCopy);
                ShadeMatCopy = new Material(ShadeMatCopy);
            }
        }
        else
        {
            // Apply main texture tiling
            ShadeMatCopy.mainTextureScale = new Vector2(matTiling[ShadeIndex], 1);
        }

        // Assign the materials
        Base.sharedMaterial = BaseMatCopy;
        Renderer[] shadeRenderers = Shade.GetComponentsInChildren<Renderer>();
        foreach (Renderer shadeRenderer in shadeRenderers)
        {
            shadeRenderer.sharedMaterial = ShadeMatCopy;
        }
    }

    public void ChangeLightColor()
    {
        // Save to undo stack 
        Undo.RecordObject(LightSource, "Change Light Color");
        Undo.RecordObject(BaseMatCopy, "Change Base Material");
        Undo.RecordObject(ShadeMatCopy, "Change Shade Material");

        BaseMatCopy.SetColor("_EmissionColor", LightColor);
        ShadeMatCopy.SetColor("_EmissionColor", LightColor);
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

