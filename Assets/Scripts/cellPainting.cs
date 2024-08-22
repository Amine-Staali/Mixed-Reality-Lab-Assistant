using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class cellPainting : MonoBehaviour
{
    public string targetTag = "microscope";
    public RawImage cell;      // First image object
    public RawImage newCell;   // Second image object
    private float delay = 2f;  // Delay between showing the images
    private bool firstCollision = true;  // Tracks if this is the first collision

    private void Start()
    {
        Debug.Log("Start method called!");
        cell.gameObject.SetActive(false);    // Hide the first image at the start
        newCell.gameObject.SetActive(false); // Hide the second image at the start
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            StartCoroutine(DelayFunction());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            cell.gameObject.SetActive(false);
            newCell.gameObject.SetActive(false);
        }
    }

    private IEnumerator DelayFunction()
    {
        yield return new WaitForSeconds(delay);
        ShowImage();
    }

    private void ShowImage()
    {
        if (firstCollision)
        {
            cell.gameObject.SetActive(true); // Show the first image on the first collision
            firstCollision = false;
        }
        else
        {
            cell.gameObject.SetActive(false); // Hide the first image
            newCell.gameObject.SetActive(true); // Show the second image on the second collision
        }
    }
}
