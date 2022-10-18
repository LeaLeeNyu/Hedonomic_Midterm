using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetSpawnManager : MonoBehaviour
{
    private MissionObjectSpawn spawnManager;

     void Awake()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<MissionObjectSpawn>();
    }

    public void SpawnMissionObject()
    {
        if(spawnManager != null)
            spawnManager.SpawnMissionObject();
    }

    public void RemoveObjectFromList()
    {
        if (spawnManager != null)
            spawnManager.RemoveObjectFromList();
    }

    public void RemoveMissionPanel()
    {
        if(spawnManager != null)
        spawnManager.RemoveMissionPanel();
    }
}
