using UnityEngine;

namespace LiquorStore;

public class SmokingHelper : MonoBehaviour
{
    public Animation anim;
    public LookAt lookAt;

    private void OnEnable() => this.lookAt.isSmoking = true;

    private void OnDisable() => this.lookAt.isSmoking = false;

    private void Update()
    {
        if (this.anim.isPlaying)
            return;
        ((Behaviour) this).enabled = false;
    }
}