using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRMissionBoxGrabbable : XRGrabInteractable
{
    [SerializeField] private MissionBoxController boxController;

    //[Obsolete]
    //protected override void OnSelectEntered(XRBaseInteractor interactor)
    //{
    //    base.OnSelectEntered(interactor);

    //    if (interactor.CompareTag("Hand"))
    //    {
    //        Debug.Log("Select by hand!");
    //        boxController.ShowHandInfo();
    //    }
    //    else if (interactor.CompareTag("MissionSocket"))
    //    {
    //        boxController.ShowMissionInfo();
    //    }
    //}

    //[Obsolete]
    //protected override void OnSelectExited(XRBaseInteractor interactor)
    //{
    //    base.OnSelectExited(interactor);
    //    if (interactor.CompareTag("Hand"))
    //    {
    //        Debug.Log("Exit by hand!");
    //        boxController.HideHandInfo();
    //    }
    //    else if (interactor.CompareTag("MissionSocket"))
    //    {
    //        boxController.HideMissionInfo();
    //    }
    //}

    //Select by socket
    [HideInInspector] public static event UnityAction PackageBackSelectedEvent = delegate { };
    [HideInInspector] public static event UnityAction PackageLockerSelectedEvent = delegate { };


    //Deselect by socket
    [HideInInspector] public static event UnityAction PackageBackDeselectedEvent = delegate { };
    [HideInInspector] public static event UnityAction PackageLockerDeselectedEvent = delegate { };


    [Obsolete]
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);

        if (interactor.CompareTag("Hand"))
        {
            //Debug.Log("Select by hand!");
            boxController.ShowHandInfo();
            //PackageHandSelectedEvent.Invoke();
        }
        else if (interactor.CompareTag("MissionSocket"))
        {
            Debug.Log("Select by MissionSocket!");
            boxController.ShowMissionInfo();
            //PackageMissionSelectedEvent.Invoke();
        }
        else if (interactor.CompareTag("LockerSocket"))
        {
            PackageLockerSelectedEvent.Invoke();
            Debug.Log("select by locker");
        }
        else if (interactor.CompareTag("BackSocket"))
        {
            PackageBackSelectedEvent.Invoke();
        }
    }

    [Obsolete]
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        if (interactor.CompareTag("Hand"))
        {
            Debug.Log("Leave hand!");
            boxController.HideHandInfo();
            //PackageHandDeselectedEvent.Invoke();
        }
        else if (interactor.CompareTag("MissionSocket"))
        {
            Debug.Log("Unselect by MissionSocket!");
            boxController.HideMissionInfo();
            //PackageMissionDeselectedEvent.Invoke();
        }
        else if (interactor.CompareTag("LockerSocket"))
        {
            PackageLockerDeselectedEvent.Invoke();
        }
        else if (interactor.CompareTag("BackSocket"))
        {
            PackageBackDeselectedEvent.Invoke();
        }
    }

}
