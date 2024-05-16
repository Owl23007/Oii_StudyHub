using System.Collections.Generic;
using TMPro; // 引用TextMeshPro命名空间
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RatePrint : MonoBehaviour
{
  
    public Button closeButton; // 用于关闭Canvas的按钮
    public Canvas scoreCanvas; // 用于显示分数的Canvas
    public TextMeshProUGUI scoreText; // 用于显示分数的TextMeshProUGUI对象

    void Start()
    {
        closeButton.onClick.AddListener(CloseCanvas);
        ShowScores();

    }
    void CloseCanvas()
    {
        // 隐藏Canvas
        scoreCanvas.gameObject.SetActive(false);
    }
    void ShowScores()
    {
        // 从PlayerPrefs加载分数
        int index = PlayerPrefs.GetInt("Index", 1);
        Dictionary<string, int> scores = new Dictionary<string, int>();
        for (int i = 1; i < index; i++)
        {
            string time = PlayerPrefs.GetString("Time" + i, "");
            int score = PlayerPrefs.GetInt("Score" + i, 0);
            scores[time] = score;
        }

        // 按分数排序
        var sortedScores = from entry in scores
                           orderby entry.Value descending
                           select entry;

        // 显示分数
        string scoreTextString = "";
        int count = 0;
        foreach (var entry in sortedScores)
        {
            if (count >= 8)
            {
                break;
            }
            scoreTextString += "Time: " + entry.Key + ", Score: " + entry.Value + "\n";
            count++;
        }
        scoreText.text = scoreTextString;
    }
}

