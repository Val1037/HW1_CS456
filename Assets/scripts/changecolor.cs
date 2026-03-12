using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class changecolor : MonoBehaviour
{
    public MeshRenderer rend;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;

    public Color norm = Color.blue;
    public Color diff = Color.white;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        rend.material.color = norm;
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();

        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
    }

    void OnHoverEnter(HoverEnterEventArgs args)
    {
        rend.material.color = diff;
    }

    void OnHoverExit(HoverExitEventArgs args)
    {
        rend.material.color = norm;
    }
}
