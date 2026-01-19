using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class penCode : MonoBehaviour
{
    [Header("Pen Properties")]
    public Transform tip;
    public Material drawingMaterial;
    public Material tipMaterial;
    [Range(0.01f, 0.1f)]
    public float penWidth = 0.01f;

    public Color[] penColors;

    [Header("XR")]
    public XRGrabInteractable grabInteractable;

    public InputActionProperty rightTrigger;   // Value 0–1
    public InputActionProperty leftTrigger;    // Value 0–1

    private LineRenderer currentDrawing;
    private int index;
    private int currentColorIndex;

    void Start()
    {
        currentColorIndex = 0;
        tipMaterial.color = penColors[currentColorIndex];
    }

    void Update()
    {
        bool isGrabbed = grabInteractable.isSelected;

        // Use interactorsSelecting to get the current selecting interactor
        var selectingInteractor = (grabInteractable.interactorsSelecting != null && grabInteractable.interactorsSelecting.Count > 0)
            ? grabInteractable.interactorsSelecting[0] : null;

        bool isRightHandDrawing =
            isGrabbed &&
            selectingInteractor != null &&
            selectingInteractor.transform.name.Contains("Right") &&
            rightTrigger.action.ReadValue<float>() > 0.1f;

        bool isLeftHandDrawing =
            isGrabbed &&
            selectingInteractor != null &&
            selectingInteractor.transform.name.Contains("Left") &&
            leftTrigger.action.ReadValue<float>() > 0.1f;

        if (isRightHandDrawing || isLeftHandDrawing)
        {
            Draw();
        }
        else if (currentDrawing != null)
        {
            currentDrawing = null;
        }

        if (Keyboard.current != null && Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SwitchColor();
        }
    }

    private void Draw()
    {
        if (currentDrawing == null)
        {
            index = 0;

            GameObject lineObj = new GameObject("DrawingLine");
            currentDrawing = lineObj.AddComponent<LineRenderer>();

            currentDrawing.material = drawingMaterial;
            currentDrawing.startColor = currentDrawing.endColor = penColors[currentColorIndex];
            currentDrawing.startWidth = currentDrawing.endWidth = penWidth;
            currentDrawing.positionCount = 1;
            currentDrawing.SetPosition(0, tip.position);
        }
        else
        {
            Vector3 lastPos = currentDrawing.GetPosition(index);

            if (Vector3.Distance(lastPos, tip.position) > 0.014f)
            {
                index++;
                currentDrawing.positionCount = index + 1;
                currentDrawing.SetPosition(index, tip.position);
            }
        }
    }

    private void SwitchColor()
    {
        if (currentColorIndex == penColors.Length - 1)
        {
            currentColorIndex = 0;
        }
        else
        {
            currentColorIndex++;
        }

        tipMaterial.color = penColors[currentColorIndex];
    }
}