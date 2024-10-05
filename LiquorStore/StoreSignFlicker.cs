using UnityEngine;
using Random = UnityEngine.Random;

namespace LiquorStore;

public class StoreSignFlicker : MonoBehaviour
{
    public Material storeSign;
    public Light storeLight;
    public float minEmission = 0.075f;
    public float maxEmission = 0.5f;
    private float emissionStrength;
    private float strength;

    private void Update()
    {
        this.emissionStrength = Random.Range(this.minEmission, this.maxEmission);
        this.strength = Mathf.Lerp(this.strength, this.emissionStrength, Time.deltaTime * 6f);
        this.storeSign.SetColor("_EmissionColor", new Color(this.strength, this.strength, this.strength, 1f));
        this.storeLight.intensity = this.strength * 1.5f;
    }
}