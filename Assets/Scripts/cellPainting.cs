using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class cellPainting : MonoBehaviour
{
    public string targetTag = "microscope";
    public RawImage cell;
    public RawImage newCell;
    private float delay = 2f;
    private bool firstCollision = true;

    private void Start()
    {
        Debug.Log("Start method called!");
        cell.gameObject.SetActive(false);
        newCell.gameObject.SetActive(false);
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
            cell.gameObject.SetActive(true);
            firstCollision = false;
        }
        else
        {
            cell.gameObject.SetActive(false);
            newCell.gameObject.SetActive(true);
        }
    }
}
