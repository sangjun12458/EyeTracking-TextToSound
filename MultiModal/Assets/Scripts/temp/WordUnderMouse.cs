using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;

public class WordUnderMouse : MonoBehaviour //IPointerEnterHandler, IPointerExitHandler
{
    public Text displayText;

    private Vector2 mp;
    private Vector2 tp;
    private float textWidth;
    private float textHeight;

    private void Start()
    {
        RectTransform rectTransform = displayText.GetComponent<RectTransform>();
        textWidth = rectTransform.rect.width;
        textHeight = rectTransform.rect.height;
    }
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, Camera.main, out localMousePosition);

        Text textComponent = GetComponent<Text>();
        string[] words = textComponent.text.Split(' ');
        int index = GetWordIndexUnderMouse(localMousePosition, words);

        if (index >= 0 && index < words.Length)
        {
            string word = words[index];
            displayText.text = "Word under mouse: " + word;
            //Debug.Log("Word under mouse: " + word);
        }
        else
        {
            displayText.text = "";
        }

        mp = localMousePosition;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Mouse Pos: " + mp + ", " + "Text Pos: " + tp);
        }
    }

    int GetWordIndexUnderMouse(Vector2 localMousePosition, string[] words)
    {
        float totalWidth = -textWidth * 1.5f;
        for (int i = 0; i < words.Length; i++)
        {
            totalWidth += TextWidth(words[i]);
            if (localMousePosition.x < totalWidth)
            {
                tp.x = totalWidth;
                return i;
            }
        }

        return -1;
    }

    float TextWidth(string text)
    {
        TextGenerator textGenerator = new TextGenerator();
        TextGenerationSettings generationSettings = new TextGenerationSettings();
        generationSettings.generationExtents = new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height);
        generationSettings.textAnchor = TextAnchor.UpperLeft;
        generationSettings.pivot = GetComponent<RectTransform>().pivot;
        generationSettings.richText = false;
        generationSettings.font = GetComponent<Text>().font;
        generationSettings.fontSize = GetComponent<Text>().fontSize;
        generationSettings.fontStyle = GetComponent<Text>().fontStyle;
        generationSettings.lineSpacing = GetComponent<Text>().lineSpacing;
        generationSettings.scaleFactor = 1f;
        generationSettings.color = GetComponent<Text>().color;

        float width = textGenerator.GetPreferredWidth(text, generationSettings);
        return width;
    }
}
