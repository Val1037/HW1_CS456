using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
public class distancecolor : MonoBehaviour
{
    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color selectColor = Color.green;

    [Header("References")]
    public GrabInteractable grabInteractable;
    public HandGrabInteractable handGrabInteractable;

    private Renderer _renderer;
    private bool _isSelected = false;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();

        if (grabInteractable != null)
            grabInteractable.WhenStateChanged += OnStateChanged;

        if (handGrabInteractable != null)
            handGrabInteractable.WhenStateChanged += OnStateChanged;
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
            grabInteractable.WhenStateChanged -= OnStateChanged;

        if (handGrabInteractable != null)
            handGrabInteractable.WhenStateChanged -= OnStateChanged;
    }

    private void OnStateChanged(InteractableStateChangeArgs args)
    {
        if(args.NewState == InteractableState.Select){
            _isSelected = true;
        }else{
            _isSelected = false;
        }
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (_isSelected)
            _renderer.material.color = selectColor;
        else if ((grabInteractable != null && grabInteractable.State == InteractableState.Hover)
              || (handGrabInteractable != null && handGrabInteractable.State == InteractableState.Hover))
            _renderer.material.color = hoverColor;
        else
            _renderer.material.color = normalColor;
    }
}
