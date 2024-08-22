using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class open_dna_sequencer : MonoBehaviour, IMixedRealityPointerHandler
{
    private bool isHolding = false;
    private Vector3 initialRotation;
    private Vector3 initialPosition;
    public float openAngle = -90f;
    public float rotationSpeed = 1.0f;
    private float currentAngle = 0f;
    
    void Start()
    {
        initialRotation = transform.rotation.eulerAngles;
        initialPosition = transform.position;
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        // When pointer down (i.e., holding starts)
        isHolding = true;
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        if (isHolding)
        {
            // Calculate the rotation based on the drag amount
            float dragAmount = -eventData.Pointer.Rotation.y * rotationSpeed;
            currentAngle = Mathf.Clamp(currentAngle + dragAmount, openAngle, 0);
            Vector3 targetRotation = initialRotation + new Vector3(currentAngle, 0, 0);
            transform.rotation = Quaternion.Euler(targetRotation);
            transform.position = initialPosition;
        }
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        // When pointer up (i.e., holding stops)
        isHolding = false;
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData) { }
}
