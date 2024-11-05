using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int score; //�ܺο��� ��밡���ϵ���!
    public Text textScore;
    public int bestScore; // �ְ� ���� ������ ����
    public Text bestScoreText; // �ְ� ������ ��Ÿ�� ���� ����
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
