using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;

    public GameObject bulletPrefab; // 발사할 총알 프리팹

    public int numBullets = 3;      // 한 번에 발사할 총알의 개수
    public float angleSpread = 30f; // 퍼지는 각도

    private Vector2 movement;
    public Transform firePoint;

    void Start()
    {
        StartCoroutine(FireBullet());
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 입력 받기
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY).normalized; // 방향 벡터 정규화
    }

    void FixedUpdate()
    {
        // 리지드바디의 velocity로 움직임 처리
        rb.velocity = movement * speed;
    }

    IEnumerator FireBullet() // 총알 발사
    {
        while (true)
        {
            if (numBullets == 1)
            {
                // 한 발만 발사
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse); // 위쪽으로 발사
            }
            else
            {
                float angleStep = angleSpread / (numBullets - 1); // 각도 간격 계산

                for (int i = 0; i < numBullets; i++)
                {
                    // 각도 계산
                    float angle = -angleSpread / 2 + i * angleStep;

                    // 회전 생성
                    Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

                    // 총알 생성
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);

                    // 발사
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.AddForce(bulletRotation * Vector2.up * 5f, ForceMode2D.Impulse);
                }
            }

            yield return new WaitForSeconds(0.1f); // 0.1초 간격으로 발사
        }
    }
}
