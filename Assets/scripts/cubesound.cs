using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
public class cubesound : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip proximityClip;
    public float maxHearingDistance = 0.5f; // meters — tune this to your liking

    [Header("References")]
    public GrabInteractable grabInteractable;
    public HandGrabInteractable handGrabInteractable;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = proximityClip;
        _audioSource.loop = true;
        _audioSource.spatialBlend = 1f; // 3D sound
        _audioSource.volume = 0f;
        _audioSource.Play();
    }

    void Update()
    {
        // Check both interactables for hover or select
        bool isNear = false;

        if (grabInteractable != null)
            isNear |= grabInteractable.State == InteractableState.Hover
                   || grabInteractable.State == InteractableState.Select;

        if (handGrabInteractable != null)
            isNear |= handGrabInteractable.State == InteractableState.Hover
                   || handGrabInteractable.State == InteractableState.Select;

        // Smoothly fade volume in/out
        float targetVolume = isNear ? 1f : 0f;
        _audioSource.volume = Mathf.Lerp(_audioSource.volume, targetVolume, Time.deltaTime * 5f);
    }
}
