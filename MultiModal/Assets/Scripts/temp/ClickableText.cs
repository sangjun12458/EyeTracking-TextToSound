using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickableText : MonoBehaviour
{
    //private TextMeshProUGUI textField;
    private TextMeshPro _textMeshPro;
    
    void Start()
    {
        // 스크립트가 연결된 GameObject의 자식 중에 TextMeshProUGUI 컴포넌트를 찾아 할당합니다.
        //textField = GetComponentInChildren<TextMeshProUGUI>();
        _textMeshPro = gameObject.GetComponent<TextMeshPro>();

        if (_textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI component not found! Make sure it exists in children of this GameObject.");
        }
    }
    void Update()
    {
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //    _textMeshPro.rectTransform,
        //    clickPosition,
        //    Camera.main,
        //    out Vector2 localPoint
        //);
        Vector2 clickPosition = Input.mousePosition;
        int index = TMP_TextUtilities.FindIntersectingWord(_textMeshPro, clickPosition, Camera.main);
        if (index != -1)
        {
            string clickedText = _textMeshPro.textInfo.wordInfo[index].GetWord();
            Debug.Log("Clicked Text: " + clickedText);
        }
    }
}
