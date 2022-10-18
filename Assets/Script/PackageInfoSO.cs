using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Tool,
    MissionPackage,
}

[CreateAssetMenu(fileName ="PackageInfo",menuName ="ScriptableObject/PackageInfo")]

public class PackageInfoSO : ScriptableObject
{
    public float weight;
    public string size;
    public Type type;
}
