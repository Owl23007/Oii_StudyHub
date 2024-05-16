using UnityEngine;
using TMPro; // 引用TextMeshPro命名空间
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnControl : MonoBehaviour
{
   
    private TextMeshProUGUI scoreText; // 使用TextMeshProUGUI代替Text
    public Action Action;
    void Start()
    {
        
        scoreText = GetComponent<TextMeshProUGUI>(); // 获取TextMeshProUGUI组件
        StartCoroutine(CountTime());
        StartCoroutine(UpdateScore());
    }
    IEnumerator CountTime()
    {
        for (int i = 3; i >0; i--)
        {
            
            scoreText.text = "QE控制左右倾斜\r\n       W加速\r\n      AD转向\r\n            " + i;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator UpdateScore()
    {
        yield return new WaitForSeconds(3);
        
        while (true)
        {
            float score = Action.GetScore();
            int putscore = (int)score;
            if (Action.isTrue)
            {
                int index = PlayerPrefs.GetInt("Index", 1);
                // 获取当前时间
                string currentTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // 将时间和分数保存到PlayerPrefs
                PlayerPrefs.SetString("Time" + index, currentTime);
                PlayerPrefs.SetInt("Score" + index, putscore);
                
                index++; // 增加索引
                PlayerPrefs.SetInt("Index", index);
                PlayerPrefs.Save(); // 确保数据被保存

                scoreText.text = "\n\n\nFinal Score: " + putscore;
                yield return new WaitForSeconds(3);

                SceneManager.LoadScene("StartScene");
            }
            // 更新文本
            scoreText.text =  " ";

            // 等待1秒
            yield return new WaitForSeconds(1);
        }
    }
}
