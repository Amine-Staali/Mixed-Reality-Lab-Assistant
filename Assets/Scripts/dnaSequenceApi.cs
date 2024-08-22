using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;

[System.Serializable]
public class dnaResponse
{
    public string predicted_family;
}

public class dnaSequenceApi : MonoBehaviour
{
    public InputField InputdnaSequence;
    public Text dnaGeneFamily;
    public Button buttonGetADNSequence;
    private float delay = 5f;
    private string baseUrl = "";

    private void Start()
    {
        // Assigne la méthode GetDefinition au clic du bouton
        buttonGetADNSequence.onClick.AddListener(GetAnalysis);
    }

    private IEnumerator delayFunction(string dnaSequence)
    {
        dnaGeneFamily.text = "Predicted Gene Family:   Loading...";
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        StartCoroutine(FetchDnaGeneFamily(dnaSequence, baseUrl));
    }

    private void GetAnalysis()
    {
        string dnaSequence = InputdnaSequence.text;

        if (!string.IsNullOrEmpty(dnaSequence.Trim()))
        {
            StartCoroutine(delayFunction(dnaSequence));
        }
        else
        {
            dnaGeneFamily.text = "Please Enter a valid DNA Sequence";
        }
    }

    private IEnumerator FetchDnaGeneFamily(string dnaSequence, string baseUrl)
    {
        byte[] data = Encoding.UTF8.GetBytes("{\"sequence\": \"" + dnaSequence + "\", \"kmer_size\": 6}");

        using (UnityWebRequest request = new UnityWebRequest(baseUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(data);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Traite la réponse JSON ici
                string jsonResponse = request.downloadHandler.text;
                dnaResponse response = JsonUtility.FromJson<dnaResponse>(jsonResponse);

                // Affiche la première phrase de la définition du mot
                if (response != null && !string.IsNullOrEmpty(response.predicted_family))
                {
                    dnaGeneFamily.text = "Predicted Gene Family: " + response.predicted_family;
                }
                else
                {
                    dnaGeneFamily.text = "Not a valid DNA";
                }
            }
            else
            {
                dnaGeneFamily.text = $"Error: {request.responseCode} {request.error}";
            }
        }
    }

}