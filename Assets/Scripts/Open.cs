using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System;

public class Open : MonoBehaviour
{
    private bool isOpen = false;
    private Vector3 initialRotation;
    private Vector3 initialPosition;
    public float openAngleExp = -92f;
    public float openAngleDna = -90f;
    public float rotationSpeed = 10.0f;
    public Text alert;
    public GameObject Panel;
    private float delay = 5f;

    void Start()
    {
        initialRotation = transform.rotation.eulerAngles;
        initialPosition = transform.position;
        Panel.SetActive(false);
    }

    private IEnumerator ShowAlertIfDoorIsOpen()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);


        // Call the function after the delay
        if (isOpen) // Check if the door is still open
        {
            Panel.SetActive(true);
            alert.text = "Close The Door !";
        }

    }

    public void OnButtonPressed()
    {
        print("Pressed");
        if (isOpen)
        {
            // Close the door
            StartCoroutine(RotateDoor(initialRotation));
            alert.text = "";
            Panel.SetActive(false);
        }
        else
        {
            // Open the door based on tag
            if (tag == "exp1" || tag == "exp2")
            {
                print("Pressed exp");
                Vector3 targetRotation = initialRotation + new Vector3(0, openAngleExp, 0);
                StartCoroutine(RotateDoor(targetRotation));
            }
            else if (tag == "dna_sequencer")
            {
                print("Pressed DNA");
                Vector3 targetRotation = initialRotation + new Vector3(openAngleDna, 0, 0);
                StartCoroutine(RotateDoor(targetRotation));
            }
            StartCoroutine(ShowAlertIfDoorIsOpen());
        }

        // Toggle the door state
        isOpen = !isOpen;
    }

    private IEnumerator RotateDoor(Vector3 targetRotation)
    {
        Quaternion targetQuaternion = Quaternion.Euler(targetRotation);
        while (Quaternion.Angle(transform.rotation, targetQuaternion) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, Time.deltaTime * rotationSpeed);
            yield return null;
        }
        transform.rotation = targetQuaternion; // Ensure final position is exact
    }
}

