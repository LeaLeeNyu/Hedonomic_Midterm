using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ApexLight : MonoBehaviour
{
#if UNITY_EDITOR

    public Light LightSource;
    public Renderer Bulb;
    public Renderer Fixture;
    public Renderer Shade;
    public Renderer ShadowCaster;
    public Material BaseMat, ShadeMat;
    public Texture[] ShadeEmissiveTextures, ShadeTransparencyTextures;
    [ColorUsage(false, true)]
    public Color LightColor;
    public float LightIntensity;
    public bool IsInstanced;

    [SerializeField]
    private string ThisTargetName;
    [SerializeField]
    private int ShadeTexIndex;
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

        ShadeTexIndex = 1;

        IsInstanced = true;
    }

    public void DuplicateSetup()
    {
        AssignMaterials(true);
        UpdateName();
    }

    public void ChangeTexture()
    {
        // Save to undo stack 
        Undo.RecordObject(ShadeMatCopy, "Change Shade Material");

        Shade.sharedMaterial.SetTexture("_EmissionMap", ShadeEmissiveTextures[ShadeTexIndex]);
        Shade.sharedMaterial.mainTexture = ShadeTransparencyTextures[ShadeTexIndex];
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

        // Assign the materials
        Bulb.sharedMaterial = BaseMatCopy;
        Fixture.sharedMaterial = BaseMatCopy;
        Shade.sharedMaterial = ShadeMatCopy;
        ShadowCaster.sharedMaterial = ShadeMatCopy;
    }

    public void ChangeLightColor()
    {
        // Save to undo stack 
        Undo.RecordObject(LightSource, "Change Light Color");
        Undo.RecordObject(BaseMatCopy, "Change Base Material");
        Undo.RecordObject(ShadeMatCopy, "Change Shade Material");

        BaseMatCopy.SetColor("_EmissionColor", LightColor * 2);
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