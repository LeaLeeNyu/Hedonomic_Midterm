using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MissionObjectSpawn : MonoBehaviour
{
    public List<GameObject> missionObjects;
    [SerializeField]private Vector3 spawnPos = Vector3.zero;
    [SerializeField] private GameObject spawnParent;
    [SerializeField] private GameObject packageSocket;
    [SerializeField] private GameObject missionPanel;
    [SerializeField] private XRSimpleInteractable lightToggle;


    private void Start()
    {
        SpawnMissionObject();
        lightToggle.enabled = false;
    }

    //Spawn a new mission object when player grab the object from the socket
    //Enqueue the last gameObject on the list
    public void SpawnMissionObject()
    {
        if (missionObjects.Count > 0)
        {
            GameObject spawnObject = missionObjects[missionObjects.Count - 1];
            Instantiate(spawnObject, spawnParent.transform);
            //.transform.localPosition = Vector3.zero;
        }
    }

    public void RemoveObjectFromList()
    {
        if (missionObjects.Count > 0)
            missionObjects.Remove(missionObjects[missionObjects.Count - 1]);
    }

    //Destory the missionPanel if there is no object in mission list
    public void RemoveMissionPanel()
    {
        if(missionObjects.Count == 0)
        {
            Destroy(packageSocket);
            packageSocket = null;

            HideMissionPanel();
            lightToggle.enabled=true;
        }           
    }

    public void ShowMissionPanel()
    {
        missionPanel.SetActive(true);
    }

    public void HideMissionPanel()
    {
        missionPanel.SetActive(false);
    }

}
