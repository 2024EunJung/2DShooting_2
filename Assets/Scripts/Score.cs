using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int score; //외부에서 사용가능하도록!
    public Text textScore;
    public int bestScore; // 최고 점수 저장할 변수
    public Text bestScoreText; // 최고 점수를 나타낼 변수 선언
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textScore.text = "Score : " + score;
        bestScore = PlayerPrefs.GetInt("Best");
        bestScoreText.text = "BestScore : " + bestScore;
        if(bestScore < score)
        {
            bestScore = score;
            PlayerPrefs.SetInt("Best",bestScore);
        }
    }
}
