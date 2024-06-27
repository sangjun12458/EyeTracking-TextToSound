using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Net.Sockets;
using System.IO;
using System.Text;
//using System.Drawing;

public class InteractibleText : MonoBehaviour
{
    private TextMeshProUGUI _textMeshPro;
    private Color originalColor = Color.black;
    public Color highlightColor = Color.white;
    public float maxHighlightWeight = 1f;
    //private float weightThreshold = 0.5f;
    public float highlightSpeed = 1f;
    //private float decaySpeed = 0.2f;
    private int highlightedWordIndex = -1;
    private float currentWordWeight = 0f;

    public float fontSizeIncrease = 2f;

    private Vector2 previousMousePosition;
    private int previousWordIndex = -1;
    private int previousLineIndex = -1;
    private int lineIndex;
    private float fixationThreshold = 0.5f;
    private float fixationTimer = 0.0f;
    private bool soundPlayed = false;
    private bool isFixating = false;

    private List<FixationData> fixationDataList = new List<FixationData>();
    private SaliencyMap saliencyMap = new SaliencyMap();
    private ScanPath scanPath = new ScanPath();

    private Dictionary<int, float> sentenceWeights = new Dictionary<int, float>(); // 문장의 가중치를 저장하는 딕셔너리
    private List<int> trackedIndexes = new List<int>(); // 가중치를 트래킹할 인덱스들의 리스트

    public Text targetWord = null;
    public Text[] texts = new Text[4];
    public Slider[] sliders = new Slider[4];


    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        // 텍스트 메쉬 프로 설정
        _textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
        if (_textMeshPro == null)
        {
            Debug.LogError("TextMeshPro component not found! Make sure it exists in children of this GameObject.");
        }

        // 텍스트에 시각 효과 설정 (필요없음)
        originalColor = _textMeshPro.color;
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 위치를 기반으로 바라보는 단어 추출
        Vector2 mousePosition = Input.mousePosition;
        int wordIndex = TMP_TextUtilities.FindIntersectingWord(_textMeshPro, mousePosition, Camera.main);
        lineIndex = TMP_TextUtilities.FindIntersectingLine(_textMeshPro, mousePosition, Camera.main);
        if (targetWord != null && wordIndex != -1) { targetWord.text = _textMeshPro.textInfo.wordInfo[wordIndex].GetWord(); }
        else { targetWord.text = ""; }

        if (wordIndex == -1 || wordIndex != previousWordIndex)
        {
            fixationTimer = 0.0f;
            //_textMeshPro.text = FileReader.originalText;
            soundPlayed = false;
            isFixating = false;
            previousWordIndex = -1;
        }
        else
        {
            fixationTimer += Time.deltaTime;

            if (fixationTimer >= fixationThreshold)
            {
                Vector3 wordPosition = GetWordPosition(wordIndex);
                if (fixationDataList.Count > 100)
                {
                    fixationDataList.Remove(fixationDataList[0]);
                }
                fixationDataList.Add(new FixationData(new Vector2(wordPosition.x, wordPosition.y), fixationTimer));
                scanPath.AddPosition(new Vector2(wordPosition.x, wordPosition.y)); 
                fixationTimer = 0.0f; // Reset timer after adding a fixation point

                if (!soundPlayed)
                {
                    AudioManager.Instance.PlaySound(lineIndex + FileReader.targetIndex);
                    soundPlayed = true;
                }
            }
 
            isFixating = true;
        }

        previousLineIndex = lineIndex;
        previousWordIndex = wordIndex;
        previousMousePosition = mousePosition;

        // 디버깅을 위해 가중치를 출력
        /*        foreach (var index in trackedIndexes)
                {
                    Debug.Log("Index: " + index + ", Weight: " + (sentenceWeights.ContainsKey(index) ? sentenceWeights[index] : 0f));
                }*/

        // 바라보는 단어의 가중치 증가. 추적 중인 4개의 문장 중 작은 문장과 대체
        /*        if (lineIndex != -1)
                {
                    if (!trackedIndexes.Contains(lineIndex))
                    {
                        if (trackedIndexes.Count >= 4)
                        {
                            int minWeightIndex = trackedIndexes[0];
                            float minWeight = sentenceWeights[minWeightIndex];

                            foreach (var index in trackedIndexes)
                            {
                                if (sentenceWeights.ContainsKey(index) && sentenceWeights[index] < minWeight)
                                {
                                    minWeightIndex = index;
                                    minWeight = sentenceWeights[index];
                                }
                            }

                            trackedIndexes.Remove(minWeightIndex);
                            sentenceWeights.Remove(minWeightIndex);
                        }

                        trackedIndexes.Add(lineIndex);
                        sentenceWeights[lineIndex] = 0f;
                    }

                    float currentHiglightWeight = sentenceWeights[lineIndex];
                    if (currentHiglightWeight < maxHighlightWeight)
                    {
                        currentHiglightWeight += highlightSpeed * Time.deltaTime;
                        sentenceWeights[lineIndex] = currentHiglightWeight;
                    }

                    // 현재 바라보고있는 문장의 가중치와 추적 중인 4개 문장 중 가장 작은 가중치를 비교. 0으로 만들어서 제거. 
                    foreach (var index in trackedIndexes)
                    {
                        if (lineIndex != index && sentenceWeights[lineIndex] > sentenceWeights[index])
                        {
                            sentenceWeights[index] = 0f;
                        }
                    }
                }*/

        // 추적 중인 4개의 문장의 가중치를 일정값 감소
        /*        foreach (var index in trackedIndexes)
                {
                    float currentWeight = sentenceWeights[index];
                    currentWeight -= decaySpeed * Time.deltaTime;
                    sentenceWeights[index] = Mathf.Max(currentWeight, 0f);
                }*/

        // 추적 중인 가중치가 임계값보다 크면 오디오 매니저에게 인덱스와 가중치를 넘겨줌
        /*        if (currentWordWeight >= weightThreshold) 
                {
                    int[] indices = new int[4];
                    float[] weights = new float[4];
                    for (int i = 0; i < trackedIndexes.Count; i++) 
                    {
                        indices[i] = (int)trackedIndexes[i];
                        weights[i] = sentenceWeights[indices[i]];
                    }
                    //AudioManager.Instance.PassParameter(indices, weights);
                }*/

        // 가중치 추적해서 하이라이트 효과 주기
        //TrackWordWeights(wordIndex);
        // 현재 보고 있는 4개 문장의 가중치를 슬라이더로 표현
        //DisplayWeights();
        // 0.2초 동안 바라본 단어와 좌표를 모두 기록. Space로 저장.
        //RecordData(wordIndex);
    }

    public SaliencyMap GetSaliencyMap()
    {
        return saliencyMap;
    }

    public ScanPath GetScanPath()
    {
        return scanPath;
    }

    public List<FixationData> GetFixationDataList()
    {
        return fixationDataList;
    }

    private Vector3 GetWordPosition(int wordIndex)
    {
        TMP_WordInfo wordInfo = _textMeshPro.textInfo.wordInfo[wordIndex];
        int firstCharacterIndex = wordInfo.firstCharacterIndex;
        Vector3 wordPosition = _textMeshPro.textInfo.characterInfo[firstCharacterIndex].bottomLeft;
        return _textMeshPro.transform.TransformPoint(wordPosition);
    }

    void OnApplicationQuit()
    {
        SaveFixationDataToFile();
        scanPath.SaveScanPathToFile();

    }

    private void SaveFixationDataToFile()
    {
        string path = Application.dataPath + "/fixation_data.csv";
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("x,y,duration");
            foreach (var data in fixationDataList)
            {
                writer.WriteLine($"{data.Position.x},{data.Position.y},{data.Duration}");
            }
        }
        Debug.Log("Fixation data saved to fixation_data.csv");
    }

    private void ChangeWordColor(int wordIndex, Color color)
    {
        TMP_WordInfo wordInfo = _textMeshPro.textInfo.wordInfo[wordIndex];
        int startIndex = wordInfo.firstCharacterIndex;
        int length = wordInfo.characterCount;
        string originalText = _textMeshPro.text;

        string newText = originalText.Substring(0, startIndex) +
                         $"<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>" + 
                         originalText.Substring(startIndex, length) + 
                         "</color>" +
                         originalText.Substring(startIndex + length);

        _textMeshPro.text = newText;
    }


    // 단어 추적해서 하이라이트 효과 주기
    void TrackWordWeights(int wordIndex)
    {
        if (wordIndex != -1)
        {
            if (wordIndex != highlightedWordIndex)
            {
                highlightedWordIndex = wordIndex;
                currentWordWeight = 0f;
            }

            if (currentWordWeight < maxHighlightWeight)
            {
                currentWordWeight += highlightSpeed * Time.deltaTime;
            }

            SetWordHighlight(highlightedWordIndex, currentWordWeight);
        }
        else
        {
            if (highlightedWordIndex != -1)
            {
                currentWordWeight -= highlightSpeed * Time.deltaTime;
                SetWordHighlight(highlightedWordIndex, currentWordWeight);

                if (currentWordWeight <= 0f)
                {
                    currentWordWeight = 0f;
                    highlightedWordIndex = -1;
                }
            }
        }
    }

    // 현재 추적 중인 문장의 가중치를 슬라이더에 표현
    void DisplayWeights()
    {
        for(int i = 0; i < trackedIndexes.Count; ++i)
        {
            texts[i].text = trackedIndexes[i].ToString();
            if (trackedIndexes[i] >= 0)
                sliders[i].value = sentenceWeights[trackedIndexes[i]];
            else
                sliders[i].value = 0;
        }
    }

    void SetWordHighlight(int wordIndex, float weight)
    {
/*        TMP_WordInfo wordInfo = _textMeshPro.textInfo.wordInfo[wordIndex];

        Color highlightColorWithWeight = Color.Lerp(originalColor, highlightColor, weight);

        string word = "";
        for (int i = wordInfo.firstCharacterIndex; i <= wordInfo.lastCharacterIndex; i++)
        {
            _textMeshPro.textInfo.characterInfo[i].color = highlightColorWithWeight;
            word += _textMeshPro.textInfo.characterInfo[i].character;
        }
        text.text = word;
        Debug.Log(highlightColorWithWeight);

        _textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
*/
        TMP_WordInfo wordInfo = _textMeshPro.textInfo.wordInfo[wordIndex];

        float originalFontSize = _textMeshPro.fontSize; // 원래 폰트 크기 저장
        float highlightFontSize = originalFontSize + weight * fontSizeIncrease; // 가중치에 따른 크기 조절


        string word = "";
        // 단어의 시작과 끝 인덱스를 사용하여 단어를 추출하고 크기를 조절
        for (int i = wordInfo.firstCharacterIndex; i <= wordInfo.lastCharacterIndex; i++)
        {
            _textMeshPro.textInfo.characterInfo[i].pointSize = highlightFontSize;
            word += _textMeshPro.textInfo.characterInfo[i].character;

        }
        texts[0].text = word + ": " + weight;

        // 텍스트 업데이트
        _textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }



    private string outputFilePath = "Assets/CSV/eye_tracking_data.csv"; // 엑셀 파일 경로
    private StringBuilder csvContent = new StringBuilder(); // CSV 내용
    private bool isRecording = true; // 녹화 중 여부
    private Vector3 wordPosition; // 단어의 스크린 좌표
    private float elapsedTime = 0f; // 머문 시간

    // 0.2초 동안 보는 단어와 좌표를 CSV에 기록. Space를 누르면 파일로 저장.
    void RecordData(int wordIndex)
    {

        if (isRecording)
        {
            // 단어 인덱스를 기반으로 단어의 좌표를 가져옵니다.
            if (wordIndex != -1)
            {
                TMP_WordInfo wordInfo = _textMeshPro.textInfo.wordInfo[wordIndex];

                // 단어의 첫 번째 문자와 마지막 문자의 좌표를 가져옵니다.
                Vector3 wordWorldPositionStart = _textMeshPro.textInfo.characterInfo[wordInfo.firstCharacterIndex].bottomLeft;
                Vector3 wordWorldPositionEnd = _textMeshPro.textInfo.characterInfo[wordInfo.lastCharacterIndex].topRight;

                // 단어의 중앙 좌표를 계산합니다.
                Vector3 wordWorldPosition = (wordWorldPositionStart + wordWorldPositionEnd) / 2f;

                wordPosition = Camera.main.WorldToScreenPoint(wordWorldPosition);
                elapsedTime += Time.deltaTime;
                // 머문 시간이 0.2초를 초과하면 CSV 파일에 기록합니다.
                if (elapsedTime >= 0.2f)
                {
                    elapsedTime = 0f;
                    RecordWordCoordinate(wordInfo);
                }
            }

            // 스페이스바를 누르면 녹화를 종료하고 CSV 파일을 저장합니다.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Recording");
                StopRecording();
            }
        }
    }

    // 단어 좌표와 머문 시간을 CSV 파일에 기록하는 메서드
    private void RecordWordCoordinate(TMP_WordInfo wordInfo)
    {
        int firstCharacterIndex = wordInfo.firstCharacterIndex;
        int lastCharacterIndex = wordInfo.lastCharacterIndex;
        string word = _textMeshPro.text.Substring(firstCharacterIndex, lastCharacterIndex - firstCharacterIndex + 1);


        string wordCharacter = _textMeshPro.textInfo.characterInfo[wordInfo.firstCharacterIndex].character.ToString();
        string csvLine = $"{word},{wordPosition.x},{wordPosition.y},{elapsedTime}\n";
        csvContent.Append(csvLine);
    }

    // 녹화 시작을 위한 메서드
    private void StartRecording()
    {
        isRecording = true;
        Debug.Log("Recording started.");
    }

    // 녹화 종료 및 CSV 파일 저장을 위한 메서드
    private void StopRecording()
    {
        isRecording = false;
        SaveCSV();
        Debug.Log("Recording stopped.");
    }

    // CSV 파일 저장을 위한 메서드
    private void SaveCSV()
    {
        File.WriteAllText(outputFilePath, csvContent.ToString());
        Debug.Log("CSV file saved.");
    }
}