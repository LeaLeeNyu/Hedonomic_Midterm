using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlateLight : MonoBehaviour
{
#if UNITY_EDITOR

    public Light LightSource;
    public GameObject BasePart;
    public GameObject[] ShadeParts;
    public Material BaseMat;
    public Material ShadeMat;
    [ColorUsage(false, true)]
    public Color LightColor;
    public float LightIntensity;
    public bool IsInstanced;

    [SerializeField]
    private string ThisTargetName;
    [SerializeField]
    private float[] ShadeHeights;
    [SerializeField]
    private float BulbHeight;
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

        // Set bulb height
        SkinnedMeshRenderer bulb = BasePart.transform.GetComponentInChildren<SkinnedMeshRenderer>();
        BulbHeight = 1;
        bulb.SetBlendShapeWeight(0, 100 - (BulbHeight * 100));

        // Set initial shade heights
        ShadeHeights = new float[] { 0, 0, 0, 0 };
        for (int i = 0; i < ShadeParts.Length; i++)
        {
            ShadeHeights[i] = Mathf.InverseLerp(-.04f, 0, ShadeParts[i].transform.localPosition.y);
        }

        AssignMaterials(true);

        // Configure light
        LightColor = ShadeMatCopy.GetColor("_EmissionColor");
        LightSource.color = LightColor;
        LightIntensity = LightSource.intensity;

        IsInstanced = true;
    }

    public void DuplicateSetup()
    {
        AssignMaterials(true);
        UpdateName();
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

        BasePart.GetComponentInChildren<Renderer>().sharedMaterial = BaseMatCopy;
        BasePart.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = BaseMatCopy;

        foreach (GameObject shadePart in ShadeParts)
        {
            shadePart.GetComponentInChildren<Renderer>().sharedMaterial = ShadeMatCopy;
        }
    }

    public void ChangeShadeHeights(int index)
    {
        Transform shade = ShadeParts[index].transform;

        // Save to undo stack 
        Undo.RecordObject(shade, "Change Shade Height");

        float y = Mathf.Lerp(-.4f, 0, ShadeHeights[index]);
        shade.localPosition = new Vector3(0, y, 0);
    }

    public void SetHangDistance()
    {
        SkinnedMeshRenderer bulb = BasePart.transform.GetComponentInChildren<SkinnedMeshRenderer>();

        // Save to undo stack 
        Undo.RecordObject(bulb, "Change Hang Distance");

        bulb.SetBlendShapeWeight(0, 100 - (BulbHeight * 100));

        // Save to undo stack 
        Undo.RecordObject(LightSource.transform, "Change Point Light Position");

        float y = Mathf.Lerp(-0.675f, -0.225f, BulbHeight);
        LightSource.transform.localPosition = new Vector3(0, y, 0);
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

