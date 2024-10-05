using UnityEngine;

namespace LiquorStore;

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
        if (!this.FPScam)
            return;
        this.hasHit = Physics.Raycast(this.FPScam.ScreenPointToRay(Input.mousePosition), out this.hitInfo, this.rayDistance, this.layerMask);
    }

    public bool GetHit(Collider collider)
    {
        return this.hasHit = Physics.Raycast(this.FPScam.ScreenPointToRay(Input.mousePosition), out this.hitInfo, this.rayDistance, this.layerMask);
    }

    public bool GetHitAny(Collider[] colliders)
    {
        return this.hasHit && colliders.Any(collider => collider == this.hitInfo.collider);
    }

    public bool GetHitAny(List<Collider> colliders)
    {
        return this.hasHit && colliders.Any(collider => collider == this.hitInfo.collider);

    }
}