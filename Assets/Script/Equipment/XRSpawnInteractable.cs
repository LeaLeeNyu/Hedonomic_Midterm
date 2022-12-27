using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class XRSpawnInteractable : XRSimpleInteractable
{
    [SerializeField] private string controllerTag;
    [SerializeField] private GameObject equipP;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private int amountInt;
    [SerializeField] private TMP_Text amount;

    public GameObject SpawnEqip()
    {
        GameObject spawnObject = SpawnObject(equipP);
        return spawnObject;
    }

    private GameObject SpawnObject(GameObject equip)
    {
        if (amountInt > 0)
        {
            GameObject spawnObject = Instantiate(equip, spawnPos.position, equip.transform.rotation);

            amountInt -= 1;
            amount.text = amountInt.ToString();

            return spawnObject;
        }
        else
        {
            return null;
        }
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
        {
            var controller = controllerInteractor.xrController;

            if (controller.tag == controllerTag)
            {
                GameObject spawnObject = SpawnEqip();

                IXRSelectInteractable spawnInteractable = spawnObject.GetComponent<IXRSelectInteractable>();
                IXRSelectInteractor interactor = controller.GetComponent<IXRSelectInteractor>();
                interactionManager.SelectEnter(interactor, spawnInteractable);
            }

        }
    }
}
