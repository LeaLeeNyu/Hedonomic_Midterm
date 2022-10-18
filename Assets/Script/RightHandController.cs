using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RightHandController : MonoBehaviour
{
    public HandType handType;

    //Stores what kind of characteristics we¡¯re looking for with our Input Device when we search for it later
    [HideInInspector] public InputDeviceCharacteristics inputDeviceCharacteristics;

    //Stores the InputDevice that we¡¯re Targeting once we find it in InitializeHand()
    private InputDevice _targetDevice;
    private Animator _handAnimator;

    //Hand Model Name String
    [SerializeField] private string leftHandName = "LeftHand(Clone)";
    [SerializeField] private string rightHandName = "RightHandProto(Clone)";

    void Update()
    {
        //Since our target device might not register at the start of the scene, we continously check until one is found.
        if (!_targetDevice.isValid)
        {
            InitializeHand();
        }
        else
        {
            UpdateHandPos();
        }
    }

    private void InitializeHand()
    {
        GameObject spawnedHand;

        if (handType == HandType.Left)
        {
            inputDeviceCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
            spawnedHand = GameObject.Find(leftHandName);
        }
        else
        {
            inputDeviceCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
            spawnedHand = GameObject.Find(rightHandName);
        }


        List<InputDevice> devices = new List<InputDevice>();
        //Call InputDevices to see if it can find any devices with the characteristics we¡¯re looking for
        InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, devices);

        //Our hands might not be active and so they will not be generated from the search.
        //We check if any devices are found here to avoid errors.
        if (devices.Count > 0)
        {

            _targetDevice = devices[0];
            _handAnimator = spawnedHand.GetComponent<Animator>();
        }
    }

    void UpdateHandPos()
    {

        if (_targetDevice.TryGetFeatureValue(CommonUsages.grip, out float grip))
        {
            _handAnimator.SetFloat("Trigger", grip);
        }
        else
        {
            _handAnimator.SetFloat("Trigger", 0);
        }
    }
}
