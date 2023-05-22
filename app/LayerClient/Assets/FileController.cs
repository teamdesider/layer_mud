using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileController : MonoBehaviour
{

    private string filePath;
    private string textToWrite;
    
    public class TestData
    {
        public string test;
        public int test1;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WriteFile()
    {
        filePath = Application.persistentDataPath + "/test.txt";
        // ����Ҫд����ı�
        textToWrite = "Hello, World!";

        // ���ı�д���ļ�
        File.WriteAllText(filePath, textToWrite);
    }

    public void ReadFile()
    {
        string filePath = Application.persistentDataPath + "/playerData.json";
        string json = File.ReadAllText(filePath);

        // �����л�JSON�ַ���Ϊ����
        TestData playerData = JsonUtility.FromJson<TestData>(json);

        Debug.Log(playerData.test);
    }
}

