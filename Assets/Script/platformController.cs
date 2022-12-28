using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class platformController : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotate;

    public Vector3 normalPosition;
    public Vector3 leftPosition;
    public Vector3 rightPosition;
    public Vector3 footPosition;
    public Vector3 backPosition;
    public Quaternion normalRotate;
    public Quaternion leftRotate;
    public Quaternion rightRotate;
    public Quaternion footRotate;
    public Quaternion backRotate;
    public float lerpSpeed;

    private float normalLerp;
    private float leftLerp;
    private float rightLerp;
    private float footLerp;
    private float backLerp;

    private bool normalBool = false;
    private bool leftBool = false;
    private bool rightBool =false;
    private bool footBool = false;
    private bool backBool = false;

    //Update Weight Canvas
    [SerializeField] private TMP_Text weight;

    private void OnEnable()
    {
        XRHandController.LeftEvent += LeftEvent;
        XRHandController.RightEvent += RightEvent;
        XRHandController.FootEvent += FootEvent;
        XRHandController.BackEvent += BackEvent;
        XRHandController.NormalEvent += NormalEvent;
    }

    private void OnDisable()
    {
        XRHandController.LeftEvent -= LeftEvent;
        XRHandController.RightEvent -= RightEvent;
        XRHandController.FootEvent -= FootEvent;
        XRHandController.BackEvent -= BackEvent;
        XRHandController.NormalEvent -= NormalEvent;
    }

    private void Update()
    {      
        if(normalBool == true)
        {
            normalLerp += lerpSpeed;
            MovePlatform(startPosition,normalPosition, startRotate,normalRotate, normalLerp, normalBool);
        }else if(rightBool == true)
        {
            rightLerp += lerpSpeed;
            MovePlatform(startPosition, rightPosition, startRotate,rightRotate, rightLerp, rightBool);
        }else if(leftBool == true)
        {
            leftLerp += lerpSpeed;
            MovePlatform(startPosition,leftPosition, startRotate,leftRotate, leftLerp, leftBool);
        }else if(footBool == true)
        {
            footLerp += lerpSpeed;
            MovePlatform(startPosition,footPosition, startRotate, footRotate, footLerp, footBool);
        }else if (backBool == true)
        {
            backLerp += lerpSpeed;
            MovePlatform(startPosition,backPosition, startRotate, backRotate, backLerp, backBool);
        }

        //Update weight
        weight.text = WeightController.samWeight.ToString();

    }

    void LeftEvent()
    {
        ResetLerp();

        leftBool = true;

        normalBool = false;
        rightBool = false;
        footBool = false;
        backBool = false;
        Debug.Log(leftBool);

        startPosition = gameObject.transform.position;
        startRotate = gameObject.transform.rotation;
    }

    void NormalEvent()
    {
        ResetLerp();

        normalBool = true;

        leftBool = false;
        rightBool = false;
        footBool = false;
        backBool = false;

        startPosition = gameObject.transform.position;
        startRotate = gameObject.transform.rotation;
    }

    void RightEvent()
    {
        ResetLerp();

        rightBool = true;

        normalBool = false;
        leftBool = false;
        footBool = false;
        backBool = false;

        startPosition = gameObject.transform.position;
        startRotate = gameObject.transform.rotation;
    }

    void BackEvent()
    {
        ResetLerp();

        backBool = true;

        normalBool = false;
        leftBool = false;
        footBool = false;
        rightBool = false;

        startPosition = gameObject.transform.position;
        startRotate = gameObject.transform.rotation;
    }

    void FootEvent()
    {
        ResetLerp();

        footBool = true;

        normalBool = false;
        leftBool = false;
        backBool = false;
        rightBool = false;

        startPosition = gameObject.transform.position;
        startRotate = gameObject.transform.rotation;
    }

    void ResetLerp()
    {
        backLerp = 0;
        rightLerp = 0;
        leftLerp = 0;
        footLerp = 0;
        normalLerp = 0;
    }

    void ResetBool()
    {
        backBool = false;
        rightBool = false;
        leftBool = false;
        footBool = false;
        normalBool = false;
    }

    public void MovePlatform(Vector3 startPos, Vector3 endPos,Quaternion startRotate, Quaternion endRotate, float lerp,bool lerpBool)
    {
        if (lerp < 1)
        {
           // lerp += lerpSpeed;
           // Debug.Log(lerp);
            Vector3 movePos = SmoothLerp(startPos, endPos, lerp);
            Quaternion quaternion = Quaternion.Slerp(startRotate, endRotate, lerp);
            gameObject.transform.position = movePos;
            gameObject.transform.rotation = quaternion;
        }
        else
        {
            ResetBool();
            ResetLerp();
        }
    }

    private Vector3 SmoothLerp(Vector3 startPos, Vector3 endPos, float lerpPercent)
    {
        return new Vector3(
            Mathf.SmoothStep(startPos.x, endPos.x, lerpPercent),
            Mathf.SmoothStep(startPos.y, endPos.y, lerpPercent),
            Mathf.SmoothStep(startPos.z, endPos.z, lerpPercent));
    }

}
