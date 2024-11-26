using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public TMP_Text scoreText; // 점수를 표시할 텍스트

    void Start()
    {
        // 최고 점수를 불러와 텍스트에 표시
        if (scoreText != null)
        {

            int maxScore = PlayerPrefs.GetInt("MaxScore", 0);
            scoreText.text = $"WIN\nSCORE: {maxScore}";
        }
    }

    // 다시하기 버튼 함수
    public void RestartGame()
    {
        SceneManager.LoadScene(0); // 씬 1 로드
    }
}
