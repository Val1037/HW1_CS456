using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;
using TMPro;
public class gestureDetector : MonoBehaviour
{
     [Header("XR Hand Subsystem")]
    private XRHandSubsystem handSubsystem;

    [Header("UI")]
    [SerializeField] public TextMeshPro gestureText; // Drag your TextMeshPro object here
    public Transform cameraTransform; // Assign your XR camera here (for facing text)

    [Header("Thresholds")]
    public float fingerCurlThreshold = 0.06f;
    public float thumbExtendedThreshold = 0.07f;
    public float thumbUpDotThreshold = 0.7f;

    void Start()
    {
        // Get XRHandSubsystem
        handSubsystem = XRGeneralSettings.Instance
            .Manager
            .activeLoader
            .GetLoadedSubsystem<XRHandSubsystem>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (handSubsystem == null || gestureText == null) return;

        string display = "";

        display += ProcessHand(handSubsystem.leftHand, "Left");
        display += ProcessHand(handSubsystem.rightHand, "Right");

        gestureText.text = display;

        // Make the text always face the camera
        if (cameraTransform != null)
            gestureText.transform.rotation = Quaternion.LookRotation(gestureText.transform.position - cameraTransform.position);
    }

    string ProcessHand(XRHand hand, string handName)
    {
        if (!hand.isTracked) return "";

        bool isFist = IsFist(hand);
        bool isThumbsUp = IsThumbsUp(hand);

        string result = "";

        if (isFist)
        {
            result = $"<color=black>{handName} FIST</color>\n";
        }
        else if (isThumbsUp)
        {
            result = $"<color=black>{handName} THUMBS UP</color>\n";
        }

        return result;
    }

    // --- Fist Detection ---
    bool IsFist(XRHand hand)
    {
        return IsFingerCurled(hand, XRHandJointID.IndexTip) &&
               IsFingerCurled(hand, XRHandJointID.MiddleTip) &&
               IsFingerCurled(hand, XRHandJointID.RingTip) &&
               IsFingerCurled(hand, XRHandJointID.LittleTip) &&
               IsFingerCurled(hand, XRHandJointID.ThumbTip);
    }

    // --- Thumbs-Up Detection ---
    bool IsThumbsUp(XRHand hand)
    {
        // Other fingers curled
        bool fingersCurled =
            IsFingerCurled(hand, XRHandJointID.IndexTip) &&
            IsFingerCurled(hand, XRHandJointID.MiddleTip) &&
            IsFingerCurled(hand, XRHandJointID.RingTip) &&
            IsFingerCurled(hand, XRHandJointID.LittleTip);

        if (!fingersCurled) return false;

        // Thumb joints
        var thumbTip = hand.GetJoint(XRHandJointID.ThumbTip);
        var thumbBase = hand.GetJoint(XRHandJointID.ThumbMetacarpal);

        if (!thumbTip.TryGetPose(out Pose tipPose) ||
            !thumbBase.TryGetPose(out Pose basePose))
            return false;

        // Thumb extended
        float thumbLength = Vector3.Distance(tipPose.position, basePose.position);
        if (thumbLength < thumbExtendedThreshold) return false;

        // Thumb pointing upward in WORLD space
        Vector3 thumbDir = (tipPose.position - basePose.position).normalized;
        float dot = Vector3.Dot(thumbDir, Vector3.up);

        return dot > thumbUpDotThreshold;
    }
     bool IsFingerCurled(XRHand hand, XRHandJointID tipID)
    {
        var tip = hand.GetJoint(tipID);
        var palm = hand.GetJoint(XRHandJointID.Palm);

        if (!tip.TryGetPose(out Pose tipPose) || !palm.TryGetPose(out Pose palmPose))
            return false;

        float distance = Vector3.Distance(tipPose.position, palmPose.position);
        return distance < fingerCurlThreshold;
    }
}
