using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCamera : MonoBehaviour
{
    private void Awake()
    {
        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
    }
}
