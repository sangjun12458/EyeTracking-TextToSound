using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class MouseOverWord : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public string[] targetWords;

    void Start()
    {
        // Get the text info
        TMP_TextInfo textInfo = textMeshPro.textInfo;

        foreach (string word in targetWords)
        {
            // Find the index of the word
            int wordIndex = textMeshPro.text.IndexOf(word);

            if (wordIndex != -1)
            {
                // Find the first character index of the word
                int startIndex = textMeshPro.textInfo.wordInfo[wordIndex].firstCharacterIndex;
                // Find the last character index of the word
                int endIndex = textMeshPro.textInfo.wordInfo[wordIndex].lastCharacterIndex;

                // Get the positions of the word's start and end characters
                Vector3 startWorldPos = textMeshPro.transform.TransformPoint(textInfo.characterInfo[startIndex].bottomLeft);
                Vector3 endWorldPos = textMeshPro.transform.TransformPoint(textInfo.characterInfo[endIndex].topRight);

                // Display the positions
                Debug.Log("Position of '" + word + "': Start - " + startWorldPos + ", End - " + endWorldPos);
            }
            else
            {
                Debug.LogWarning("Word '" + word + "' not found in the text.");
            }
        }
    }
}
