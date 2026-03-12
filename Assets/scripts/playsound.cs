using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class playsound : MonoBehaviour
{
    private AudioSource sound;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Awake()
    {
        sound = GetComponent<AudioSource>();
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

        interactable.hoverEntered.AddListener(OnHoverEnter);
    }

    void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (!sound.isPlaying)
        {
            sound.Play();
        }
    }

    void onHoverExit(HoverExitEventArgs args)
    {
        sound.Stop();
    }
}
