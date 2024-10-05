using UnityEngine;

namespace LiquorStore;

[Serializable]
public class CaseInfo
{
    public string brand;
    public Vector3 position;
    public Vector3 euler;
    public bool[] activeBottles;
}