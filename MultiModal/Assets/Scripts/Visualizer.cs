using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Visualizer : MonoBehaviour
{
    public int imageWidth = 1024;
    public int imageHeight = 768;
    public int brushSize = 10;
    public Color fixationColor = Color.red;
    public Color backgroundColor = Color.gray;
    public Color scanPathColor = Color.blue;
    private InteractibleText textReader;

    private Texture2D saliencyMapTexture;
    private Texture2D scanPathTexture;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        textReader = FindObjectOfType<InteractibleText>();

        saliencyMapTexture = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
        scanPathTexture = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);

        // Initialize textures with background color
        for (int x = 0; x < imageWidth; x++)
        {
            for (int y = 0; y < imageHeight; y++)
            {
                saliencyMapTexture.SetPixel(x, y, backgroundColor);
                scanPathTexture.SetPixel(x, y, backgroundColor);
            }
        }

        mainCamera = Camera.main;
        saliencyMapTexture.Apply();
        scanPathTexture.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            GenerateSaliencyMap();
            GenerateScanPath();
            SaveTexturesAsImages();
        }
    }

    private void GenerateSaliencyMap()
    {
        //List<FixationData> fixationDataList = textReader.GetFixationDataList();
        List<Vector2> positions = textReader.GetScanPath().Positions;

        /*        foreach (var fixation in fixationDataList)
                {
                    int centerX = (int)(fixation.Position.x / Screen.width * imageWidth);
                    int centerY = (int)((Screen.height - fixation.Position.y) / Screen.height * imageHeight);
                    for (int x = centerX - brushSize / 2; x < centerX + brushSize / 2; x++)
                    {
                        for (int y = centerY - brushSize / 2; y < centerY + brushSize / 2; y++)
                        {
                            if (x >= 0 && x < imageWidth && y >= 0 && y < imageHeight)
                            {
                                saliencyMapTexture.SetPixel(x, y, fixationColor);
                            }
                        }
                    }
                }*/
        foreach (var position in positions)
        {
            Vector2Int pixelPosition = WorldToPixel(position, imageWidth, imageHeight);

            for (int x = pixelPosition.x - brushSize / 2; x < pixelPosition.x + brushSize / 2; x++)
            {
                for (int y = pixelPosition.y - brushSize / 2; y < pixelPosition.y + brushSize / 2; y++)
                {
                    if (x >= 0 && x < imageWidth && y >= 0 && y < imageHeight)
                    {
                        saliencyMapTexture.SetPixel(x, y, fixationColor);
                    }
                }
            }
        }

        saliencyMapTexture.Apply();
    }

    private void GenerateScanPath()
    {
        //ScanPath scanPath = textReader.GetScanPath();
        List<Vector2> positions = textReader.GetScanPath().Positions;

        for (int i = 0; i < positions.Count - 1; i++)
        {
            Vector2 startPoint = WorldToPixel(positions[i], imageWidth, imageHeight);
            Vector2 endPoint = WorldToPixel(positions[i + 1], imageWidth, imageHeight);
            DrawLine(startPoint, endPoint, scanPathColor);
        }

        scanPathTexture.Apply();
    }

    private void DrawLine(Vector2 startPoint, Vector2 endPoint, Color color)
    {
        // Bresenham's line algorithm
        int x0 = (int)startPoint.x;
        int y0 = (int)startPoint.y;
        int x1 = (int)endPoint.x;
        int y1 = (int)endPoint.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = (x0 < x1) ? 1 : -1;
        int sy = (y0 < y1) ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            if (x0 >= 0 && x0 < imageWidth && y0 >= 0 && y0 < imageHeight)
            {
                scanPathTexture.SetPixel(x0, y0, color);
            }

            if (x0 == x1 && y0 == y1) break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

    private void SaveTexturesAsImages()
    {
        byte[] saliencyMapBytes = saliencyMapTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/SaliencyMap.png", saliencyMapBytes);
        Debug.Log("Saliency Map saved as SaliencyMap.png");

        byte[] scanPathBytes = scanPathTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/ScanPath.png", scanPathBytes);
        Debug.Log("Scan Path saved as ScanPath.png");
    }

    private Vector2Int WorldToPixel(Vector3 worldPoint, int textureWidth, int textureHeight)
    {
        Vector2 viewportPoint = mainCamera.WorldToViewportPoint(worldPoint);
        Vector2Int pixelPoint = new Vector2Int(
            Mathf.Clamp((int)(viewportPoint.x * textureWidth), 0, textureWidth - 1),
            Mathf.Clamp((int)((1.0f - viewportPoint.y) * textureHeight), 0, textureHeight - 1)
        );
        return pixelPoint;
    }
}
