using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionBoxController : BoxController
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private GameObject onGround;

    //line direction
    private GameObject player;


    private void OnEnable()
    {
        XRMissionBoxGrabbable.PackageLockerSelectedEvent += AddLockerWeight;
        XRMissionBoxGrabbable.PackageLockerDeselectedEvent += ReduceLockerWeight;
        XRMissionBoxGrabbable.PackageBackSelectedEvent += AddSamWeight;
        XRMissionBoxGrabbable.PackageBackDeselectedEvent += ReduceSamWeight;
    }

    private void OnDisable()
    {
        XRMissionBoxGrabbable.PackageLockerSelectedEvent -= AddLockerWeight;
        XRMissionBoxGrabbable.PackageLockerDeselectedEvent -= ReduceLockerWeight;
        XRMissionBoxGrabbable.PackageBackSelectedEvent -= AddSamWeight;
        XRMissionBoxGrabbable.PackageBackDeselectedEvent -= ReduceSamWeight;
    }

    private void Awake()
    {
        player = GameObject.Find("XR Origin");
    }

    private void Update()
    {
        if (line != null)
        {
            Vector3 lookAtDirection = player.transform.position - gameObject.transform.position;
            Quaternion lineRotation = Quaternion.LookRotation(lookAtDirection.normalized, Vector3.up);
            line.transform.rotation = lineRotation;
        }     

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground"&& line == null)
        {
            //instantiate line
            Vector3 lookAtDirection = player.transform.position - gameObject.transform.position;
            Quaternion lineRotation = Quaternion.LookRotation(lookAtDirection.normalized, Vector3.up);
            Vector3 linePosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.9f, gameObject.transform.position.z);
            line = Instantiate(lineP, linePosition, lineRotation);

            //show on ground panel
            onGround.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(line);

            //hide on ground panel
            onGround.SetActive(false);
        }
    }



}
