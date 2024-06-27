using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FixationMapGenerator : MonoBehaviour
{
    public string outputDirectory = "Assets/Output/"; // 출력 디렉토리 경로
    public float sigma = 25f; // 가우시안 필터 시그마 값
    public int duration = 7; // 지속 시간
    public bool weighted = true; // 가중치 사용 여부

    private List<Vector2> fixationCoords = new List<Vector2>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼이 클릭되었는지 확인
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector2 worldMousePosition = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);

            // 마우스 클릭 위치를 리스트에 추가
            fixationCoords.Add(worldMousePosition);
        }

        // 스페이스바가 눌렸을 때
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 고정맵과 히트맵 생성
            GenerateFixationMapAndHeatmap();
        }
    }

    public void GenerateFixationMapAndHeatmap()
    {
        int w = Screen.width;
        int h = Screen.height;

        float scalar = Mathf.Min(w, h);

        string fixmapsDir = Path.Combine(outputDirectory, "fixmaps_" + duration + "s");
        string heatmapsDir = Path.Combine(outputDirectory, "heatmaps_" + duration + "s");

        Directory.CreateDirectory(fixmapsDir);
        Directory.CreateDirectory(heatmapsDir);

        float[,] fixations = new float[w, h];
        float[,] heatmap = new float[w, h];

        foreach (Vector2 coord in fixationCoords)
        {
            int x = Mathf.FloorToInt(coord.x * w);
            int y = Mathf.FloorToInt(coord.y * h);

            if (x >= 0 && x < w && y >= 0 && y < h)
            {
                float timeDuration = 1.0f;
                if (weighted)
                {
                    // 가중치를 사용할 경우 마우스 클릭 지속 시간을 반영
                    timeDuration = duration;
                }
                fixations[x, y] += timeDuration;
                heatmap[x, y] = 1;
            }
        }

        float[,] heatmapBlurred = GaussianBlur(heatmap, sigma);

        // Find max value in blurred heatmap
        float maxVal = 0;
        for (int i = 0; i < heatmapBlurred.GetLength(0); i++)
        {
            for (int j = 0; j < heatmapBlurred.GetLength(1); j++)
            {
                maxVal = Mathf.Max(maxVal, heatmapBlurred[i, j]);
            }
        }

        // Normalize heatmap
        if (maxVal != 0)
        {
            for (int i = 0; i < heatmapBlurred.GetLength(0); i++)
            {
                for (int j = 0; j < heatmapBlurred.GetLength(1); j++)
                {
                    heatmapBlurred[i, j] /= maxVal;
                }
            }
        }

        SaveTextureToFile(fixmapsDir, "fixmap.png", fixations);
        SaveTextureToFile(heatmapsDir, "heatmap.png", heatmap);
    }

    // 가우시안 블러링 함수
    private float[,] GaussianBlur(float[,] input, float sigma)
    {
        int size = Mathf.CeilToInt(sigma * 6);

        // Ensure that size is odd
        if (size % 2 == 0)
        {
            size++;
        }

        int halfSize = size / 2;
        float[,] kernel = new float[size, size];
        float[,] output = new float[input.GetLength(0), input.GetLength(1)];

        // Generate Gaussian kernel
        float twoSigmaSquare = 2 * sigma * sigma;
        float constant = 1 / (Mathf.PI * twoSigmaSquare);
        float total = 0;

        for (int x = -halfSize; x <= halfSize; x++)
        {
            for (int y = -halfSize; y <= halfSize; y++)
            {
                float distance = x * x + y * y;
                int i = x + halfSize;
                int j = y + halfSize;
                kernel[i, j] = constant * Mathf.Exp(-distance / twoSigmaSquare);
                total += kernel[i, j];
            }
        }

        // Normalize the kernel
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                kernel[x, y] /= total;
            }
        }

        // Convolution
        for (int i = 0; i < input.GetLength(0); i++)
        {
            for (int j = 0; j < input.GetLength(1); j++)
            {
                float sum = 0;
                for (int k = -halfSize; k <= halfSize; k++)
                {
                    for (int l = -halfSize; l <= halfSize; l++)
                    {
                        int ii = Mathf.Clamp(i + k, 0, input.GetLength(0) - 1);
                        int jj = Mathf.Clamp(j + l, 0, input.GetLength(1) - 1);
                        sum += kernel[k + halfSize, l + halfSize] * input[ii, jj];
                    }
                }
                output[i, j] = sum;
            }
        }

        return output;
    }

    // 2차원 배열을 이미지 파일로 저장하는 함수
    private void SaveTextureToFile(string directory, string fileName, float[,] data)
    {
        Debug.Log("Save texture to file");
        Texture2D texture = new Texture2D(data.GetLength(0), data.GetLength(1));
        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                texture.SetPixel(x, y, new Color(data[x, y], data[x, y], data[x, y]));
            }
        }
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Path.Combine(directory, fileName), bytes);
    }
}
