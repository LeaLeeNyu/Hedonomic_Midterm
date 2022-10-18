using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockerTrigger : MonoBehaviour
{
    [SerializeField] private GameObject sockets;
    [SerializeField] private GameObject locker;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private float lerpSpeed;

    //Sphere Collider Info
    [SerializeField] private Vector3 spherePos;
    [SerializeField] private float sphereRadius;

    //Locker move parameter
    private float moveLerp = 0;
    private bool lerpBool = false;
    private bool _socketState;
    private Vector3 _start;
    private Vector3 _end;

    //Locker weight canvas
    [SerializeField] private TMP_Text lockerWight;
    //[SerializeField] private TMP_Text addWight;
    [SerializeField] private GameObject lockerCanvas;


    private void Update()
    {
       // Collider[] colliders = Physics.OverlapSphere(spherePos,sphereRadius);

        //Debug.Log(lerpBool);
        //If player enter the area that trigger the locker to open
        
        //move locker
        if (lerpBool)
        {
            moveLerp += lerpSpeed;
            MovePlatform(_start, _end, moveLerp, _socketState);
        }

        //update locker weight
        UpdateLockerWeight();


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            //Set locker
            Debug.Log("Hand enter locker area");
            lerpBool = true;

            _start = startPos;
            _end = endPos;
            _socketState = true;

            //Set locker canvas state
            lockerCanvas.SetActive(true);
        }       
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            Debug.Log("Hand exit locker area");
            lerpBool = true;

            _start = endPos;
            _end = startPos;
            _socketState = false;

            lockerCanvas.SetActive(false);
        }
    }

    public void MovePlatform(Vector3 startPos, Vector3 endPos, float lerp,bool socketStateBool)
    {
        if (lerp < 1)
        {
            Vector3 movePos = SmoothLerp(startPos, endPos, lerp);
            
            locker.gameObject.transform.localPosition = movePos;
        }
        //When the locaker get the open position
        else
        {
            //stop moving 
            lerpBool = false;
            moveLerp = 0;
            //show the sockets
            //sockets.SetActive(socketStateBool);
        }
    }
    private Vector3 SmoothLerp(Vector3 startPos, Vector3 endPos, float lerpPercent)
    {
        return new Vector3(
            Mathf.SmoothStep(startPos.x, endPos.x, lerpPercent),
            Mathf.SmoothStep(startPos.y, endPos.y, lerpPercent),
            Mathf.SmoothStep(startPos.z, endPos.z, lerpPercent));
    }

    private void UpdateLockerWeight()
    {
        lockerWight.text = BoxController.lockerWeight.ToString();
    }
}
