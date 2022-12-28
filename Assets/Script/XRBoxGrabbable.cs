using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRBoxGrabbable : XRGrabInteractable
{
    [SerializeField]private BoxController boxController;

    //Select by socket
    [HideInInspector] public static event UnityAction BoxPackageBackSelectedEvent = delegate { };
    [HideInInspector] public static event UnityAction BoxPackageLockerSelectedEvent = delegate { };

    //Deselect by socket
    [HideInInspector] public static event UnityAction BoxPackageBackDeselectedEvent = delegate { };
    [HideInInspector] public static event UnityAction BoxPackageLockerDeselectedEvent = delegate { };


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
            BoxPackageLockerSelectedEvent.Invoke();
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
            BoxPackageLockerDeselectedEvent.Invoke();
        }
    }

}
