using System;
using UnityEngine;

#nullable disable
namespace LiquorStore;
    [Serializable]
    public class BottleInfo
    {
        public string brand;
        public bool isEmpty;
        public Vector3 position;
        public Vector3 euler;
    }