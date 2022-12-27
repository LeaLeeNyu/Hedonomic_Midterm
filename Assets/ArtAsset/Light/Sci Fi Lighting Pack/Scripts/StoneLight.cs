using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StoneLight : MonoBehaviour
{
#if UNITY_EDITOR

    public Light LightSource;
    public SkinnedMeshRenderer Fixture;
    public GameObject Shell;
    public Material Mat;
    [ColorUsage(false, true)]
    public Color LightColor;
    public float LightIntensity;
    public bool IsInstanced, IsBaked;

    [SerializeField]
    private string ThisTargetName;
    [SerializeField]
    private float HangDistance;
    [SerializeField]
    private Material MatCopy;
    [SerializeField]
    private GameObject BakedFixture;
    [SerializeField]
    private Mesh BakedMesh;

    private void Start()
    {
        Destroy(this);
    }

    public void FirstSetup()
    {
        // Unpack prefab to allow for modification
        PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);

        // Set bulb height
        HangDistance = 1;
        Fixture.SetBlendShapeWeight(0, 100 - (HangDistance * 100));
        float y = Mathf.Lerp(-1, -0.2f, HangDistance);
        Shell.transform.localPosition = new Vector3(0, y, 0);

        BakedMesh = new Mesh();
        BakeMesh();

        AssignMaterials(true);

        // Configure light  
        LightColor = MatCopy.GetColor("_EmissionColor");
        LightSource.color = LightColor;
        LightIntensity = LightSource.intensity;

        IsInstanced = true;
    }

    public void DuplicateSetup()
    {
        // Store new references
        BakedFixture = Fixture.transform.GetChild(0).gameObject;
        BakedMesh = new Mesh();

        BakeMesh();
        AssignMaterials(true);
        UpdateName();
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
        Renderer[] renderers = transform.GetChild(0).GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.sharedMaterial = MatCopy;
        }
    }

    public void SetHangDistance()
    {
        if (IsBaked)
        {
            BakedFixture.GetComponent<Renderer>().enabled = false;
            Fixture.enabled = true;

            IsBaked = false;
        }

        // Save to undo stack 
        Undo.RecordObject(Fixture, "Change Hang Distance");

        Fixture.SetBlendShapeWeight(0, 100 - (HangDistance * 100));

        // Save to undo stack 
        Undo.RecordObject(Shell.transform, "Change Fixture Position");

        float y = Mathf.Lerp(-1, -0.2f, HangDistance);
        Shell.transform.localPosition = new Vector3(0, y, 0);
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

    public void BakeMesh()
    {
        // Create the bakedBase gameobject
        if (!BakedFixture)
        {
            BakedFixture = new GameObject(Fixture.name + " Baked Copy");
            BakedFixture.transform.parent = Fixture.transform;
            BakedFixture.transform.localPosition = Vector3.zero;
            BakedFixture.transform.localRotation = Quaternion.identity;
            BakedFixture.transform.localScale = Vector3.one;
            BakedFixture.AddComponent<MeshFilter>();
            BakedFixture.AddComponent<MeshRenderer>();
            BakedFixture.isStatic = true;

            // Save to undo stack 
            Undo.RegisterCreatedObjectUndo(BakedFixture, "Create Baked Light Part");
        }

        // Update the baked mesh
        BakedFixture.GetComponent<MeshFilter>().sharedMesh = BakedMesh;
        Fixture.GetComponent<SkinnedMeshRenderer>().BakeMesh(BakedMesh);
        BakedMesh.RecalculateBounds();
        Fixture.GetComponent<SkinnedMeshRenderer>().enabled = false;
        BakedFixture.GetComponent<Renderer>().enabled = true;

        IsBaked = true;
    }

    public void UpdateName()
    {
        ThisTargetName = gameObject.name;
    }

#endif
}


