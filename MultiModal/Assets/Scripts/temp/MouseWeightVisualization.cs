using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MouseWeightVisualization : MonoBehaviour
{
    public Color weightColor = Color.red; // 가중치 색상
    public float maxDistance = 50f; // 최대 거리
    public float maxWeight = 1f; // 최대 가중치
    public float weightDecayRate = 0.1f; // 가중치 감소율
    public string csvFileName = "Assets/CSV/MousePositions.csv"; // CSV 파일 이름
    public KeyCode saveKey = KeyCode.E; // 저장을 위한 키

    private Texture2D weightTexture; // 시각화할 가중치 텍스처
    private int screenWidth;
    private int screenHeight;
    private Dictionary<Vector2, float> weightMap = new Dictionary<Vector2, float>(); // 좌표와 가중치를 저장할 맵

    public float updateInterval = 0.1f; // 업데이트 간격 설정 (초 단위)
    private float elapsedTime = 0f; // 경과 시간을 저장할 변수

    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        // 텍스처 생성과 초기화
        weightTexture = new Texture2D(screenWidth, screenHeight);
        ClearTexture();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= updateInterval)
        {
            // 가중치 감소
            DecayWeights();

            // 마우스 위치 가져오기
            Vector2 mousePos = Input.mousePosition;
            int mouseX = (int)mousePos.x;
            int mouseY = (int)mousePos.y;

            // 스크린 범위 안에 있는지 확인
            if (mouseX >= 0 && mouseX < screenWidth && mouseY >= 0 && mouseY < screenHeight)
            {
                // 반경 내의 픽셀에 대해 가중치 적용
                for (int x = mouseX - (int)maxDistance; x < mouseX + (int)maxDistance; x++)
                {
                    for (int y = mouseY - (int)maxDistance; y < mouseY + (int)maxDistance; y++)
                    {
                        // 픽셀이 스크린 범위 안에 있는지 확인
                        if (x >= 0 && x < screenWidth && y >= 0 && y < screenHeight)
                        {
                            // 거리에 따른 가중치 계산
                            float distance = Vector2.Distance(new Vector2(mouseX, mouseY), new Vector2(x, y));
                            float weight = Mathf.Clamp01(1f - (distance / maxDistance)) * maxWeight;

                            // 기존 가중치와 새로운 가중치를 고려하여 가중치 맵에 저장
                            Vector2 pixelCoord = new Vector2(x, y);
                            if (weightMap.ContainsKey(pixelCoord))
                            {
                                weightMap[pixelCoord] += weight;
                            }
                            else
                            {
                                weightMap[pixelCoord] = weight;
                            }
                        }
                    }
                }

                // 텍스처 업데이트
                UpdateTexture();
            }

            elapsedTime = 0f;
        }

        // E 키를 눌렀을 때 CSV 파일로 저장
        if (Input.GetKeyDown(saveKey))
        {
            SaveWeightsToCSV();
        }
    }

    void OnGUI()
    {
        // 시각화된 가중치 표시
        GUI.DrawTexture(new Rect(0, 0, screenWidth, screenHeight), weightTexture);
    }

    // 가중치 감소 함수
    void DecayWeights()
    {
        float decayAmount = weightDecayRate * Time.deltaTime;

        // 전체 텍스처의 가중치를 일괄적으로 감소시키기
        for (int x = 0; x < screenWidth; x++)
        {
            for (int y = 0; y < screenHeight; y++)
            {
                // 현재 픽셀의 가중치 가져오기
                Color currentColor = weightTexture.GetPixel(x, y);
                float currentWeight = currentColor.a; // 예시로 가중치를 알파 값으로 저장하고 있다고 가정합니다.

                // 가중치를 감소시키고 텍스처에 적용하기
                float newWeight = Mathf.Max(0f, currentWeight - decayAmount);
                weightTexture.SetPixel(x, y, new Color(currentColor.r, currentColor.g, currentColor.b, newWeight));
            }
        }

        // 텍스처 업데이트
        weightTexture.Apply();
    }

    // 텍스처 업데이트 함수
    void UpdateTexture()
    {
        ClearTexture();

        foreach (var entry in weightMap)
        {
            Vector2 pixelCoord = entry.Key;
            float weight = entry.Value;

            Color currentColor = weightTexture.GetPixel((int)pixelCoord.x, (int)pixelCoord.y);
            Color newColor = Color.Lerp(currentColor, weightColor, weight / maxWeight);

            weightTexture.SetPixel((int)pixelCoord.x, (int)pixelCoord.y, newColor);
        }

        weightTexture.Apply();
    }

    // 텍스처 초기화 함수
    void ClearTexture()
    {
        Color[] clearColorArray = new Color[screenWidth * screenHeight];
        for (int i = 0; i < clearColorArray.Length; i++)
        {
            clearColorArray[i] = Color.clear; // 초기 색상을 투명으로 설정
        }
        weightTexture.SetPixels(clearColorArray);
        weightTexture.Apply();
    }

    // 가중치를 CSV 파일로 저장하는 함수
    void SaveWeightsToCSV()
    {
        //string filePath = Path.Combine(Application.persistentDataPath, csvFileName);
        string filePath = csvFileName;

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var entry in weightMap)
            {
                Vector2 coord = entry.Key;
                float weight = entry.Value;
                string line = string.Format("{0},{1},{2}", coord.x, coord.y, weight);
                writer.WriteLine(line);
            }
        }

        Debug.Log("Weights saved to CSV: " + filePath);
    }
}
