using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MousePositionLogger : MonoBehaviour
{
    private List<Vector3> mousePositions = new List<Vector3>();
    public float timer = 0f;
    public float totalTimer = 0f;
    private float interval = 0.2f;
    private float duration = 10f;
    private string csvFilePath = "Assets/CSV/MousePositions.csv";
    [SerializeField] private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            totalTimer += Time.deltaTime;

            // 매 interval(1초)마다 마우스의 좌표 기록
            if (timer >= interval && totalTimer < duration)
            {
                timer = 0f;
                Vector3 mousePos = Input.mousePosition;
                mousePositions.Add(mousePos);
            }

            // duration(10초)이 지나면 CSV 파일로 저장
            if (totalTimer >= duration)
            {
                SaveMousePositionsToCSV();
            }
        }
    }

    void SaveMousePositionsToCSV()
    {
        // CSV 파일에 헤더 추가
        string csvContent = "Mouse X,Mouse Y\n";

        // 각 좌표를 CSV 형식으로 추가
        foreach (Vector3 pos in mousePositions)
        {
            csvContent += pos.x + "," + pos.y + "\n";
        }

        // CSV 파일로 저장
        File.WriteAllText(csvFilePath, csvContent);

        // 마우스 좌표를 기록한 리스트 초기화
        //mousePositions.Clear();

        Debug.Log("Mouse positions saved to CSV: " + csvFilePath);
    }
}
