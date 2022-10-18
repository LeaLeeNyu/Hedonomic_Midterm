using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRBoxGrabbable : XRGrabInteractable
{
    [SerializeField]private BoxController boxController;

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
        }
        else if (interactor.CompareTag("MissionSocket"))
        {
            //Debug.Log("Select by MissionSocket!");
            boxController.ShowMissionInfo();
        }else if (interactor.CompareTag("LockerSocket"))
        {
            PackageLockerSelectedEvent.Invoke();
            Debug.Log("select by locker");
        }
    }

    [Obsolete]
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        if (interactor.CompareTag("Hand"))
        {
            Debug.Log("Select by hand!");
            boxController.HideHandInfo();
        }
        else if (interactor.CompareTag("MissionSocket"))
        {
            Debug.Log("Unselect by MissionSocket!");
            boxController.HideMissionInfo();
        }
        else if (interactor.CompareTag("LockerSocket"))
        {
            PackageLockerDeselectedEvent.Invoke();
        }
    }

}
