// Decompiled with JetBrains decompiler
// Type: LiquorStore.InteractionRaycast
// Assembly: LiquorStore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FC65CEA2-D49C-4FF1-A770-819DB08A2A29
// Assembly location: C:\Users\Kotovskiy\Documents\MySummerCar\Mods\LiquorStore.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace LiquorStore
{
    public class InteractionRaycast : MonoBehaviour
    {
        public RaycastHit hitInfo;
        private Camera FPScam;
        public bool hasHit;
        public float rayDistance = 1.35f;
        public LayerMask layerMask;

        private void Start()
        {
            this.hitInfo = new RaycastHit();
            this.FPScam = ((Component) GameObject.Find("PLAYER").transform.Find("Pivot/AnimPivot/Camera/FPSCamera/FPSCamera")).GetComponent<Camera>();
        }

        private void FixedUpdate()
        {
            if (!Object.op_Implicit((Object) this.FPScam))
                return;
            this.hasHit = Physics.Raycast(this.FPScam.ScreenPointToRay(Input.mousePosition), ref this.hitInfo, this.rayDistance, LayerMask.op_Implicit(this.layerMask));
        }

        public bool GetHit(Collider collider)
        {
            return this.hasHit && Object.op_Equality((Object) ((RaycastHit) ref this.hitInfo).collider, (Object) collider);
        }

        public bool GetHitAny(Collider[] colliders)
        {
            return this.hasHit && ((IEnumerable<Collider>) colliders).Any<Collider>((Func<Collider, bool>) (collider => Object.op_Equality((Object) collider, (Object) ((RaycastHit) ref this.hitInfo).collider)));
        }

        public bool GetHitAny(List<Collider> colliders)
        {
            return this.hasHit && colliders.Any<Collider>((Func<Collider, bool>) (collider => Object.op_Equality((Object) collider, (Object) ((RaycastHit) ref this.hitInfo).collider)));
        }
    }
}