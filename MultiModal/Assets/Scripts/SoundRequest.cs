using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class SoundRequest : MonoBehaviour
{
    public Text responseText;  // ����Ƽ UI�� ����� ���� ���� ��� �ʵ�
    public string filePath = "";
    string basePath = @"C:\Unity\MultiModal\";
    // ��ư Ŭ�� �� ȣ��� �޼���
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            StartCoroutine(RequestTextFromServer(filePath));
    }


    IEnumerator RequestTextFromServer(string filePath)
    {
        // ���� ���� ����
        TcpClient client = new TcpClient("127.0.0.1", 65432);

        // ������ ���� ��Ʈ�� ����
        NetworkStream stream = client.GetStream();

        // ���� ��� ����
        byte[] filePathData = Encoding.ASCII.GetBytes(filePath);
        stream.Write(filePathData, 0, filePathData.Length);
        Debug.Log("Sent file path: " + filePath);

        // �����κ��� ���� ��� �ޱ�
        byte[] responseFilePathData = new byte[1024];
        int filePathBytes = stream.Read(responseFilePathData, 0, responseFilePathData.Length);
        string receivedFilePath = Encoding.ASCII.GetString(responseFilePathData, 0, filePathBytes).Trim();
        Debug.Log("Received file path from server: " + receivedFilePath);

        // ���� ����
        stream.Close();
        client.Close();

        receivedFilePath = receivedFilePath.Replace(basePath, "");

        // ���� ���� �о����
        string[] lines = File.ReadAllLines(receivedFilePath);

        // ������ List<string>�� ����
        SoundEffect.quotes.Clear();  // ���� ���� �ʱ�ȭ
        SoundEffect.quotes.Add("");
        SoundEffect.quotes.Add("�����");
        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();
            if (!string.IsNullOrEmpty(trimmedLine))
            {
                SoundEffect.quotes.Add(trimmedLine);
            }
        }
        SoundEffect.numOfQuotes = SoundEffect.quotes.Count;

        // UI�� ǥ�� (�����δ� Unity UI�� Text � ǥ��)
        responseText.text = "Loaded quotes from file: " + receivedFilePath;

        yield return null;
    }
}