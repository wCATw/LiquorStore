// Decompiled with JetBrains decompiler
// Type: LiquorStore.LookAt
// Assembly: LiquorStore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FC65CEA2-D49C-4FF1-A770-819DB08A2A29
// Assembly location: C:\Users\Kotovskiy\Documents\MySummerCar\Mods\LiquorStore.dll

using UnityEngine;

#nullable disable
namespace LiquorStore
{
    public class LookAt : MonoBehaviour
    {
        public Transform target;
        public Transform head;
        public Transform stopLook;
        public Transform idlePos;
        public float stopLookPos = 0.1f;
        public float maxSpeed = 500f;
        public bool isSmoking;
        private Quaternion desiredRot;
        private Quaternion lastRot;

        private void Start() => this.target = GameObject.Find("PLAYER").transform;

        private void Update()
        {
            this.stopLook.position = Vector3.MoveTowards(this.stopLook.position, this.target.position, this.maxSpeed * Time.deltaTime);
            ((Component) this).transform.position = Vector3.MoveTowards(((Component) this).transform.position, this.isSmoking ? this.idlePos.position : ((double) this.stopLook.localPosition.z > (double) this.stopLookPos ? this.stopLook.position : this.idlePos.position), this.maxSpeed * Time.deltaTime);
            Vector3 position = ((Component) this).transform.position;
            position.y = this.head.position.y;
            Vector3 vector3 = Vector3.op_Subtraction(position, this.head.position);
            if (Vector3.op_Inequality(vector3, Vector3.zero) && (double) ((Vector3) ref vector3).sqrMagnitude > 0.0)
            this.desiredRot = Quaternion.LookRotation(vector3, Vector3.up);
            this.lastRot = Quaternion.Slerp(this.lastRot, this.desiredRot, 1f * Time.deltaTime);
            this.head.rotation = this.lastRot;
        }
    }
}