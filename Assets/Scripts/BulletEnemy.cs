using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float speed = 2.0f;      // 적의 이동 속도
    public GameObject bulletPrefab; // 발사할 총알 프리팹
    public float fireRate = 1.5f;   // 총알 발사 주기 (초 단위)
    private Transform tr;
    public Transform player;        // 플레이어의 Transform
    public int health = 3;          // 적의 체력 (3으로 설정)

    private SpriteRenderer spriteRenderer;
    public Sprite originalSprite;   // 원래 스프라이트 저장
    public Sprite flashSprite;      // 하얗게 깜빡일 때 사용할 스프라이트

    void Start()
    {
        tr = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 위치 찾기
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 참조
        originalSprite = spriteRenderer.sprite; // 원래 스프라이트 저장

        StartCoroutine(DestroySelf());     // 일정 시간이 지나면 파괴
        InvokeRepeating("FireBullet", 1.0f, fireRate); // 주기적으로 총알 발사
    }

    void Update()
    {
        tr.Translate(Vector2.down * speed * Time.deltaTime); // 아래로 이동
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10.0f); // 10초 후 파괴
        Destroy(this.gameObject);
    }

    void FireBullet()
    {
        // 플레이어를 향하는 방향 계산
        Vector2 direction = (player.position - tr.position).normalized;

        // 적의 위치에서 플레이어를 향해 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, tr.position, Quaternion.identity);

        // 총알 발사 방향 설정 및 힘을 가하여 발사
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * 4, ForceMode2D.Impulse); // 총알 발사
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
