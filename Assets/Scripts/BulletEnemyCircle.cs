using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyCircle : MonoBehaviour
{
    public float speed;      // 적의 이동 속도
    public GameObject bulletPrefab; // 발사할 총알 프리팹
    public float fireRate = 1.0f;   // 총알 발사 주기 (초 단위)
    private Transform tr;
    public Transform player;        // 플레이어의 Transform
    public int health = 5;          // 적의 체력 (5로 설정)
    public float easeTime = 2.0f;   // Ease Out 효과의 지속 시간

    private bool hasStopped = false; // 이동이 완료되었는지 확인
    private float moveStartTime;    // 이동 시작 시간을 기록할 변수
    private SpriteRenderer spriteRenderer;
    public Sprite originalSprite;   // 원래 스프라이트 저장
    public Sprite flashSprite;      // 하얗게 깜빡일 때 사용할 스프라이트

    private float angleOffset = 0f; // 각도를 회전시키기 위한 변수

    void Start()
    {
        tr = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 위치 찾기
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 참조
        originalSprite = spriteRenderer.sprite; // 원래 스프라이트 저장

        // speed 값을 5에서 10 사이로 랜덤하게 설정
        speed = Random.Range(5f, 10f);

        moveStartTime = Time.time; // 이동 시작 시간 기록
        StartCoroutine(DestroySelf());     // 일정 시간이 지나면 파괴
        InvokeRepeating("FireBullet", 1.0f, fireRate); // 주기적으로 총알 발사
    }

    void Update()
    {
        // Ease Out 이동 구현 (처음에는 빠르게, 시간이 지나면서 점차 느려짐)
        if (!hasStopped)
        {
            // t는 이동 시작부터 현재까지 경과한 시간에 비례
            float t = Mathf.Clamp01((Time.time - moveStartTime) / easeTime); // 이동 시작부터 현재까지의 시간 비율
            float easeOutSpeed = speed * Mathf.Pow(1 - t, 2); // Ease Out 효과: 시간 경과에 따라 속도가 점점 감소

            tr.Translate(Vector2.down * easeOutSpeed * Time.deltaTime); // 아래로 이동

            if (t >= 1) // 이동이 완료되면 원형으로 발사
            {
                hasStopped = true; // 이동 완료
            }
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10.0f); // 10초 후 파괴
        Destroy(this.gameObject);
    }

    void FireBullet()
    {
        if (hasStopped) // 이동이 끝난 후 총알 발사
        {
            // 원형 경로로 발사할 총알의 수 (8발)
            int numBullets = 8;
            float angleStep = 360.0f / numBullets; // 각 총알 간의 각도 차이

            for (int i = 0; i < numBullets; i++)
            {
                // 각도를 계산하여 총알 발사 방향을 설정
                float angle = angleOffset + (i * angleStep); // 각도를 계속 돌리기 위해 angleOffset을 추가
                Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

                // 적의 위치에서 발사
                GameObject bullet = Instantiate(bulletPrefab, tr.position, bulletRotation);

                // 원형 경로로 총알 발사
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(bulletRotation * Vector2.up * 4, ForceMode2D.Impulse); // 원형으로 발사
            }

            // 발사 후 각도를 회전시킴 (다음 발사를 위해)
            angleOffset += 10f; // 각도를 10도씩 증가시켜서 발사 방향을 바꾼다.
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet")) // 플레이어의 총알에 맞으면
        {
            // 체력 감소
            health--;

            // 하얗게 깜빡이는 효과
            StartCoroutine(FlashWhite());

            if (health <= 0) // 체력이 0이 되면 적 파괴
            {
                GameObject.Find("GameManager").GetComponent<Score>().score += 10;
                Destroy(gameObject); // 적 파괴
            }

            Destroy(collision.gameObject); // 플레이어의 총알 파괴
        }
    }

    // 하얗게 깜빡이는 모션 구현 (스프라이트 이미지 변경)
    IEnumerator FlashWhite()
    {
        spriteRenderer.sprite = flashSprite; // 하얀색 스프라이트로 변경
        yield return new WaitForSeconds(0.1f); // 잠시 대기
        spriteRenderer.sprite = originalSprite; // 원래 스프라이트로 복원
    }
}
