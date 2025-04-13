using UnityEngine;

public class DistanceShaderLerp : MonoBehaviour
{
    public Transform objectA;
    public Transform objectB;
    public Material targetMaterial;
    public string shaderPropertyName = "_EffectAmount";
    public float maxDistance = 5f;
    public float lerpSpeed = 2f; // how fast it eases to the value

    float currentValue = 0f;

    void Update()
    {
        if (objectA == null || objectB == null || targetMaterial == null)
            return;

        float distance = Vector3.Distance(objectA.position, objectB.position);
        float targetValue = Mathf.Clamp01(distance / maxDistance);

        // Smoothly lerp the current value toward the target
        currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * lerpSpeed);

        targetMaterial.SetFloat(shaderPropertyName, currentValue);
    }
}
