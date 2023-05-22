using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI debugAreaText = null;

    [SerializeField]
    private bool enableDebug = true;

    [SerializeField]
    private int maxLines = 8;

    public static DebugManager Instance { get; private set; }

    private void Awake()
    {
        // 检查是否已经存在实例，如果存在则销毁当前实例，保证只有一个实例存在
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 设置当前实例为唯一实例
        Instance = this;

        // 在场景切换时不销毁实例
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        debugAreaText.enabled = enableDebug;
        //enabled = enableDebug;
    }

    public void LogInfo(string message)
    {
        ClearLines();
        Debug.Log(message);
        debugAreaText.text += $"{DateTime.Now:yyyy-dd-M HH:mm:ss}: <color=\"white\">{message}</color>\n";
    }

    public void LogError(string message)
    {
        ClearLines();
        Debug.Log(message);
        debugAreaText.text += $"{DateTime.Now:yyyy-dd-M HH:mm:ss}: <color=\"red\">{message}</color>\n";
    }

    public void LogWarning(string message)
    {
        ClearLines();
        Debug.Log(message);
        debugAreaText.text += $"{DateTime.Now.ToString("yyyy-dd-M HH:mm:ss")}: <color=\"yellow\">{message}</color>\n";
    }

    public void LogSuccess(string message)
    {
        ClearLines();
        Debug.Log(message);
        debugAreaText.text += $"{DateTime.Now.ToString("yyyy-dd-M HH:mm:ss")}: <color=\"green\">{message}</color>\n";
    }

    private void ClearLines()
    {
        if (debugAreaText.text.Split('\n').Count() >= maxLines)
        {
            debugAreaText.text = string.Empty;
        }
    }

    public void ToggleDebugLogUI() {
        debugAreaText.gameObject.SetActive(!debugAreaText.gameObject.activeSelf);
    }
}