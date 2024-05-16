using UnityEngine;
using TMPro; // 引用TextMeshPro命名空间
using System.Collections;

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI scoreText; // 使用TextMeshProUGUI代替Text
    public Action Action;
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>(); // 获取TextMeshProUGUI组件
        
        StartCoroutine(UpdateScore());
    }

    IEnumerator UpdateScore()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            float score = Action.GetScore();
            int putscore = (int)score;
            
            // 读取得分
            
           
          
            // 更新文本
            scoreText.text = "Score: " + putscore;

            // 等待1秒
            yield return new WaitForSeconds(1);
        }
    }
}
