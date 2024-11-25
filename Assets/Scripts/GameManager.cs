using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // 싱글톤 인스턴스

    public int maxHp = 10; // 최대 HP
    public int hp;          // 현재 HP
    public int maxXp = 500; // 최대 XP
    public int xp;          // 현재 XP
    public int level = 1;   // 현재 레벨
    public int score;       // Score 변수
    public int power = 1;
    public int maxPower;

    
    public TMP_Text scoreTxt;
    public TMP_Text hp_Text;
    public TMP_Text xp_Text;
    public TMP_Text level_Text;
    public Image hpBar;
    public Image xpBar;
    public Image powerBar;

    public GameObject player;

    public Transform cameraTransform; // 카메라 Transform
    private Vector3 originalCameraPosition; // 카메라의 원래 위치

    private void Awake()
    {
        // 싱글톤 인스턴스 초기화
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of GameManager found! Destroying duplicate.");
            Destroy(gameObject); // 중복된 인스턴스를 삭제
            return;
        }

        // 게임 오브젝트가 씬 전환 시 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);
    }

    // 카메라 흔들림 함수
    public void CameraShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            cameraTransform.localPosition = new Vector3(
                originalCameraPosition.x + offsetX,
                originalCameraPosition.y + offsetY,
                originalCameraPosition.z
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalCameraPosition; // 원래 위치로 복원
    }

    private void OnDestroy()
    {
        // 씬이 종료되거나 게임이 종료될 때 싱글톤 인스턴스 해제
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        // 초기 HP 설정
        hp = maxHp;
        xp = 0;
        originalCameraPosition = cameraTransform.localPosition;
    }

    private void Update()
    {
        // UI 업데이트
        scoreTxt.text = "Score\n" + score.ToString("D10");
        hp_Text.text = $"{hp}";
        xp_Text.text = $"{xp}/{maxXp}";
        level_Text.text = $"{level}";

        // Update fill amounts
        hpBar.fillAmount = (float)hp / maxHp;
        xpBar.fillAmount = (float)xp / maxXp;
        powerBar.fillAmount = (float)power / maxPower;

        PlayerMove pl = player.GetComponent<PlayerMove>();
        pl.numBullets = power;

        // Update HP bar color
        UpdateHpBarColor();
    }

    private void FixedUpdate()
    {
        // 점수 증가 (매 프레임)
        score += 1;
    }

    // 점수를 추가하는 메서드
    public void AddScore(int points)
    {
        score += points;
    }

    public void AddPower(int points)
    {
        power += points;
        if(power >= maxPower)
        {
            power = maxPower;
        }
    }

    // XP를 추가하는 메서드
    public void AddXp(int amount)
    {
        xp += amount;

        // 레벨업 체크
        while (xp >= maxXp)
        {
            LevelUp();
        }
    }

    // 레벨업 처리
    private void LevelUp()
    {
        xp -= maxXp; // 남은 XP를 계산
        level++;     // 레벨 증가
        maxXp += 500; // 다음 레벨업에 필요한 XP 증가

        // 체력 회복 등 레벨업 보상 (선택 사항)
        maxHp += 10; 
        hp = maxHp;
    }

    // HP를 감소시키는 메서드
    public void TakeDamage(int damage)
    {
        hp -= damage;
        CameraShake(0.1f, (float)damage/2);
        if (hp <= 0)
        {
            hp = 0;
            GameOver();
        }
    }

    // HP를 회복시키는 메서드
    public void Heal(int amount)
    {
        hp += amount;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
    }

    // HP 바의 색상 업데이트
    private void UpdateHpBarColor()
    {
        // 체력 비율 계산 (0.0 ~ 1.0)
        float hpRatio = (float)hp / maxHp;

        // 체력에 따른 색상 계산 (초록 -> 빨강)
        Color hpColor = Color.Lerp(Color.red, Color.green, hpRatio);

        // hpBar의 색상 적용
        hpBar.color = hpColor;
    }

    // 게임 오버 처리
    private void GameOver()
    {
        SceneManager.LoadScene(1);
    }
}
