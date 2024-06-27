using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEditor;

public class FileReader : MonoBehaviour
{
    private List<string> words;
    private List<string> sentences;
    private List<string> soundPaths;
    private int currentPage = 0;
    private int totalCharacters = 0;

    //public Text textDisplay;
    //public TextMeshProUGUI textDisplay;
    //public TextMeshPro textMesh;
    public TextMeshProUGUI textMesh;
    public int fontSize = 20;
    public int linesPerPage = 30;
    public int charactersPerPage = 200;
    public float lineSpacing = 1.5f;
    public float characterSpacing = 1.5f;

    private string[] currentWords;
    private Vector3[] wordPositions;
    public int wordIndex = 0;
    private Camera mainCamera; // 메인 카메라

    public InputField inputField;
    public Slider slider;
    private int intValue;

    static public int targetIndex;
    static public string originalText;

    void Start()
    {
        mainCamera = Camera.main;
        //LoadTextFromFile("Assets/Texts/TheLittlePrince.txt");
        SimpleLoadTextFromFile("Assets/Texts/TheLittlePrince_refine.txt");
        LoadSoundpathsFromFile("Assets/Texts/TheLittlePrince_sounds.txt");
        TotalCharacters();
        ShowCurrentPage();

        StartCoroutine(WaitForTextLayoutAndCalculate());

        textMesh.fontSize = fontSize;
        textMesh.lineSpacing = lineSpacing;
        textMesh.characterSpacing = characterSpacing;

        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        slider.value = charactersPerPage;
    }

    private void Update()
    {
        // 이전 페이지로 이동
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ShowPreviousPage();
        }
        // 다음 페이지로 이동
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ShowNextPage();
        }
        // charactersPerPage가 변경될 때마다 현재 페이지를 업데이트
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            int newCharactersPerPage;
            if (int.TryParse(inputField.text, out newCharactersPerPage))
            {
                charactersPerPage = newCharactersPerPage;
                ShowCurrentPage();
            }
        }

    }

    void SimpleLoadTextFromFile(string fileName)
    {
        sentences = new List<string>();
        string path = fileName;
        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            sentences.Add(line + ".");
        }
        reader.Close();
    }
    void LoadTextFromFile(string fileName)
    {
        sentences = new List<string>();
        //string path = Path.Combine(Application.streamingAssetsPath, fileName);
        string path = fileName;
        StreamReader reader = new StreamReader(path);
        string currentSentence = "";
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            int dotIndex = line.IndexOf('.');
            while (dotIndex != -1)
            {
                currentSentence += line.Substring(0, dotIndex + 1).Trim();
                sentences.Add(currentSentence);
                currentSentence = "";
                line = line.Substring(dotIndex + 1).Trim();
                dotIndex = line.IndexOf('.');
            }
            if (!string.IsNullOrEmpty(line))
            {
                currentSentence += line + " ";
            }
            else
            {
                currentSentence += line.Trim() + " ";
            }

        }
        reader.Close();
    }
    void LoadSoundpathsFromFile(string fileName)
    {
        soundPaths = new List<string>();
        string path = fileName;
        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            int lastIndex = line.LastIndexOf('/');
            line = line.Substring(lastIndex + 1);
            soundPaths.Add(line);
        }
        reader.Close();
    }
    void LoadSounds()
    {

    }

    void ShowCurrentPage()
    {
        textMesh.text = "";
        int startIndex = currentPage * charactersPerPage;

        int totalCharacters = 0;
        int sentenceIndex = 0;

        // 현재 페이지의 시작 위치를 찾음
        while (sentenceIndex < sentences.Count && totalCharacters < startIndex)
        {
            totalCharacters += sentences[sentenceIndex].Length + 1; // 현재 문장의 길이와 공백을 더하여 총 문자 수를 계산
            sentenceIndex++;
        }

        targetIndex = sentenceIndex;
        // 현재 페이지의 텍스트를 표시함
        while (sentenceIndex < sentences.Count && totalCharacters < startIndex + charactersPerPage)
        {
            textMesh.text += sentences[sentenceIndex] + " ";
            totalCharacters += sentences[sentenceIndex].Length + 1; // 현재 문장의 길이와 공백을 더하여 총 문자 수를 계산
            sentenceIndex++;
        }

        originalText = textMesh.text;
    }

    public void ShowPreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowCurrentPage();
        }
    }

    public void ShowNextPage()
    {
        int maxPage = Mathf.CeilToInt((float)totalCharacters / (float)charactersPerPage) - 1;
        //Debug.Log(sentences.Count + ", " + charactersPerPage);
        if (currentPage < maxPage)
        {
            currentPage++;
            ShowCurrentPage();
        }
    }
    //if (!string.IsNullOrEmpty(line))
    //{
    //    currentSentence += line.Trim();
    //    if (!line.EndsWith("."))
    //        continue;
    //}
    //sentences.Add(currentSentence);
    //currentSentence = "";
    //string[] splitSentences = line.Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries);
    //string[] splitSentences = Regex.Split(line, @"(?<=[.!?])\s+"); 
    //foreach (string sentence in splitSentences)
    //{
    //    sentences.Add(sentence.Trim() + ".");
    //}
    void TotalCharacters()
    {
        foreach (string sentence in sentences)
        {
            totalCharacters += sentence.Length;
        }
    }

    void OnInputFieldValueChanged(string newValue)
    {
        // 사용자의 입력을 정수로 변환하여 intValue 변수에 저장합니다.
        if (int.TryParse(newValue, out intValue))
            charactersPerPage = intValue;
        else
            Debug.Log("Invalid Input: " + newValue); // 잘못된 입력이 있을 경우 경고 메시지를 출력합니다.
    }
    // 슬라이더 값이 변경될 때 호출되는 함수입니다.
    public void OnSliderValueChanged(float newValue)
    {
        // 슬라이더의 값이 변경될 때마다 정수로 변환하여 변수에 저장합니다.
        charactersPerPage = (int)newValue;
    }


    IEnumerator WaitForTextLayoutAndCalculate()
    {
        yield return new WaitUntil(() => textMesh.textInfo.lineCount > 0);

        CalculateWordPositions();
    }
    void CalculateWordPositions()
    {
        currentWords = textMesh.text.Split(' ');
        wordPositions = new Vector3[currentWords.Length];

        //int charactersPerLine = textMesh.maxVisibleCharacters / textMesh.textInfo.lineCount;
        //int totalCharacters = 0;
        float totalWidth = 0f;

        for (int i = 0; i < currentWords.Length; i++)
        {
            totalWidth += (currentWords[i].Length + 1) * (fontSize / 10 + characterSpacing);
            float currentWidth = totalWidth % 512;
            int currentLine = (int)(totalWidth / 512);
            float currentHeight = currentLine * (fontSize / 10 + lineSpacing);
            Vector3 textOffset = new Vector3(-256, 128, 0);
            Vector3 canvasOffset = textMesh.transform.position;
            Vector3 wordPosition = new Vector3(currentWidth, currentHeight, 0);
            wordPosition += textOffset;
            wordPosition /= 100;
            wordPosition += canvasOffset;
            wordPositions[i] = mainCamera.WorldToScreenPoint(wordPosition);
        }
    }
    Vector3 CalculateWordPosition(int wordIndex)
    {
        Vector3 startPosition = textMesh.transform.position;
        float fontSize = textMesh.fontSize;
        float lineSpacing = textMesh.lineSpacing;
        float characterSpacing = textMesh.characterSpacing;
        int charactersPerLine = textMesh.maxVisibleCharacters / textMesh.textInfo.lineCount;

        int wordsPerLine = charactersPerLine / currentWords.Length;
        int lineIndex = wordIndex / wordsPerLine;
        int wordIndexInLine = wordIndex % wordsPerLine;

        float wordWidth = currentWords[wordIndex].Length * (fontSize + characterSpacing);
        float wordHeight = fontSize + lineSpacing;

        float xOffset = wordIndexInLine * (wordWidth + characterSpacing);
        float yOffset = lineIndex * wordHeight;

        Vector3 wordPosition = startPosition + new Vector3(xOffset, -yOffset, 0f);
        return mainCamera.WorldToScreenPoint(wordPosition);
    }

    private void PrintWordPosition(int index)
    {
        if (index >= 0 && index < words.Count)
        {
            string word = words[index];
            Debug.Log("Word: " + word + ", Position: " + wordPositions[index]);
        }
        else
        {
            Debug.LogWarning("Invalid index.");
        }
    }

    private void FindWord()
    {
        Vector3 mousePosition = Input.mousePosition;
        
        float minDistance = float.MaxValue;
        Vector3 closestWordPosition = Vector3.zero;

        int idx = 0;
        for (int i = 0; i < wordPositions.Length; i++)
        {
            Vector3 wordPosition = wordPositions[i];
            float distance = Vector3.Distance(mousePosition, wordPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestWordPosition = wordPosition;
                idx = i;
            }
        }

        Debug.Log("MousePos: " + mousePosition + ", WordPos: " + closestWordPosition + ", Word: " + currentWords[idx]);
    }
}
