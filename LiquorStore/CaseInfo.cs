using System;
using UnityEngine;

#nullable disable
namespace LiquorStore;
    [Serializable]
    public class CaseInfo
    {
        public string brand;
        public Vector3 position;
        public Vector3 euler;
        public bool[] activeBottles;
    }