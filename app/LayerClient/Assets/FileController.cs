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
        // 设置要写入的文本
        textToWrite = "Hello, World!";

        // 将文本写入文件
        File.WriteAllText(filePath, textToWrite);
    }

    public void ReadFile()
    {
        string filePath = Application.persistentDataPath + "/playerData.json";
        string json = File.ReadAllText(filePath);

        // 反序列化JSON字符串为对象
        TestData playerData = JsonUtility.FromJson<TestData>(json);

        Debug.Log(playerData.test);
    }
}

