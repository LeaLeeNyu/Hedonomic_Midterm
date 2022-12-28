using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxController : MonoBehaviour
{
    [SerializeField] private GameObject boxTitleScreen;
    [SerializeField] private GameObject boxSelected;
    [SerializeField] private GameObject boxHover;
    [SerializeField] private GameObject boxMesh;
    [SerializeField] private float socketScale = 1.2f;
    [SerializeField] private float normalScale = 1.2f;

    //Line
    protected GameObject line;
    public GameObject lineP;

    public PackageInfoSO packageInfo;


    protected virtual void OnEnable()
    {
        XRBoxGrabbable.BoxPackageLockerSelectedEvent += AddLockerWeight;
        XRBoxGrabbable.BoxPackageLockerDeselectedEvent += ReduceLockerWeight;
        XRBoxGrabbable.BoxPackageBackSelectedEvent += AddSamWeight;
        XRBoxGrabbable.BoxPackageBackDeselectedEvent += ReduceSamWeight;
    }

    protected virtual void OnDisable()
    {
        XRBoxGrabbable.BoxPackageLockerSelectedEvent -= AddLockerWeight;
        XRBoxGrabbable.BoxPackageLockerDeselectedEvent -= ReduceLockerWeight;
        XRBoxGrabbable.BoxPackageBackSelectedEvent -= AddSamWeight;
        XRBoxGrabbable.BoxPackageBackDeselectedEvent -= ReduceSamWeight;
    }

    public void ShowHandInfo()
    {
        boxSelected.SetActive(true);
    }

    public void HideHandInfo()
    {
        boxSelected.SetActive(false);
    }

    public void ShowMissionInfo()
    {
        //Debug.Log("In Mission Socket");
        boxMesh.transform.localScale = new Vector3(socketScale, socketScale, socketScale);
        boxTitleScreen.SetActive(true);
    }

    public void HideMissionInfo()
    {
        Debug.Log("Leave Mission Socket");
        boxMesh.transform.localScale = new Vector3(normalScale, normalScale, normalScale);
        boxTitleScreen.SetActive(false);
    }

    //Hover
    public void ShowFrontInfo()
    {
        boxHover.SetActive(true);
    }

    public void HideFrontInfo()
    {
        boxHover.SetActive(false);
    }

    public void AddLockerWeight()
    {
        WeightController.lockerWeight += 10f;
    }

    public void ReduceLockerWeight()
    {
        WeightController.lockerWeight -= 10f;
    }

    public void AddSamWeight()
    {
        WeightController.samWeight += packageInfo.weight;
    }

    public void ReduceSamWeight()
    {
        WeightController.samWeight -= packageInfo.weight;
    }
}
