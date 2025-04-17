using UnityEngine;

public class DistanceShaderLerp : MonoBehaviour
{
    public Transform objectA;
    public Transform objectB;
    public Material targetMaterial;
    public string shaderPropertyName = "_EffectAmount";
    public float maxDistance = 5f;
    public float lerpSpeed = 2f; // how fast it eases to the value

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip proximityClip;
    public float audioFadeSpeed = 0.5f;
    public float minPlaybackSpeed = 0.5f; // Slowest playback speed
    public float maxPlaybackSpeed = 2f; // Fastest playback speed
    public float minVolume = 0.2f;
    public float maxVolume = 1f;

    private float currentValue = 0f;

    void Update()
    {
        if (objectA == null || objectB == null || targetMaterial == null)
            return;

        float distance = Vector3.Distance(objectA.position, objectB.position);
        float targetValue = Mathf.Clamp01(distance / maxDistance);

        // Smoothly lerp the current value toward the target
        currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * lerpSpeed);
        targetMaterial.SetFloat(shaderPropertyName, currentValue);

        // Map proximity (inverted value) to audio playback speed and volume
        float proximity = 1f - currentValue;

        // Audio response
        if (proximity > 0.05f)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = proximityClip;
                audioSource.loop = true;
                audioSource.Play();
            }

            // Adjust volume based on proximity
            audioSource.volume = Mathf.Lerp(audioSource.volume, Mathf.Lerp(minVolume, maxVolume, proximity), Time.deltaTime * audioFadeSpeed);

            // Adjust playback speed instead of pitch
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, Mathf.Lerp(minPlaybackSpeed, maxPlaybackSpeed, proximity), Time.deltaTime * audioFadeSpeed);
        }
        else
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, Time.deltaTime * audioFadeSpeed);
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, minPlaybackSpeed, Time.deltaTime * audioFadeSpeed);

            /*if (audioSource.volume <= 0.01f)
            {
                audioSource.Stop();
                audioSource.volume = 0f;
            }*/
        }
    }
}
