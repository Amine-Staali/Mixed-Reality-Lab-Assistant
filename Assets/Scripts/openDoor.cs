using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class openDoor : MonoBehaviour, IMixedRealityPointerHandler
{
    private bool isHolding = false;
    private Vector3 initialRotation;
    private Vector3 initialPosition;
    public float openAngle = -92f;
    public float rotationSpeed = 10.0f;
    private float currentAngle = 0f;
    Vector3 targetRotation;
    float dragAmount;
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
            if (tag == "exp1" || tag == "exp2")
            {
                // cell painting (Réfrigérateur) & analyse d'une séquence ADN (Réfrigérateur)
                if(tag == "exp1")
                {
                    dragAmount = -eventData.Pointer.Rotation.w * rotationSpeed;
                }else if(tag == "exp2")
                {
                    dragAmount = eventData.Pointer.Rotation.w * rotationSpeed;
                }
                currentAngle = Mathf.Clamp(currentAngle + dragAmount, openAngle, 0);
                targetRotation = initialRotation + new Vector3(0, currentAngle, 0);
                transform.rotation = Quaternion.Euler(targetRotation);
                transform.position = initialPosition;

            }
            else if (tag == "dna_sequencer")
            {
                // séquenceur ADN
                dragAmount = -eventData.Pointer.Rotation.y * 1.0f;
                currentAngle = Mathf.Clamp(currentAngle + dragAmount, openAngle, 0);
                targetRotation = initialRotation + new Vector3(currentAngle, 0, 0);
                transform.rotation = Quaternion.Euler(targetRotation);
                transform.position = initialPosition;
            }
        }
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        // When pointer up (i.e., holding stops)
        isHolding = false;
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData) { }
}
