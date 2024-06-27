using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class SoundRequest : MonoBehaviour
{
    public Text responseText;  // 유니티 UI에 연결된 서버 응답 출력 필드
    public string filePath = "";
    string basePath = @"C:\Unity\MultiModal\";
    // 버튼 클릭 시 호출될 메서드
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            StartCoroutine(RequestTextFromServer(filePath));
    }


    IEnumerator RequestTextFromServer(string filePath)
    {
        // 서버 연결 설정
        TcpClient client = new TcpClient("127.0.0.1", 65432);

        // 데이터 전송 스트림 설정
        NetworkStream stream = client.GetStream();

        // 파일 경로 전송
        byte[] filePathData = Encoding.ASCII.GetBytes(filePath);
        stream.Write(filePathData, 0, filePathData.Length);
        Debug.Log("Sent file path: " + filePath);

        // 서버로부터 파일 경로 받기
        byte[] responseFilePathData = new byte[1024];
        int filePathBytes = stream.Read(responseFilePathData, 0, responseFilePathData.Length);
        string receivedFilePath = Encoding.ASCII.GetString(responseFilePathData, 0, filePathBytes).Trim();
        Debug.Log("Received file path from server: " + receivedFilePath);

        // 연결 종료
        stream.Close();
        client.Close();

        receivedFilePath = receivedFilePath.Replace(basePath, "");

        // 파일 내용 읽어오기
        string[] lines = File.ReadAllLines(receivedFilePath);

        // 문장을 List<string>에 저장
        SoundEffect.quotes.Clear();  // 기존 내용 초기화
        SoundEffect.quotes.Add("");
        SoundEffect.quotes.Add("어린왕자");
        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();
            if (!string.IsNullOrEmpty(trimmedLine))
            {
                SoundEffect.quotes.Add(trimmedLine);
            }
        }
        SoundEffect.numOfQuotes = SoundEffect.quotes.Count;

        // UI에 표시 (실제로는 Unity UI의 Text 등에 표시)
        responseText.text = "Loaded quotes from file: " + receivedFilePath;

        yield return null;
    }
}