using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // 싱글톤 인스턴스
    public int score; // Score 변수
    public TMP_Text scoreTxt;

    private void Awake()
    {
        // 싱글톤 인스턴스 초기화
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of Score found! Destroying duplicate.");
            Destroy(gameObject); // 중복된 인스턴스를 삭제
            return;
        }

        // 게임 오브젝트가 씬 전환 시 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        // 씬이 종료되거나 게임이 종료될 때 싱글톤 인스턴스 해제
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Update는 매 프레임마다 호출
    void Update()
    {
        scoreTxt.text = "Score\n" + score.ToString("D10");
    }

    void FixedUpdate()
    {
        score += 1;
    }

    // 점수를 추가하는 메서드
    public void AddScore(int points)
    {
        score += points;
    }
}
