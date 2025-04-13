using UnityEngine;

public class IRShaderController : MonoBehaviour
{
    public Material targetMaterial;
    public string shaderPropertyName = "_EffectAmount";
    public float lerpSpeed = 2f; // how fast it eases to the value
    public IRInterface Kumar;
    bool irstate = true;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip irClip;
    public float audioFadeSpeed = 2f;
    public float maxVolume = 1f;

    float currentValue = 0f;
    float targetValue = 1f;
    void Update()
    {

        irstate = Kumar.IRStateChange();


        if (irstate)
        {
            targetValue = 0f;  // IR detected
        }
        else
        {
            targetValue = 1f;  // IR not detected
        }

        currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * lerpSpeed);
        targetMaterial.SetFloat(shaderPropertyName, currentValue);

        if (!irstate)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = irClip;
                audioSource.Play();
            }

            // Fade volume in
            audioSource.volume = Mathf.Lerp(audioSource.volume, maxVolume*2, Time.deltaTime * audioFadeSpeed);
        }
        else
        {
            // Fade volume out
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, Time.deltaTime * audioFadeSpeed);

            if (audioSource.volume <= 0.01f)
            {
                audioSource.Stop();
                audioSource.volume = 0f; // reset fully
            }
        }
    }
}
