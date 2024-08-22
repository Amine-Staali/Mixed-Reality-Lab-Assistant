using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System;

[System.Serializable]

public class echantillon_DNA : MonoBehaviour
{
    public string targetTag = "dna_sequencer";
    public Text dnaGeneFamily;
    private float delay = 5f;
    private string baseUrl = "";
    private string dnaSequence;

    private void Start()
    {
        if (tag == "dna_1")
        {
            dnaSequence = "ATGCCCCAACTAAATACTACCGTATGGCCCACCATAATTACCCCCATACTCCTTACACTATTCCTCATCACCCAACTAAAAATATTAAACACAAACTACCACCTACCTCCCTCACCAAAGCCCATAAAAATAAAAAATTATAACAAACCCTGAGAACCAAAATGAACGAAAATCTGTTCGCTTCATTCATTGCCCCCACAATCCTAG";
        }
        else if (tag == "dna_2")
        {
            dnaSequence = "ATGGCGGATTCCAGCGAAGGCCCCCGCGCGGGGCCCGGGGAGGTGGCTGAGCTCCCCGGGGATGAGAGTGGCACCCCAGGTGGGGAGGCTTTTCCTCTCTCCTCCCTGGCCAATCTGTTTGAGGGGGAGGATGGCTCCCTTTCGCCCTCACCGGCTGATGCCAGTCGCCCTGCTGGCCCAGGCGATGGGCGACCAAATCTGCGCATGAAGTTCCAGGGCGCCTTCCGCAAGGGGGTGCCCAACCCCATCGATCTGCTGGAGTCCACCCTATATGAGTCCTCGGTGGTGCCTGGGCCCAAGAAAGCACCCATGGACTCACTGTTTGACTACGGCACCTATCGTCACCACTCCAGTGACAACAAGAGGTGGAGGAAGAAGATCATAGAGAAGCAGCCGCAGAGCCCCAAAGCCCCTGCCCCTCAGCCGCCCCCCATCCTCAAAGTCTTCAACCGGCCTATCCTCTTTGACATCGTGTCCCGGGGCTCCACTGCTGACCTGGACGGGCTGCTCCCATTCTTGCTGACCCACAAGAAACGCCTAACTGATGAGGAGTTTCGAGAGCCATCTACGGGGAAGACCTGCCTGCCCAAGGCCTTGCTGAACCTGAGCAATGGCCGCAACGACACCATCCCTGTGCTGCTGGACATCGCGGAGCGCACCGGCAACATGAGGGAGTTCATTAACTCGCCCTTCCGTGACATCTACTATCGAGGTCAGACAGCCCTGCACATCGCCATTGAGCGTCGCTGCAAACACTACGTGGAACTTCTCGTGGCCCAGGGAGCTGATGTCCACGCCCAGGCCCGTGGGCGCTTCTTCCAGCCCAAGGATGAGGGGGGCTACTTCTACTTTGGGGAGCTGCCCCTGTCGCTGGCTGCCTGCACCAACCAGCCCCACATTGTCAACTACCTGACGGAGAACCCCCACAAGAAGGCGGACATGCGGCGCCAGGACTCGCGAGGCAACACAGTGCTGCATGCGCTGGTGGCCATTGCTGACAACACCCGTGAGAACACCAAGTTTGTTACCAAGATGTACGACCTGCTGCTGCTCAAGTGTGCCCGCCTCTTCCCCGACAGCAACCTGGAGGCCGTGCTCAACAACGACGGCCTCTCGCCCCTCATGATGGCTGCCAAGACGGGCAAGATTGGGATCTTTCAGCACATCATCCGGCGGGAGGTGACGGATGAGGACACACGGCACCTGTCCCGCAAGTTCAAGGACTGGGCCTATGGGCCAGTGTATTCCTCGCTTTATGACCTCTCCTCCCTGGACACGTGTGGGGAAGAGGCCTCCGTGCTGGAGATCCTGGTGTACAACAGCAAGATTGAGAACCGCCACGAGATGCTGGCTGTGGAGCCCATCAATGAACTGCTGCGGGACAAGTGGCGCAAGTTCGGGGCCGTCTCCTTCTACATCAACGTGGTCTCCTACCTGTGTGCCATGGTCATCTTCACTCTCACCGCCTACTACCAGCCGCTGGAGGGCACACCGCCGTACCCTTACCGCACCACGGTGGACTACCTGCGGCTGGCTGGCGAGGTCATTACGCTCTTCACTGGGGTCCTGTTCTTCTTCACCAACATCAAAGACTTGTTCATGAAGAAATGCCCTGGAGTGAATTCTCTCTTCATTGATGGCTCCTTCCAGCTGCTCTAG";
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the target object
        if (collision.gameObject.CompareTag(targetTag))
        {

            // Call the function
            StartCoroutine(delayFunction());
        }
    }

    private IEnumerator delayFunction()
    {
        dnaGeneFamily.text = "Predicted Family:   Loading...";

        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        
        // Call the function after the delay
        GetAnalysis();
       
    }

    private void GetAnalysis()
    {
        if (!string.IsNullOrEmpty(dnaSequence))
        {
            StartCoroutine(FetchDnaGeneFamily(dnaSequence, baseUrl));
        }
        else
        {
            dnaGeneFamily.text = "Predicted Family: ";
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
                    dnaGeneFamily.text = "Predicted Family:   " + response.predicted_family;
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