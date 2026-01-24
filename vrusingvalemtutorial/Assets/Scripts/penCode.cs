using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class penCode : MonoBehaviour
{
    [Header("Pen Properties")]
    public Transform tip;
    public Material drawingMaterial;
    public Material tipMaterial;
    public float penWidth = 0.01f;
    public Color[] penColors;

    [Header("XR")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    public InputActionProperty rightTrigger;
    public InputActionProperty leftTrigger;

    private LineRenderer currentLine;
    private int index;
    private int colorIndex;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor grabbingInteractor;

    void Start()
    {
        colorIndex = 0;
        tipMaterial.color = penColors[colorIndex];

        rightTrigger.action.Enable();
        leftTrigger.action.Enable();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        grabbingInteractor = args.interactorObject;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        grabbingInteractor = null;
        currentLine = null;
    }

    void Update()
    {
        if (grabbingInteractor == null)
            return;

        string name = grabbingInteractor.transform.name.ToLower();

        bool isRightHand =
            name.Contains("right") &&
            rightTrigger.action.ReadValue<float>() > 0.1f;

        bool isLeftHand =
            name.Contains("left") &&
            leftTrigger.action.ReadValue<float>() > 0.1f;

        if (isRightHand || isLeftHand)
        {
            Draw();
        }
        else
        {
            currentLine = null;
        }

        if (Keyboard.current != null && Keyboard.current.digit1Key.wasPressedThisFrame)
            SwitchColor();
    }

    private void Draw()
    {
        if (currentLine == null)
        {
            index = 0;

            GameObject lineObj = new GameObject("DrawingLine");
            currentLine = lineObj.AddComponent<LineRenderer>();

            currentLine.material = drawingMaterial;
            currentLine.startColor = currentLine.endColor = penColors[colorIndex];
            currentLine.startWidth = currentLine.endWidth = penWidth;
            currentLine.positionCount = 1;
            currentLine.SetPosition(0, tip.position);
        }
        else
        {
            Vector3 lastPos = currentLine.GetPosition(index);

            if (Vector3.Distance(lastPos, tip.position) > 0.014f)
            {
                index++;
                currentLine.positionCount = index + 1;
                currentLine.SetPosition(index, tip.position);
            }
        }
    }

    private void SwitchColor()
    {
        colorIndex = (colorIndex + 1) % penColors.Length;
        tipMaterial.color = penColors[colorIndex];
    }
}
