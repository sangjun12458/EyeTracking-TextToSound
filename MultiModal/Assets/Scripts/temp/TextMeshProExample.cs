using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshProExample : MonoBehaviour
{
    public TMP_Text textMeshPro; // Text Mesh Pro 텍스트 객체 참조

    void Start()
    {
        // 텍스트 설정
        textMeshPro.text = "Hello World! This is a Text Mesh Pro Example.";

        // 각 단어에 대한 메시 생성 및 조작
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        for (int i = 0; i < textInfo.wordCount; i++)
        {
            TMP_WordInfo wordInfo = textInfo.wordInfo[i];

            // 각 단어의 메시 생성
            Vector3[] vertices = textInfo.meshInfo[wordInfo.firstCharacterIndex].vertices;
            int vertexIndex = wordInfo.firstCharacterIndex * 4;

            // 각 단어의 메시 조작 예시: 단어의 첫 글자 위치 이동
            vertices[vertexIndex] += new Vector3(0.5f, 0f, 0f); // X축으로 0.5만큼 이동
            vertices[vertexIndex + 1] += new Vector3(0.5f, 0f, 0f);
            vertices[vertexIndex + 2] += new Vector3(0.5f, 0f, 0f);
            vertices[vertexIndex + 3] += new Vector3(0.5f, 0f, 0f);
        }

        // 메시 업데이트
        textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 위치로부터 Ray 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Ray와 충돌한 객체 확인
            if (Physics.Raycast(ray, out hit))
            {
                // 충돌한 객체가 Text Mesh Pro 텍스트인지 확인
                TMP_Text hitText = hit.collider.GetComponent<TMP_Text>();
                if (hitText != null && hitText == textMeshPro)
                {
                    // 마우스 위치에서 가장 가까운 문자의 인덱스 계산
                    int charIndex = TMP_TextUtilities.FindIntersectingCharacter(hitText, hit.point, Camera.main, false);

                    // 인덱스를 기반으로 해당 문자가 속한 단어의 범위 찾기
                    TMP_WordInfo wordInfo = default(TMP_WordInfo); // 기본값으로 초기화

                    TMP_TextInfo textInfo = hitText.textInfo;
                    for (int i = 0; i < textInfo.wordCount; i++)
                    {
                        TMP_WordInfo currentWord = textInfo.wordInfo[i];
                        if (charIndex >= currentWord.firstCharacterIndex && charIndex < currentWord.firstCharacterIndex + currentWord.characterCount)
                        {
                            wordInfo = currentWord;
                            break;
                        }
                    }

                    // 단어 처리
                    string clickedWord = hitText.text.Substring(wordInfo.firstCharacterIndex, wordInfo.characterCount);
                    Debug.Log("Clicked Word: " + clickedWord);
                    // 여기에서 단어 클릭에 대한 추가 작업을 수행할 수 있습니다.
                }
            }
        }
    }
}
